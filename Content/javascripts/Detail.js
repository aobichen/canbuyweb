jQuery(function () {
    var min = "";
    var max = "";

    $('.body-content').delegate("#btnRandom", "click", function () {
        if (CanNext == false) {
            var elm = $(this);
            min = $('.min').val();
            max = $('.max').val();
            $('#randomModal').modal('show');
            ajaxPrice($(this).attr("data-id"));
        } else {
            $("#AlertModal").modal('show');
        }
        //randomMoal(min, max);
    });



    $('#randomModal').on('shown.bs.modal', function () {
        startrotate();
        randomModal();
    });

    $('#randomModal').on('hidden.bs.modal', function () {        
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
            circle2[0].style.transform = "rotate(" + -(deg - 1) + "deg)"
        }, 20);
    }

    function randomModal() {
       
        var minn = parseInt(min);
        var mxxn = parseInt(max);
      
        var timer = 100;
        timerId = setInterval(function () {

            var isOdd = msLeft % 2 == 0 ? true : false;
            msLeft -= progress++;
            var body = $('#randomModal').find('.modal-body #price');
            var r = Math.floor(Math.random() * (mxxn - minn + 1)) + minn;
            body.text(r);

            if (msLeft <= 0) {
                body.text(Price);
                window.clearInterval(timerId);

            }
        }, 100);
    }

    var Price = "";
    var CanNext = true;
    getCanNext();

    function getCanNext() {
        $.ajax({
            url: $('#getnexteurl').val(),
            method: "POST",
            data: { id: 1 },
            type: 'GET',
            cache: false,
            dataType: 'json',
            //processData: false, 
            //contentType: false, 
            success: function (data, textStatus, jqXHR) {
                CanNext = data.cannext;
                console.log(CanNext);
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }

    function ajaxPrice(id) {
      
        $.ajax({
            url: $('#getpriceurl').val(),
            method: "POST",
            data: { id: id },
            type: 'GET',
            cache: false,
            dataType: 'json',
            //processData: false, 
            //contentType: false, 
            success: function (data, textStatus, jqXHR) {
                Price = data.price;
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
});