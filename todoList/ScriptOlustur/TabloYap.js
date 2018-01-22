$(document).ready(function () {

    $.ajax({
        url: '/Yaps/BuildToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
                }
        

    });
});


