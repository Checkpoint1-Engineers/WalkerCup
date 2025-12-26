import { get } from 'svelte/store';
import { auth } from '$lib/stores/auth';

const API_BASE = 'http://localhost:5270/api';

async function request(endpoint, options = {}) {
    const headers = { ...options.headers };

    // Automatically set Content-Type for JSON bodies
    if (options.body && typeof options.body === 'string') {
        headers['Content-Type'] = 'application/json';
    }

    // Add auth token if available
    const { token } = get(auth);
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    const config = {
        ...options,
        headers
    };

    const res = await fetch(`${API_BASE}${endpoint}`, config);

    if (!res.ok) {
        // Try to parse error message from backend
        let errorMessage = `API Error: ${res.status}`;
        try {
            const errorData = await res.json();
            if (errorData.message) errorMessage = errorData.message;
            if (errorData.error) errorMessage = errorData.error;
        } catch (e) {
            // Ignore JSON parse error if body is empty or not JSON
        }
        throw new Error(errorMessage);
    }
    return res.json();
}

export const api = {
    auth: {
        login: (email, password) => request('/auth/login', {
            method: 'POST',
            body: JSON.stringify({ email, password })
        })
    },
    tournaments: {
        list: () => request('/tournaments'),
        get: (id) => request(`/tournaments/${id}`),
        create: (data) => request('/tournaments', {
            method: 'POST',
            body: JSON.stringify(data)
        }),
        open: (id) => request(`/tournaments/${id}/open`, { method: 'POST' }),
        lock: (id) => request(`/tournaments/${id}/lock`, { method: 'POST' }),
        draw: (id) => request(`/tournaments/${id}/draw`, { method: 'POST' }),
        extend: (id, newDeadline) => request(`/tournaments/${id}/extend`, {
            method: 'POST',
            body: JSON.stringify({ newDeadline })
        }),
        join: (tournamentId, walkerId, walkerName, email) => request(`/tournaments/${tournamentId}/join`, {
            method: 'POST',
            body: JSON.stringify({ walkerId, walkerName, email })
        }),
        removeMember: (tournamentId, walkerId) => request(`/tournaments/${tournamentId}/members/${walkerId}`, {
            method: 'DELETE'
        })
    },
    matches: {
        setWinner: (id, winnerId) => request(`/matches/${id}/winner`, {
            method: 'POST',
            body: JSON.stringify({ winnerId })
        })
    }
};
