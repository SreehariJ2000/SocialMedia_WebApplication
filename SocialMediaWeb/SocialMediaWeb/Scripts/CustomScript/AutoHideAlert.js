window.setTimeout(function () {
    $(".alert").fadeTo(300, 0).slideUp(400, function () {
        $(this).remove();
    });
}, 3000);