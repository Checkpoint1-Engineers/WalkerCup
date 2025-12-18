<script>
    import { navigate } from "./router.js";
    import { spring } from "svelte/motion";

    export let id = "0000";
    export let title;
    export let imageUrl;
    export let status;
    export let joinDeadline;

    function handleClick() {
        navigate(`/${encodeURIComponent(title)}`);
    }

    // Format Date
    $: deadlineDate = new Date(joinDeadline).toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
    });

    // Generate Lore Data
    $: serialId = `OP-${(id || "XXXX").toString().slice(0, 4).toUpperCase()}`;
    $: priorityLevel = status === 3 ? "CRITICAL" : "STANDARD";
    $: region = "GLOBAL";

    // Status Logic
    let statusText = "OPEN";
    let accentColor = "var(--wow-cyan)";

    if (status === 1) {
        statusText = "RECRUITING";
        accentColor = "var(--wow-cyan)";
    }
    if (status === 3) {
        statusText = "LIVE OPS";
        accentColor = "#ff3e3e";
    }
    if (status === 4) {
        statusText = "ARCHIVED";
        accentColor = "#a0b6c5";
    }

    // 3D TILT LOGIC
    let rx = spring(0, { stiffness: 0.1, damping: 0.5 });
    let ry = spring(0, { stiffness: 0.1, damping: 0.5 });

    function handleMouseMove(e) {
        const rect = e.currentTarget.getBoundingClientRect();
        const x = e.clientX - rect.left;
        const y = e.clientY - rect.top;

        const xPct = x / rect.width - 0.5;
        const yPct = y / rect.height - 0.5;

        ry.set(xPct * 15);
        rx.set(yPct * -15);
    }

    function handleMouseLeave() {
        rx.set(0);
        ry.set(0);
    }
</script>

<!-- svelte-ignore a11y-click-events-have-key-events -->
<!-- svelte-ignore a11y-no-static-element-interactions -->
<div
    class="game-card"
    on:click={handleClick}
    on:mousemove={handleMouseMove}
    on:mouseleave={handleMouseLeave}
    style="--accent: {accentColor}; --rx: {$rx}deg; --ry: {$ry}deg"
>
    <!-- Border Flow -->
    <div class="border-gradient"></div>

    <!-- Inner Plate -->
    <div class="card-inner">
        <!-- Visual Texture overlay -->
        <div class="texture-overlay"></div>

        <!-- Image Content -->
        <div class="card-media">
            {#if imageUrl}
                <img src={imageUrl} alt={title} />
            {:else}
                <div class="placeholder"></div>
            {/if}
            <div class="media-overlay"></div>

            <div class="status-badge">
                <span class="status-dot"></span>
                {statusText}
            </div>

            <div class="serial-tag">{serialId}</div>
        </div>

        <!-- Text Content -->
        <div class="card-content">
            <h3 class="card-title">{title}</h3>

            <!-- Tech Specs Grid -->
            <div class="tech-grid">
                <div class="tech-item">
                    <span class="tech-label">PRIORITY</span>
                    <span class="tech-val" class:alert={status === 3}
                        >{priorityLevel}</span
                    >
                </div>
                <div class="tech-item">
                    <span class="tech-label">REGION</span>
                    <span class="tech-val">{region}</span>
                </div>
                <div class="tech-item">
                    <span class="tech-label">SQUAD</span>
                    <span class="tech-val">4-MAN</span>
                </div>
                <div class="tech-item">
                    <span class="tech-label">ACCESS</span>
                    <span class="tech-val">CLEARED</span>
                </div>
            </div>

            <div class="divider"></div>

            <div class="meta-row">
                <div class="meta-item">
                    <span class="label">DEADLINE</span>
                    <span class="value">{deadlineDate}</span>
                </div>
                <div class="action-btn">
                    INITIALIZE <span class="arrow">›</span>
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    .game-card {
        position: relative;
        height: 380px; /* Taller for more info */
        border-radius: 4px;
        cursor: pointer;
        perspective: 1000px;
        z-index: 1;
    }

    .card-inner {
        position: relative;
        width: 100%;
        height: 100%;
        background: #050a10;
        border-radius: 4px;
        overflow: hidden;
        transform-style: preserve-3d;
        transform: rotateX(var(--rx)) rotateY(var(--ry));
        box-shadow: 0 10px 40px rgba(0, 0, 0, 0.5);
    }

    /* Diagonal Lines Texture */
    .texture-overlay {
        position: absolute;
        inset: 0;
        background-image: repeating-linear-gradient(
            45deg,
            rgba(255, 255, 255, 0.01) 0px,
            rgba(255, 255, 255, 0.01) 1px,
            transparent 1px,
            transparent 10px
        );
        pointer-events: none;
        z-index: 1;
    }

    .border-gradient {
        position: absolute;
        inset: -2px;
        border-radius: 6px;
        background: conic-gradient(
            from 0deg,
            transparent 0deg,
            transparent 90deg,
            var(--accent) 180deg,
            transparent 270deg
        );
        opacity: 0;
        transition: opacity 0.3s;
        transform: rotateX(var(--rx)) rotateY(var(--ry)) translateZ(-1px);
        animation: spinBorder 4s linear infinite;
        z-index: 0;
    }
    .game-card:hover .border-gradient {
        opacity: 1;
    }
    @keyframes spinBorder {
        to {
            transform: rotate(360deg);
        }
    }

    /* MEDIA */
    .card-media {
        height: 50%;
        position: relative;
        overflow: hidden;
        border-bottom: 2px solid rgba(255, 255, 255, 0.05);
    }
    .card-media img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        filter: grayscale(100%) contrast(1.1) brightness(0.8);
        transition: all 0.5s ease;
    }
    .game-card:hover .card-media img {
        filter: none;
        transform: scale(1.1);
    }
    .media-overlay {
        position: absolute;
        inset: 0;
        background: linear-gradient(to top, #050a10 0%, transparent 60%);
    }

    /* BADGES */
    .status-badge {
        position: absolute;
        top: 10px;
        right: 10px;
        background: rgba(0, 0, 0, 0.8);
        border: 1px solid var(--accent);
        color: var(--accent);
        padding: 4px 10px;
        font-size: 0.7rem;
        font-weight: 700;
        letter-spacing: 1px;
        backdrop-filter: blur(4px);
        display: flex;
        gap: 6px;
        align-items: center;
    }
    .status-dot {
        width: 6px;
        height: 6px;
        background: var(--accent);
        border-radius: 50%;
        box-shadow: 0 0 5px var(--accent);
        animation: pulse 2s infinite;
    }
    @keyframes pulse {
        0% {
            opacity: 1;
        }
        50% {
            opacity: 0.5;
        }
        100% {
            opacity: 1;
        }
    }

    .serial-tag {
        position: absolute;
        bottom: 10px;
        left: 10px;
        font-family: monospace;
        font-size: 0.65rem;
        color: rgba(255, 255, 255, 0.5);
        background: rgba(0, 0, 0, 0.6);
        padding: 2px 6px;
        border-radius: 2px;
    }

    /* CONTENT */
    .card-content {
        height: 50%;
        padding: 1.5rem;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        background: #050a10;
        position: relative;
        z-index: 2;
    }

    .card-title {
        font-family: var(--wow-font-display);
        color: #fff;
        font-size: 1.3rem;
        margin: 0 0 1rem 0;
        line-height: 1.1;
        text-transform: uppercase;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
    }

    /* TECH GRID */
    .tech-grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 0.8rem;
        margin-bottom: auto;
    }
    .tech-item {
        display: flex;
        flex-direction: column;
    }
    .tech-label {
        font-size: 0.6rem;
        color: #555;
        font-weight: 700;
        letter-spacing: 0.5px;
        margin-bottom: 2px;
    }
    .tech-val {
        font-size: 0.8rem;
        color: #aaa;
        font-family: monospace;
    }
    .tech-val.alert {
        color: var(--accent);
    }

    .divider {
        height: 1px;
        background: rgba(255, 255, 255, 0.1);
        margin: 1rem 0;
    }

    .meta-row {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .meta-item {
        display: flex;
        flex-direction: column;
    }
    .label {
        font-size: 0.65rem;
        color: #555;
        font-weight: 700;
    }
    .value {
        font-family: monospace;
        color: var(--wow-cyan);
        font-size: 0.85rem;
    }

    /* ACTION BUTTON */
    .action-btn {
        font-size: 0.8rem;
        color: #fff;
        font-weight: 700;
        display: flex;
        align-items: center;
        gap: 5px;
        transition: all 0.3s;
        opacity: 0.5;
    }
    .game-card:hover .action-btn {
        opacity: 1;
        color: var(--accent);
        transform: translateX(5px);
    }
    .arrow {
        font-size: 1.2rem;
        line-height: 0.8;
    }

    .placeholder {
        width: 100%;
        height: 100%;
        background: #111;
    }
</style>
