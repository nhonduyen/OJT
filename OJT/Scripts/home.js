$(document).ready(function () {
    $('#lhome').addClass('active');
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
    console.log(subjects);
    console.log(level);
    function updateVal(currentEle, value, data) {
        var html = "";
        if (typeof data[0] == "string") {
            for (var i = 0; i < data.length; i++) {
                var selected="";
                if (value == data[i]) {
                    selected = "selected";
                }
                html += "<option value='" + data[i] + "' "+selected+">" + data[i] + "</option>";
            }
        }
        else {
            for (var i = 0; i < data.length; i++) {
                var selected = "";
                if (value == data[i].NAME) {
                    selected = "selected";
                }
                html += "<option value='" + data[i].ID + "' "+selected+">" + data[i].NAME + "</option>"
            }
        }
        $(currentEle).html('<select class="thVal">'+html+'</select>');
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
            saveDetail($(currentEle).attr('data-id'), $(currentEle).attr('class'), t);
        });
    }
    function saveDetail(ID, COLUMN, VALUE) {
        $.ajax({
            url: $('#hdUrl').val().replace("Action", "UpdateDetail"),
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
                    alert(status);
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
    }
    $('.status').dblclick(function () {
        updateVal($(this), $(this).text(), status);
        return false;
    });
});