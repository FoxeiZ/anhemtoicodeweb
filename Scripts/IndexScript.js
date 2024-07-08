$(document).ready(function () {
    $('#slide1 .slide-container').slick({
        lazyLoad: 'ondemand',
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "#slide1 .slick-prev",
        nextArrow: "#slide1 .slick-next"
    });

    $('#product-slider .slide-container').slick({
        lazyLoad: 'ondemand',
        slidesToShow: 5,
        draggable: false,
        infinite: false,
        prevArrow: "#product-slider .slick-prev-prod",
        nextArrow: "#product-slider .slick-next-prod",
    });

    $('body').css('display', 'block');


    //$("div.category").slick({
    //    lazyLoad: 'ondemand',
    //    slidesToShow: 6,
    //    slidesToScroll: 1
    //});
});