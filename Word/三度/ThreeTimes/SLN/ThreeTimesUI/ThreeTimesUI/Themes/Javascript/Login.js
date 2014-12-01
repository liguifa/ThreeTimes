

$(document).ready(function () {
    $("#username").blur(function () {

        $('#username').validatebox({
            required: true,
            validType: 'email'
        });
    });
    $("#password").blur(function () {

        $('#password').validatebox({
            required: true,
            validType: 'length[6,20]'
        });

    });
    $("#yzmText").blur(function () {

        $('#yzmText').validatebox({
            required: true,
            validType: 'length[4,4]'
        });

    });
    $("#sub").click(function () {
        check();
    });
    $("#logform").keydown(function (event) {
        if (event.keyCode == 13) {
            check();
        }
       
    });
})
function check() {
    if ($("#username").validatebox('isValid') && $("#password").validatebox('isValid') && $("#yzmText").validatebox('isValid')) {
        $.ajax({
            method: "post",
            url: "common/loginCheck",
            async: true,
            data: {
                username: $("#username").val(),
                password: $("#password").val(),
                yzm: $("#yzmText").val()
            },
            datatype: JSON,
            success: ajaxsuccess
        });
    }
    else {
        $.messager.alert("提示", "请检查输入！");
    }
}
function ajaxsuccess(data) {
    data = eval("(" + data + ")");
    if (data.res == "ok") {
        window.location.href = "common/sureMsg";
    }
    else {
        $.messager.alert("登录提示", "用户名或密码错误！");
    }
}
window.onmousewheel = function () {
    return false;
}