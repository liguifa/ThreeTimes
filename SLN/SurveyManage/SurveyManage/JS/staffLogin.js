$(document).ready(function ()
{
    $("#submit").click(function ()
    {
        $("#error").text("");
        $("#submit").val("登录中...");
        $("#submit").attr("disabled", "disabled");
        $.ajax({
            type: "post",
            url: "/Staff/LoginIn",
            data: {
                user: $("#value_1").val(),
                pwd: $("#value_2").val(),
                yzm: $("#value_3").val()
            },
            success: function (data)
            {
                data = eval("(" + data + ")");
                var datas = new Array(data.count);
                for (var x in data.data)
                {
                    datas[x] = data.data[x].datakey;
                }
                if (datas[0] == 1)
                {
                    window.location.href = "/Staff/Index";
                }
                else
                {
                    $("#error").append(datas[1]);
                    $("#submit").val("登录");
                    $("#submit").removeAttr("disabled");
                }
            }
        })
    });
    $("#VerCode").click(function ()
    {
        $("#VerCode").attr("src", "/Staff/VerCode/" + parseInt(Math.random() * 10));
    });
})