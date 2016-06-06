jQuery(function () {
    // $('.main-detail').find('#detail').height($('main-carousel').height())

    var height1 = $('.navbar').find('.nav-contaniner').outerHeight();
    var height2 = $('.navbar').find('.contaniner').outerHeight();
    var height3 = $('.navbar').find('.jumbotron').outerHeight();
    var height4 = $('.carousel-inner').find('.cimg').outerHeight();

    var height = height3;
    height = height < 335 ? 335 : height;
    $('.body-content').css({ 'margin-top': height });

    var windowWidth = $(window).width()
    var $navContainer = $('.nav-contaniner');

    var navContainerWidth = $navContainer.width();
    if (windowWidth > 767) {
        $navContainer.css({ width: windowWidth * 0.6 });
        $navContainer.css({ top: 0, left: (windowWidth / 2) - (navContainerWidth / 2) });
    } else {

        $navContainer.css({ top: 0, left: 0 });
        $navContainer.css({ width: windowWidth });
    }

    $(window).resize(function () {

        if ($(window).width() > 767) {
            $navContainer.css({ width: $(window).width() * 0.6 });
            $navContainer.css({ top: 0, left: ($(window).width() / 2) - ($navContainer.width() / 2) });
        } else {
            $navContainer.css({ top: 0, left: 0 });
            $navContainer.css({ width: $(window).width() });
        }
    });

    var min = "";
    var max = "";

    $('.body-content').delegate("#btnRandom", "click", function () {
        var elm = $(this);
         min = elm.closest('.Price').find('.min').text();
         max = elm.closest('.Price').find('.max').text();

        //randomMoal(min, max);
    });



    $('#randomModal').on('shown.bs.modal', function () {
        startrotate();
        randomModal();
    });

    $('#randomModal').on('hidden.bs.modal', function () {
        console.log("timerId");
        window.clearInterval(timerId);
        timerId = null;
        window.clearInterval(cytimer);
        cytimer = null;
        msLeft = 1500;
        deg = 0;
    })

    var msLeft = 1000;
    var deg = 0;
    var progress = 1;
 
    var timerId;
    var cytimer
    var circle1 = $('#random-wheel');
    var circle2 = $('#random-wheel-sub');

    function startrotate() {
         cytimer = setInterval(function () {
            deg += 1;
         
            if (msLeft <= 0) {
                window.clearInterval(cytimer);
                cytimer = null;
            }
            else if (deg >= 3000) {
                deg += 1
            } else if (deg >= 2500) {
                deg += 2
            } else if (deg >= 2000) {
                deg += 3
            }
            else if (deg >= 1800) {
                deg += 4
            }
            else if (deg >= 1500) {
                deg += 5
            }
            else if (deg >= 1300) {
                deg += 6
            }
            else if (deg >= 1200) {
                deg += 7
            }
            else if (deg >= 900) {
                deg += 8
            }
            else if (deg >= 700) {
                deg += 9
            }
            else if (deg >= 400) {
                deg += 10
            } else if (deg >= 10) {
                deg += 11
            }


            circle1[0].style.transform = "rotate(" + deg + "deg)"
            circle2[0].style.transform = "rotate(" + -(deg-1) + "deg)"
        }, 20);
    }

    function randomModal() {
        //console.log($('.Price').find('.min').length);
        //var min = $('.Price').find('.min').text();
        //var max = $('.Price').find('.max').text();
        var minn = parseInt(min);
        var mxxn = parseInt(max);
        //timerId(minn,mxxn);
        var timer = 100;
         timerId = setInterval(function () {
            
            var isOdd = msLeft % 2 == 0 ? true : false;
            msLeft -= progress++;
            var body = $('#randomModal').find('.modal-body #price');
            var r = Math.floor(Math.random() * (mxxn - minn + 1)) + minn;
            body.text(r);
           
            if (msLeft <= 0) {
                body.text(r);
                window.clearInterval(timerId);
               
            }
        }, 100);
    }
 
})