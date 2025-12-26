<script>
    import { onMount } from "svelte";
    import { auth } from "$lib/stores/auth";
    import { goto } from "$app/navigation";
    import { get } from "svelte/store";

    onMount(() => {
        const user = get(auth).user;
        const token = get(auth).token;

        if (!token || !user || user.role !== "TeamAlan") {
            goto("/login");
        }
    });
</script>

<div class="admin-shell">
    <header class="admin-header">
        <div class="brand">
            <span class="icon">⚡</span>
            TEAM ALAN COMMAND
        </div>
        <div class="user-info">
            OPERATOR: {$auth.user?.email}
        </div>
    </header>

    <main class="admin-viewport">
        <slot />
    </main>
</div>

<style>
    :global(body) {
        background: #000;
        /* Override default background if needed for admin mode */
    }

    .admin-shell {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
        font-family: "JetBrains Mono", monospace; /* or fallback */
        color: #00ff41; /* Classic Matrix/Hacker Green or Keep Cyan */
        color: var(--wow-cyan);
    }

    .admin-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 1rem 2rem;
        background: rgba(0, 20, 30, 0.95);
        border-bottom: 1px solid var(--wow-cyan);
        box-shadow: 0 0 20px rgba(0, 243, 255, 0.1);
        z-index: 10; /* Lower than modal 99999 */
        position: sticky;
        top: 0;
    }

    .brand {
        font-size: 1.2rem;
        font-weight: 900;
        letter-spacing: 2px;
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }

    .user-info {
        font-size: 0.9rem;
        opacity: 0.7;
    }

    .admin-viewport {
        flex: 1;
        padding: 2rem;
        background: radial-gradient(
            circle at top center,
            #001a2c 0%,
            #000 100%
        );
        position: relative;
        z-index: auto; /* Ensure no new stacking context if possible */
    }

    /* Grid overlay effect */
    .admin-viewport::before {
        content: "";
        position: absolute;
        inset: 0;
        background-image: linear-gradient(
                rgba(0, 243, 255, 0.03) 1px,
                transparent 1px
            ),
            linear-gradient(90deg, rgba(0, 243, 255, 0.03) 1px, transparent 1px);
        background-size: 50px 50px;
        pointer-events: none;
    }
</style>
