<script>
    import { page } from "$app/stores";
    import { goto } from "$app/navigation";
    import { scramble } from "$lib/actions/scramble.js";
    import { onMount } from "svelte";

    // --- CONFIGURATION ---
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
    let y = 0;
    let lastY = 0;
    let isHidden = false;

    // --- PURE CSS APPROACH ---
    // No scroll or hover logic needed in JS
    // CSS handles everything via :hover on .nav-trigger

    onMount(() => {
        // Intro: visible initially, then hide to showcase the SYSTEM ONLINE badge
        if (typeof window !== "undefined" && window.innerWidth >= 1024) {
            setTimeout(() => {
                isHidden = true; // After this, nav is hidden by default (CSS controls reveal)
            }, 2000);
        }
    });

    // --- ACTIONS ---
    function toggleMenu() {
        isMenuOpen = !isMenuOpen;
        // Lock body scroll when menu is open
        if (typeof document !== "undefined") {
            document.body.style.overflow = isMenuOpen ? "hidden" : "";
        }
    }

    function closeMenu() {
        isMenuOpen = false;
        if (typeof document !== "undefined") {
            document.body.style.overflow = "";
        }
    }
</script>

<svelte:window bind:scrollY={y} />

<!-- Hover Trigger Zone and Visual Handle -->
<div class="nav-trigger">
    <!-- Visual Handle: Logo badge that appears when nav is hidden -->
    <div class="nav-handle" class:visible={isHidden}>
        <div class="pulse-dot"></div>
        <img
            src="/assets/images/logo.png"
            alt="Walker Logo"
            class="handle-logo"
        />
    </div>
</div>

<header class="app-navbar" class:hidden={isHidden}>
    <!-- DESKTOP LEFT -->
    <nav class="nav-group left desktop-only">
        {#each leftLinks as link}
            <a
                href={link.path}
                class="nav-item"
                class:active={$page.url.pathname === link.path}
                on:click|preventDefault={() => goto(link.path)}
            >
                <div class="glitch-wrapper">
                    <span class="nav-text" use:scramble>{link.label}</span>
                </div>
            </a>
        {/each}
    </nav>

    <!-- BRAND (CENTER DESKTOP / LEFT MOBILE) -->
    <div class="brand-container">
        <button
            class="brand"
            on:click={() => {
                goto("/");
                closeMenu();
            }}
            aria-label="Home"
        >
            <img
                src="/assets/images/logo.png"
                alt="Walker Logo"
                class="logo-img"
            />
        </button>
    </div>

    <!-- DESKTOP RIGHT -->
    <nav class="nav-group right desktop-only">
        {#each rightLinks as link}
            <a
                href={link.path}
                class="nav-item"
                class:active={$page.url.pathname === link.path}
                on:click|preventDefault={() => goto(link.path)}
            >
                <div class="glitch-wrapper">
                    <span class="nav-text" use:scramble>{link.label}</span>
                </div>
            </a>
        {/each}
    </nav>

    <!-- MOBILE HAMBURGER -->
    <button
        class="hamburger mobile-only"
        class:active={isMenuOpen}
        on:click={toggleMenu}
        aria-label="Toggle Menu"
    >
        <span class="line top"></span>
        <span class="line mid"></span>
        <span class="line bot"></span>
    </button>
</header>

<!-- MOBILE OVERLAY (Moved outside header to escape backdrop-filter containing block) -->
<div class="mobile-overlay" class:open={isMenuOpen}>
    <nav class="mobile-nav">
        {#each allLinks as link, i}
            <a
                href={link.path}
                class="mobile-item"
                class:active={$page.url.pathname === link.path}
                style="--delay: {i * 0.1}s"
                on:click|preventDefault={() => {
                    goto(link.path);
                    closeMenu();
                }}
            >
                {link.label}
            </a>
        {/each}
    </nav>
</div>

<style>
    /* --- VARIABLES & LAYOUT --- */
    .app-navbar {
        position: fixed; /* Fixed positioning as requested */
        top: 0;
        left: 0;
        right: 0;
        width: 100%;
        box-sizing: border-box;
        z-index: 3000; /* High z-index to be above CyberCard and all content */
        padding: 1rem 0;
        display: flex;
        align-items: center;
        justify-content: center;
        background: linear-gradient(
            180deg,
            rgba(2, 6, 12, 0.95) 0%,
            transparent 100%
        );
        backdrop-filter: blur(4px);
        opacity: 1;
        transform: translateY(0);
        transition:
            opacity 0.3s ease,
            transform 0.3s ease;
    }

    .app-navbar.hidden {
        display: none; /* Use display:none as requested */
        opacity: 0;
        transform: translateY(-10px);
    }

    /* Animate navbar before it gets hidden */
    @keyframes navFadeOut {
        from {
            opacity: 1;
            transform: translateY(0);
        }
        to {
            opacity: 0;
            transform: translateY(-10px);
        }
    }

    /* Trigger zone - always present on desktop */
    .nav-trigger {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        height: 80px; /* Taller for easier hover */
        z-index: 2001; /* Above navbar when hidden */
        display: none;
        justify-content: center;
        align-items: flex-start;
    }

    /* Enhanced reveal on hover with stagger */
    .nav-trigger:hover ~ .app-navbar.hidden,
    .app-navbar.hidden:hover {
        display: flex;
        animation: headerSlideDown 0.4s cubic-bezier(0.16, 1, 0.3, 1) forwards;
    }

    /* Stagger nav items on reveal */
    .nav-trigger:hover ~ .app-navbar.hidden .nav-item,
    .app-navbar.hidden:hover .nav-item {
        animation: navItemReveal 0.5s cubic-bezier(0.16, 1, 0.3, 1) forwards;
    }

    .nav-trigger:hover ~ .app-navbar.hidden .nav-item:nth-child(1),
    .app-navbar.hidden:hover .nav-item:nth-child(1) {
        animation-delay: 0.1s;
    }

    .nav-trigger:hover ~ .app-navbar.hidden .nav-item:nth-child(2),
    .app-navbar.hidden:hover .nav-item:nth-child(2) {
        animation-delay: 0.15s;
    }

    @keyframes headerSlideDown {
        from {
            opacity: 0;
            transform: translateY(-20px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    @keyframes navItemReveal {
        from {
            opacity: 0;
            transform: translateY(-5px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    /* Hide handle instantly when hovering */
    .nav-trigger:hover .nav-handle {
        opacity: 0 !important;
        transition: opacity 0.15s ease-out;
    }

    /* "System Online" Handle - Premium Enhanced Badge */
    .nav-handle {
        position: absolute;
        top: 1rem;
        display: inline-flex;
        align-items: center;
        gap: 0.6rem;
        padding: 0.5rem 1rem;
        background: linear-gradient(
            135deg,
            rgba(2, 6, 12, 0.98) 0%,
            rgba(0, 20, 30, 0.98) 100%
        );
        border: 1px solid rgba(0, 243, 255, 0.4);
        border-radius: 30px;
        backdrop-filter: blur(20px);
        opacity: 0;
        transform: translateY(-20px) scale(0.95);
        transition: all 0.5s cubic-bezier(0.19, 1, 0.22, 1);
        pointer-events: none;
        box-shadow:
            0 4px 20px rgba(0, 0, 0, 0.6),
            0 0 30px rgba(0, 243, 255, 0.15),
            inset 0 1px 0 rgba(255, 255, 255, 0.1);
        cursor: pointer;
    }

    .nav-handle .handle-logo {
        height: 24px;
        width: auto;
        filter: brightness(1.1);
        transition: filter 0.3s ease;
    }

    .nav-handle:hover .handle-logo {
        filter: brightness(1.3) drop-shadow(0 0 8px rgba(0, 243, 255, 0.5));
    }

    .nav-handle.visible {
        opacity: 1;
        transform: translateY(0) scale(1);
        pointer-events: all;
        animation: badgePulse 3s ease-in-out infinite;
    }

    .nav-handle:hover {
        border-color: rgba(0, 243, 255, 0.8);
        box-shadow:
            0 6px 30px rgba(0, 0, 0, 0.7),
            0 0 40px rgba(0, 243, 255, 0.3),
            inset 0 1px 0 rgba(255, 255, 255, 0.2);
        transform: translateY(0) scale(1.05);
    }

    @keyframes badgePulse {
        0%,
        100% {
            box-shadow:
                0 4px 20px rgba(0, 0, 0, 0.6),
                0 0 30px rgba(0, 243, 255, 0.15),
                inset 0 1px 0 rgba(255, 255, 255, 0.1);
        }
        50% {
            box-shadow:
                0 4px 25px rgba(0, 0, 0, 0.6),
                0 0 40px rgba(0, 243, 255, 0.3),
                inset 0 1px 0 rgba(255, 255, 255, 0.15);
        }
    }

    /* Pulse dot style reused from Home */
    .nav-handle .pulse-dot {
        width: 7px;
        height: 7px;
        background: var(--wow-cyan);
        border-radius: 50%;
        box-shadow: 0 0 10px var(--wow-cyan);
        animation: pulseDot 2s infinite;
        box-shadow: 0 0 8px var(--wow-cyan);
        animation: pulseDot 2s infinite;
    }

    @keyframes pulseDot {
        0%,
        100% {
            opacity: 1;
        }
        50% {
            opacity: 0.3;
        }
    }

    @media (min-width: 1024px) {
        .nav-trigger {
            display: block;
        }
    }

    /* --- DESKTOP NAVIGATION --- */
    .nav-group {
        display: flex;
        gap: 2rem;
        align-items: center;
    }
    .nav-group.left {
        padding-right: 3rem;
    }
    .nav-group.right {
        padding-left: 3rem;
    }

    .nav-item {
        color: rgba(255, 255, 255, 0.6);
        text-decoration: none;
        font-family: var(--wow-font-display, monospace);
        font-size: 0.9rem;
        letter-spacing: 1px;
        position: relative;
        padding: 0.5rem 0.75rem;
        transition: all 0.3s cubic-bezier(0.19, 1, 0.22, 1);
        text-transform: uppercase;
        border-radius: 4px;
    }

    .nav-item:hover,
    .nav-item.active {
        color: var(--wow-cyan);
        background: rgba(0, 243, 255, 0.05);
        text-shadow: 0 0 10px rgba(0, 243, 255, 0.4);
        transform: translateY(-2px);
    }

    /* Enhanced underline effect */
    .nav-item::after {
        content: "";
        position: absolute;
        bottom: 0;
        left: 0;
        width: 0;
        height: 2px;
        background: linear-gradient(
            90deg,
            var(--wow-cyan),
            rgba(0, 243, 255, 0.5)
        );
        box-shadow: 0 0 8px var(--wow-cyan);
        transition: width 0.4s cubic-bezier(0.19, 1, 0.22, 1);
    }

    .nav-item:hover::after,
    .nav-item.active::after {
        width: 100%;
    }

    /* --- BRAND --- */
    .brand-container {
        width: 60px;
        height: 60px;
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 2100;
    }
    .brand {
        background: none;
        border: none;
        cursor: pointer;
        padding: 0.25rem;
        transition: all 0.3s cubic-bezier(0.19, 1, 0.22, 1);
        border-radius: 8px;
    }

    .brand:hover {
        transform: scale(1.08) rotate(-2deg);
        background: rgba(0, 243, 255, 0.05);
    }

    .brand:active {
        transform: scale(0.95);
    }

    .logo-img {
        width: auto;
        height: 40px;
        filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
        transition: filter 0.3s ease;
    }

    .brand:hover .logo-img {
        filter: drop-shadow(0 4px 12px rgba(0, 243, 255, 0.4));
    }

    /* --- MOBILE ELEMENTS (Default Hidden) --- */
    .mobile-only {
        display: none;
    }
    .mobile-overlay {
        display: none;
    }

    /* --- MEDIA QUERY: MOBILE/TABLET --- */
    @media (max-width: 1024px) {
        .desktop-only {
            display: none !important;
        }
        .mobile-only {
            display: flex !important;
        }

        .app-navbar {
            justify-content: space-between;
            padding: 0.75rem 1.5rem;
            background: linear-gradient(
                180deg,
                rgba(2, 6, 12, 0.98) 0%,
                rgba(2, 6, 12, 0.95) 100%
            );
            border-bottom: 1px solid rgba(0, 243, 255, 0.15);
            box-shadow: 0 2px 20px rgba(0, 0, 0, 0.5);
        }

        /* Hide desktop trigger and handle on mobile */
        .nav-trigger {
            display: none !important;
        }

        /* Adjust Brand specific to mobile */
        .brand-container {
            width: auto;
            height: auto;
            margin: 0;
            justify-content: flex-start;
        }
        .logo-img {
            height: 36px;
            filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
        }

        /* ENHANCED HAMBURGER BUTTON */
        .hamburger {
            width: 48px;
            height: 48px;
            padding: 14px 10px;
            flex-direction: column;
            justify-content: space-between;
            background: rgba(0, 243, 255, 0.05);
            border: 1px solid rgba(0, 243, 255, 0.2);
            border-radius: 8px;
            cursor: pointer;
            z-index: 3000;
            transition: all 0.3s ease;
        }

        .hamburger:hover {
            background: rgba(0, 243, 255, 0.1);
            border-color: rgba(0, 243, 255, 0.4);
            box-shadow: 0 0 15px rgba(0, 243, 255, 0.2);
        }

        .line {
            width: 100%;
            height: 2.5px;
            background: var(--wow-cyan);
            border-radius: 2px;
            box-shadow: 0 0 8px rgba(0, 243, 255, 0.3);
            transition:
                transform 0.3s cubic-bezier(0.4, 0, 0.2, 1),
                opacity 0.2s,
                box-shadow 0.3s;
        }

        .hamburger:hover .line {
            box-shadow: 0 0 12px rgba(0, 243, 255, 0.5);
        }

        /* Active State Animations */
        .hamburger.active {
            background: rgba(0, 243, 255, 0.15);
            border-color: rgba(0, 243, 255, 0.6);
        }

        .hamburger.active .top {
            transform: translateY(10px) rotate(45deg);
        }
        .hamburger.active .mid {
            opacity: 0;
            transform: translateX(10px);
        }
        .hamburger.active .bot {
            transform: translateY(-10px) rotate(-45deg);
        }

        /* OVERLAY MENU */
        .mobile-overlay {
            display: flex;
            position: fixed;
            inset: 0;
            background: #02060c; /* Force solid black/dark background */
            background-image: linear-gradient(
                    rgba(255, 255, 255, 0.03) 1px,
                    transparent 1px
                ),
                linear-gradient(
                    90deg,
                    rgba(255, 255, 255, 0.03) 1px,
                    transparent 1px
                );
            background-size: 50px 50px;
            z-index: 9999; /* Max z-index */
            padding-top: 80px;
            flex-direction: column;
            align-items: center;
            overflow-y: auto;

            /* Animation: Circular Reveal */
            clip-path: circle(0% at 90% 5%);
            transition: clip-path 0.5s cubic-bezier(0.19, 1, 0.22, 1);
            pointer-events: none;
        }
        .mobile-overlay.open {
            clip-path: circle(150% at 90% 5%);
            pointer-events: all;
        }

        .mobile-nav {
            display: flex;
            flex-direction: column;
            gap: 2.5rem;
            text-align: center;
            width: 100%;
            padding-bottom: 2rem;
        }

        .mobile-item {
            font-family: var(--wow-font-display, sans-serif);
            font-size: clamp(1.5rem, 5vw, 2.5rem);
            color: rgba(255, 255, 255, 0.7);
            text-transform: uppercase;
            text-decoration: none;
            letter-spacing: 2px;
            padding: 0.5rem 1rem;
            border-radius: 8px;
            transition: all 0.4s cubic-bezier(0.19, 1, 0.22, 1);

            /* Staggered Entrance */
            opacity: 0;
            transform: translateY(20px);
            transition:
                opacity 0.3s ease,
                transform 0.3s ease,
                color 0.3s,
                background 0.3s,
                text-shadow 0.3s;
        }

        .mobile-item:hover,
        .mobile-item:active {
            color: var(--wow-cyan);
            background: rgba(0, 243, 255, 0.1);
            text-shadow: 0 0 20px rgba(0, 243, 255, 0.6);
            transform: translateX(10px) scale(1.05);
        }

        .mobile-item:active {
            transform: translateX(10px) scale(0.98);
        }

        /* Show items when Open - staggered entrance */
        .mobile-overlay.open .mobile-item {
            opacity: 1;
            transform: translateY(0);
        }
        .mobile-overlay.open .mobile-item:nth-child(1) {
            transition-delay: 0.1s;
        }
        .mobile-overlay.open .mobile-item:nth-child(2) {
            transition-delay: 0.15s;
        }
        .mobile-overlay.open .mobile-item:nth-child(3) {
            transition-delay: 0.2s;
        }
        .mobile-overlay.open .mobile-item:nth-child(4) {
            transition-delay: 0.25s;
        }

        /* Reverse stagger on close for smooth fade-out */
        .mobile-overlay:not(.open) .mobile-item:nth-child(1) {
            transition-delay: 0.15s;
        }
        .mobile-overlay:not(.open) .mobile-item:nth-child(2) {
            transition-delay: 0.1s;
        }
        .mobile-overlay:not(.open) .mobile-item:nth-child(3) {
            transition-delay: 0.05s;
        }
        .mobile-overlay:not(.open) .mobile-item:nth-child(4) {
            transition-delay: 0s;
        }
    }
</style>
