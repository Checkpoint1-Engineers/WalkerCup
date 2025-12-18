<script>
    import { onMount } from "svelte";

    let visible = false;
    let section;

    const features = [
        {
            title: "ELITE TOURNAMENTS",
            desc: "Compete in high-stakes coding brackets. Only the most efficient algorithms survive.",
            icon: "M13 10V3L4 14h7v7l9-11h-7z", // Lightning
        },
        {
            title: "GLOBAL RANKINGS",
            desc: "Ascend the leaderboard. Prove to the world that you are the apex predator.",
            icon: "M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 002 2h2a2 2 0 002-2z", // Graph
        },
        {
            title: "CIPHER REWARDS",
            desc: "Earn crypto bounties and rare NFT artifacts for your victories.",
            icon: "M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z", // Coin
        },
    ];

    onMount(() => {
        const observer = new IntersectionObserver(
            (entries) => {
                if (entries[0].isIntersecting) visible = true;
            },
            { threshold: 0.2 },
        );
        if (section) observer.observe(section);
        return () => observer.disconnect();
    });
</script>

<section class="features-section" bind:this={section} class:visible>
    <div class="grid">
        {#each features as feature, i}
            <div class="feature-card" style="transition-delay: {i * 200}ms">
                <div class="icon-wrap">
                    <svg
                        viewBox="0 0 24 24"
                        class="icon"
                        fill="none"
                        stroke="currentColor"
                        stroke-width="1.5"
                    >
                        <path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            d={feature.icon}
                        />
                    </svg>
                    <div class="icon-glow"></div>
                </div>
                <h3>{feature.title}</h3>
                <p>{feature.desc}</p>
            </div>
        {/each}
    </div>
</section>

<style>
    .features-section {
        padding: 6rem 2rem;
        max-width: 1400px;
        margin: 0 auto;
    }

    .grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 3rem;
    }

    .feature-card {
        background: rgba(255, 255, 255, 0.02);
        border: 1px solid rgba(255, 255, 255, 0.05);
        padding: 3rem 2rem;
        border-radius: 12px;
        text-align: center;
        transition:
            transform 0.3s,
            background 0.3s,
            opacity 0.8s,
            transform 0.8s;
        opacity: 0;
        transform: translateY(30px);
        position: relative;
        overflow: hidden;
    }
    .visible .feature-card {
        opacity: 1;
        transform: translateY(0);
    }

    .feature-card:hover {
        transform: translateY(-10px);
        background: rgba(255, 255, 255, 0.05);
        border-color: rgba(0, 243, 255, 0.3);
    }

    .icon-wrap {
        width: 80px;
        height: 80px;
        background: rgba(0, 0, 0, 0.3);
        border-radius: 50%;
        margin: 0 auto 2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        color: var(--wow-cyan);
    }
    .icon {
        width: 40px;
        height: 40px;
        z-index: 2;
    }
    .icon-glow {
        position: absolute;
        inset: 0;
        border-radius: 50%;
        background: var(--wow-cyan);
        filter: blur(20px);
        opacity: 0;
        transition: opacity 0.3s;
    }
    .feature-card:hover .icon-glow {
        opacity: 0.4;
    }

    h3 {
        color: #fff;
        font-family: var(--wow-font-display);
        font-size: 1.5rem;
        margin-bottom: 1rem;
        letter-spacing: 1px;
    }
    p {
        color: #888;
        line-height: 1.5;
    }
</style>
