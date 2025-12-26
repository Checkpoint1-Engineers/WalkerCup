<script>
    import { scramble } from "$lib/actions/scramble.js";
    import CyberButton from "$lib/components/CyberButton.svelte";
    import { onMount } from "svelte";

    let visible = false;
    let mouseX = 0;
    let mouseY = 0;

    onMount(() => {
        setTimeout(() => (visible = true), 200);
    });

    function handleMouseMove(e) {
        const rect = e.currentTarget.getBoundingClientRect();
        mouseX = (e.clientX - rect.left) / rect.width - 0.5;
        mouseY = (e.clientY - rect.top) / rect.height - 0.5;
    }
</script>

<!-- svelte-ignore a11y-no-static-element-interactions -->
<div class="home-wrapper" class:visible on:mousemove={handleMouseMove}>
    <!-- Reference-style Background -->
    <div class="bg-effects">
        <!-- Deep Void Gradient -->
        <div class="void-bg"></div>
        <div class="hero-bg-img"></div>

        <!-- Fog Layers -->
        <div class="fog-layer fog-1"></div>
        <div class="fog-layer fog-2"></div>

        <!-- Rising Energy Particles (The Reference Look) -->
        <div class="energy-streams">
            {#each Array(15) as _, i}
                <div
                    class="energy-beam"
                    style="
                        --left: {10 + i * 6}%; 
                        --delay: -{Math.random() * 5}s; 
                        --duration: {3 + Math.random() * 4}s;
                        --scale: {0.5 + Math.random() * 1};
                        --opacity: {0.3 + Math.random() * 0.5};
                    "
                ></div>
            {/each}
        </div>

        <!-- Interactive Glow Orb (Mouse Following) -->
        <div
            class="interactive-glow"
            style="transform: translate({mouseX * 50}px, {mouseY * 50}px)"
        ></div>
    </div>

    <!-- Hero Content -->
    <section class="hero">
        <div class="hero-inner">
            <!-- Zone Badge -->
            <div class="status-badge">
                <div class="pulse-dot"></div>
                <span>SYSTEM ONLINE</span>
            </div>

            <!-- Main Heading -->
            <h1
                class="zone-title"
                use:scramble={{
                    autoStart: true,
                    speed: 1,
                    chars: "WALKERZONE",
                }}
            >
                WALKER ZONE
            </h1>

            <!-- Tagline -->
            <p class="tagline">The Ultimate Competitive Arena</p>

            <!-- Separator -->
            <div class="separator">
                <div class="line"></div>
                <div class="diamond"></div>
                <div class="line"></div>
            </div>

            <!-- Description -->
            <p class="description">
                Enter the void. Compete in high-stakes tournaments. Forge your
                legacy in the digital realm.
            </p>

            <!-- Action Buttons -->
            <div class="actions">
                <CyberButton
                    href="/tournaments"
                    variant="primary"
                    label="ENTER ARENA"
                />
                <CyberButton
                    href="/recruiting"
                    variant="secondary"
                    label="JOIN FORCES"
                />
            </div>
        </div>
    </section>
</div>

<style>
    /* === WRAPPER === */
    .home-wrapper {
        position: relative;
        /* Counteract padding:
           - Top: -40px (consumes content-area top padding)
           - Sides: -20px (consumes content + container side padding)
           - Bottom: -10px (consumes content-area bottom padding ONLY, preserving border flow)
        */
        margin: -40px -20px -10px -20px;
        width: calc(100% + 40px);
        display: flex;
        align-items: center;
        justify-content: center;
        overflow: hidden;
        opacity: 0;
        transform: translateY(20px);
        transition:
            opacity 1s ease,
            transform 1s ease;
        /* Height adjustment */
        min-height: 100%;
    }
    .home-wrapper.visible {
        opacity: 1;
        transform: translateY(0);
    }

    /* === BACKGROUND EFFECTS === */
    .bg-effects {
        position: absolute;
        inset: 0;
        z-index: 0;
        pointer-events: none;
        overflow: hidden;
    }

    .void-bg {
        position: absolute;
        inset: 0;
        /* Match the main card background exactly so it blends seamlessly */
        background: var(--wow-card-bg);
    }

    .hero-bg-img {
        position: absolute;
        inset: 0;
        background-image: url("/assets/images/darkside.svg");
        background-position: center 40%;
        background-repeat: no-repeat;
        background-size: 220vh; /* Even bigger per user request */
        opacity: 0; /* Nearly invisible initially */
        transform: translateY(100vh) scale(0.9); /* Start completely off-screen from bottom */
        filter: blur(5px) drop-shadow(0 0 0 rgba(0, 243, 255, 0)); /* Start blurry and no glow */
        mix-blend-mode: multiply; /* True shadow blending */
        pointer-events: none;
        z-index: 0;
        transition:
            opacity 0.7s ease-out,
            transform 0.7s cubic-bezier(0.19, 1, 0.22, 1),
            filter 0.7s ease-out;
        transition-delay: 0.1s; /* Reduced delay for immediate impact */
    }

    .home-wrapper.visible .hero-bg-img {
        opacity: 0.7; /* Increased final opacity for better visibility */
        transform: translateY(0) scale(1);
        filter: blur(0px) drop-shadow(0 0 30px rgba(0, 243, 255, 0.15)); /* Sharp with subtle cyan glow */
    }

    /* Fog Layers */
    .fog-layer {
        position: absolute;
        inset: 0;
        background: radial-gradient(
            circle at 50% 50%,
            rgba(0, 243, 255, 0.05),
            transparent 70%
        );
        filter: blur(60px);
        opacity: 0.6;
    }
    .fog-1 {
        animation: fog-drift 20s infinite alternate ease-in-out;
    }
    .fog-2 {
        animation: fog-drift 15s infinite alternate-reverse ease-in-out;
    }

    @keyframes fog-drift {
        0% {
            transform: translateX(-5%) translateY(-5%);
        }
        100% {
            transform: translateX(5%) translateY(5%);
        }
    }

    /* Rising Energy Streams */
    .energy-streams {
        position: absolute;
        inset: 0;
        perspective: 1000px;
        transform-style: preserve-3d;
    }

    .energy-beam {
        position: absolute;
        bottom: -20%;
        left: var(--left);
        width: 3px; /* Slightly thicker for visibility */
        height: 60vh; /* Taller beams */
        background: linear-gradient(
            to top,
            transparent,
            var(--wow-cyan),
            transparent
        );
        opacity: var(--opacity);
        transform: scale(var(--scale)) rotateZ(-15deg);
        filter: blur(2px);
        box-shadow: 0 0 15px var(--wow-cyan);
        animation: rise var(--duration) linear infinite;
        animation-delay: var(--delay);
        /* Add some variation to the beam width */
        width: calc(2px * var(--scale));
    }

    .energy-beam::after {
        content: "";
        position: absolute;
        top: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 4px;
        height: 4px;
        background: #fff;
        border-radius: 50%;
        box-shadow:
            0 0 10px #fff,
            0 0 20px var(--wow-cyan);
    }

    @keyframes rise {
        0% {
            transform: translateY(120%) scale(var(--scale)) rotateZ(-15deg);
            opacity: 0;
        }
        15% {
            opacity: var(--opacity);
        }
        50% {
            opacity: calc(var(--opacity) * 1.5);
        } /* Pulse in middle */
        85% {
            opacity: var(--opacity);
        }
        100% {
            transform: translateY(-120%) scale(var(--scale)) rotateZ(-15deg);
            opacity: 0;
        }
    }

    /* Interactive Glow */
    .interactive-glow {
        position: absolute;
        top: 50%;
        left: 50%;
        width: 800px; /* Larger glow area */
        height: 800px;
        background: radial-gradient(
            circle,
            rgba(0, 243, 255, 0.06),
            transparent 60%
        );
        border-radius: 50%;
        pointer-events: none;
        margin-top: -400px;
        margin-left: -400px;
        transition: transform 0.1s ease-out; /* Smoother tracking */
        mix-blend-mode: screen;
    }

    /* === HERO CONTENT === */
    .hero {
        position: relative;
        z-index: 2;
        text-align: center;
        width: 100%;
        max-width: 1000px;
        padding: 90px 1rem 60px; /* Reduced top, added bottom padding */
        box-sizing: border-box;
        display: flex;
        flex-direction: column;
        justify-content: flex-start;
        height: auto;
        min-height: 100%;
    }

    .hero-inner {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 1rem; /* Compact gap */
    }

    /* Badge */
    .status-badge {
        display: inline-flex;
        align-items: center;
        gap: 0.5rem;
        padding: 0.4rem 1rem;
        background: rgba(0, 243, 255, 0.05);
        border: 1px solid rgba(0, 243, 255, 0.2);
        border-radius: 20px;
        font-family: var(--wow-font-display);
        font-size: 0.7rem;
        color: var(--wow-cyan);
        letter-spacing: 0.15em;
        backdrop-filter: blur(5px);
        margin-bottom: 0.25rem; /* Reduced margin */
    }
    .pulse-dot {
        width: 6px;
        height: 6px;
        background: var(--wow-cyan);
        border-radius: 50%;
        box-shadow: 0 0 8px var(--wow-cyan);
        animation: pulse 2s infinite;
    }

    /* Title */
    .zone-title {
        font-family: var(--wow-font-display);
        font-size: clamp(3rem, 8vw, 6rem); /* Reduced size */
        font-weight: 900;
        margin: 0;
        line-height: 0.9;
        color: #fff;
        text-shadow: 0 0 40px rgba(0, 243, 255, 0.3);
        letter-spacing: 0.02em;
        background: linear-gradient(180deg, #fff 0%, #bdeeff 100%);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        background-clip: text;
    }

    .tagline {
        font-family: var(--wow-font-body);
        font-size: clamp(0.9rem, 1.5vw, 1.1rem); /* Slightly smaller */
        color: rgba(255, 255, 255, 0.8);
        letter-spacing: 0.25em;
        text-transform: uppercase;
        margin: 0;
    }

    /* Separator */
    .separator {
        display: flex;
        align-items: center;
        gap: 1rem;
        width: 100%;
        max-width: 200px;
        opacity: 0.6;
        margin: 0.25rem 0; /* Reduced margin */
    }
    .line {
        height: 1px;
        flex: 1;
        background: linear-gradient(90deg, transparent, var(--wow-cyan));
    }
    .line:last-child {
        background: linear-gradient(90deg, var(--wow-cyan), transparent);
    }
    .diamond {
        width: 6px;
        height: 6px;
        background: var(--wow-cyan);
        transform: rotate(45deg);
        box-shadow: 0 0 8px var(--wow-cyan);
    }

    /* Description */
    .description {
        font-family: var(--wow-font-body);
        font-size: 1rem; /* Slightly smaller */
        color: rgba(255, 255, 255, 0.7);
        max-width: 550px;
        line-height: 1.4;
        margin: 0;
    }

    /* Actions */
    .actions {
        display: flex;
        gap: 1.5rem;
        margin-top: 1rem; /* Reduced margin */
    }

    @keyframes pulse {
        0%,
        100% {
            opacity: 1;
        }
        50% {
            opacity: 0.3;
        }
    }

    @media (max-width: 768px) {
        .actions {
            flex-direction: column;
            width: 100%;
            max-width: 280px;
            gap: 1rem;
        }
        .zone-title {
            font-size: 12vw; /* Ensure it fits on mobile */
        }
    }
</style>
