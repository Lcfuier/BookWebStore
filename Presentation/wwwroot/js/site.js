// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", () => {
    let lastScrollTop = 0;
    const navbar = document.querySelector('.navbar');

    window.addEventListener('scroll', () => {
        const scrollTop = window.scrollY || document.documentElement.scrollTop;

        if (scrollTop > lastScrollTop && scrollTop > navbar.clientHeight) {
            // Scrolling down
            navbar.classList.remove('navbar-show');
            navbar.classList.add('navbar-hide');
        } else {
            // Scrolling up
            navbar.classList.remove('navbar-hide');
            navbar.classList.add('navbar-show');
        }

        lastScrollTop = scrollTop;
    });
});