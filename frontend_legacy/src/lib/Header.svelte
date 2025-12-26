<script>
    import { curRoute, navigate } from "./router.js";

    const leftLinks = [
        { label: "HOME", path: "/" },
        { label: "RECRUITING", path: "/recruiting" },
    ];
    const rightLinks = [
        { label: "LIVE OPS", path: "/live" },
        { label: "ARCHIVES", path: "/archives" },
    ];
    const allLinks = [...leftLinks, ...rightLinks];

    let isMenuOpen = false;
    let y = 0; // Scroll position

    function toggleMenu() {
        isMenuOpen = !isMenuOpen;
    }

    function closeMenu() {
        isMenuOpen = false;
    }

    // Text Scramble Effect
    const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    function scramble(node) {
        let text = node.innerText;
        let interval = null;

        node.onmouseenter = () => {
            let iteration = 0;
            clearInterval(interval);

            interval = setInterval(() => {
                node.innerText = text
                    .split("")
                    .map((char, index) => {
                        if (index < iteration) return text[index];
                        return chars[Math.floor(Math.random() * chars.length)];
                    })
                    .join("");

                if (iteration >= text.length) clearInterval(interval);
                iteration += 1 / 3;
            }, 30);
        };
    }
</script>

<svelte:window bind:scrollY={y} />

<header class="app-navbar" class:scrolled={y > 50}>
    <!-- 1. LEFT NAV (Desktop) -->
    <nav class="nav-group left desktop-only">
        {#each leftLinks as link}
            <a
                href={link.path}
                class="nav-item"
                class:active={$curRoute === link.path ||
                    ($curRoute === "/" && link.path === "/")}
                on:click|preventDefault={() => navigate(link.path)}
            >
                <div class="glitch-wrapper">
                    <span class="nav-text" use:scramble data-text={link.label}
                        >{link.label}</span
                    >
                </div>
            </a>
        {/each}
    </nav>

    <!-- 2. CENTER BRAND -->
    <div class="brand-container">
        <button
            class="brand"
            on:click={() => navigate("/")}
            aria-label="Go to Home"
        >
            <img src="/logo.png" alt="Walker Logo" class="logo-img" />
        </button>
    </div>

    <!-- 3. RIGHT NAV (Desktop) -->
    <nav class="nav-group right desktop-only">
        {#each rightLinks as link}
            <a
                href={link.path}
                class="nav-item"
                class:active={$curRoute === link.path}
                on:click|preventDefault={() => navigate(link.path)}
            >
                <div class="glitch-wrapper">
                    <span class="nav-text" use:scramble data-text={link.label}
                        >{link.label}</span
                    >
                </div>
            </a>
        {/each}
    </nav>

    <!-- 4. MOBILE HAMBURGER (Far Right) -->
    <button
        class="hamburger"
        class:active={isMenuOpen}
        on:click={toggleMenu}
        aria-label="Toggle Menu"
    >
        <span class="line top"></span>
        <span class="line mid"></span>
        <span class="line bot"></span>
    </button>

    <!-- 5. MOBILE OVERLAY -->
    <div class="mobile-overlay" class:open={isMenuOpen}>
        <nav class="mobile-nav">
            {#each allLinks as link, i}
                <a
                    href={link.path}
                    class="mobile-item"
                    class:active={$curRoute === link.path ||
                        ($curRoute === "/" && link.path === "/")}
                    style="--delay: {i * 0.1}s"
                    on:click|preventDefault={() => {
                        navigate(link.path);
                        closeMenu();
                    }}
                >
                    {link.label}
                </a>
            {/each}
        </nav>
    </div>
</header>

<style>
    /* Sticky Glass Pro Header */
    .app-navbar {
        position: sticky;
        top: 0;
        z-index: 1000;
        padding: 2rem 3rem; /* Taller initially */

        display: grid;
        grid-template-columns: 1fr auto 1fr;
        align-items: center;

        /* Glassmorphism */
        background: transparent; /* Start transparent-ish */
        transition:
            padding 0.4s ease,
            background 0.4s ease,
            backdrop-filter 0.4s ease;
    }

    /* Scrolled State */
    .app-navbar.scrolled {
        padding: 0.8rem 3rem;
        background: rgba(2, 6, 10, 0.9);
        backdrop-filter: blur(20px);
        -webkit-backdrop-filter: blur(20px);
        border-bottom: 1px solid rgba(255, 255, 255, 0.08);
        box-shadow: 0 4px 30px rgba(0, 0, 0, 0.5);
    }

    /* NAV GROUPS */
    .nav-group {
        display: flex;
        gap: 3rem;
    }
    .nav-group.left {
        justify-content: flex-end; /* Align towards center */
        padding-right: 3rem;
    }
    .nav-group.right {
        justify-content: flex-start; /* Align towards center */
        padding-left: 3rem;
    }

    /* NAV ITEMS (Desktop) */
    .nav-item {
        position: relative;
        text-decoration: none;
        font-family: var(--wow-font-display, monospace);
        font-weight: 700;
        font-size: 0.9rem;
        letter-spacing: 2px;
        color: rgba(255, 255, 255, 0.6);
        padding: 0.5rem 0;
        transition: color 0.3s;
        text-transform: uppercase;
        display: flex;
        align-items: center;
        justify-content: center;
        overflow: visible; /* Needed for glitch spill */
    }
    .nav-item:hover,
    .nav-item.active {
        color: #fff;
        /* No Text Shadow - Clean Look */
    }
    .nav-item.active {
        color: var(--wow-cyan);
    }

    /* Hover Bracket Effect */
    .hover-bracket {
        position: absolute;
        width: 100%;
        height: 100%;
        opacity: 0;
        transform: scaleX(0.8);
        transition:
            transform 0.3s,
            opacity 0.3s;
        border-bottom: 2px solid var(--wow-cyan);
    }
    .nav-item:hover .hover-bracket {
        opacity: 1;
        transform: scaleX(1);
        box-shadow: 0 2px 10px var(--wow-cyan);
    }

    /* BRAND */
    .brand-container {
        display: flex;
        justify-content: center;
    }
    .brand {
        background: none;
        border: none;
        cursor: pointer;
        padding: 0;
        transition: transform 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
    }
    .brand:hover {
        transform: scale(1.1);
        filter: drop-shadow(0 0 15px var(--wow-cyan));
    }
    .logo-img {
        height: 52px;
        width: auto;
        transition: height 0.4s ease;
    }
    .app-navbar.scrolled .logo-img {
        height: 40px; /* Shrink logo on scroll */
    }

    /* CHROMATIC GLITCH EFFECT */
    .glitch-wrapper {
        position: relative;
        display: inline-block;
    }
    .nav-text {
        position: relative;
        display: inline-block;
    }

    /* Glitch Layers */
    .nav-item:hover .nav-text::before,
    .nav-item:hover .nav-text::after {
        content: attr(data-text);
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: var(--wow-bg); /* Use bg color to mask creates cut effect */
        opacity: 0.8;
    }

    .nav-item:hover .nav-text::before {
        color: #0ff0fc;
        z-index: -1;
        animation: glitch-anim-1 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94) both
            infinite;
    }
    .nav-item:hover .nav-text::after {
        color: #ff2525;
        z-index: -2;
        animation: glitch-anim-2 0.4s cubic-bezier(0.25, 0.46, 0.45, 0.94) both
            infinite;
    }

    @keyframes glitch-anim-1 {
        0% {
            transform: translate(0);
        }
        20% {
            transform: translate(-2px, 2px);
        }
        40% {
            transform: translate(-2px, -2px);
        }
        60% {
            transform: translate(2px, 2px);
        }
        80% {
            transform: translate(2px, -2px);
        }
        100% {
            transform: translate(0);
        }
    }
    @keyframes glitch-anim-2 {
        0% {
            transform: translate(0);
        }
        20% {
            transform: translate(2px, -2px);
        }
        40% {
            transform: translate(2px, 2px);
        }
        60% {
            transform: translate(-2px, -2px);
        }
        80% {
            transform: translate(-2px, 2px);
        }
        100% {
            transform: translate(0);
        }
    }

    /* HAMBURGER */
    .hamburger {
        display: none; /* Desktop hidden */
        justify-self: end; /* Far right on mobile grid */
        flex-direction: column;
        justify-content: space-between;
        width: 32px;
        height: 22px;
        background: none;
        border: none;
        cursor: pointer;
        padding: 0;
        z-index: 2002; /* Above overlay */
    }
    .line {
        width: 100%;
        height: 2px;
        background: #fff;
        transition: 0.3s;
    }
    .hamburger:hover .line {
        background-color: var(--wow-cyan);
        box-shadow: 0 0 8px var(--wow-cyan);
    }
    .hamburger.active .top {
        transform: translateY(10px) rotate(45deg);
    }
    .hamburger.active .mid {
        opacity: 0;
    }
    .hamburger.active .bot {
        transform: translateY(-10px) rotate(-45deg);
    }

    /* MOBILE OVERLAY */
    .mobile-overlay {
        position: fixed;
        inset: 0;
        background: rgba(5, 11, 20, 0.98);
        /* Cyber Grid Background */
        background-image: linear-gradient(
                rgba(0, 243, 255, 0.03) 1px,
                transparent 1px
            ),
            linear-gradient(90deg, rgba(0, 243, 255, 0.03) 1px, transparent 1px);
        background-size: 40px 40px;
        backdrop-filter: blur(20px);
        z-index: 2000; /* Super high z-index */
        display: flex;
        justify-content: center;
        align-items: center;
        opacity: 0;
        pointer-events: none;
        transition: opacity 0.4s ease;
    }
    .mobile-overlay.open {
        opacity: 1;
        pointer-events: all;
    }
    .mobile-nav {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 3rem;
    }
    .mobile-item {
        font-family: var(--wow-font-display);
        font-size: 2.5rem;
        font-weight: 800;
        color: rgba(255, 255, 255, 0.5);
        text-decoration: none;
        text-transform: uppercase;
        letter-spacing: 2px;
        position: relative;

        /* Animation Props */
        opacity: 0;
        transform: translateY(20px);
        transition:
            opacity 0.4s ease,
            transform 0.4s ease,
            color 0.3s,
            letter-spacing 0.3s;
    }

    /* Animation when open */
    .mobile-overlay.open .mobile-item {
        opacity: 1;
        transform: translateY(0);
        transition-delay: var(--delay, 0s);
    }

    .mobile-item:hover,
    .mobile-item.active {
        color: var(--wow-cyan);
        transform: scale(
            1.1
        ); /* This might conflict with transform translate if hovered during anim, but ok */
        text-shadow: 0 0 20px var(--wow-cyan);
        letter-spacing: 6px;
        transition-delay: 0s; /* Instant hover */
    }
    .mobile-item.active::after {
        content: "<ACTIVE>";
        display: block;
        font-size: 0.8rem;
        letter-spacing: 2px;
        color: var(--wow-cyan);
        opacity: 0.7;
        position: absolute;
        bottom: -20px;
        left: 50%;
        transform: translateX(-50%);
    }

    /* RESPONSIVE */
    @media (max-width: 1024px) {
        .app-navbar {
            padding: 1rem 1.5rem; /* Standard padding mobile */
            grid-template-columns: auto 1fr; /* Logo left, Hamburger right */
        }
        .app-navbar.scrolled {
            padding: 0.8rem 1.5rem;
        }
        .desktop-only {
            display: none;
        }
        .brand-container {
            justify-self: start;
        }
        .hamburger {
            display: flex;
            justify-self: end;
        }
    }
</style>
