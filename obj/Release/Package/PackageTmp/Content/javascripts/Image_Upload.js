jQuery(function () {
    var $fileupload = $('#fileupload');
    var $thumbnails = $('#thumbnails');

    $('#btnupload').on("click", function () {

        $('#fileupload').trigger('click');
    });
    $fileupload.on("change", function (event) {
        upload(event);
    });

    setImage();

    function setImage() {
        var images = $('#thumbnails').find('.thumbnail-img');
        var hidImages = $('#hidImages').val('');
        var arryImg = [];
        if (images.length > 0) {
            for (var i = 0, max = images.length; i < max; i++) {
                var id = $(images[i]).attr("data-id");
             
                arryImg.push(id);
            }

            hidImages.val(arryImg.join(','));

            var chkimgs = $('input[name=chkimg]');
            for (var i=0,max=chkimgs.length;i<max;i++){
                if (i == 0) {
                    $(chkimgs[i]).attr('checked',true);
                }else {
                    $(chkimgs[i]).attr('checked', false);
                }
            }

           
        }
    }


    

    $('#thumbnails').delegate(".removeofproduct", "click", function (event) {
        event.stopPropagation();
        event.preventDefault();
        var id = $(this).attr('id');

        $(this).closest('.col-md-3').remove();
        var img = { id: id, path: "" };

        $.ajax({
            url: $('#imagedeleteproducturl').val(),
            type: 'POST',
            data: JSON.stringify(img),
            cache: false,
            dataType: 'json',
            processData: false, // Don't process the files
            contentType: "application/json",
            success: function (data, textStatus, jqXHR) {

                //$fileupload.val("");
                //for (var i = 0, max = data.length; i < max; i++) {
                //    var isPrimary = i == 0;
                //    renderThumbnail(data[i].id, data[i].path, isPrimary);
                //}
                //setImage();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $fileupload.val("");
                // Handle errors here
                console.log('ERRORS: ' + textStatus);
                // STOP LOADING SPINNER
            }
        });

    });

    $('#thumbnails').delegate(".removeimg", "click", function (event) {
        event.stopPropagation();
        event.preventDefault();
        var id = $(this).attr('id');
     
        $(this).closest('.col-md-3').remove();
        var img = { id: id, path: "" };

        $.ajax({
            url: $('#imagedeleteurl').val(),
            type: 'POST',
            data: JSON.stringify(img),
            cache: false,
            dataType: 'json',
            processData: false, // Don't process the files
            contentType: "application/json",
            success: function (data, textStatus, jqXHR) {

                //$fileupload.val("");
                for (var i = 0, max = data.length; i < max; i++) {
                    var isPrimary = i == 0;
                    renderThumbnail(data[i].id, data[i].path, isPrimary);
                }
                setImage();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $fileupload.val("");
                // Handle errors here
                console.log('ERRORS: ' + textStatus);
                // STOP LOADING SPINNER
            }
        });

    });

    function renderThumbnail(id, path, isprimary) {
       
        var col = $('<div class="col-md-3"></div>');
        var thumbnail = $('<div class="thumbnail"></div>');
        var image = $('<img/>').addClass('thumbnail-img').attr('src', path).attr('data-id', id);

        var checkbox = $('<input type="checkbox" name="chkimg"/>').val(id);
        if (isprimary) {
            checkbox.attr('checked', true);
        }
        var chkdiv = $('<div class="col-md-6 text-left"></div>');
        chkdiv.append(checkbox);
        chkdiv.append('<span>主圖</span>');
        var removeitag = $('<i class="glyphicon glyphicon-remove removeimg"></i>').attr('id', id);
        var remove = $('<div class="col-md-6 text-right"></div>').append(removeitag);
        thumbnail.append(image);
        thumbnail.append(remove);
        col.append(thumbnail);
        col.append(chkdiv);
        col.append(remove);
        col.appendTo($thumbnails);
        // $thumbnails.append(col);

    }

    function upload(event) {
        event.stopPropagation(); // Stop stuff happening
        event.preventDefault(); // Totally stop stuff happening
        var data = new FormData();
        files = event.target.files;
        $.each(files, function (key, value) {
            data.append(key, value);
        });

        $.ajax({
            url: $('#imageuploadurl').val(),

            type: 'POST',
            data: data,
            cache: false,
            dataType: 'json',
            processData: false, // Don't process the files
            contentType: false, // Set content type to false as jQuery will tell the server its a query string request
            success: function (data, textStatus, jqXHR) {

                $fileupload.val("");
                for (var i = 0, max = data.length; i < max; i++) {
                    var isPrimary = (i == 0 && $('#thumbnails').find('img').length <= 0) ? true : false;
                   
                    renderThumbnail(data[i].id, data[i].path, isPrimary);
                }
                setImage();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $fileupload.val("");
                // Handle errors here
                console.log('ERRORS: ' + textStatus);
                // STOP LOADING SPINNER
            }
        });
    }

})