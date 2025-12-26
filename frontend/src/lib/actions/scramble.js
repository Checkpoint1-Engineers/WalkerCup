/**
 * Enhanced Scramble Text Action
 * Applies a "decryption" effect to text on hover or trigger.
 * 
 * Features:
 * - Uses requestAnimationFrame for native 60fps smoothness.
 * - Preserves whitespace to maintain layout stability.
 * - Accessibility: Sets aria-label so screen readers read the real text, not the gibberish.
 * - Handles rapid hover/unhover states gracefully.
 * 
 * Usage: <span use:scramble={{ speed: 0.5, chars: '01' }}>TEXT</span>
 */
export function scramble(node, params = {}) {
    // Configuration
    const defaults = {
        speed: 0.5, // Characters revealed per frame (lower = slower)
        chars: "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*",
        autoStart: false,
        preserveSpace: true
    };

    let options = { ...defaults, ...params };
    let originalText = node.innerText || node.textContent; // Fallback
    let frameId = null;
    let iteration = 0;
    let running = false;

    // Accessibility: Ensure the original text is readable
    node.setAttribute('aria-label', originalText);
    node.setAttribute('role', 'text');

    // Store original text to prevent drift if called multiple times
    if (!node.dataset.originalText) {
        node.dataset.originalText = originalText;
    } else {
        originalText = node.dataset.originalText;
    }

    function update(newParams) {
        options = { ...defaults, ...newParams };
        // If text content changed externally (unlikely but possible), update our ref
        if (node.dataset.originalText !== (node.innerText || node.textContent)) {
            // Optional: Handle dynamic text updates if needed
        }
    }

    // Core Animation Loop
    function loop() {
        if (!running) return;

        let currentText = "";

        for (let i = 0; i < originalText.length; i++) {
            // If we've passed this index, show the real character
            if (i < Math.floor(iteration)) {
                currentText += originalText[i];
            }
            // If it's a space and we preserve it, show space
            else if (options.preserveSpace && originalText[i] === ' ') {
                currentText += " ";
            }
            // Otherwise show a random character
            else {
                currentText += options.chars[Math.floor(Math.random() * options.chars.length)];
            }
        }

        node.innerText = currentText;

        if (iteration >= originalText.length) {
            running = false;
        } else {
            iteration += options.speed;
            frameId = requestAnimationFrame(loop);
        }
    }

    function startScramble() {
        if (running) return; // Already running
        running = true;
        iteration = 0;
        cancelAnimationFrame(frameId);
        frameId = requestAnimationFrame(loop);
    }

    function reset() {
        running = false;
        cancelAnimationFrame(frameId);
        node.innerText = originalText;
    }

    // Event Listeners
    const onEnter = () => startScramble();
    const onLeave = () => {
        // Optional: Reset immediately on leave, or let it finish?
        // Let's reset to ensure it's clean for next time, or we could let it finish.
        // For 'tech' feel, usually letting it finish is better, 
        // OR we can leave it at 'scrambled' state? 
        // Standard UX is ensure readability:
        reset();
    };

    if (options.autoStart) {
        startScramble();
    } else {
        node.addEventListener('mouseenter', onEnter);
        node.addEventListener('mouseleave', onLeave);
    }

    return {
        update,
        destroy() {
            cancelAnimationFrame(frameId);
            node.removeEventListener('mouseenter', onEnter);
            node.removeEventListener('mouseleave', onLeave);
            // Restore text ensures cleanliness
            node.innerText = originalText;
        }
    };
}
