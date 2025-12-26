<script>
    import CyberButton from "$lib/components/CyberButton.svelte";
    import { createEventDispatcher, onMount, onDestroy } from "svelte";
    import { api } from "$lib/api";
    import { browser } from "$app/environment";
    import { portal } from "$lib/actions/portal";

    export let isOpen = false;

    const dispatch = createEventDispatcher();

    let name = "";
    let description = "";
    let joinDeadline = "";
    let xpPerWin = 100;
    let maxParticipants = 16;
    let imageUrl = "";

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

    async function handleSubmit() {
        error = null;
        if (!name || !joinDeadline) {
            error = "NAME AND DEADLINE REQUIRED";
            return;
        }

        if (new Date(joinDeadline) <= new Date()) {
            error = "DEADLINE MUST BE IN FUTURE";
            return;
        }

        loading = true;
        try {
            await api.tournaments.create({
                name,
                description,
                joinDeadline: new Date(joinDeadline).toISOString(),
                xpPerWin: parseInt(xpPerWin),
                maxParticipants: parseInt(maxParticipants),
                imageUrl,
            });

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
    <div class="modal-backdrop" use:portal on:click={close}>
        <!-- svelte-ignore a11y-click-events-have-key-events, a11y-no-static-element-interactions -->
        <div class="modal-content" on:click|stopPropagation>
            <div class="modal-header">
                <h2>INITIATE TOURNAMENT PROTOCOL</h2>
                <button class="close-btn" on:click={close}>X</button>
            </div>

            <div class="modal-body">
                {#if error}
                    <div class="error-banner">
                        <span class="error-icon">⚠</span>
                        {error}
                    </div>
                {/if}

                <div class="input-group">
                    <label for="name">OPERATION NAME</label>
                    <input
                        type="text"
                        id="name"
                        bind:value={name}
                        placeholder="e.g. NEON STRIKE"
                    />
                </div>

                <div class="input-group">
                    <label for="desc">MISSION BRIEF</label>
                    <textarea
                        id="desc"
                        bind:value={description}
                        rows="3"
                        placeholder="OBJECTIVE..."
                    ></textarea>
                </div>

                <div class="row">
                    <div class="input-group">
                        <label for="deadline">DEADLINE</label>
                        <input
                            type="datetime-local"
                            id="deadline"
                            bind:value={joinDeadline}
                        />
                    </div>
                    <div class="input-group">
                        <label for="capacity">SQUAD CAP</label>
                        <input
                            type="number"
                            id="capacity"
                            bind:value={maxParticipants}
                            min="2"
                            max="1000"
                        />
                    </div>
                </div>

                <div class="row">
                    <div class="input-group">
                        <label for="xp">XP REWARD</label>
                        <input
                            type="number"
                            id="xp"
                            bind:value={xpPerWin}
                            min="0"
                            step="10"
                        />
                    </div>
                    <div class="input-group">
                        <label for="img">VISUAL ASSET (URL)</label>
                        <input
                            type="text"
                            id="img"
                            bind:value={imageUrl}
                            placeholder="HTTPS://..."
                        />
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <CyberButton
                    label={loading ? "COMPILING..." : "LAUNCH PROTOCOL"}
                    variant="primary"
                    on:click={handleSubmit}
                />
            </div>
        </div>
    </div>
{/if}

<style>
    /* ... */
    .modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        width: 100vw;
        height: 100vh;
        background: rgba(0, 5, 10, 0.95);
        backdrop-filter: blur(8px);
        z-index: 99999; /* Ensure it's above everything */
        display: flex;
        align-items: center;
        justify-content: center;
        overflow-y: auto;
    }

    .modal-content {
        background: rgba(0, 20, 30, 0.95);
        border: 1px solid var(--wow-cyan);
        width: 90%;
        max-width: 600px;
        max-height: 85vh; /* Responsive height */
        overflow-y: auto; /* Internal scroll */
        padding: 2rem;
        box-shadow: 0 0 50px rgba(0, 243, 255, 0.2);
        position: relative;
        /* Remove clip-path if it interferes with scrollbar, but keep for aesthetics if tested.
           Clip-path can cut off scrollbars. Safest to remove or adjusting padding.
           Lets keep it simpler for robustness. 
        */
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

    .input-group {
        margin-bottom: 1.5rem;
        flex: 1;
    }

    .row {
        display: flex;
        gap: 1.5rem;
    }

    label {
        display: block;
        font-family: var(--wow-font-display);
        color: var(--wow-cyan);
        font-size: 0.8rem;
        margin-bottom: 0.5rem;
        letter-spacing: 0.1em;
    }

    input,
    textarea {
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

    input:focus,
    textarea:focus {
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

    input[type="datetime-local"]::-webkit-calendar-picker-indicator {
        filter: invert(1);
        cursor: pointer;
    }

    @media (max-width: 600px) {
        .row {
            flex-direction: column;
            gap: 0;
        }

        .modal-content {
            padding: 1rem;
            width: 95%;
            max-height: 90vh;
        }

        .modal-header h2 {
            font-size: 1.2rem;
        }

        .input-group {
            margin-bottom: 1rem;
        }
    }
</style>
