function toggleMenu() {
    const menu = document.getElementById('mobileMenu');
    const button = document.getElementById('hamburgerButton');
    const bottom = document.querySelector('.mobile-bottom');

    if (!button || !menu) return;

    menu.classList.toggle('show');
    bottom.classList.toggle('open');

    button.textContent = menu.classList.contains('show') ? 'x' : '☰';

}


function toggleSubMenu() {
    const submenu = document.getElementById('mobileSubmenu');
    submenu.classList.toggle('show');
}

document.addEventListener('click', function (event) {
    const menu = document.getElementById('mobileMenu');
    const button = document.getElementById('hamburgerButton');

    if (!menu || !menu.classList.contains('show')) return;

    const clickedInside = menu.contains(event.target) || button.contains(event.target);
    if (!clickedInside) {
        menu.classList.remove('show');
        button.textContent = '☰';
        document.querySelector('.mobile-bottom')?.classList.remove('open');
    }
});
