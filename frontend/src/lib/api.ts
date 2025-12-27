const API_BASE = 'http://localhost:5270/api';

interface Tournament {
	id: string;
	name: string;
	status: number;
	imageUrl?: string;
	joinDeadline: string;
	entryFee?: number;
	prizePool?: number;
	maxParticipants?: number;
}

async function request<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
	const res = await fetch(`${API_BASE}${endpoint}`, options);
	if (!res.ok) {
		throw new Error(`API Error: ${res.status}`);
	}
	return res.json();
}

export const api = {
	tournaments: {
		list: () => request<Tournament[]>('/tournaments'),
		get: (id: string) => request<Tournament>(`/tournaments/${id}`)
	}
};

export type { Tournament };
