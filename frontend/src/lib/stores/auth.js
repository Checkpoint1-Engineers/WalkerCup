import { writable } from 'svelte/store';
import { browser } from '$app/environment';

// Initialize from localStorage if available
const storedToken = browser ? localStorage.getItem('walker_token') : null;
const storedUser = browser ? JSON.parse(localStorage.getItem('walker_user') || 'null') : null;

function createAuthStore() {
    const { subscribe, set, update } = writable({
        token: storedToken,
        user: storedUser,
        isAuthenticated: !!storedToken
    });

    return {
        subscribe,
        login: (token, user) => {
            if (browser) {
                localStorage.setItem('walker_token', token);
                // In a real app, you might decode the JWT to get user info if not provided separate
                // For now we'll store what we get or a placeholder
                localStorage.setItem('walker_user', JSON.stringify(user || {}));
            }
            set({ token, user, isAuthenticated: true });
        },
        logout: () => {
            if (browser) {
                localStorage.removeItem('walker_token');
                localStorage.removeItem('walker_user');
            }
            set({ token: null, user: null, isAuthenticated: false });
        }
    };
}

export const auth = createAuthStore();
