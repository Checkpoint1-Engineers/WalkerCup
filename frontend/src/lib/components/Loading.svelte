<script>
    import { scramble } from "$lib/actions/scramble.js";
    import { onMount } from "svelte";

    let progress = 0;
    let statusText = "INITIALIZING";

    const statuses = ["INITIALIZING", "CONNECTING", "LOADING DATA", "READY"];

    onMount(() => {
        const interval = setInterval(() => {
            progress = (progress + 1) % 100;
            if (progress % 25 === 0) {
                const index = Math.floor(progress / 25) % statuses.length;
                statusText = statuses[index];
            }
        }, 50);

        return () => clearInterval(interval);
    });
</script>

<div class="loading-screen">
    <div class="loading-frame">
        <!-- Corner Accents -->
        <div class="corner corner-tl"></div>
        <div class="corner corner-tr"></div>
        <div class="corner corner-bl"></div>
        <div class="corner corner-br"></div>

        <!-- Loading Content -->
        <div class="loading-content">
            <!-- Animated Logo/Icon -->
            <div class="tech-logo">
                <div class="circle-ring ring-1"></div>
                <div class="circle-ring ring-2"></div>
                <div class="circle-ring ring-3"></div>
                <div class="core"></div>
            </div>

            <!-- Status Text with Scramble -->
            <div class="status-section">
                <div class="status-label">SYSTEM STATUS</div>
                <div
                    class="status-text"
                    use:scramble={{ speed: 2, autoStart: false }}
                >
                    {statusText}
                </div>
            </div>

            <!-- Progress Bar -->
            <div class="progress-container">
                <div class="progress-track">
                    <div class="progress-bar" style="width: {progress}%"></div>
                </div>
                <div class="progress-value">{progress}%</div>
            </div>

            <!-- Tech Grid Background -->
            <div class="grid-lines"></div>
        </div>
    </div>
</div>

<style>
    .loading-screen {
        display: flex;
        align-items: center;
        justify-content: center;
        min-height: 50vh;
        padding: 2rem;
    }

    /* Loading Frame (matches CyberCard style) */
    .loading-frame {
        position: relative;
        width: 100%;
        max-width: 500px;
        padding: 3rem 2rem;
        background: rgba(0, 12, 20, 0.6);
        border: 1px solid rgba(0, 243, 255, 0.2);
        clip-path: polygon(
            20px 0,
            100% 0,
            100% calc(100% - 20px),
            calc(100% - 20px) 100%,
            0 100%,
            0 20px
        );
    }

    /* Corner Accents */
    .corner {
        position: absolute;
        width: 15px;
        height: 15px;
        border: 2px solid var(--wow-cyan);
    }
    .corner-tl {
        top: 0;
        left: 0;
        border-right: none;
        border-bottom: none;
    }
    .corner-tr {
        top: 0;
        right: 0;
        border-left: none;
        border-bottom: none;
    }
    .corner-bl {
        bottom: 0;
        left: 0;
        border-right: none;
        border-top: none;
    }
    .corner-br {
        bottom: 0;
        right: 0;
        border-left: none;
        border-top: none;
    }

    /* Content */
    .loading-content {
        position: relative;
        z-index: 1;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 2rem;
    }

    /* Tech Logo */
    .tech-logo {
        position: relative;
        width: 100px;
        height: 100px;
    }

    .circle-ring {
        position: absolute;
        border-radius: 50%;
        border: 2px solid var(--wow-cyan);
        opacity: 0.3;
    }

    .ring-1 {
        width: 100%;
        height: 100%;
        animation: rotate 3s linear infinite;
    }

    .ring-2 {
        width: 70%;
        height: 70%;
        top: 15%;
        left: 15%;
        animation: rotate 2s linear infinite reverse;
    }

    .ring-3 {
        width: 40%;
        height: 40%;
        top: 30%;
        left: 30%;
        animation: pulse-ring 2s ease-in-out infinite;
    }

    .core {
        position: absolute;
        width: 20px;
        height: 20px;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background: var(--wow-cyan);
        border-radius: 50%;
        box-shadow: 0 0 20px var(--wow-cyan);
        animation: pulse-core 1.5s ease-in-out infinite;
    }

    @keyframes rotate {
        from {
            transform: rotate(0deg);
        }
        to {
            transform: rotate(360deg);
        }
    }

    @keyframes pulse-ring {
        0%,
        100% {
            opacity: 0.3;
            transform: scale(1);
        }
        50% {
            opacity: 0.8;
            transform: scale(1.1);
        }
    }

    @keyframes pulse-core {
        0%,
        100% {
            opacity: 1;
        }
        50% {
            opacity: 0.5;
        }
    }

    /* Status Section */
    .status-section {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 0.5rem;
    }

    .status-label {
        font-family: var(--wow-font-body);
        font-size: 0.7rem;
        color: rgba(255, 255, 255, 0.4);
        letter-spacing: 0.2em;
        text-transform: uppercase;
    }

    .status-text {
        font-family: var(--wow-font-display);
        font-size: 1.2rem;
        color: var(--wow-cyan);
        letter-spacing: 0.15em;
        font-weight: bold;
    }

    /* Progress Bar */
    .progress-container {
        width: 100%;
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    .progress-track {
        width: 100%;
        height: 4px;
        background: rgba(0, 243, 255, 0.1);
        position: relative;
        overflow: hidden;
    }

    .progress-bar {
        height: 100%;
        background: linear-gradient(
            90deg,
            var(--wow-cyan),
            #0af,
            var(--wow-cyan)
        );
        background-size: 200% 100%;
        animation: shimmer 2s linear infinite;
        box-shadow: 0 0 10px var(--wow-cyan);
        transition: width 0.3s ease;
    }

    @keyframes shimmer {
        0% {
            background-position: 200% 0;
        }
        100% {
            background-position: -200% 0;
        }
    }

    .progress-value {
        font-family: var(--wow-font-display);
        font-size: 0.8rem;
        color: var(--wow-cyan);
        text-align: right;
        letter-spacing: 0.1em;
    }

    /* Grid Background */
    .grid-lines {
        position: absolute;
        inset: 0;
        background-image: linear-gradient(
                rgba(0, 243, 255, 0.02) 1px,
                transparent 1px
            ),
            linear-gradient(90deg, rgba(0, 243, 255, 0.02) 1px, transparent 1px);
        background-size: 30px 30px;
        opacity: 0.5;
        pointer-events: none;
    }

    /* Responsive */
    @media (max-width: 768px) {
        .loading-frame {
            padding: 2rem 1.5rem;
        }

        .tech-logo {
            width: 80px;
            height: 80px;
        }

        .status-text {
            font-size: 1rem;
        }
    }
</style>
