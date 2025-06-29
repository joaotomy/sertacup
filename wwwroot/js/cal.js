document.addEventListener("DOMContentLoaded", () => {
    const isCalendario = document.getElementById("calendario");
    if (!isCalendario) return;

    const dayHeader = document.getElementById('calendar-day');
    const timeHeader = document.querySelector('.cal-time-header');
    const allDays = document.querySelectorAll('.cal-day');
    const allBlocks = document.querySelectorAll('.cal-time-block');
    const allGames = document.querySelectorAll('.cal-game');

    const now = new Date();
    let currentDay = "";
    let currentTime = "";

    // Sticky Headers
    window.addEventListener('scroll', () => {
        let found = false;

        for (let i = allDays.length - 1; i >= 0; i--) {
            const rect = allDays[i].getBoundingClientRect();
            if (rect.top <= 140) {
                const newHeader = allDays[i].querySelector('.cal-fixed-header')?.textContent;
                if (newHeader && newHeader !== currentDay) {
                    dayHeader.style.opacity = 0;
                    setTimeout(() => {
                        dayHeader.textContent = newHeader;
                        dayHeader.style.opacity = 1;
                    }, 150);
                    currentDay = newHeader;
                }
                found = true;
                break;
            }
        }

        if (!found && currentDay) {
            dayHeader.style.opacity = 0;
            setTimeout(() => {
                dayHeader.textContent = "";
                dayHeader.style.opacity = 1;
            }, 150);
            currentDay = "";
        }

        let foundBlock = false;
        for (let i = 0; i < allBlocks.length; i++) {
            const block = allBlocks[i];
            const rect = block.getBoundingClientRect();
            if (rect.top <= 160) {
                const newHour = block.getAttribute('data-hour');
                if (newHour && newHour !== currentTime) {
                    const local = block.querySelector('.cal-time')?.textContent || "";
                    const phase = block.querySelector('.cal-phase')?.textContent || "";
                    const tagContent = block.querySelector('.cal-livetag')?.cloneNode(true);
                    const targetTag = timeHeader.querySelector('.cal-livetag');
                    timeHeader.style.opacity = 0;
                    setTimeout(() => {
                        timeHeader.querySelector('.cal-time').textContent = local;
                        timeHeader.querySelector('.cal-phase').textContent = phase;
                        if (tagContent && targetTag) {
                            targetTag.innerHTML = tagContent.innerHTML;
                            targetTag.classList.toggle("visible", tagContent.classList.contains("visible"));
                        }
                        timeHeader.style.opacity = 1;
                    }, 150);
                    currentTime = newHour;
                }
                foundBlock = true;
            }
        }

        if (!foundBlock && currentTime !== "") {
            currentTime = "";
            timeHeader.querySelector('.cal-time').textContent = "";
            timeHeader.querySelector('.cal-phase').textContent = "";
            timeHeader.querySelector('.cal-livetag')?.classList.remove("visible");
        }
    });

    const previousEstados = new Map();

    window.toggleScorers = function (el) {
        el.classList.toggle("expanded");
    };

    function highlightLiveBlocks() {
        allBlocks.forEach(block => {
            const liveGames = block.querySelectorAll(".cal-game.live");
            const tag = block.querySelector(".cal-livetag");
            tag?.classList.toggle("visible", liveGames.length > 0);
            block.classList.toggle("live-block", liveGames.length > 0);
        });
    }

    function applyGameStateClasses() {
        allGames.forEach(game => {
            const estado = game.dataset.estado?.trim();
            const clock = game.querySelector(".cal-game-clock");
            const status = game.querySelector(".cal-game-status");
            if (!estado || !clock || !status) return;
            if (previousEstados.get(game) === estado) return;
            previousEstados.set(game, estado);
            game.classList.remove("live", "finished", "atrasado", "pen");
            clock.classList.remove("live-blink", "interval-blink", "pen-blink", "hidden");
            status.classList.remove("interval-blink", "pen-blink");
            if (estado === "1ªP" || estado === "2ªP") {
                game.classList.add("live");
                clock.classList.remove("hidden");
                clock.classList.add("live-blink");
            } else if (estado === "Intervalo") {
                game.classList.add("live");
                clock.classList.add("hidden");
                status.classList.add("interval-blink");
            } else if (estado === "Resultado Final") {
                game.classList.add("finished");
                clock.classList.add("hidden");
            } else if (estado.startsWith("Em Atraso")) {
                game.classList.add("atrasado");
                clock.classList.add("hidden");
            } else {
                clock.classList.add("hidden");
            }
        });

        highlightLiveBlocks();

        allGames.forEach(game => {
            const g1 = parseInt(game.dataset.g1 || "0", 10);
            const g2 = parseInt(game.dataset.g2 || "0", 10);
            if (g1 + g2 > 0) {
                game.addEventListener("click", () => toggleScorers(game));
            }
        });
    }

    window.applyGameStateClasses = applyGameStateClasses;

    function startLiveCheckLoop() {
        setInterval(() => {
            applyGameStateClasses();
        }, 15000);
    }
    setTimeout(() => {
        const calendarioTab = document.querySelector("#calendario.tab-content.active");
        if (calendarioTab) {
            const firstLiveGame = calendarioTab.querySelector(".cal-game.live");
            if (firstLiveGame) {
                firstLiveGame.scrollIntoView({ behavior: "smooth", block: "center" });
            }
        }
    }, 100);

    applyGameStateClasses();
    startLiveCheckLoop();
});
