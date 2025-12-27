import { api } from '$lib/api';

export async function load() {
	const tournaments = await api.tournaments.list();
	return {
		tournaments: tournaments.filter((t) => t.status === 3)
	};
}
