$(document).ready(function () {
    $('#lmanage').addClass('active');
    $('#selPeriod').change(function () {
        var id = $(this).val();
        var mentor = $('#mentor').val();

        $.ajax({
            url: $('#hdUrl').val().replace("Action", "GetListByCourse"),
            data: JSON.stringify({
                ID: id,
                MENTOR: mentor
            }),
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            crossBrowser: true,
            success: function (data, status) {
                $('#mentor').find('option').not(':first').remove();
                $('#mentee').find('option').not(':first').remove();
                for (var i = 0; i < data.MENTOR.length; i++) {
                    $('#mentor').append('<option value="' + data.MENTOR[i].ID + '">' + data.MENTOR[i].NAME + '</option>');
                }
                for (var i = 0; i < data.MENTEE.length; i++) {
                    $('#mentee').append('<option value="' + data.MENTEE[i].ID + '">' + data.MENTEE[i].NAME + '</option>');
                }
            },
            error: function (xhr, status, error) {
                bootbox.alert("Error! " + xhr.status);
            },
        });
        return false;
    });
    $('#btnExport').click(function (e) {
        window.location = $(this).attr('data-url') + "?COURSE_ID=" + $('#selPeriod').val() + "&MENTOR=" + $('#mentor').val()
        + "&EMP_ID=" + $('#mentee').val();
        return false;
    });
});