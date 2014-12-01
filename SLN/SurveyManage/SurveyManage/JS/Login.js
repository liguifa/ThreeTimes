

$(document).ready(function () {
    $("#username").blur(function () {

        $('#username').validatebox({
            required: true,
            validType: 'Companyid'
        });
    });
    $("#password").blur(function () {

        $('#password').validatebox({
            required: true,
            validType: 'Companyid'
        });

    });
    $("#yzmText").blur(function () {

        $('#yzmText').validatebox({
            required: true,
            validType: 'length[4,4]'
        });

    });
    $("#sub").click(function ()
    {
        $("#sub").text("登录中....");
        $("#sub").attr("disabled", "disabled");
        check();
    });
    $("#logform").keydown(function (event) {
        if (event.keyCode == 13) {
            check();
        }
       
    });
})
function check() {
    if ($("#username").validatebox('isValid') && $("#password").validatebox('isValid') && $("#yzmText").validatebox('isValid'))
    {

        $("#error").text("");
        $.ajax({
            method: "post",
            url: "/Staff/LoginIn",
            async: true,
            data: {
                username: $("#username").val(),
                password: $("#password").val(),
                yzm: $("#yzmText").val()
            },
            datatype: JSON,
            success: function (data)
            {
                $("#sub").text("登录");
                $("#sub").removeAttr("disabled");
                data = eval("(" + data + ")");
                var datas = new Array(data.count);
                for (var x in data.data)
                {
                    datas[x] = data.data[x].datakey;
                }
                if (datas[0] == "1")
                {
                    window.location.href = "/Staff/sureMsg";
                }
                else if(datas[0]=="2")
                {
                    window.location.href = "/staff/reLogin";
                }
                else
                {
                    $("#error").append(datas[1]);
                }
            }
        });
    }
    else
    {
        $("#error").append("你的输入非法！请检查输入....");
        
    }
}
window.onmousewheel = function () {
    return false;
}

$("#yzm").ready(function ()
{
    $("#yzm").click(function ()
    {
        $("#yzm").attr("src", "/Staff/VerCode/" + parseInt(Math.random() * 1000));
    });
});