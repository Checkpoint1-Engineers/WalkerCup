import { api, type Tournament } from '$lib/api';

export async function load() {
	const tournaments = await api.tournaments.list();
	return {
		tournaments: tournaments.filter((t: Tournament) => t.status === 4)
	};
}
