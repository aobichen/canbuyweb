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

})