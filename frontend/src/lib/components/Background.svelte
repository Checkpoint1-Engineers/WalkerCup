<script>
    import { onMount } from "svelte";

    export let mode = "HOLOGRAM";

    let canvas;
    let width, height;

    // Config
    const droneSrc = "/assets/drone/2.webp"; // Using existing asset

    // State
    let mouse = { x: 0, y: 0 };
    let animationFrame;
    let time = 0;
    let entry = 0;

    // Assets
    let droneImg;
    let loaded = false;

    // HUD Config
    const hudColor = "#00f3ff";
    const hudColorDim = "rgba(0, 243, 255, 0.3)";

    // Rings Data
    const rings = [
        { r: 180, w: 2, speed: 0.002, dash: [0], opacity: 0.8 },
        { r: 220, w: 1, speed: -0.003, dash: [10, 10], opacity: 0.5 },
        { r: 240, w: 5, speed: 0.001, dash: [2, 20], opacity: 0.3 },
        { r: 350, w: 1, speed: 0.005, dash: [50, 50], opacity: 0.4 },
        { r: 360, w: 20, speed: -0.001, dash: [2, 100], opacity: 0.1 }, // Decorative big segments
    ];

    onMount(() => {
        const ctx = canvas.getContext("2d");
        let isMobile = false;

        // Load Drone
        const img = new Image();
        img.src = droneSrc;
        img.onload = () => {
            droneImg = img;
            loaded = true;
        };

        function resize() {
            const rect = canvas.getBoundingClientRect();
            width = rect.width;
            height = rect.height;
            canvas.width = width;
            canvas.height = height;
            isMobile = width < 768;
        }

        function handleMouseMove(e) {
            const rect = canvas.getBoundingClientRect();
            mouse.x = e.clientX - rect.left;
            mouse.y = e.clientY - rect.top;
        }

        // Helper: Draw Ring
        function drawRing(ctx, cx, cy, ring) {
            ctx.save();
            ctx.translate(cx, cy);
            ctx.rotate(time * ring.speed);

            // Pulse effect on rings
            const pulse = 1 + Math.sin(time * 0.05) * 0.02;
            ctx.scale(pulse, pulse);

            ctx.beginPath();
            ctx.arc(0, 0, ring.r * (isMobile ? 0.6 : 1), 0, Math.PI * 2); // Scale rings on mobile
            ctx.lineWidth = ring.w;
            ctx.strokeStyle = hudColor;
            ctx.globalAlpha = ring.opacity;
            if (ring.dash) ctx.setLineDash(ring.dash);
            ctx.stroke();

            ctx.restore();
        }

        // Helper: Draw Tech Text
        function drawTechText(ctx, x, y, text, align = "left") {
            ctx.save();
            ctx.fillStyle = hudColor;
            ctx.font = isMobile ? "8px monospace" : "10px monospace";
            ctx.textAlign = align;
            ctx.shadowBlur = 5;
            ctx.shadowColor = hudColor;
            ctx.fillText(text, x, y);
            ctx.restore();
        }

        // Helper: Draw Peripheral UI
        function drawUI(ctx, cx, cy) {
            // Hide complex brackets on mobile, too cluttered
            if (isMobile) {
                // Simple mobile corners instead
                const mSize = 100;
                ctx.save();
                ctx.strokeStyle = hudColorDim;
                ctx.lineWidth = 1;

                // Top Left
                ctx.beginPath();
                ctx.moveTo(20, 100);
                ctx.lineTo(20, 20);
                ctx.lineTo(100, 20);
                ctx.stroke();

                // Bottom Right
                ctx.beginPath();
                ctx.moveTo(width - 20, height - 100);
                ctx.lineTo(width - 20, height - 20);
                ctx.lineTo(width - 100, height - 20);
                ctx.stroke();

                ctx.restore();
                return;
            }

            // Desktop UI
            ctx.save();
            ctx.strokeStyle = hudColorDim;
            ctx.lineWidth = 1;

            // Left Bracket
            ctx.beginPath();
            ctx.moveTo(cx - 500, cy - 200);
            ctx.lineTo(cx - 550, cy - 200);
            ctx.lineTo(cx - 550, cy + 200);
            ctx.lineTo(cx - 500, cy + 200);
            ctx.stroke();

            // Right Bracket
            ctx.beginPath();
            ctx.moveTo(cx + 500, cy - 200);
            ctx.lineTo(cx + 550, cy - 200);
            ctx.lineTo(cx + 550, cy + 200);
            ctx.lineTo(cx + 500, cy + 200);
            ctx.stroke();

            // Random Data Streams
            const randomH = Math.sin(time * 0.05) * 50;
            drawTechText(
                ctx,
                cx - 540,
                cy - 50 + randomH,
                `SYS.ROT: ${(time * 0.1).toFixed(2)}`,
                "left",
            );
            drawTechText(
                ctx,
                cx - 540,
                cy - 30 + randomH,
                `TGT.LOCK: ACQUIRED`,
                "left",
            );
            drawTechText(ctx, cx - 540, cy - 10 + randomH, `MEM: 64TB`, "left");

            drawTechText(ctx, cx + 540, cy, `DRONE_ID: WK-77`, "right");
            drawTechText(ctx, cx + 540, cy + 20, `STATUS: ONLINE`, "right");

            ctx.restore();
        }

        function drawDrone(ctx, cx, cy, easeEntry = 1) {
            if (!droneImg) return;

            ctx.save();
            ctx.translate(cx, cy);

            // Entry scale effect
            const entryScale = 0.5 + 0.5 * easeEntry;
            ctx.scale(entryScale, entryScale);

            // Subtle Float + Pulse
            const floatY = Math.sin(time * 0.03) * (isMobile ? 5 : 10);
            ctx.translate(0, floatY);

            // Scale to fit inside rings
            const baseScale = isMobile ? 0.2 : 0.28; // Reduced from 0.3/0.4
            const dw = droneImg.width * baseScale;
            const dh = droneImg.height * baseScale;

            // Draw Drone
            ctx.globalAlpha = 0.9;
            ctx.shadowBlur = 15 + Math.sin(time * 0.1) * 10; // Pulsing shadow
            ctx.shadowColor = hudColor;
            ctx.drawImage(droneImg, -dw / 2, -dh / 2, dw, dh);

            // Scan effect over entire screen width
            ctx.save();
            ctx.globalCompositeOperation = "lighter";
            const scanY = ((time * 3) % (height + 100)) - height / 2 - 50;

            // Draw full width scanline (relative to center cx, cy)
            ctx.beginPath();
            ctx.moveTo(-cx, scanY);
            ctx.lineTo(width - cx, scanY);
            ctx.strokeStyle = "rgba(0, 243, 255, 0.5)";
            ctx.lineWidth = 2;
            ctx.shadowBlur = 20;
            ctx.shadowColor = hudColor;
            ctx.stroke();

            // Additional Scan Particles
            if (Math.random() > 0.8) {
                ctx.fillStyle = hudColor;
                ctx.fillRect(Math.random() * width - cx, scanY, 2, 2);
            }

            ctx.restore();

            ctx.restore();
        }

        function draw() {
            time += 1;

            // Clear & BG
            const bgGrad = ctx.createRadialGradient(
                width / 2,
                height / 2,
                0,
                width / 2,
                height / 2,
                width,
            );
            bgGrad.addColorStop(0, "#050b14");
            bgGrad.addColorStop(1, "#000000");
            ctx.fillStyle = bgGrad;
            ctx.fillRect(0, 0, width, height);

            if (!loaded) {
                animationFrame = requestAnimationFrame(draw);
                return;
            }

            // Entry Animation
            if (entry < 1) {
                entry += 0.015;
                if (entry > 1) entry = 1;
            }
            // Ease out cubic
            const easeEntry = 1 - Math.pow(1 - entry, 3);

            const cx = width / 2;
            const cy = height / 2;

            // Parallax based on mouse
            const px = (mouse.x - cx) * 0.02;
            const py = (mouse.y - cy) * 0.02;

            ctx.save();
            ctx.translate(px, py);

            // Only draw HUD/Drone in HOME mode
            if (mode === "HOME" || mode === "HOLOGRAM") {
                // Global entry fade
                ctx.globalAlpha = easeEntry;

                // 1. Draw Rings
                // Scale rings up from 0.8 to 1 during entry
                ctx.save();
                const ringEntryScale = 0.8 + 0.2 * easeEntry;
                ctx.scale(ringEntryScale, ringEntryScale);
                rings.forEach((ring) => drawRing(ctx, cx, cy, ring));
                ctx.restore();

                // 2. Draw Drone
                // Fly in from "back" (smaller to normal) or opacity
                drawDrone(ctx, cx, cy, easeEntry); // Pass easeEntry to drawDrone

                // 3. Draw UI
                drawUI(ctx, cx, cy);
            }

            ctx.restore();

            animationFrame = requestAnimationFrame(draw);
        }

        window.addEventListener("resize", resize);
        window.addEventListener("mousemove", handleMouseMove);

        // Touch move for pseudo-parallax on mobile
        window.addEventListener(
            "touchmove",
            (e) => {
                const rect = canvas.getBoundingClientRect();
                mouse.x = e.touches[0].clientX - rect.left;
                mouse.y = e.touches[0].clientY - rect.top;
            },
            { passive: true },
        );

        setTimeout(() => {
            resize();
            draw();
        }, 0);

        return () => {
            window.removeEventListener("resize", resize);
            window.removeEventListener("mousemove", handleMouseMove);
            cancelAnimationFrame(animationFrame);
        };
    });
</script>

<canvas bind:this={canvas} class="artifact-bg"></canvas>

<style>
    .artifact-bg {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: #000;
        z-index: 0;
        pointer-events: none;
    }
</style>
