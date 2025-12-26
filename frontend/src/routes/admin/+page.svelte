<script>
    import { onMount } from "svelte";
    import { api } from "$lib/api";
    import CyberButton from "$lib/components/CyberButton.svelte";
    import CreateTournamentModal from "./CreateTournamentModal.svelte";

    let tournaments = [];
    let loading = true;
    let showCreateModal = false;

    const STATUS_MAP = {
        0: { text: "DRAFT", color: "#888" },
        1: { text: "OPEN", color: "#00ff41" }, // Green
        2: { text: "LOCKED", color: "#ffbd00" }, // Orange
        3: { text: "IN PROGRESS", color: "#00f3ff" }, // Cyan
        4: { text: "COMPLETED", color: "#ff0055" }, // Red/Pink
        // Fallback for strings if serialization changes
        Draft: { text: "DRAFT", color: "#888" },
        Open: { text: "OPEN", color: "#00ff41" },
        Locked: { text: "LOCKED", color: "#ffbd00" },
        InProgress: { text: "IN PROGRESS", color: "#00f3ff" },
        Completed: { text: "COMPLETED", color: "#ff0055" },
    };

    onMount(async () => {
        await loadTournaments();
    });

    async function loadTournaments() {
        loading = true;
        try {
            tournaments = await api.tournaments.list();
        } catch (e) {
            console.error(e);
        } finally {
            loading = false;
        }
    }

    function formatDate(dateStr) {
        return (
            new Date(dateStr).toLocaleDateString() +
            " " +
            new Date(dateStr).toLocaleTimeString()
        );
    }

    function handleCreateSuccess() {
        loadTournaments();
    }

    async function handleAction(action, t) {
        if (!confirm(`CONFIRM ${action} PROTOCOL FOR ${t.name}?`)) return;

        try {
            if (action === "OPEN") {
                await api.tournaments.open(t.id);
            } else if (action === "LOCK") {
                await api.tournaments.lock(t.id);
            } else if (action === "DRAW") {
                await api.tournaments.draw(t.id);
            } else if (action === "EXTEND") {
                const newDate = prompt(
                    "NEW DEADLINE (YYYY-MM-DDTHH:MM)",
                    t.joinDeadline,
                );
                if (newDate)
                    await api.tournaments.extend(
                        t.id,
                        new Date(newDate).toISOString(),
                    );
            }
            loadTournaments();
        } catch (e) {
            console.error("Action Failed:", e);
            alert("PROTOCOL FAILED: " + (e.message || "UNKNOWN ERROR"));
        }
    }
</script>

<div class="dashboard">
    <div class="dash-header">
        <h1>GLOBAL OPERATIONS</h1>
        <div class="controls">
            <CyberButton
                label="NEW OPERATION"
                variant="primary"
                on:click={() => (showCreateModal = true)}
            />
        </div>
    </div>

    <CreateTournamentModal
        isOpen={showCreateModal}
        on:close={() => (showCreateModal = false)}
        on:success={handleCreateSuccess}
    />

    {#if loading}
        <div class="loading">ESTABLISHING UPLINK...</div>
    {:else}
        <div class="grid">
            {#each tournaments as t}
                <div class="card status-{t.status}">
                    <div class="card-header">
                        <span
                            class="status-badge"
                            style="background: {STATUS_MAP[t.status]?.color ||
                                '#fff'}"
                        >
                            {STATUS_MAP[t.status]?.text || "UNKNOWN"}
                        </span>
                        <span class="id">#{t.id.substring(0, 6)}</span>
                    </div>
                    <h2>{t.name}</h2>
                    <div class="stats">
                        <div class="stat">
                            <label>PLAYERS</label>
                            <span
                                >{t.memberCount || 0} / {t.maxParticipants}</span
                            >
                        </div>
                        <div class="stat">
                            <label>XP</label>
                            <span>{t.xpPerWin}</span>
                        </div>
                        <div class="stat">
                            <label>DEADLINE</label>
                            <span>{formatDate(t.joinDeadline)}</span>
                        </div>
                    </div>
                    <div class="actions">
                        {#if t.status === 0}
                            <!-- DRAFT -->
                            <button
                                class="action-btn"
                                on:click={() => handleAction("OPEN", t)}
                                >OPEN</button
                            >
                        {:else if t.status === 1}
                            <!-- OPEN -->
                            {#if t.memberCount === t.maxParticipants}
                                <!-- Show DRAW when max participants reached -->
                                <button
                                    class="action-btn draw-ready"
                                    on:click={() => handleAction("DRAW", t)}
                                    >DRAW</button
                                >
                            {:else}
                                <!-- Show LOCK when not at max capacity yet -->
                                <button
                                    class="action-btn"
                                    on:click={() => handleAction("LOCK", t)}
                                    >LOCK</button
                                >
                            {/if}
                            <button
                                class="action-btn"
                                on:click={() => handleAction("EXTEND", t)}
                                >EXTEND</button
                            >
                        {:else if t.status === 2}
                            <!-- LOCKED -->
                            {#if t.memberCount === t.maxParticipants}
                                <button
                                    class="action-btn"
                                    on:click={() => handleAction("DRAW", t)}
                                    >DRAW</button
                                >
                            {:else}
                                <span class="action-info"
                                    >WAITING FOR FULL ROSTER</span
                                >
                            {/if}
                            <button
                                class="action-btn"
                                on:click={() => handleAction("OPEN", t)}
                                >RE-OPEN</button
                            >
                        {/if}
                        <a
                            href={`/tournaments/${t.id}`}
                            class="action-btn manage-link">MANAGE</a
                        >
                    </div>
                </div>
            {/each}
        </div>
    {/if}
</div>

<style>
    .dashboard {
        max-width: 1400px;
        margin: 0 auto;
    }

    .dash-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 3rem;
        border-bottom: 1px solid rgba(0, 243, 255, 0.2);
        padding-bottom: 1rem;
    }

    h1 {
        font-family: var(--wow-font-display);
        font-size: 2.5rem;
        color: #fff;
        margin: 0;
        letter-spacing: 4px;
        text-shadow: 0 0 20px rgba(0, 243, 255, 0.3);
    }

    .grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
        gap: 2rem;
    }

    .card {
        background: rgba(0, 20, 30, 0.6);
        border: 1px solid rgba(0, 243, 255, 0.1);
        padding: 1.5rem;
        position: relative;
        transition: all 0.3s ease;
    }
    .card:hover {
        background: rgba(0, 20, 30, 0.8);
        border-color: var(--wow-cyan);
        transform: translateY(-5px);
        box-shadow: 0 0 30px rgba(0, 243, 255, 0.1);
    }

    /* Status borders */
    .card.status-1 {
        border-left: 4px solid #00ff41;
    } /* Open */
    .card.status-3 {
        border-left: 4px solid #00f3ff;
    } /* In Progress */
    .card.status-4 {
        border-left: 4px solid #ff0055;
    } /* Completed */

    .card-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
    }

    .status-badge {
        color: #000;
        font-weight: bold;
        padding: 2px 8px;
        font-size: 0.7rem;
        border-radius: 2px;
    }

    .id {
        font-family: monospace;
        color: rgba(255, 255, 255, 0.3);
    }

    h2 {
        font-family: var(--wow-font-display);
        color: #fff;
        font-size: 1.2rem;
        margin: 0 0 1.5rem 0;
        letter-spacing: 1px;
    }

    .stats {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 1rem;
        margin-bottom: 1.5rem;
        font-size: 0.9rem;
    }

    .stat {
        display: flex;
        flex-direction: column;
        gap: 0.2rem;
    }

    .stat label {
        color: var(--wow-cyan);
        opacity: 0.7;
        font-size: 0.7rem;
        font-family: var(--wow-font-display);
    }

    .actions {
        display: flex;
        justify-content: flex-end;
        gap: 0.5rem;
        border-top: 1px solid rgba(255, 255, 255, 0.05);
        padding-top: 1rem;
    }

    .action-btn {
        background: transparent;
        border: 1px solid rgba(255, 255, 255, 0.3);
        color: #fff;
        padding: 0.5rem 1rem;
        cursor: pointer;
        transition: all 0.2s;
        font-family: var(--wow-font-display);
        letter-spacing: 1px;
        font-size: 0.8rem;
    }
    .action-btn:hover {
        border-color: var(--wow-cyan);
        color: var(--wow-cyan);
        background: rgba(0, 243, 255, 0.1);
    }

    .loading {
        text-align: center;
        font-size: 2rem;
        color: var(--wow-cyan);
        animation: pulse 1s infinite;
    }

    .manage-link {
        text-decoration: none;
        display: inline-block;
        text-align: center;
    }

    .action-info {
        font-family: var(--wow-font-display);
        color: #888;
        font-size: 0.7rem;
        align-self: center;
        margin-right: 0.5rem;
    }

    .draw-ready {
        border-color: #00ff41 !important;
        color: #00ff41 !important;
        background: rgba(0, 255, 65, 0.1) !important;
        font-weight: bold;
        animation: pulse-glow 2s infinite;
    }

    .draw-ready:hover {
        background: rgba(0, 255, 65, 0.2) !important;
        box-shadow: 0 0 15px rgba(0, 255, 65, 0.3);
    }

    @keyframes pulse-glow {
        0%,
        100% {
            box-shadow: 0 0 5px rgba(0, 255, 65, 0.3);
        }
        50% {
            box-shadow: 0 0 20px rgba(0, 255, 65, 0.5);
        }
    }
</style>
