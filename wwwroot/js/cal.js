const isCalendario = document.getElementById('calendario');
    if (!isCalendario) return;

    const dayHeader = document.getElementById('calendar-day');
    const timeHeader = document.querySelector('.cal-time-header');
    const allDays = document.querySelectorAll('.cal-day');
    const allBlocks = document.querySelectorAll('.cal-time-block');
    const allGames = document.querySelectorAll('.cal-game');

    const showPastBtn = document.getElementById("show-past-games");
    const backToLiveBtn = document.getElementById("back-to-live");

    const now = new Date();
    let currentDay = "";
    let currentTime = "";

    function hidePastDays() {
        allDays.forEach(day => {
            const games = day.querySelectorAll('.cal-game');
            const anyFuture = Array.from(games).some(game => {
                const estado = game.dataset.estado?.trim();
                const start = new Date(game.dataset.start);
                return (
                    estado === "1ªP" || estado === "2ªP" || estado === "Intervalo" || (start >= now)
                );
            });
            if (!anyFuture) {
                day.classList.add("hidden-day");
            }
        });
        showPastBtn.classList.remove("hidden");
        backToLiveBtn.classList.add("hidden");
    }

    function showAllDays() {
        document.querySelectorAll(".cal-day.hidden-day").forEach(d => d.classList.remove("hidden-day"));
        showPastBtn.classList.add("hidden");
        backToLiveBtn.classList.remove("hidden");
    }

    function scrollBackToLive() {
        const target = document.querySelector(".cal-game.live");
        if (target) {
            target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
        showPastBtn.classList.remove("hidden");
        backToLiveBtn.classList.add("hidden");
        hidePastDays();
    }

    showPastBtn.addEventListener("click", showAllDays);
    backToLiveBtn.addEventListener("click", scrollBackToLive);

    // Auto Scroll to Live or Closest
    let scrollTarget = null;
    let closestDiff = Infinity;

    allGames.forEach(game => {
        const startTime = new Date(game.dataset.start);
        const diffMin = (startTime - now) / 60000;
        if (diffMin <= 0 && diffMin >= -50 && !scrollTarget) {
            scrollTarget = game;
        }
        if (!scrollTarget && diffMin >= 0 && diffMin < closestDiff) {
            scrollTarget = game;
            closestDiff = diffMin;
        }
    });

    if (scrollTarget) {
        scrollTarget.scrollIntoView({ behavior: 'smooth', block: 'start' });
        setTimeout(() => window.dispatchEvent(new Event('scroll')), 500);
    }

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
            if (rect.top <= 140) {
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
    }

    window.applyGameStateClasses = applyGameStateClasses;

    function highlightLiveBlocks() {
        allBlocks.forEach(block => {
            const liveGames = block.querySelectorAll(".cal-game.live");
            const tag = block.querySelector(".cal-livetag");
            tag?.classList.toggle("visible", liveGames.length > 0);
            block.classList.toggle("live-block", liveGames.length > 0);
        });
    }

    window.toggleScorers = function (el) {
        el.classList.toggle("expanded");
    };

    function startLiveCheckLoop() {
        setInterval(() => {
            applyGameStateClasses();
        }, 15000);
    }

    applyGameStateClasses();
    startLiveCheckLoop();
    allGames.forEach(game => {
        const estado = game.dataset.estado?.trim();
        const comecou = estado === "Resultado Final" || estado === "1ªP" || estado === "2ªP" || estado === "Intervalo";
        if (comecou) {
            game.addEventListener("click", () => toggleScorers(game));
        }
    });


