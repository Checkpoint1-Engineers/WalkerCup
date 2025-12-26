<script>
    // Pure structural component
</script>

<div class="cyber-card-container">
    <div class="cyber-card">
        <!-- Top Border Area -->
        <div class="card-top-border">
            <div class="top-structure">
                <div class="corner-block left"></div>
                <!-- Tech detail: small text near corner -->
                <div class="tech-label left">W47k3r</div>

                <!-- Segmented Top Line Complex -->
                <div class="top-line-complex">
                    <div class="line-segment-h start"></div>
                    <div class="gap-marker-h"></div>
                    <div class="line-segment-h mid-thick"></div>
                    <div class="gap-marker-h"></div>
                    <div class="line-segment-h end"></div>
                </div>

                <div class="tech-label right">ACT.04</div>
                <div class="corner-block right"></div>
            </div>
        </div>

        <!-- Left Border Complex -->
        <div class="border-complex left">
            <div class="line-segment top"></div>
            <div class="gap-marker"></div>
            <div class="line-segment middle-thick"></div>
            <!-- Thicker section seen in screenshot -->
            <div class="line-segment bottom"></div>
        </div>

        <!-- Right Border Complex -->
        <div class="border-complex right">
            <div class="ruler-marks">
                {#each Array(20) as _, i}
                    <div class="mark"></div>
                {/each}
            </div>
            <div class="line-segment top"></div>
            <div class="gap-marker"></div>
            <div class="line-segment bottom"></div>
        </div>

        <!-- Main Content (Wrapped) -->
        <div class="card-content-area">
            <slot />
        </div>

        <!-- Bottom Border Area -->
        <div class="card-bottom-border">
            <div class="bottom-structure">
                <div class="bottom-corner-decor left">
                    <span class="tiny-tech">ONLINE</span>
                </div>
                <div class="bottom-line"></div>
                <div class="bottom-corner-decor right">
                    <span class="tiny-tech">SECURE</span>
                </div>
            </div>

            <div class="bottom-code">W47K3R5 J01N</div>
        </div>
    </div>
</div>

<style>
    :root {
        --wow-card-border: var(--wow-cyan, #00f3ff);
        --wow-card-bg: rgba(2, 11, 20, 0.95);
        --wow-card-glow: rgba(0, 243, 255, 0.15);
    }

    .cyber-card-container {
        position: relative;
        padding: 0px 10px 40px; /* Reduced top padding to 0 as requested */
        width: 100%;
        max-width: 100%;
        box-sizing: border-box;
        background: var(--wow-card-bg);
        /* ... clip-path ... */
        clip-path: polygon(
            0 20px,
            20px 0,
            100% 0,
            100% calc(100% - 20px),
            calc(100% - 20px) 100%,
            0 100%
        );
        display: flex;
        flex-direction: column;
    }

    /* ... */

    /* Content Padding */
    .card-content-area {
        padding: 40px 10px 10px; /* Added top padding as requested */
        width: 100%;
        box-sizing: border-box;
        position: relative;
        z-index: 10;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    /* THE CARD FRAME - Using absolute positioning to draw the sci-fi border */
    .cyber-card {
        position: relative;
        width: 100%;
        height: auto; /* Allow growth with content as requested */
        min-height: 450px; /* Reduced min-height to fit smaller viewports better */
        /* No simple borders anymore, we build them */
        display: flex;
        flex-direction: column;
    }

    /* REMOVED OLD PSEUDO BORDERS */
    .cyber-card::before,
    .cyber-card::after {
        display: none;
    }

    /* --- SIDE BORDERS COMPLEX --- */
    .border-complex {
        position: absolute;
        top: 0;
        bottom: 0;
        width: 10px;
        display: flex;
        flex-direction: column;
        pointer-events: none;
        z-index: 20; /* Ensure borders are above content */
    }
    .border-complex.left {
        left: 0;
        align-items: flex-start;
    }
    .border-complex.right {
        right: 0;
        align-items: flex-end;
    }

    /* Line Segments */
    .line-segment {
        width: 2px;
        background: var(--wow-card-border);
        box-shadow: 0 0 5px var(--wow-card-glow);
        position: relative;
    }
    .border-complex.left .line-segment {
        margin-right: auto;
    } /* Align left */
    .border-complex.right .line-segment {
        margin-left: auto;
    } /* Align right */

    /* Specific Segments */
    .border-complex .top {
        height: 150px;
    }

    .border-complex .middle-thick {
        height: 100px;
        width: 4px; /* Thicker section */
        background: linear-gradient(
            180deg,
            var(--wow-card-border),
            transparent
        );
        opacity: 0.8;
        margin-top: 20px;
    }

    .border-complex .bottom {
        flex: 1; /* Fills rest */
        margin-top: 20px;
        margin-bottom: 20px; /* Space for corner */
    }

    /* Gap Markers (Little triangles or dots in gaps) */
    .gap-marker {
        height: 6px;
        width: 6px;
        background: var(--wow-card-border);
        margin: 10px 0;
        clip-path: polygon(0 0, 100% 50%, 0 100%); /* Triangle */
    }

    /* Ruler Marks (Right Side) */
    .ruler-marks {
        position: absolute;
        top: 40px;
        right: 5px; /* Inside the border line */
        display: flex;
        flex-direction: column;
        gap: 4px;
        align-items: flex-end;
    }
    .ruler-marks .mark {
        height: 2px;
        background: var(--wow-card-border);
        opacity: 0.6;
    }
    /* Randomize lengths via CSS logic is hard, so we use fixed pattern or nth-child */
    .ruler-marks .mark:nth-child(odd) {
        width: 8px;
    }
    .ruler-marks .mark:nth-child(even) {
        width: 4px;
        opacity: 0.3;
    }

    /* --- TOP AREA UPDATES - INLINE REFACTOR --- */
    .card-top-border {
        position: absolute;
        top: -15px; /* Aligned closer to the edge */
        left: 0;
        right: 0;
        height: 30px;
        display: flex;
        align-items: center; /* Center everything vertically */
        z-index: 20;
        pointer-events: none;
    }
    .top-structure {
        width: 100%;
        display: flex;
        align-items: center; /* Inline alignment */
        justify-content: space-between;
        position: relative;
        padding: 0 10px;
    }

    /* Tech Labels - Inline with border */
    .tech-label {
        font-family: monospace;
        font-size: 10px;
        color: var(--wow-card-border);
        opacity: 0.9;
        letter-spacing: 2px;
        background: var(--wow-card-bg); /* Mask line behind */
        padding: 0 8px;
        border: 1px solid var(--wow-card-border);
        height: 16px;
        display: flex;
        align-items: center;
        box-shadow: 0 0 5px var(--wow-card-glow);
        z-index: 2; /* Sit on top of line */
        margin: 0; /* Remove old margins */
    }
    /* Specific positioning */
    .tech-label.left {
        margin-right: 10px;
    }
    .tech-label.right {
        margin-left: 10px;
    }

    /* Corner Blocks */
    .corner-block {
        width: 20px;
        height: 20px; /* Square blocks */
        background: transparent;
        border: 2px solid var(--wow-card-border);
        box-shadow: 0 0 8px var(--wow-card-glow);
        position: relative;
    }
    /* Add details to blocks */
    .corner-block::after {
        content: "";
        position: absolute;
        width: 6px;
        height: 6px;
        background: var(--wow-card-border);
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
    }

    /* Structured Top Stream Line */
    /* --- TOP LINE SEGMENTATION --- */
    .top-line-complex {
        flex: 1;
        display: flex;
        align-items: center;
        height: 4px;
        position: relative;
        margin: 0 10px; /* Space from labels */
    }

    .line-segment-h {
        height: 2px;
        background: var(--wow-card-border);
        box-shadow: 0 0 5px var(--wow-card-glow);
        position: relative;
    }

    .line-segment-h.start,
    .line-segment-h.end {
        flex: 1; /* These take up remaining space */
        opacity: 0.6;
    }

    .line-segment-h.mid-thick {
        width: 100px;
        height: 4px; /* Thicker center */
        background: linear-gradient(
            90deg,
            var(--wow-card-border),
            rgba(0, 243, 255, 0.4),
            var(--wow-card-border)
        );
        box-shadow: 0 0 8px var(--wow-card-glow);
    }

    /* Horizontal Gap Markers */
    .gap-marker-h {
        width: 6px;
        height: 6px;
        background: var(--wow-card-border);
        margin: 0 5px;
        clip-path: polygon(
            50% 0%,
            0% 100%,
            100% 100%
        ); /* Triangle pointing up */
    }

    /* Optional: Overlay Dashes on thin segments */
    .line-segment-h.start::after,
    .line-segment-h.end::after {
        content: "";
        position: absolute;
        top: -6px;
        left: 0;
        right: 0;
        height: 1px;
        background: repeating-linear-gradient(
            90deg,
            var(--wow-card-border) 0,
            var(--wow-card-border) 2px,
            transparent 2px,
            transparent 10px
        );
        opacity: 0.4;
    }

    /* Central Element - Simple Diamond */

    /* Hiding old module classes to clean up */
    .top-center-module,
    .module-content,
    .status-dot,
    .system-text {
        display: none;
    }

    /* --- BOTTOM AREA --- */
    .card-bottom-border {
        position: absolute;
        bottom: -35px;
        left: 0;
        right: 0;
        height: 50px;
        display: flex;
        justify-content: center;
        align-items: flex-start;
    }

    .bottom-structure {
        width: 100%;
        display: flex;
        align-items: flex-start;
        position: relative;
    }

    /* Side decorations on bottom */
    .bottom-corner-decor {
        width: 80px;
        height: 15px;
        background: var(--wow-card-border);
        position: relative;
        /* Technical angled shape */
        clip-path: polygon(0 0, 100% 0, 85% 100%, 15% 100%);
        box-shadow: 0 0 10px var(--wow-card-glow);
    }
    .bottom-corner-decor.left {
        margin-right: 10px;
    }
    .bottom-corner-decor.right {
        margin-left: 10px;
    }

    /* The glowing "status" texts inside corners */
    .tiny-tech {
        position: absolute;
        top: 2px;
        left: 50%;
        transform: translateX(-50%);
        font-size: 8px;
        font-family: monospace;
        color: var(--wow-card-bg);
        font-weight: bold;
        letter-spacing: 1px;
    }

    /* Central bottom complexity */
    .bottom-line {
        flex: 1;
        height: 2px;
        background: var(--wow-card-border);
        box-shadow: 0 0 5px var(--wow-card-glow);
        position: relative;
        margin-top: 5px; /* Alignment */
    }

    /* Double line effect */
    .bottom-line::after {
        content: "";
        position: absolute;
        top: 6px;
        left: 10px;
        right: 10px;
        height: 1px;
        background: var(--wow-card-border);
        opacity: 0.4;
    }

    /* Central Lock/Status Icon */
    .bottom-center-status {
        position: absolute;
        bottom: -25px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 2px;
    }
    .status-light {
        width: 40px;
        height: 4px;
        background: var(--wow-cyan);
        box-shadow: 0 0 10px var(--wow-cyan);
    }
    .bottom-code {
        font-size: 9px;
        font-family: monospace;
        color: rgba(255, 255, 255, 0.4);
        letter-spacing: 2px;
    }

    /* Content Padding - Retain verification */
    .card-content-area {
        padding: 40px 10px 10px;
        width: 100%;
        box-sizing: border-box;
        position: relative;
        z-index: 10;
        display: flex;
        flex-direction: column;
        align-items: center;
    }

    /* Responsive adjustments */
    @media (max-width: 768px) {
        .ruler-marks {
            display: none;
        }
        .tech-label {
            display: none;
        }
        .border-complex {
            width: 4px;
        }
        .gap-marker {
            display: none;
        }
        .corner-block {
            width: 25px;
        }
    }
</style>
