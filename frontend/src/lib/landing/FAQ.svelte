<script>
    import { onMount } from "svelte";

    let visible = false;
    let section;

    const faqs = [
        {
            q: "WHAT IS THE WALKER PROTOCOL?",
            a: "The Walker Protocol is a decentralized initiative to identify the world's top coding talents. We use competitive tournaments to filter out the noise and find the signal.",
        },
        {
            q: "IS MEMBERSHIP FREE?",
            a: "Yes. The Resistance does not charge for entry. We only ask for your loyalty and your skill.",
        },
        {
            q: "HOW DO I RECRUIT A TEAM?",
            a: "Once you have proven yourself in the solo trials, you will unlock the Recruit module to form your own squad.",
        },
    ];

    let openIndex = 0;

    function toggle(i) {
        openIndex = openIndex === i ? -1 : i;
    }

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

<section class="faq-section" bind:this={section} class:visible>
    <h2 class="section-title">
        FREQUENTLY ASKED <span class="highlight">QUESTIONS</span>
    </h2>

    <div class="faq-grid">
        {#each faqs as faq, i}
            <div
                class="faq-item"
                class:open={openIndex === i}
                on:click={() => toggle(i)}
            >
                <div class="question">
                    {faq.q}
                    <span class="indicator">{openIndex === i ? "-" : "+"}</span>
                </div>
                {#if openIndex === i}
                    <div class="answer">
                        <p>{faq.a}</p>
                    </div>
                {/if}
            </div>
        {/each}
    </div>
</section>

<style>
    .faq-section {
        padding: 6rem 2rem 10rem; /* Bottom padding for footer space */
        max-width: 1000px;
        margin: 0 auto;
        opacity: 0;
        transform: translateY(30px);
        transition:
            opacity 1s ease,
            transform 1s ease;
    }
    .visible {
        opacity: 1;
        transform: translateY(0);
    }

    .section-title {
        text-align: center;
        font-family: var(--wow-font-display);
        color: #fff;
        font-size: 3rem;
        margin-bottom: 4rem;
    }
    .highlight {
        color: var(--wow-cyan);
    }

    .faq-item {
        background: rgba(255, 255, 255, 0.03);
        border: 1px solid rgba(255, 255, 255, 0.1);
        margin-bottom: 1rem;
        border-radius: 4px;
        overflow: hidden;
        cursor: pointer;
        transition: all 0.3s;
    }
    .faq-item:hover {
        background: rgba(255, 255, 255, 0.05);
        border-color: var(--wow-cyan);
    }
    .faq-item.open {
        background: rgba(0, 243, 255, 0.05);
        border-color: var(--wow-cyan);
    }

    .question {
        padding: 1.5rem 2rem;
        font-weight: bold;
        color: #fff;
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-family: var(--wow-font-display);
        letter-spacing: 1px;
    }
    .indicator {
        color: var(--wow-cyan);
        font-size: 1.5rem;
    }

    .answer {
        padding: 0 2rem 1.5rem;
        color: #aaa;
        line-height: 1.6;
        animation: slideDown 0.3s ease;
    }

    @keyframes slideDown {
        from {
            opacity: 0;
            transform: translateY(-10px);
        }
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
</style>
