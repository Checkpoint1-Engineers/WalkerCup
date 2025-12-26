<script>
    import { createEventDispatcher, onMount, onDestroy } from "svelte";
    import CyberButton from "./CyberButton.svelte";
    import { api } from "$lib/api";
    import { browser } from "$app/environment";

    export let isOpen = false;
    export let tournamentId = null;

    const dispatch = createEventDispatcher();

    let walkerId = "";
    let walkerName = "";
    let email = ""; // Added email
    let error = null;
    let loading = false;

    // Lock scroll when open
    $: if (browser && isOpen) {
        document.body.style.overflow = "hidden";
    } else if (browser) {
        document.body.style.overflow = "";
    }

    onDestroy(() => {
        if (browser) document.body.style.overflow = "";
    });

    function close() {
        isOpen = false;
        error = null;
        dispatch("close");
    }

    async function handleJoin() {
        error = null;

        // Basic validation
        if (!walkerId || !walkerName || !email) {
            error = "WALKER ID, NAME AND EMAIL REQUIRED";
            return;
        }

        if (isNaN(walkerId)) {
            error = "WALKER ID MUST BE A NUMBER";
            return;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            error = "INVALID EMAIL FORMAT";
            return;
        }

        loading = true;
        try {
            await api.tournaments.join(
                tournamentId,
                parseInt(walkerId),
                walkerName,
                email,
            );

            // Success
            loading = false;
            dispatch("success");
            close();
        } catch (err) {
            console.error(err);
            error = err.message.toUpperCase();
            loading = false;
        }
    }
</script>

{#if isOpen}
    <!-- svelte-ignore a11y-click-events-have-key-events, a11y-no-static-element-interactions -->
    <div class="modal-overlay" on:click={close}>
        <div class="modal-content" on:click|stopPropagation>
            <div class="modal-header">
                <h2>JOIN MISSION?</h2>
                <button class="close-btn" on:click={close}>X</button>
            </div>

            <div class="target-name">TARGET ID: #{tournamentId}</div>

            {#if error}
                <div class="error-banner">
                    <span class="error-icon">⚠</span>
                    {error}
                </div>
            {/if}

            <div class="input-group">
                <label for="wid">WALKER ID</label>
                <input
                    type="number"
                    id="wid"
                    bind:value={walkerId}
                    placeholder="ENTER ID..."
                />
            </div>

            <div class="input-group">
                <label for="wname">CODENAME</label>
                <input
                    type="text"
                    id="wname"
                    bind:value={walkerName}
                    placeholder="ENTER ALIAS..."
                />
            </div>

            <div class="input-group">
                <label for="email">EMAIL</label>
                <input
                    type="email"
                    id="email"
                    bind:value={email}
                    placeholder="ENTER EMAIL..."
                />
            </div>

            <div class="modal-footer">
                <CyberButton
                    label={loading ? "SYNCING..." : "CONFIRM ENTRY"}
                    variant="primary"
                    on:click={handleJoin}
                />
            </div>
        </div>
    </div>
{/if}

<style>
    /* ... */
    .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background: rgba(0, 10, 15, 0.95);
        backdrop-filter: blur(8px);
        z-index: 99999;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .modal-content {
        background: rgba(0, 20, 30, 0.95);
        border: 1px solid var(--wow-cyan);
        width: 90%;
        max-width: 500px;
        max-height: 85vh;
        overflow-y: auto;
        padding: 2rem;
        box-shadow: 0 0 50px rgba(0, 243, 255, 0.2);
        position: relative;
        clip-path: polygon(
            20px 0,
            100% 0,
            100% calc(100% - 20px),
            calc(100% - 20px) 100%,
            0 100%,
            0 20px
        );
    }

    .modal-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 2rem;
        border-bottom: 1px solid rgba(0, 243, 255, 0.3);
        padding-bottom: 1rem;
    }

    h2 {
        font-family: var(--wow-font-display);
        color: var(--wow-cyan);
        margin: 0;
        font-size: 1.5rem;
        letter-spacing: 2px;
    }

    .close-btn {
        background: none;
        border: none;
        color: rgba(255, 255, 255, 0.5);
        font-family: var(--wow-font-display);
        font-size: 1.2rem;
        cursor: pointer;
        transition: color 0.3s;
    }

    .close-btn:hover {
        color: var(--wow-cyan);
    }

    .target-name {
        color: rgba(255, 255, 255, 0.8);
        font-family: var(--wow-font-body);
        font-size: 0.9rem;
        margin-bottom: 1.5rem;
        text-transform: uppercase;
        letter-spacing: 0.1em;
    }

    .input-group {
        margin-bottom: 1.5rem;
    }

    label {
        display: block;
        font-family: var(--wow-font-display);
        color: var(--wow-cyan);
        font-size: 0.8rem;
        margin-bottom: 0.5rem;
        letter-spacing: 0.1em;
    }

    input {
        width: 100%;
        background: rgba(0, 243, 255, 0.05);
        border: 1px solid rgba(0, 243, 255, 0.2);
        padding: 0.8rem;
        color: #fff;
        font-family: var(--wow-font-body);
        font-size: 1rem;
        outline: none;
        transition: all 0.3s ease;
        box-sizing: border-box;
    }

    input:focus {
        border-color: var(--wow-cyan);
        box-shadow: 0 0 10px rgba(0, 243, 255, 0.2);
    }

    .modal-footer {
        display: flex;
        justify-content: flex-end;
        margin-top: 2rem;
    }

    .error-banner {
        background: rgba(255, 0, 0, 0.1);
        border: 1px solid #ff3e3e;
        color: #ff3e3e;
        padding: 0.8rem;
        margin-bottom: 1.5rem;
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-family: var(--wow-font-display);
        font-size: 0.9rem;
    }

    @media (max-width: 600px) {
        .modal-content {
            padding: 1.5rem;
            width: 95%;
        }

        .modal-header h2 {
            font-size: 1.2rem;
        }
    }
</style>
