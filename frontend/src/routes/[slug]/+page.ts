import { api } from '$lib/api';
import { error } from '@sveltejs/kit';

export async function load({ params }) {
	const tournaments = await api.tournaments.list();
	const tournament = tournaments.find((t) => t.name === decodeURIComponent(params.slug));

	if (!tournament) {
		throw error(404, 'Tournament not found');
	}

	return { tournament };
}
