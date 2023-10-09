//function make_slide(divId, interval = 3000) {
//    let slideIndex = 1;
//    let div = document.getElementById(divId);
//    let img = div.getElementsByClassName("image_banner");

//    function start() {
//        console.log("start banner")
//        return setInterval(function () {
//            showDivs(slideIndex += 1);
//        }, interval)
//    }

//    function stop() {
//        console.log("call stop")
//        if (stop_id !== undefined) {
//            return;
//        }
//        clearInterval(start_id);
//        console.log("stop!")
//        stop_id = setTimeout(function () {
//            start_id = start();
//            stop_id = undefined;
//        }, 3000);
//    }

//    function plusDivs(n) {
//        showDivs(slideIndex += n);
//        stop();
//    }

//    let start_id = start();
//    let stop_id = undefined;

//    function showDivs(n) {
//        if (n > img.length) { slideIndex = 1 }
//        if (n < 1) { slideIndex = img.length }
//        for (i = 0; i < img.length; i++) {
//            img[i].style.display = "none";
//        }
//        img[slideIndex - 1].style.display = "block";
//    }
//    showDivs(slideIndex);
//    return plusDivs;
//}

//let slide1 = make_slide("slide1")
//let slide2 = make_slide("slide2")

$(document).ready(function () {
    $('#slide1 .slide-container').slick({
        autoplay: true,
        autoplaySpeed: 3000,
        prevArrow: "#slide1 .slick-prev",
        nextArrow: "#slide1 .slick-next"

    });
});


$(document).ready(function () {
    $('#slide2 .slide-container').slick({
        autoplay: true,
        autoplaySpeed: 2000,
        prevArrow: "#slide2 .slick-prev",
        nextArrow: "#slide2 .slick-next"

    });
});

console.log("a")