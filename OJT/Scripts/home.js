$(document).ready(function () {
    $('#lhome').addClass('active');
    var approve = ["Unconfirmed", "Confirmed"];
    var status = ["Low", "High", "Medium"];
    var level = ["Beginer", "Intermediate", "Advanced"];
    var subjects = [];
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


    $('.STATUS').dblclick(function () {
        updateVal($(this), $(this).text(), status);
        return false;
    });
    $('.SUB_ID').dblclick(function () {
        updateVal($(this), $(this).text(), subjects);
        return false;
    });
    $('.SUB_LEVEL').dblclick(function () {
        updateVal($(this), $(this).text(), level);
        return false;
    });
    $('.APPROVE').dblclick(function () {
        updateVal($(this), $(this).text(), approve);
        return false;
    });
    $('.START_DT,.END_DT,.REC_START_DT,.REC_END_DT,.TEST_TIME').dblclick(function () {
        //updateValDate($(this), $(this).text());
        //updateValDate($(this));
        $('.thVal1')
   .datepicker({
       format: 'yyyy-mm-dd'
   });
        return false;
    });
    $('.RESULT_LEVEL').dblclick(function () {
        updateVal($(this), $(this).text(), status);
        return false;
    });
    $('.SCORE').dblclick(function () {
        updateVal1($(this), $(this).text(), status);
        return false;
    }); 
    $('.HR_CMT, .MANAGER_CMT').dblclick(function () {
        $('#frmCMT').attr('data-id', $(this).attr('data-did'));
        $('#frmCMT').attr('data-class',$(this).attr('class'));
        $('#frmCMT')[0].reset();
        $('#txtCmt').val($(this).text());
        $("#mdCMT").modal({
            backdrop: 'static',
            keyboard: false
        });
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
                            if ($(this).attr('data-did') == id) {
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
                        console.log(subjects[id].NAME);
                    }
                }

            }
        });
        return false;
    }
    function updateVal(currentEle, value, data) {
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
            saveDetail($(currentEle).attr('data-id'), $(currentEle).attr('class'), t, $(currentEle));
            if ($(currentEle).attr('class') == 'SUB_ID') {
                changeSubject();
            }
           
        });
    }
    function updateVal1(currentEle, value) {
        $(currentEle).html('<input type="number" class="thScore" value="'+value+'" style="max-width:30px;" />');
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
        if (element.attr('class') == 'RESULT_LEVEL' || element.attr('class') == 'SCORE') {
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

});