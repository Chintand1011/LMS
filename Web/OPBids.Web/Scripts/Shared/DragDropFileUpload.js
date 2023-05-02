$().ready(function () {


    $("#dataEntryModal").on('shown.bs.modal', function () {



        $('[data-toggle="popover"]').popover();

        $("#formAttachments").unbind();
        $("#addfile").unbind("click");
        $("#file1").unbind();

        $("#addfile").click(function () {
            $("#file1").click();
        });

        // file selected
        $("#file1").change(function () {


            var files = $("#file1").get(0).files;
            var fd = new FormData();

            for (var i = 0; i < files.length; i++) {
                fd.append("file", files[i]);
            }

            uploadData(fd);
        });


        $("html").on("dragover", function (e) {
            e.preventDefault();
            e.stopPropagation();
            //$("h1").text("Drag files here");
            $("#formAttachments").removeClass("upload-area-over");
        });

        $("html").on("drop", function (e) { e.preventDefault(); e.stopPropagation(); });

        // Drag enter
        $('#formAttachments').on('dragenter', function (e) {
            e.stopPropagation();
            e.preventDefault();
            //$("h1").text("Drop");
            $("#formAttachments").addClass("upload-area-over");
        });

        // Drag over
        $('#formAttachments').on('dragover', function (e) {
            e.stopPropagation();
            e.preventDefault();

        });


        // Drop
        $('#formAttachments').on('drop', function (e) {
            e.stopPropagation();
            e.preventDefault();


            var filex = e.originalEvent.dataTransfer.files;
            var fd = new FormData();

            for (var i = 0; i <= filex.length - 1; i++) {

                fd.append('file', filex[i]);
                console.log(filex[i].name);

            }

            uploadData(fd);
        });

        var resetBadgeCounter = function () {

            $(".attachmentbadge").each(function (index) {

                $(this).html(index + 1);

            });

        };

        var deleteFile = function (e) {



            var fileguid = $(this).data("fileguid");

            $('.attachmentItem').filter('[data-guid="' + fileguid + '"]').remove();

            resetBadgeCounter();
        };

        var loadFiles = function (data) {

            $('#formAttachments').removeClass("upload-area-over");
            $(".loading").addClass("hidden")
            $('#formAttachments').append(data);


            $(".removeItem").each(function () {

                $(this).on("click", deleteFile);

            });

            resetBadgeCounter();

            $('[data-toggle="popover"]').popover();
        }

        // Sending AJAX request and upload file
        function uploadData(formdata) {

            $(".loading").removeClass("hidden")

            $.ajax({
                url: '/Shared/UploadDocsBarcode',
                type: 'post',
                data: formdata,
                contentType: false,
                processData: false,
                //dataType: 'json',
                success: function (response) {

                    loadFiles(response);

                }
            });
        }


    });

});