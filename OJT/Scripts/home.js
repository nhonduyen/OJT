﻿$(document).ready(function () {
    $('#lhome').addClass('active');
    var approve = ["Unconfirmed", "Confirmed"];
    var status = ["", "B", "A", "S"];
    var level = ["", "B", "A", "S"];
    var grade = ["0", "1", "2", "3"];
    var subjects = [];
    $('.datepicker')
 .datepicker({
     format: 'yyyy-mm-dd'
 }).on('changeDate', function (ev) {
     $(this).datepicker('hide');
     var id = $(this).closest('tr').attr('data-id');
     var column = $(this).parent().attr('class');
     var value = $(this).val();
     saveDetail(id, column, value, $(this));
     $(this).siblings().text(value);
     $('span').show();
     $('.datepicker').hide();
 });
    $("[data-toggle='tooltip']").tooltip();
    $.ajax({
        url: $('#hdUrl').val().replace("Action", "GetSubject"),
        data: JSON.stringify({
        }),
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        crossBrowser: true,
        success: function (data, status) {
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var t = {
                        ID: data[i].ID,
                        NAME: data[i].NAME
                    };
                    subjects.push(t);
                }
            }
            else {
                bootbox.alert('fail');
            }
            return false;
        },
        error: function (xhr, status, error) {
            bootbox.alert("Error! " + xhr.status);
        },
    });

    $('.activity').click(function () {
        var id = $(this).closest('tr').attr('data-id');
        var num = $(this).find('.badge').text();
        var src = $('#img-content').attr('data-url') + id;
        $('#img-content').html('');
        $('#hdUploadImg').val(id);
        if (parseInt(num) > 0) {
            $.ajax({
                url: $('#hdUrl').val().replace("Action", "GetActivityImg"),
                data: JSON.stringify({
                    ID: id
                }),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                crossBrowser: true,
                success: function (data, status) {
                    var html = '';
                    var row = "";
                    var count = 0;

                    for (var i = 0; i < data.length; i++) {
                        count++;

                        row += '<a href="#" class="col-sm-4">' +
                       '<img src="' + src + "/" + $.trim(data[i].IMG_URL) + '" width="280" height="300" class="img-fluid">' +
                   '</a>';
                        if (count % 3 == 0) {
                            html += '<div class="row" style="margin-top:5px;">' + row + '</div>';
                            $('#img-content').append(html);
                            html = "";
                            row = "";
                        }
                        else if (i == data.length - 1) {
                            html += '<div class="row" style="margin-top:5px;">' + row + '</div>';
                            $('#img-content').append(html);
                            html = "";
                            row = "";
                        }
                    }


                    return false;
                },
                error: function (xhr, status, error) {
                    bootbox.alert("Error! " + xhr.status);
                },
            });
        }
        $("#mdImg").modal({
            backdrop: 'static',
            keyboard: false
        });
        return false;
    });
    $('.STATUS').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var emp_id = $('#username').val();
        if (emp_id == $(this).closest('tr').attr('data-mentor')) {
            updateVal($(this), $(this).text(), status);
        }
        return false;
    });
    $('.SUB_ID').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var emp_id = $('#username').val();
        if (emp_id == $(this).closest('tr').attr('data-mentor')) {
            updateVal($(this), $(this).text(), subjects);
        }
        return false;
    });
    $('.SUB_LEVEL').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var emp_id = $('#username').val();
        if (emp_id == $(this).closest('tr').attr('data-mentor')) {
            updateVal($(this), $(this).text(), level);
        }
        return false;
    });
    $('.APPROVE').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var dep = $('#username').attr('data-dept');
        var role = $('#username').attr('data-role');
        if (role == 2 && dep == $(this).closest('tr').attr('data-dept')) {
            updateVal($(this), $(this).text(), approve);
        }
        return false;
    });

    $('.START_DT,.END_DT').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var emp_id = $('#username').val();
        if (emp_id == $(this).closest('tr').attr('data-mentor')) {
            $(this).find('.datepicker').val($(this).find('span').text());
            $(this).find('.datepicker').show();
            $(this).find('span').hide();
        }
        return false;
    });
    $('.REC_START_DT,.REC_END_DT').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var emp_id = $('#username').val();
        if (emp_id == $(this).closest('tr').attr('data-emp')) {
            $(this).find('.datepicker').val($(this).find('span').text());
            $(this).find('.datepicker').show();
            $(this).find('span').hide();
        }
        return false;
    });
    $('.TEST_TIME').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var dep = $('#username').attr('data-dept');
        var role = $('#username').attr('data-role');
        if (role == 2 && dep == $(this).closest('tr').attr('data-dept')) {
            $(this).find('.datepicker').val($(this).find('span').text());
            $(this).find('.datepicker').show();
            $(this).find('span').hide();
        }
        return false;
    });
    $('.RESULT_LEVEL').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var dep = $('#username').attr('data-dept');
        var role = $('#username').attr('data-role');
        if (role == 2 && dep == $(this).closest('tr').attr('data-dept')) {
            updateVal($(this), $(this).text(), status);
        }
        return false;
    });
    $('.SCORE').dblclick(function () {
        $('.datepicker').hide();
        $('span').show();
        var dep = $('#username').attr('data-dept');
        var role = $('#username').attr('data-role');
        if (role == 2 && dep == $(this).closest('tr').attr('data-dept')) {
            updateVal1($(this), $(this).text(), status);
        }
        return false;
    });
    $('.ARCHIEVEMENT').dblclick(function () {
        var role = $('#username').attr('data-role');
        if (role == 3) {
            updateVal($(this), $(this).text(), grade);
        }
        return false;
    });
    $('.HR_CMT, .MANAGER_CMT').dblclick(function () {
        $('#frmCMT').attr('data-id', $(this).closest('tr').attr('data-did'));
        $('#frmCMT').attr('data-class', $(this).attr('class'));
        $('#frmCMT')[0].reset();
        $('#txtCmt').val($(this).text());
        var dep = $('#username').attr('data-dept');
        var role = $('#username').attr('data-role');
        if ($(this).attr('class') == 'MANAGER_CMT' && role == 2 && dep == $(this).closest('tr').attr('data-dept')) {
            $("#mdCMT").modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        if ($(this).attr('class') == 'HR_CMT' && role == 3) {
            $("#mdCMT").modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        return false;
    });
    $('.uploadfile').click(function () {
        if ($(this).closest('tr').attr('data-emp') == $('#username').val()) {
            $('#hdUpload').val($(this).closest('tr').attr('data-id'));
            $("#mdUpload").modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        return false;
    });

    $('#frmCMT').submit(function () {
        var id = $(this).attr('data-id');
        var column = $(this).attr('data-class');
        var value = $('#txtCmt').val();
        url = $('#hdUrl').val().replace("Action", "UpdateDetail");
        if (id && column) {
            $.ajax({
                url: url,
                data: JSON.stringify({
                    ID: id,
                    COLUMN: column,
                    VALUE: value
                }),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                crossBrowser: true,
                success: function (data, status) {
                    if (data > 0) {
                        $("#mdCMT").modal('hide');
                        $('.' + column).each(function () {
                            if ($(this).closest('tr').attr('data-did') == id) {
                                $(this).text(value);
                            }
                        });
                        alert(status);
                    }
                    else {
                        alert(status);
                    }
                    return false;
                },
                error: function (xhr, status, error) {
                    bootbox.alert("Error! " + xhr.status);
                },
            });
        }
        return false;
    });
    function changeSubject() {
        $('.SUB_ID').each(function () {
            var id = $(this).text();
            if (!isNaN(id)) {
                for (var i = 0; i < subjects.length; i++) {
                    if (subjects[i].ID == id) {
                        $(this).text(subjects[i].NAME);
                    }
                }

            }
        });
        return false;
    }
    function updateVal(currentEle, value, data) {
        value = $.trim(value);
        var html = "";
        if (typeof data[0] == "string") {
            for (var i = 0; i < data.length; i++) {
                var selected = "";
                if (value == data[i]) {
                    selected = "selected";
                }
                html += "<option value='" + data[i] + "' " + selected + ">" + data[i] + "</option>";
            }
        }
        else {
            for (var i = 0; i < data.length; i++) {
                var selected = "";
                if (value == data[i].NAME) {
                    selected = "selected";
                }
                html += "<option value='" + data[i].ID + "' " + selected + ">" + data[i].NAME + "</option>"
            }
        }
        $(currentEle).html('<select class="thVal">' + html + '</select>');
        $(".thVal").focus();
        $(".thVal").keyup(function (event) {
            if (event.keyCode == 13) {
                var t1 = $(".thVal").val();
                $(currentEle).html($(".thVal").val());
            }
        });

        $(".thVal").unbind('focusout').focusout(function () {
            var t = $(".thVal").val();
            $(currentEle).html($(".thVal").val());
            var id = ($(currentEle).attr('class') == 'RESULT_LEVEL' || $(currentEle).attr('class') == 'ARCHIEVEMENT') ? $(currentEle).attr('data-id') : $(currentEle).closest('tr').attr('data-id');
            saveDetail(id, $(currentEle).attr('class'), t, $(currentEle));
            if ($(currentEle).attr('class') == 'SUB_ID') {
                changeSubject();
            }

        });
    }
    function updateVal1(currentEle, value) {
        $(currentEle).html('<input type="text" class="thScore" value="' + value + '" style="max-width:30px;" />');
        $(".thScore").focus();
        $(".thScore").keyup(function (event) {
            if (event.keyCode == 13) {
                var t1 = $(".thScore").val();
                $(currentEle).html($(".thVal").val());
            }
        });
        $(".thScore").unbind('focusout').focusout(function () {
            var t = $(".thScore").val();
            $(currentEle).html(t);
            saveDetail($(currentEle).attr('data-id'), $(currentEle).attr('class'), t, $(currentEle));
        });
    }
    function saveDetail(ID, COLUMN, VALUE, element) {
        var url = $('#hdUrl').val().replace("Action", "UpdateDetail");
        if (element.attr('class') == 'RESULT_LEVEL' || element.attr('class') == 'SCORE' || element.attr('class') == 'ARCHIEVEMENT') {
            url = $('#hdUrl').val().replace("Action", "UpdateHistory");
        }
        $.ajax({
            url: url,
            data: JSON.stringify({
                ID: ID,
                COLUMN: COLUMN,
                VALUE: VALUE
            }),
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            crossBrowser: true,
            success: function (data, status) {
                console.log(data);
                if (data > 0) {
                    $('#noti').hide();
                    $('#message').text("Save " + status);
                    $("#noti").fadeTo(2000, 500).slideUp(500, function () {
                        $("#noti").slideUp(500);
                    });
                }
                else {
                    $('#noti').hide();
                    $('#message').text("Save " + status);

                    $("#noti").fadeTo(2000, 500).slideUp(500, function () {
                        $("#noti").slideUp(500);
                    });
                }
                return false;
            },
            error: function (xhr, status, error) {
                bootbox.alert("Error! " + xhr.status);
            },
        });
    }
    $('#btnExport').click(function (e) {
        window.location = $(this).attr('data-url') + "?COURSE_ID=" + $('#selPeriod').val() + "&MENTOR=" + $('#mentor').val()
        + "&EMP_ID=" + $('#mentee').val();
        return false;
    });
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
});