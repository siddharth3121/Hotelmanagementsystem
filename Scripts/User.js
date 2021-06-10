//Load Data in Table when documents is ready  
$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
    $.ajax({
        url: "/Admin/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.nid + '</td>';
                html += '<td>' + item.vusername + '</td>';
                html += '<td>' + item.vusertype + '</td>';
                html += '<td>' + item.vmobnumber + '</td>';
                html += '<td>' + item.vpassword + '</td>';
                html += '<td><a style="color:red" href="#" onclick="return getbyID(' + item.nid + ')">Edit</a> | <a style="color:red" href="#" onclick="Delele(' + item.nid + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Add Data Function   
function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        nid: $('#nid').val(),
        vusername: $('#vusername').val(),
        vusertype: $('#vusertype').val(),
        vmobnumber: $('#vmobnumber').val(),
        vpassword: $('#vpassword').val()
    };
    $.ajax({
        url: "/Admin/Add",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//Function for getting the Data Based upon Employee ID  
function getbyID(EmpID) {
    $('#vusername').css('border-color', 'lightgrey');
    $('#vusertype').css('border-color', 'lightgrey');
    $('#vmobnumber').css('border-color', 'lightgrey');
    $('#vpassword').css('border-color', 'lightgrey');
    $.ajax({
        url: "/Admin/getbyID/" + EmpID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#nid').val(result.nid);
            $('#vusername').val(result.vusername);
            $('#vusertype').val(result.vusertype);
            $('#vmobnumber').val(result.vmobnumber);
            $('#vpassword').val(result.vpassword);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

//function for updating employee's record  
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var empObj = {
        nid: $('#nid').val(),
        vusername: $('#vusername').val(),
        vusertype: $('#vusertype').val(),
        vmobnumber: $('#vmobnumber').val(),
        vpassword: $('#vpassword').val(),
    };
    $.ajax({
        url: "/Admin/Update",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#nid').val("");
            $('#vusername').val("");
            $('#vusertype').val("");
            $('#vmobnumber').val("");
            $('#vpassword').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record  
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Admin/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    $('#nid').val("");
    $('#vusername').val("");
    $('#vusertype').val("");
    $('#vmobnumber').val("");
    $('#vpassword').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#vusername').css('border-color', 'lightgrey');
    $('#vusertype').css('border-color', 'lightgrey');
    $('#vmobnumber').css('border-color', 'lightgrey');
    $('#vpassword').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
function validate() {
    var isValid = true;
    if ($('#vusername').val().trim() == "") {
        $('#vusername').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#vusername').css('border-color', 'lightgrey');
    }
    if ($('#vusertype').val().trim() == "") {
        $('#vusertype').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#vusertype').css('border-color', 'lightgrey');
    }
    if ($('#vmobnumber').val().trim() == "") {
        $('#vmobnumber').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#vmobnumber').css('border-color', 'lightgrey');
    }
    if ($('#vpassword').val().trim() == "") {
        $('#vpassword').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#vpassword').css('border-color', 'lightgrey');
    }
    return isValid;
}  
