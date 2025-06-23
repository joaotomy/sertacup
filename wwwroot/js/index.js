function updateScrollingTeamNames() {
    const teamNames = document.querySelectorAll('.team-name span');

    teamNames.forEach(span => {
        const container = span.parentElement;
        span.classList.remove('scrolling-text');
        void span.offsetWidth;

        if (span.scrollWidth > container.clientWidth) {
            span.classList.add('scrolling-text');
        }
    });
}

['load', 'resize'].forEach(e =>
    window.addEventListener(e, updateScrollingTeamNames)
);

new MutationObserver(updateScrollingTeamNames).observe(document.body, {
    childList: true,
    subtree: true,
    characterData: true
});

function toggleLiveGridScroll() {
    const liveGrid = document.querySelector('.live-grid');
    if (!liveGrid) return;

    const totalWidth = Array.from(liveGrid.children)
        .reduce((sum, card) => sum + card.offsetWidth + parseFloat(getComputedStyle(card).marginRight || 0), 0);

    const containerWidth = liveGrid.offsetWidth;

    if (totalWidth <= containerWidth + 1) {
        liveGrid.classList.add('no-scroll');
    } else {
        liveGrid.classList.remove('no-scroll');
    }
}

['load', 'resize'].forEach(e =>
    window.addEventListener(e, toggleLiveGridScroll)
);
