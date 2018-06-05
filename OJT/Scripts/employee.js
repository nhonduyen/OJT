$(document).ready(function () {
    $('#lemployee').addClass('active');
    $('#txtFrom,#txtTo')
    .datepicker({
        format: 'yyyy-mm',
        viewMode: 'months'
    }).on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
    $('#btnRegEmp').click(function () {
        $('#frmRegEmp').attr('data-action', 0);
        $('#frmRegEmp')[0].reset();
        $("#tbCus tbody").empty();
        $("#mdRegCus").modal({
            backdrop: 'static',
            keyboard: false
        });
        return false;
    });
    $('#btnAddCourse').click(function () {
      
        $("#mdAddCourse").modal({
            backdrop: 'static',
            keyboard: false
        });
        return false;
    });
    $('#btnAddSubject').click(function () {

        $("#mdAddSubject").modal({
            backdrop: 'static',
            keyboard: false
        });
        return false;
    });
    $('#btnAssign').click(function () {
        if ($("input:checkbox:checked").length > 0) {
            $("#mdAssign").modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        else {
            bootbox.alert("Please select employee");
        }
        return false;
    });
    $('#frmAssign').submit(function () {
        $('#lblName').val('');
        var MENTOR = $.trim($('#txtMentor').val());
        var COURSE_ID = $.trim($('#selPeriod').val());
        var ids = [];
      
        $("input:checkbox:checked").each(function () {
            var dept = $.trim($(this).closest('tr').children("td:nth-child(5)").text());
            var emp = {
                ID: $(this).attr('id'),
                DEPARTMENT: dept
            };
            ids.push(emp);
        });
        if (MENTOR && COURSE_ID && ids.length > 0) {
            $.ajax({
                url: $('#hdUrl').val().replace("Action", "Assign"),
                data: JSON.stringify({
                    MENTOR: MENTOR,
                    COURSE_ID: COURSE_ID,
                    IDS: ids
                }),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                crossBrowser: true,
                success: function (data, status) {
                    if (data > 0) {
                        tbCourse.ajax.reload();
                        alert(status);
                        $('#frmAssign')[0].reset();
                        $("#mdAssign").modal('hide');
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
        return false;
    });
    $('#btnReset').click(function () {
        if ($("input:checkbox:checked").length > 0) {
            var id = $("#tbMainDefault tr").find("input[type='checkbox']:checked").attr('id');
           
            if (id && confirm('Are you sure you want to reset this user\'s password?')) {
                $.ajax({
                    url: $('#hdUrl').val().replace("Action", "ResetPassword"),
                    data: JSON.stringify({
                        ID: id
                    }),
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    crossBrowser: true,
                    success: function (data, status) {
                        if (data > 0) {
                            tbEmp.ajax.reload();
                            bootbox.alert('Success. Password is 123456');
                        }
                        else {
                            bootbox.alert('Delete fail');
                        }
                        return false;
                    },
                    error: function (xhr, status, error) {
                        bootbox.alert("Error! " + xhr.status);
                    },
                });
            }
        }
        else {
            alert('Please select an employee');
        }
        return false;
    });
  
    $('#btnAdd').click(function () {
        var value = $.trim($('#txtSubjectName').val());
        if (value) {
            var html = "<tr>" +
                                       " <td class='tdleft'>" +
                                           " <label>Subject</label></td>" +
                                        "<td>" +
                                           " <div class='input-group'>" +
                                                "<input type='text'  name='Subjects' class='form-control' value='" + value + "'>" +
                                                "<div class='input-group-btn'>" +
                                                    "<button class='btn btn-danger btnRemove' type='button'>Remove</button>" +
                                               " </div>" +
                                            "</div>" +
                                        "</td>" +
                                   " </tr>";
            $('#tbSubject tbody').append(html);
        }
        $('#txtSubjectName').val('');
        return false;
    });
    $('#tbSubject').on('click', '.btnRemove', function () {
        $(this).closest('tr').remove();
        return false;
    });
    $('#btnSearch').on('click', function () {
        tbCourse.draw();
        return false;
    });
    var tbCourse = $('#tbCourse').DataTable(
          {
              sort: false,
              "processing": true,
              "serverSide": true,
              "searching": true,
              ajax: {
                  type: "POST",
                  contentType: "application/json",
                  url: $('#hdUrl').val().replace("Action", "GetCourseById"),
                  data: function (d) {
                      // note: d is created by datatable, the structure of d is the same with DataTableParameters model above

                      return JSON.stringify({ dataTableParameters: d, ID: $('#selPeriod1').val() });
                  }
              }

          });
    var tbEmp = $('#tbMainDefault').DataTable(
          {
              sort: false,
              "processing": true,
              "serverSide": true,
              "searching": true,
              ajax: {
                  type: "POST",
                  contentType: "application/json",
                  url: $('#hdUrl').val().replace("Action", "GetEmployee"),
                  data: function (d) {
                      // note: d is created by datatable, the structure of d is the same with DataTableParameters model above

                      return JSON.stringify({ dataTableParameters: d });
                  }
              }

          });
   
    $("#btnModify").click(function () {
        if ($("input:checkbox:checked").length > 0) {
            $("h4").html("<span class='glyphicon glyphicon-edit'></span> MODIFY EMPLOYEE");
            var id = $("#tbMainDefault tr").find("input[type='checkbox']:checked").attr('id');
            //  $('#frmRegEmp').attr('data-action', 1);
            $('#StudentId').val('1');
            $('#frmRegEmp')[0].reset();
            $("#tbCus tbody").empty();
            if (id) {
                // GetCusByEmpId(id);

                $.ajax({
                    url: $('#hdUrl').val().replace("Action", "GetEmployeeById"),
                    data: JSON.stringify({
                        ID: id

                    }),
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    crossBrowser: true,
                    success: function (data, status) {
                        if (data.ID != null) {
                            $('#txtEmpId').val($.trim(data.ID));
                            $('#txtEmpName').val($.trim(data.NAME));
                            $('#selDept').val($.trim(data.DEPARTMENT));
                            //$('#img').val($.trim(data.PICTURE));                          
                            $('#selRole').val($.trim(data.ROLE));

                        }
                    },
                    error: function (xhr, status, error) {
                        bootbox.alert("Error! " + xhr.status);
                    },
                });
            }
            $("#mdRegCus").modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        else {
            bootbox.alert("Please select a row.");
        }
        return false;
    });
    $('#txtMentor').change(function () {
        if ($(this).val()) {
            $.ajax({
                url: $('#hdUrl').val().replace("Action", "GetEmployeeById"),
                data: JSON.stringify({
                    ID: $(this).val()
                }),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                crossBrowser: true,
                success: function (data, status) {
                    if (data.ID != null) {
                        $('#lblName').val($.trim(data.NAME));
                    }
                },
                error: function (xhr, status, error) {
                    bootbox.alert("Error! " + xhr.status);
                },
            });
        }
        return false;
    });
   
});