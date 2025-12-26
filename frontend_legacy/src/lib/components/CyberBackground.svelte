<script>
    import { onMount, onDestroy } from "svelte";

    let canvas;
    let width, height;
    let ctx;
    let animationFrame;
    let particles = [];

    // Config
    const color = "rgba(0, 243, 255, 0.1)";
    const count = 50;

    function resize() {
        if (!canvas) return;
        const parent = canvas.parentElement;
        width = parent.clientWidth;
        height = parent.clientHeight;
        canvas.width = width;
        canvas.height = height;
        initParticles();
    }

    function initParticles() {
        particles = [];
        for (let i = 0; i < count; i++) {
            particles.push({
                x: Math.random() * width,
                y: Math.random() * height,
                size: Math.random() * 2 + 0.5,
                speedY: Math.random() * 0.5 + 0.1,
                opacity: Math.random() * 0.5 + 0.1,
            });
        }
    }

    function draw() {
        ctx.clearRect(0, 0, width, height);

        // Draw connecting lines
        ctx.lineWidth = 0.5;

        particles.forEach((p, i) => {
            p.y -= p.speedY;
            if (p.y < 0) p.y = height;

            // Draw particle
            ctx.fillStyle = `rgba(0, 243, 255, ${p.opacity})`;
            ctx.beginPath();
            ctx.arc(p.x, p.y, p.size, 0, Math.PI * 2);
            ctx.fill();

            // Connect nearby
            for (let j = i + 1; j < particles.length; j++) {
                const p2 = particles[j];
                const dx = p.x - p2.x;
                const dy = p.y - p2.y;
                const dist = Math.sqrt(dx * dx + dy * dy);

                if (dist < 100) {
                    ctx.strokeStyle = `rgba(0, 243, 255, ${0.1 * (1 - dist / 100)})`;
                    ctx.beginPath();
                    ctx.moveTo(p.x, p.y);
                    ctx.lineTo(p2.x, p2.y);
                    ctx.stroke();
                }
            }
        });

        // Draw Grid Overlay
        /*
        ctx.strokeStyle = "rgba(255, 255, 255, 0.02)";
        ctx.lineWidth = 1;
        const gridSize = 100;
        const timeOffset = Date.now() * 0.02;
        
        for (let x = 0; x <= width; x += gridSize) {
            ctx.beginPath();
            ctx.moveTo(x, 0);
            ctx.lineTo(x, height);
            ctx.stroke();
        }
        */

        animationFrame = requestAnimationFrame(draw);
    }

    onMount(() => {
        ctx = canvas.getContext("2d");
        window.addEventListener("resize", resize);
        resize();
        draw();
    });

    onDestroy(() => {
        if (typeof window !== "undefined")
            window.removeEventListener("resize", resize);
        cancelAnimationFrame(animationFrame);
    });
</script>

<canvas bind:this={canvas} class="cyber-bg"></canvas>

<style>
    .cyber-bg {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        pointer-events: none;
        z-index: 0;
        /* Blend mode for cool effect */
        mix-blend-mode: screen;
    }
</style>
