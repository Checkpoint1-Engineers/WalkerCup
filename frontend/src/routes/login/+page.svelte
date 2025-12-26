<script>
    import { goto } from "$app/navigation";
    import CyberButton from "$lib/components/CyberButton.svelte";
    import { scramble } from "$lib/actions/scramble.js";
    import { api } from "$lib/api.js";
    import { auth } from "$lib/stores/auth.js";

    let email = ""; // Changed from username to email per backend request
    let password = "";
    let isLoading = false;
    /** @type {string | null} */
    let error = null;

    function validateInputs() {
        // Email Regex
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!email) throw new Error("EMAIL REQUIRED");
        if (!emailRegex.test(email)) throw new Error("INVALID EMAIL FORMAT");

        if (!password) throw new Error("PASSWORD REQUIRED");
        if (password.length < 6)
            throw new Error("PASSWORD TOO SHORT (MIN 6 CHARS)");
    }

    function parseJwt(token) {
        try {
            const base64Url = token.split(".")[1];
            const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
            const jsonPayload = decodeURIComponent(
                window
                    .atob(base64)
                    .split("")
                    .map(function (c) {
                        return (
                            "%" +
                            ("00" + c.charCodeAt(0).toString(16)).slice(-2)
                        );
                    })
                    .join(""),
            );
            return JSON.parse(jsonPayload);
        } catch (e) {
            return {};
        }
    }

    async function handleLogin() {
        error = null;
        isLoading = true;

        try {
            // Validate input
            validateInputs();

            const { token, expiresAt } = await api.auth.login(email, password);

            // Extract role from token
            const payload = parseJwt(token);
            // .NET Identity often uses this long URL for role claim
            const role =
                payload[
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                ] ||
                payload.role ||
                "Walker";

            // Update auth store
            auth.login(token, { email, role }); // Storing user info including role

            // Redirect on success
            // If admin, go to admin dashboard
            if (role === "TeamAlan") {
                goto("/admin");
            } else {
                goto("/");
            }
        } catch (e) {
            console.error("Login error:", e);
            error = e.message || "ACCESS_DENIED";
        } finally {
            isLoading = false;
        }
    }
</script>

<div class="login-wrapper">
    <div class="login-container">
        <!-- Header / Logo Area -->
        <div class="login-header">
            <h1 class="term-title" use:scramble={{ chars: "ACCESS_CONTROL" }}>
                ACCESS_CONTROL
            </h1>
            <p class="subtitle">RESTRICTED AREA . AUTHORIZED PERSONNEL ONLY</p>
        </div>

        <!-- Form -->
        <form
            on:submit|preventDefault={handleLogin}
            class="login-form"
            novalidate
        >
            {#if error}
                <div class="error-banner">
                    <span class="error-icon">⚠</span>
                    <span class="error-text">ERROR: {error}</span>
                </div>
            {/if}

            <div class="input-group">
                <label for="email">EMAIL_ADDRESS</label>
                <div class="input-wrapper">
                    <input
                        type="email"
                        id="email"
                        bind:value={email}
                        placeholder="ENTER EMAIL..."
                        autocomplete="email"
                        class:has-error={!!error}
                    />
                    <div class="input-decor"></div>
                </div>
            </div>

            <div class="input-group">
                <label for="password">SECURITY_KEY</label>
                <div class="input-wrapper">
                    <input
                        type="password"
                        id="password"
                        bind:value={password}
                        placeholder="ENTER KEY..."
                        autocomplete="current-password"
                        class:has-error={!!error}
                    />
                    <div class="input-decor"></div>
                </div>
            </div>

            <div class="form-actions">
                <CyberButton
                    type="submit"
                    variant="primary"
                    label={isLoading ? "AUTHENTICATING..." : "INITIATE SESSION"}
                />
            </div>
        </form>

        <!-- Footer / Status -->
        <div class="login-footer">
            <div class="status-line">
                <span class="status-dot"></span>
                <span class="status-text">SECURE CONNECTION ESTABLISHED</span>
            </div>
            <div class="encryption-key">ENC: AES-256-GCM</div>
        </div>
    </div>
</div>

<style>
    .login-wrapper {
        min-height: 60vh;
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 2rem;
    }

    .login-container {
        width: 100%;
        max-width: 450px;
        position: relative;
        display: flex;
        flex-direction: column;
        gap: 2.5rem;
    }

    .login-header {
        text-align: center;
    }

    .term-title {
        font-family: var(--wow-font-display);
        font-size: 2rem;
        color: #fff;
        margin: 0;
        letter-spacing: 0.1em;
        text-shadow: 0 0 20px rgba(0, 243, 255, 0.5);
    }

    .subtitle {
        font-family: var(--wow-font-body);
        font-size: 0.75rem;
        color: var(--wow-primary);
        letter-spacing: 0.2em;
        margin-top: 0.5rem;
        opacity: 0.8;
    }

    /* Form Styles */
    .login-form {
        display: flex;
        flex-direction: column;
        gap: 2rem;
    }

    .input-group {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    label {
        font-family: var(--wow-font-display);
        font-size: 0.8rem;
        color: var(--wow-text-muted);
        letter-spacing: 0.1em;
    }

    .input-wrapper {
        position: relative;
        width: 100%;
        isolation: isolate;
    }

    input {
        width: 100%;
        background: rgba(0, 12, 20, 0.6);
        border: 1px solid rgba(0, 243, 255, 0.2);
        padding: 1rem 1.25rem;
        color: #fff;
        font-family: var(--wow-font-body);
        letter-spacing: 0.05em;
        outline: none;
        transition: all 0.3s ease;
        box-sizing: border-box;
    }

    input:focus {
        border-color: var(--wow-cyan);
        box-shadow: 0 0 15px rgba(0, 243, 255, 0.1);
        background: rgba(0, 12, 20, 0.8);
    }

    /* Input corner decoration */
    .input-decor {
        position: absolute;
        bottom: 0;
        right: 0;
        width: 10px;
        height: 10px;
        border-bottom: 2px solid var(--wow-cyan);
        border-right: 2px solid var(--wow-cyan);
        pointer-events: none;
        opacity: 0.5;
        transition: opacity 0.3s ease;
    }

    input:focus + .input-decor {
        opacity: 1;
    }

    .form-actions {
        display: flex;
        justify-content: center;
        margin-top: 1rem;
    }

    /* Error States */
    .error-banner {
        background: rgba(255, 0, 60, 0.1);
        border: 1px solid rgba(255, 0, 60, 0.4);
        padding: 0.75rem;
        display: flex;
        align-items: center;
        gap: 0.75rem;
        color: #ff003c;
        font-family: var(--wow-font-display);
        font-size: 0.8rem;
        animation: shake 0.4s ease-in-out;
    }

    input.has-error {
        border-color: rgba(255, 0, 60, 0.6);
        background: rgba(255, 0, 60, 0.05);
    }

    @keyframes shake {
        0%,
        100% {
            transform: translateX(0);
        }
        25% {
            transform: translateX(-5px);
        }
        75% {
            transform: translateX(5px);
        }
    }

    /* Footer */
    .login-footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        border-top: 1px solid rgba(255, 255, 255, 0.1);
        padding-top: 1rem;
        font-family: monospace;
        font-size: 0.7rem;
        color: var(--wow-text-muted);
    }

    .status-line {
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }

    .status-dot {
        width: 6px;
        height: 6px;
        background: #0f0;
        border-radius: 50%;
        box-shadow: 0 0 5px #0f0;
        animation: pulse 2s infinite;
    }

    @keyframes pulse {
        0%,
        100% {
            opacity: 1;
        }
        50% {
            opacity: 0.3;
        }
    }
</style>
