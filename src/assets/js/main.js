'use strict';

document.addEventListener('DOMContentLoaded', function () {
    /*------------------
        Preloader
    --------------------*/
    window.addEventListener('load', function () {
        document.querySelector(".loader").style.display = 'none';
        setTimeout(function () {
            document.getElementById("preloder").style.display = 'none';
        }, 200);

        /*------------------
            Filter
        --------------------*/
        var filterControls = document.querySelectorAll('.filter__controls li');
        filterControls.forEach(function (control) {
            control.addEventListener('click', function () {
                filterControls.forEach(function (el) {
                    el.classList.remove('active');
                });
                control.classList.add('active');
            });
        });
        if (document.querySelector('.filter__gallery')) {
            var containerEl = document.querySelector('.filter__gallery');
            var mixer = mixitup(containerEl);
        }
    });

    /*------------------
        Background Set
    --------------------*/
    document.querySelectorAll('.set-bg').forEach(function (element) {
        var bg = element.getAttribute('data-setbg');
        element.style.backgroundImage = 'url(' + bg + ')';
    });

    // Search model
    document.querySelector('.search-switch').addEventListener('click', function () {
        document.querySelector('.search-model').style.display = 'block';
        document.querySelector('.search-model').style.opacity = 1;
    });

    document.querySelector('.search-close-switch').addEventListener('click', closeSearchModel);

    document.getElementById('search-input').addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            closeSearchModel();
        }
    });

    function closeSearchModel() {
        document.querySelector('.search-model').style.opacity = 0;
        setTimeout(function () {
            document.querySelector('.search-model').style.display = 'none';
            document.getElementById('search-input').value = '';
        }, 400);
    }

    /*------------------
        Navigation
    --------------------*/
    $('.mobile-menu').slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    /*------------------
        Hero Slider
    --------------------*/
    var heroSlider = $(".hero__slider");
    heroSlider.owlCarousel({
        loop: true,
        margin: 0,
        items: 1,
        dots: true,
        nav: true,
        navText: ["<span class='arrow_carrot-left'></span>", "<span class='arrow_carrot-right'></span>"],
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        smartSpeed: 1200,
        autoHeight: false,
        autoplay: true,
        mouseDrag: false
    });

    /*------------------
        Video Player
    --------------------*/
    const player = new Plyr('#player', {
        controls: ['play-large', 'play', 'progress', 'current-time', 'mute', 'captions', 'settings', 'fullscreen'],
        seekTime: 25
    });

    /*------------------
        Niceselect
    --------------------*/
    $('select').niceSelect();

    /*------------------
        Scroll To Top
    --------------------*/
    document.getElementById("scrollToTopButton").addEventListener('click', function () {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });
});
