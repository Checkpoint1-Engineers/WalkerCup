<script>
    export let href = null;
    export let label = "BUTTON";
    export let variant = "primary"; // primary, secondary
    export let type = "button";
</script>

{#if href}
    <a {href} class="cyber-btn {variant}">
        <span class="btn-content">
            <span class="btn-label">{label}</span>
            <span class="btn-decor"></span>
        </span>
        <span class="btn-glitch"></span>
    </a>
{:else}
    <button {type} class="cyber-btn {variant}" on:click>
        <span class="btn-content">
            <span class="btn-label">{label}</span>
            <span class="btn-decor"></span>
        </span>
        <span class="btn-glitch"></span>
    </button>
{/if}

<style>
    .cyber-btn {
        position: relative;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        text-decoration: none;
        padding: 0 2px;
        height: 48px;
        background: transparent;
        cursor: pointer;
        overflow: hidden;
        clip-path: polygon(
            10px 0,
            100% 0,
            100% calc(100% - 10px),
            calc(100% - 10px) 100%,
            0 100%,
            0 10px
        );
        transition: all 0.3s ease;
    }

    .btn-content {
        position: relative;
        z-index: 2;
        display: flex;
        align-items: center;
        justify-content: center;
        width: 100%;
        height: 100%;
        padding: 0 2rem;
        background: rgba(0, 12, 20, 0.9);
        border: 1px solid var(--wow-cyan);
        transition: all 0.3s ease;
    }

    /* Primary Variant */
    .cyber-btn.primary .btn-content {
        background: rgba(0, 243, 255, 0.1);
        border-color: var(--wow-cyan);
    }
    .cyber-btn.primary .btn-label {
        color: var(--wow-cyan);
        text-shadow: 0 0 5px var(--wow-cyan);
    }

    /* Secondary Variant */
    .cyber-btn.secondary .btn-content {
        background: rgba(255, 255, 255, 0.05);
        border-color: rgba(255, 255, 255, 0.3);
    }
    .cyber-btn.secondary .btn-label {
        color: rgba(255, 255, 255, 0.8);
    }

    .btn-label {
        font-family: var(--wow-font-display);
        font-size: 0.9rem;
        font-weight: bold;
        letter-spacing: 0.15em;
        text-transform: uppercase;
        transition: all 0.3s ease;
    }

    /* Hover Effects */
    .cyber-btn:hover .btn-content {
        background: var(--wow-cyan);
        border-color: var(--wow-cyan);
    }
    .cyber-btn:hover .btn-label {
        color: #000;
        text-shadow: none;
    }

    .cyber-btn:active {
        transform: scale(0.98);
    }

    /* Glitch Effect */
    .btn-glitch {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: var(--wow-cyan);
        z-index: 1;
        transform: translateX(-100%);
        transition: transform 0.3s ease;
    }

    .cyber-btn:hover .btn-glitch {
        transform: translateX(0);
    }

    @media (max-width: 768px) {
        .cyber-btn {
            width: 100%;
        }
    }
</style>
