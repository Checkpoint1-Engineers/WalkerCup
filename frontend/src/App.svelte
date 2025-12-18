<script>
    import { onMount } from "svelte";
    import { curRoute } from "./lib/router.js";
    import { api } from "./lib/api.js";
    import Header from "./lib/Header.svelte";
    import Footer from "./lib/Footer.svelte";
    import TournamentGrid from "./lib/TournamentGrid.svelte";
    import Home from "./lib/Home.svelte";
    import Cursor from "./lib/components/Cursor.svelte";

    let isLoading = true;

    // Data State
    let tournaments = [];
    let loading = true;
    let selectedTournament = null;

    // Components
    import TournamentDetails from "./lib/TournamentDetails.svelte";

    // Routing State
    let view = "HOME";
    let filteredTournaments = [];
    let pageTitle = "";
    let pageSubtitle = "";

    // React to Route Changes
    $: {
        const path = $curRoute;

        if (path === "/" || path === "") {
            view = "HOME";
        } else if (path === "/recruiting") {
            view = "GRID";
            pageTitle = "OPEN MISSIONS";
            pageSubtitle =
                "The journey begins here. Prove your worth in open qualifiers.";
            filteredTournaments = tournaments.filter((t) => t.status === 1);
        } else if (path === "/live") {
            view = "GRID";
            pageTitle = "LIVE OPERATIONS";
            pageSubtitle =
                "Active warzones. Watch the elite compete for glory.";
            filteredTournaments = tournaments.filter((t) => t.status === 3);
        } else if (path === "/archives") {
            view = "GRID";
            pageTitle = "MISSION ARCHIVES";
            pageSubtitle =
                "History of the Walkers. Study past conflicts and results.";
            filteredTournaments = tournaments.filter((t) => t.status === 4);
        } else {
            // Check if it's a tournament slug
            const potentialSlug = path.substring(1); // remove leading slash
            const t = tournaments.find(
                (x) => x.name === decodeURIComponent(potentialSlug),
            );

            if (t) {
                view = "DETAILS";
                // Pass the specific tournament to the view or store ID
                selectedTournament = t;
            } else {
                view = "HOME";
            }
        }
    }

    onMount(async () => {
        // Boot Screen Timeout
        setTimeout(() => (isLoading = false), 1500);

        try {
            tournaments = await api.tournaments.list();
            // Trigger reactivity
            curRoute.set(window.location.pathname);
        } catch (e) {
            console.error(e);
        } finally {
            loading = false;
        }
    });
</script>

<div class="dashboard-layout">
    <Header />

    <main class="main-content">
        {#if loading}
            <div class="status-msg">
                <div class="loader"></div>
                ESTABLISHING CONNECTION...
            </div>
        {:else if view === "HOME"}
            <Home />
        {:else if view === "DETAILS"}
            <TournamentDetails
                tournament={{
                    ...selectedTournament,
                    title: selectedTournament.name,
                }}
            />
        {:else if view === "GRID"}
            <header class="content-header">
                <div>
                    <h2 class="section-title">{pageTitle}</h2>
                    <p class="section-sub">{pageSubtitle}</p>
                </div>
            </header>

            {#if filteredTournaments.length > 0}
                <TournamentGrid tournaments={filteredTournaments} />
            {:else}
                <div class="empty-state">NO DATA FOUND IN THIS SECTOR.</div>
            {/if}
        {/if}
    </main>

    <Footer />
</div>

<style>
    .dashboard-layout {
        min-height: 100vh;
        /* background: var(--wow-bg); removed to let Background.svelte gradient take over */
        background: #020b14; /* Fallback */
        display: flex;
        flex-direction: column;
    }
    .main-content {
        max-width: 1600px;
        margin: 0 auto;
        padding: 0 3rem 3rem;
        width: 100%;
        box-sizing: border-box;
        position: relative; /* For Background.svelte */
        min-height: 100vh; /* Fixed height to avoid layout shift */
        display: flex;
        flex-direction: column;
    }
    .content-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-end;
        margin-bottom: 2rem;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        padding-bottom: 1rem;
    }
    .section-title {
        font-family: var(--wow-font-display);
        font-size: 2.5rem;
        font-weight: 800;
        letter-spacing: 2px;
        color: #fff;
        margin: 0;
        text-transform: uppercase;
        line-height: 1;
        text-shadow: 0 0 20px rgba(0, 243, 255, 0.4);
    }
    .section-sub {
        font-family: "Consolas", "Monaco", monospace;
        color: var(--wow-cyan);
        margin: 0.5rem 0 0;
        font-size: 0.9rem;
        opacity: 0.8;
        letter-spacing: 1px;
    }
    .loader {
        border-top-color: var(--wow-cyan);
        border-radius: 50%;
        animation: spin 1s linear infinite;
    }
    @keyframes spin {
        100% {
            transform: rotate(360deg);
        }
    }
</style>
