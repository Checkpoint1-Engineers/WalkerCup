import { writable } from 'svelte/store';

export const curRoute = writable(window.location.pathname);

export const navigate = (href) => {
    window.history.pushState({}, '', href);
    curRoute.set(href);
};

window.onpopstate = () => {
    curRoute.set(window.location.pathname);
};
