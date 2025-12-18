const API_BASE = 'http://localhost:5270/api';

async function request(endpoint, options = {}) {
    const res = await fetch(`${API_BASE}${endpoint}`, options);
    if (!res.ok) {
        throw new Error(`API Error: ${res.status}`);
    }
    return res.json();
}

export const api = {
    tournaments: {
        list: () => request('/tournaments'),
        get: (id) => request(`/tournaments/${id}`)
    }
};
