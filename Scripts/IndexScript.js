$(document).ready(function () {
    $('#slide1 .slide-container').slick({
        lazyLoad: 'ondemand',
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "#slide1 .slick-prev",
        nextArrow: "#slide1 .slick-next"
    });

    $('#slide2 .slide-container').slick({
        lazyLoad: 'ondemand',
        autoplay: true,
        autoplaySpeed: 2000,
        prevArrow: "#slide2 .slick-prev",
        nextArrow: "#slide2 .slick-next"
    });

    //$("div.category").slick({
    //    lazyLoad: 'ondemand',
    //    slidesToShow: 6,
    //    slidesToScroll: 1
    //});
});