<script>
    import { goto } from "$app/navigation";

    export let id = "0000";
    export let title;
    export let imageUrl;
    export let status;
    export let joinDeadline;
    export let memberCount = 0;
    export let maxParticipants = 16;
    export let xpPerWin = 100;

    function handleClick() {
        goto(`/${encodeURIComponent(title)}`);
    }

    $: deadlineDate = new Date(joinDeadline).toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
    });

    $: serialId = `TRN-${id.toString().substring(0, 8).toUpperCase()}`;

    // Status colors
    let statusColor = "#666";
    let statusText = "DRAFT";

    $: {
        switch (status) {
            case 0:
                statusText = "DRAFT";
                statusColor = "#666";
                break;
            case 1:
                statusText = "RECRUITING";
                statusColor = "var(--wow-cyan)";
                break;
            case 2:
                statusText = "LOCKED";
                statusColor = "#f1c40f";
                break;
            case 3:
                statusText = "LIVE";
                statusColor = "#ff3e3e";
                break;
            case 4:
                statusText = "COMPLETED";
                statusColor = "#a0b6c5";
                break;
        }
    }
</script>

<!-- svelte-ignore a11y-click-events-have-key-events -->
<!-- svelte-ignore a11y-no-static-element-interactions -->
<div
    class="tournament-card"
    on:click={handleClick}
    style="--status-color: {statusColor}"
>
    <!-- Top Border -->
    <div class="card-top-border">
        <div class="top-segment seg-1"><div class="segment-fill"></div></div>
        <div class="top-gap"></div>
        <div class="top-segment seg-2"><div class="segment-fill"></div></div>
        <div class="top-gap"></div>
        <div class="top-segment seg-3"><div class="segment-fill"></div></div>
    </div>

    <!-- Content Wrapper -->
    <div class="card-content-wrapper">
        <!-- Left Border -->
        <div class="side-border left-border">
            <div class="border-segment" style="--delay: 0s"></div>
            <div class="border-gap"></div>
            <div class="border-segment" style="--delay: 0.1s"></div>
            <div class="border-gap"></div>
            <div class="border-segment" style="--delay: 0.2s"></div>
        </div>

        <!-- Content -->
        <div class="card-inner">
            <!-- Header -->
            <div class="card-header">
                <span class="serial-id">{serialId}</span>
                <div class="status-badge">
                    <span class="status-indicator"></span>
                    {statusText}
                </div>
            </div>

            <!-- Visual -->
            {#if imageUrl}
                <div class="card-visual">
                    <img src={imageUrl} alt={title} />
                    <div class="visual-overlay"></div>
                </div>
            {:else}
                <div class="card-visual placeholder">
                    <div class="tech-grid"></div>
                </div>
            {/if}

            <!-- Body -->
            <div class="card-body">
                <h3 class="card-title">{title}</h3>

                <div class="stats-row">
                    <div class="stat-item">
                        <span class="stat-label">ROSTER</span>
                        <span class="stat-value"
                            >{memberCount}/{maxParticipants}</span
                        >
                    </div>
                    <div class="stat-divider"></div>
                    <div class="stat-item">
                        <span class="stat-label">REWARD</span>
                        <span class="stat-value">{xpPerWin} XP</span>
                    </div>
                </div>

                <div class="info-divider"></div>

                <div class="card-footer">
                    <div class="deadline-info">
                        <span class="deadline-label">DEADLINE</span>
                        <span class="deadline-date">{deadlineDate}</span>
                    </div>
                    <div class="action-text">ENTER →</div>
                </div>
            </div>
        </div>

        <!-- Right Border -->
        <div class="side-border right-border">
            <div class="border-segment" style="--delay: 0s"></div>
            <div class="border-gap"></div>
            <div class="border-segment" style="--delay: 0.1s"></div>
            <div class="border-gap"></div>
            <div class="border-segment" style="--delay: 0.2s"></div>
        </div>
    </div>

    <!-- Bottom Border -->
    <div class="card-bottom-border">
        <div class="bottom-segment"></div>
        <div class="bottom-gap"></div>
        <div class="bottom-segment"></div>
        <div class="bottom-gap"></div>
        <div class="bottom-segment"></div>
    </div>
</div>

<style>
    .tournament-card {
        position: relative;
        background: rgba(2, 6, 12, 0.95);
        cursor: pointer;
        transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
        overflow: hidden;
    }

    .tournament-card:hover {
        transform: translateY(-6px);
        box-shadow:
            0 15px 50px rgba(0, 243, 255, 0.2),
            0 0 0 1px rgba(0, 243, 255, 0.3);
    }

    /* Top Border */
    .card-top-border {
        height: 4px;
        display: flex;
        gap: 0;
        background: rgba(0, 0, 0, 0.5);
    }

    .top-segment {
        flex: 1;
        position: relative;
        overflow: hidden;
    }

    .segment-fill {
        position: absolute;
        inset: 0;
        background: linear-gradient(
            90deg,
            transparent 0%,
            var(--status-color) 50%,
            transparent 100%
        );
        opacity: 0.5;
        transition: opacity 0.3s ease;
    }

    .tournament-card:hover .segment-fill {
        opacity: 0.9;
        animation: pulse-glow 2s ease-in-out infinite;
    }

    @keyframes pulse-glow {
        0%,
        100% {
            opacity: 0.6;
        }
        50% {
            opacity: 1;
        }
    }

    .top-gap {
        width: 3px;
        background: rgba(0, 0, 0, 0.8);
    }

    /* Content Wrapper */
    .card-content-wrapper {
        display: flex;
    }

    /* Side Borders */
    .side-border {
        width: 4px;
        display: flex;
        flex-direction: column;
        background: rgba(0, 0, 0, 0.5);
    }

    .border-segment {
        flex: 1;
        background: linear-gradient(
            180deg,
            transparent 0%,
            var(--status-color) 50%,
            transparent 100%
        );
        opacity: 0.4;
        animation: pulse-border 3s ease-in-out infinite;
        animation-delay: var(--delay);
        transition: opacity 0.3s ease;
    }

    .tournament-card:hover .border-segment {
        opacity: 0.8;
    }

    .border-gap {
        height: 3px;
        background: rgba(0, 0, 0, 0.8);
    }

    @keyframes pulse-border {
        0%,
        100% {
            opacity: 0.3;
        }
        50% {
            opacity: 0.7;
        }
    }

    /* Content */
    .card-inner {
        flex: 1;
        display: flex;
        flex-direction: column;
        background: linear-gradient(
            135deg,
            rgba(0, 12, 20, 0.95) 0%,
            rgba(0, 20, 30, 0.95) 100%
        );
    }

    .card-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 1rem 1.25rem;
        border-bottom: 1px solid rgba(0, 243, 255, 0.15);
        background: rgba(0, 12, 20, 0.7);
        backdrop-filter: blur(10px);
    }

    .serial-id {
        font-family: monospace;
        font-size: 0.7rem;
        color: rgba(0, 243, 255, 0.5);
        letter-spacing: 0.15em;
        transition: color 0.3s ease;
    }

    .tournament-card:hover .serial-id {
        color: rgba(0, 243, 255, 0.8);
    }

    .status-badge {
        display: flex;
        align-items: center;
        gap: 0.6rem;
        font-family: var(--wow-font-display);
        font-size: 0.65rem;
        font-weight: bold;
        color: var(--status-color);
        letter-spacing: 0.12em;
        text-transform: uppercase;
        padding: 0.4rem 0.9rem;
        border: 1px solid var(--status-color);
        background: rgba(0, 0, 0, 0.4);
        clip-path: polygon(6px 0, 100% 0, calc(100% - 6px) 100%, 0 100%);
        transition: all 0.3s ease;
    }

    .tournament-card:hover .status-badge {
        background: rgba(0, 0, 0, 0.6);
        box-shadow: 0 0 15px var(--status-color);
    }

    .status-indicator {
        width: 7px;
        height: 7px;
        border-radius: 50%;
        background: var(--status-color);
        box-shadow: 0 0 10px var(--status-color);
        animation: pulse-status 2s ease-in-out infinite;
    }

    @keyframes pulse-status {
        0%,
        100% {
            opacity: 1;
            transform: scale(1);
        }
        50% {
            opacity: 0.4;
            transform: scale(0.9);
        }
    }

    /* Visual */
    .card-visual {
        height: 190px;
        position: relative;
        overflow: hidden;
        background: #000;
    }

    .card-visual img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        filter: grayscale(60%) brightness(0.8) contrast(1.1);
        transition: all 0.6s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .tournament-card:hover .card-visual img {
        filter: grayscale(0%) brightness(1) contrast(1);
        transform: scale(1.08);
    }

    .visual-overlay {
        position: absolute;
        inset: 0;
        background: linear-gradient(
            to bottom,
            transparent 40%,
            rgba(2, 6, 12, 0.6) 70%,
            rgba(2, 6, 12, 0.95) 100%
        );
        pointer-events: none;
    }

    .card-visual.placeholder {
        background: linear-gradient(
            135deg,
            rgba(0, 12, 20, 0.95),
            rgba(0, 26, 43, 0.95)
        );
    }

    .tech-grid {
        position: absolute;
        inset: 0;
        background-image: linear-gradient(
                rgba(0, 243, 255, 0.08) 1px,
                transparent 1px
            ),
            linear-gradient(90deg, rgba(0, 243, 255, 0.08) 1px, transparent 1px);
        background-size: 20px 20px;
        animation: grid-drift 25s linear infinite;
        opacity: 0.6;
    }

    @keyframes grid-drift {
        0% {
            transform: translate(0, 0);
        }
        100% {
            transform: translate(20px, 20px);
        }
    }

    /* Body */
    .card-body {
        padding: 1.25rem;
        flex: 1;
        display: flex;
        flex-direction: column;
        gap: 0.25rem;
    }

    .card-title {
        font-family: var(--wow-font-display);
        font-size: 1.05rem;
        font-weight: 700;
        color: #fff;
        margin: 0 0 1.25rem 0;
        text-transform: uppercase;
        letter-spacing: 0.08em;
        line-height: 1.3;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        text-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
        transition: color 0.3s ease;
    }

    .tournament-card:hover .card-title {
        color: var(--wow-cyan);
    }

    .stats-row {
        display: flex;
        align-items: center;
        gap: 1.25rem;
        margin-bottom: 1rem;
        padding: 0.75rem;
        background: rgba(0, 243, 255, 0.03);
        border: 1px solid rgba(0, 243, 255, 0.1);
        border-radius: 3px;
    }

    .stat-item {
        flex: 1;
        display: flex;
        flex-direction: column;
        gap: 0.35rem;
    }

    .stat-label {
        font-size: 0.65rem;
        color: rgba(255, 255, 255, 0.4);
        letter-spacing: 0.12em;
        text-transform: uppercase;
        font-weight: 600;
    }

    .stat-value {
        font-family: monospace;
        font-size: 0.95rem;
        color: var(--wow-cyan);
        font-weight: bold;
        text-shadow: 0 0 8px rgba(0, 243, 255, 0.3);
    }

    .stat-divider {
        width: 1px;
        height: 35px;
        background: linear-gradient(
            to bottom,
            transparent,
            rgba(0, 243, 255, 0.3),
            transparent
        );
    }

    .info-divider {
        height: 1px;
        background: linear-gradient(
            90deg,
            transparent,
            rgba(0, 243, 255, 0.4),
            transparent
        );
        margin: 1rem 0;
        box-shadow: 0 0 8px rgba(0, 243, 255, 0.2);
    }

    .card-footer {
        display: flex;
        justify-content: space-between;
        align-items: flex-end;
        margin-top: auto;
        padding-top: 0.5rem;
    }

    .deadline-info {
        display: flex;
        flex-direction: column;
        gap: 0.3rem;
    }

    .deadline-label {
        font-size: 0.6rem;
        color: rgba(255, 255, 255, 0.4);
        text-transform: uppercase;
        letter-spacing: 0.12em;
        font-weight: 600;
    }

    .deadline-date {
        font-family: monospace;
        font-size: 0.9rem;
        color: rgba(255, 255, 255, 0.9);
        font-weight: 600;
    }

    .action-text {
        font-family: var(--wow-font-display);
        font-size: 0.8rem;
        color: rgba(0, 243, 255, 0.5);
        font-weight: bold;
        letter-spacing: 0.15em;
        transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        padding: 0.5rem 0.75rem;
        border: 1px solid transparent;
    }

    .tournament-card:hover .action-text {
        color: var(--wow-cyan);
        transform: translateX(4px);
        border-color: rgba(0, 243, 255, 0.3);
        background: rgba(0, 243, 255, 0.05);
        box-shadow: 0 0 10px rgba(0, 243, 255, 0.2);
    }

    /* Bottom Border */
    .card-bottom-border {
        height: 4px;
        display: flex;
        background: rgba(0, 0, 0, 0.5);
    }

    .bottom-segment {
        flex: 1;
        background: linear-gradient(
            90deg,
            transparent 0%,
            var(--status-color) 50%,
            transparent 100%
        );
        opacity: 0.5;
        transition: opacity 0.3s ease;
    }

    .tournament-card:hover .bottom-segment {
        opacity: 0.9;
    }

    .bottom-gap {
        width: 3px;
        background: rgba(0, 0, 0, 0.8);
    }

    /* Hover Glow */
    .tournament-card::before {
        content: "";
        position: absolute;
        inset: -2px;
        background: linear-gradient(
            135deg,
            transparent 0%,
            var(--status-color) 50%,
            transparent 100%
        );
        opacity: 0;
        transition: opacity 0.4s ease;
        pointer-events: none;
        z-index: -1;
        filter: blur(10px);
    }

    .tournament-card:hover::before {
        opacity: 0.3;
    }

    /* Scan Line Effect */
    .tournament-card::after {
        content: "";
        position: absolute;
        top: -100%;
        left: 0;
        right: 0;
        height: 100%;
        background: linear-gradient(
            to bottom,
            transparent 0%,
            rgba(0, 243, 255, 0.1) 50%,
            transparent 100%
        );
        pointer-events: none;
        transition: top 0.8s ease-out;
    }

    .tournament-card:hover::after {
        top: 100%;
    }
</style>
