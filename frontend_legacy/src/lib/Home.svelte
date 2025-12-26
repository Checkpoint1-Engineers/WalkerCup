<script>
    import { navigate } from "./router.js";
    import { onMount } from "svelte";
    import Mission from "./landing/Mission.svelte";
    import Features from "./landing/Features.svelte";
    import FAQ from "./landing/FAQ.svelte";
    import Background from "./components/Background.svelte";
    import CyberBackground from "./components/CyberBackground.svelte";

    const sections = [
        {
            title: "RECRUITING",
            description:
                "Identify elite operatives. Assess candidates for Protocol compatibility.",
            icon: "M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z M12 8v4 M12 16h.01", // Shield/Secure
            image: "/recruiting_prism.png",
            path: "/recruiting",
            color: "var(--wow-cyan)",
            bg: "linear-gradient(45deg, #0b1218, #1a2633)",
        },
        {
            title: "LIVE OPS",
            description:
                "Monitor real-time conflict zones. Deploy directives to field agents.",
            icon: "M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 17.93c-3.95-.49-7-3.85-7-7.93 0-.62.08-1.21.21-1.79L9 15v1c0 1.1.9 2 2 2v1.93zm6.9-2.54c-.26-.81-1-1.39-1.9-1.39h-1v-3c0-.55-.45-1-1-1H8v-2h2c.55 0 1-.45 1-1V7h2c1.1 0 2-.9 2-2v-.41c2.93 1.19 5 4.06 5 7.41 0 2.08-1.07 3.9-2.2 5.06z", // Globe/World
            image: "/live_ops_prism.png",
            path: "/live",
            color: "#ff3e3e",
            bg: "linear-gradient(45deg, #2a0a0a, #4a1a1a)",
        },
        {
            title: "ARCHIVES",
            description:
                "Access classified historical data. Analyze recovery logs and artifact intel.",
            icon: "M4 19.5A2.5 2.5 0 0 1 6.5 17H20M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z", // Book/Library
            image: "/archives_prism.png",
            path: "/archives",
            color: "#a0b6c5",
            bg: "linear-gradient(45deg, #0f171f, #232d36)",
        },
    ];

    let visible = false;
    onMount(() => {
        setTimeout(() => (visible = true), 100);
    });

    let heroText = "WELCOME WALKER";

    // Text Scramble Effect
    const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
    function scramble(node) {
        let text = node.innerText;
        let original = text;
        let iterations = 0;
        let interval = null;

        node.onmouseenter = () => {
            clearInterval(interval);
            iterations = 0;

            interval = setInterval(() => {
                node.innerText = text
                    .split("")
                    .map((char, index) => {
                        if (index < iterations) return original[index];
                        return chars[Math.floor(Math.random() * chars.length)];
                    })
                    .join("");

                if (iterations >= original.length) clearInterval(interval);
                iterations += 1 / 2; // Speed
            }, 30);
        };

        // Reset on leave? Optional. Let's keep it resolved.
        // node.onmouseleave = () => node.innerText = original;
    }
</script>

<div class="landing-container" class:visible>
    <!-- FIXED DRONE BACKGROUND (Visually sits behind everything) -->
    <div class="fixed-hero-bg">
        <Background mode="HOME" />
    </div>

    <!-- HERO SECTION (Transparent, scrolls over fixed bg) -->
    <section class="hero full-height">
        <div class="hero-content">
            <h1 class="hero-title">
                JOIN THE <span class="highlight">RESISTANCE</span>
            </h1>
            <p class="hero-sub">The Protocol Initialized. Ascend the Ranks.</p>

            <button class="cta-button" on:click={() => navigate("/recruiting")}>
                <span class="cta-text">ENTER PROTOCOL</span>
                <div class="liquid"></div>
            </button>

            <div class="hero-decoration"></div>
        </div>

        <div class="scroll-indicator">
            <div class="mouse">
                <div class="wheel"></div>
            </div>
            <div class="arrow"></div>
        </div>
    </section>

    <!-- CONTENT LAYER (Solid BG, scrolls over drone) -->
    <div class="content-layer">
        <!-- Global background for non-hero sections -->
        <CyberBackground />

        <!-- NAVIGATION GRID (As features/modules) -->
        <section class="modules-section">
            <div class="modules-header">
                <h3 class="section-label">OPERATIONAL MODULES</h3>
                <div class="line"></div>
            </div>

            <div class="paths-grid">
                {#each sections as section, i}
                    <div
                        class="path-card"
                        on:click={() => navigate(section.path)}
                        style="--accent: {section.color}; --bg-grad: {section.bg}; --idx: {i};"
                    >
                        <!-- Background Image -->
                        <img
                            src={section.image}
                            alt={section.title}
                            class="card-bg"
                            loading="lazy"
                        />
                        <div class="card-overlay"></div>

                        <!-- Content -->
                        <div class="card-content">
                            <!-- Scramble Effect Applied Here -->
                            <h2 class="path-title" use:scramble>
                                <!-- Icon Inline -->
                                <svg
                                    class="title-icon"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="currentColor"
                                    stroke-width="2"
                                >
                                    <path d={section.icon} />
                                </svg>
                                {section.title}
                            </h2>
                            <p class="path-desc">{section.description}</p>
                        </div>

                        <!-- Interactive Elements -->
                        <div class="card-hud"></div>
                    </div>
                {/each}
            </div>
        </section>

        <!-- NEW MARKETING SECTIONS -->
        <Mission />
        <Features />
        <FAQ />
    </div>
</div>

<style>
    .landing-container {
        /* padding removed to allow hero full height */
        position: relative;
        overflow-x: hidden;
        opacity: 0;
        transition: opacity 1s ease;
    }
    .landing-container.visible {
        opacity: 1;
    }

    /* Fixed Background */
    .fixed-hero-bg {
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        z-index: 0;
        pointer-events: none;
    }

    /* Content Layer - Covers the fixed background */
    .content-layer {
        position: relative;
        z-index: 10;
        background: #050b14; /* Solid dark background */
        /* Optional: top border or shadow for transition */
        box-shadow: 0 -50px 100px #050b14;
    }

    /* HERO: Pro Walker Style */
    .hero.full-height {
        min-height: 100vh; /* Full screen */
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        text-align: center;
        position: relative;
        z-index: 5; /* Above fixed bg, but content layer will scroll over it? No, content layer is below in DOM. */
        margin-bottom: 0;
        /* 
           Issue: If ContentLayer is *below* Hero in DOM, it starts after 100vh.
           When we scroll, Hero scrolls up (normal flow), Content Layer scrolls up.
           Hero Background is fixed.
           So Hero Content scrolls UP and OFF.
           Content Layer scrolls UP and COVERS the fixed bg.
           Correct.
        */
    }

    /* Animated Background Grid - Removed to let Drone HUD shine */
    .hero-bg {
        display: none;
    }

    .hero-content {
        position: relative;
        z-index: 5;
    }

    /* ... (Keep existing typography styles) ... */
    .hero-title {
        font-family: var(--wow-font-display, "Arial Black", sans-serif);
        font-size: 6rem;
        font-weight: 900;
        color: #fff;
        line-height: 1;
        letter-spacing: -2px;
        margin: 0 0 1.5rem;
        text-transform: uppercase;
        -webkit-text-stroke: 1px rgba(255, 255, 255, 0.1);
        text-shadow: 0 0 30px rgba(0, 0, 0, 0.8);
    }
    /* SCANNER FILL EFFECT */
    .highlight {
        position: relative;
        display: inline-block;
        color: rgba(255, 255, 255, 0.1); /* Faint fill */
        -webkit-text-stroke: 2px rgba(255, 255, 255, 0.8); /* Strong outline */

        /* The Scanner Beam */
        background: linear-gradient(
            to right,
            rgba(255, 255, 255, 0) 0%,
            var(--wow-cyan) 50%,
            rgba(255, 255, 255, 0) 100%
        );
        background-size: 200% auto;
        -webkit-background-clip: text;
        background-clip: text;

        /* Animation */
        animation: scan-text 4s linear infinite;

        /* Glow */
        filter: drop-shadow(0 0 10px rgba(0, 243, 255, 0.3));
    }

    @keyframes scan-text {
        0% {
            background-position: -200% center;
        }
        100% {
            background-position: 200% center;
        }
    }
    .hero-sub {
        font-family: var(--wow-font-display);
        color: rgba(255, 255, 255, 0.7);
        font-size: 1.25rem;
        letter-spacing: 4px;
        text-transform: uppercase;
        margin-bottom: 3rem;
        display: inline-block;
        padding: 0.5rem 1.5rem;
        background: rgba(0, 0, 0, 0.4);
        backdrop-filter: blur(4px);
        border: 1px solid rgba(255, 255, 255, 0.1);
        border-radius: 50px;
    }

    /* CTA Button - Liquid Fill */
    .cta-button {
        position: relative;
        background: transparent;
        color: var(--wow-cyan);
        font-family: var(--wow-font-display);
        font-weight: 900;
        font-size: 1.2rem;
        padding: 1.2rem 3rem;
        border: 1px solid var(--wow-cyan);
        cursor: pointer;
        text-transform: uppercase;
        letter-spacing: 2px;
        clip-path: polygon(10% 0, 100% 0, 100% 70%, 90% 100%, 0 100%, 0 30%);
        transition:
            transform 0.2s,
            box-shadow 0.2s,
            color 0.3s;
        box-shadow: 0 0 20px rgba(0, 243, 255, 0.1);
        overflow: hidden;
    }
    .liquid {
        position: absolute;
        top: 100%;
        left: 0;
        width: 100%;
        height: 100%;
        background: var(--wow-cyan);
        transition: top 0.4s cubic-bezier(0.4, 0, 0.2, 1);
        z-index: 0;
    }
    .cta-button:hover .liquid {
        top: 0;
    }
    .cta-text {
        position: relative;
        z-index: 1;
    }
    .cta-button:hover .cta-text {
        color: #000;
    }
    .cta-button:hover {
        transform: scale(1.05);
        box-shadow: 0 0 50px rgba(0, 243, 255, 0.4);
    }

    .hero-decoration {
        height: 1px;
        width: 100px;
        background: linear-gradient(
            90deg,
            transparent,
            var(--wow-cyan),
            transparent
        );
        margin: 4rem auto 0;
        opacity: 0.5;
    }

    /* Scroll Indicator */
    .scroll-indicator {
        position: absolute;
        bottom: 2rem;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 0.5rem;
        opacity: 0.7;
        animation: fadeScroll 2s infinite;
    }
    .mouse {
        width: 24px;
        height: 36px;
        border: 2px solid #fff;
        border-radius: 12px;
        position: relative;
    }
    .wheel {
        width: 2px;
        height: 6px;
        background: #fff;
        position: absolute;
        top: 6px;
        left: 50%;
        transform: translateX(-50%);
        animation: scrollWheel 1.5s infinite;
    }
    .arrow {
        width: 10px;
        height: 10px;
        border-right: 2px solid #fff;
        border-bottom: 2px solid #fff;
        transform: rotate(45deg);
    }
    @keyframes scrollWheel {
        0% {
            top: 6px;
            opacity: 1;
        }
        100% {
            top: 18px;
            opacity: 0;
        }
    }
    @keyframes fadeScroll {
        0%,
        100% {
            opacity: 0.4;
        }
        50% {
            opacity: 1;
        }
    }

    /* MODULES GRID SECTION */
    .modules-section {
        padding: 4rem 1.5rem;
        position: relative;
        z-index: 2;
    }

    .paths-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 2rem;
        max-width: 1400px;
        margin: 0 auto;
    }

    /* KINETIC INDUSTRIAL CARD */
    .path-card {
        position: relative;
        background: #030303; /* Deepest black */
        border-radius: 2px;
        overflow: hidden;
        cursor: pointer;
        height: 360px;
        display: flex;
        flex-direction: column;

        /* Base Border */
        border: 1px solid rgba(255, 255, 255, 0.1);
        transition:
            transform 0.3s ease,
            box-shadow 0.3s ease;
    }

    /* HOVER: Lift */
    .path-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.5);
        border-color: transparent; /* Let the gradient shine */
    }

    /* ANIMATED BORDER GRADIENT */
    .path-card::before {
        content: "";
        position: absolute;
        inset: -2px;
        background: conic-gradient(
            from 0deg,
            transparent 0deg,
            transparent 90deg,
            var(--accent) 130deg,
            transparent 180deg
        );
        z-index: 0;
        opacity: 0;
        transition: opacity 0.3s;
        animation: rotateBorder 4s linear infinite;
    }

    .path-card:hover::before {
        opacity: 1;
    }

    @keyframes rotateBorder {
        from {
            transform: rotate(0deg);
        }
        to {
            transform: rotate(360deg);
        }
    }

    /* Inner Mask to hide the center of the gradient */
    .path-card::after {
        content: "";
        position: absolute;
        inset: 1px; /* Border thickness */
        background: #030303;
        z-index: 1;
        pointer-events: none;
    }

    /* IMAGE CONTAINER (Top 60%) */
    .card-bg {
        position: absolute; /* Behind content but above ::after mask? No, need z-index dance */
        top: 0;
        left: 0;
        width: 100%;
        height: 60%;
        object-fit: cover;
        z-index: 2; /* Sit above the mask */

        /* Industrial Filter */
        filter: grayscale(100%) contrast(1.2) brightness(0.8);
        transition: all 0.5s ease;

        mask-image: linear-gradient(to bottom, black 80%, transparent 100%);
        -webkit-mask-image: linear-gradient(
            to bottom,
            black 80%,
            transparent 100%
        );
    }

    .path-card:hover .card-bg {
        filter: none; /* Color restore */
        height: 65%; /* Slight grow */
    }

    /* OVERLAY */
    .card-overlay {
        position: absolute;
        inset: 0;
        background: linear-gradient(to bottom, transparent 0%, #030303 60%);
        z-index: 3;
        pointer-events: none;
    }

    /* CONTENT (Bottom) */
    .card-content {
        position: relative;
        z-index: 5;
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: flex-end;
        padding: 2rem;
    }

    /* TEXT */
    .path-title {
        font-family: var(--wow-font-display);
        font-size: 1.8rem;
        font-weight: 800;
        color: #fff;
        margin-bottom: 0.5rem;
        text-transform: uppercase;
        letter-spacing: 1px;
        display: flex;
        align-items: center;
        gap: 0.8rem;
        transition: transform 0.3s;
    }

    .path-card:hover .path-title {
        transform: translateX(10px);
        color: var(--accent);
    }

    /* ICON */
    .title-icon {
        width: 24px;
        height: 24px;
        color: #666; /* Dim by default */
        transition: all 0.3s;
    }
    .path-card:hover .title-icon {
        color: var(--accent);
        filter: drop-shadow(0 0 5px var(--accent));
    }

    .path-desc {
        font-size: 0.95rem;
        color: #888;
        line-height: 1.5;
        max-width: 100%;

        /* Always visible but dim */
        opacity: 0.7;
        transform: translateY(0);
        transition: all 0.3s;
    }
    .path-card:hover .path-desc {
        opacity: 1;
        color: #fff;
    }

    /* TECH DECORATION (The Tab) */
    .card-hud {
        position: absolute;
        top: 20px;
        right: 20px;
        width: 40px;
        height: 4px;
        background: rgba(255, 255, 255, 0.1);
        z-index: 5;
        overflow: hidden;
    }
    .card-hud::before {
        content: "";
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: var(--accent);
        transform: translateX(-100%);
        transition: transform 0.4s ease;
    }
    .path-card:hover .card-hud::before {
        transform: translateX(0);
    }

    /* Mobile Responsiveness */
    @media (max-width: 768px) {
        .paths-grid {
            grid-template-columns: 1fr; /* Full width on mobile */
            gap: 1.5rem;
        }
        .path-card {
            height: 300px; /* Slightly shorter */
        }
        .card-bg {
            filter: none; /* Color always on mobile */
            opacity: 0.6;
        }
        .path-title {
            font-size: 1.5rem;
        }
    }
</style>
