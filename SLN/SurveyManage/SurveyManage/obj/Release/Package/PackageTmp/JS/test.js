var questionSize = dataSize;
var staffID = staffId;
var page = 0;

$(document).ready(function ()
{
    for (var x in cookies)
    {
        var temp = cookies[x].split("=");
        var xzq = "[name='" + staffId + "," + temp[0] + "']";
        var fxzq = "[value='" + temp[1] + "']";
        $(xzq).find(fxzq).attr("checked", "true");
        if (temp[0] != "" && temp[1] != "")
        {
            setCookie(staffID + "," + temp[0], temp[1]);
        }
    }
    updateSchedule();
    $("li").click(function ()
    {
        setCookie($(this).parent().parent().attr("name"), $(this).find("input").val());
        updateSchedule();
        var cookies = getCookie(staffID);
        var datas = "";
        for (var x in cookies)
        {
            datas = datas + cookies[x] + ",";
        }
        $.ajax({
            type: "post",
            url: "/Staff/RecordAnswers",
            data: {
                datas: datas
            }
        });
    });
    
    $("#submit").click(function ()
    {
        if (staffID == 0)
        {
            $.messager.alert("提示", "预览问卷时不提供提交功能！", "info");
            return null;
        }
        var mark = getCookie(staffID).length;
        if (mark == questionSize)
        {
            var w = document.documentElement.clientWidth;
            var h = document.documentElement.clientHeight;
            $("body").append("<div style='width:" + w + "px;height:" + h + "px;background:#000;position:fixed;top:0px;left:0px;z-index:999;opacity: 0.5;align:center'><img style='width:50px;height:50px;margin:" + (h - 50) / 2 + "px " + (w - 50) / 2 + "px' src='../../img/loading.gif'></div>");
            var cookies = getCookie(staffID);
            var datas = "";
            for (var x in cookies)
            {
                datas = datas + cookies[x] + ",";
            }
            $.ajax({
                type: "post",
                url: "/Staff/SubmitKey",
                data: {
                    key:datas
                },
                success: function (data)
                {
                    data = eval("(" + data + ")");
                    var datas = new Array(data.count);
                    for (var x in data.data)
                    {
                        datas[x] = data.data[x].datakey;
                    }
                    $.messager.confirm("提示", datas[1], function (r)
                    {
                        if (datas[0] == "1")
                        {
                            window.location.href = "/Staff/Login";
                        }
                    });
                    window.location.href = "/Staff/Login";
                }
            });
        }
        else
        {
            $.messager.alert("提示","还有题目未选择，不能提交！","info")
        }
    });
});

function updateSchedule()
{
    
    var mark = getCookie(staffId).length;
    $(".progress-val").html(parseInt(mark / questionSize * 100) + "%");
    $(".progress-in").css("width", parseInt(mark / questionSize * 100) + "%");
}
function getCookie(c_name)
{
    var c = new Array();;
    if (document.cookie.length > 0)
    {
        var cookie = "" + document.cookie;
        var cookies = cookie.split(";");
        var i=0;
        for (var x in cookies)
        {
            var ce = cookies[x].split(",");
            if (parseInt(ce[0]) == parseInt(c_name))
            {
                c[i++] = ce[1];
            }
        }
    }
    return c;
}
function setCookie(name, cookie)
{
    var d = new Date();
    d.setHours(d.getHours() + 24); //保存一天
    document.cookie =name + "=" + cookie + ";expires=" + d.toGMTString();
}



if (document.addEventListener)
{
    document.addEventListener('DOMMouseScroll', function ()
    {
        if (parseInt(document.body.scrollTop) >= 154 || parseInt(document.documentElement.scrollTop) >= 154)
        {
            var bar = document.getElementById("bar");
            bar.style.position = "fixed";
            bar.style.top = "-30px";
            bar.style.left = "160px";
            bar.style.zIndex = "999";
        }
        else
        {
            var bar = document.getElementById("bar");
            bar.style.position = "";
            bar.style.top = "";
            bar.style.left = "";
            bar.style.zIndex = "";
        }
    }, false);
}
window.onmousewheel = document.onmousewheel=function()
{
    if (parseInt(document.body.scrollTop) >= 154 || parseInt(document.documentElement.scrollTop)>=154)
    {
        var bar = document.getElementById("bar");
        bar.style.position = "fixed";
        bar.style.top = "-30px";
        bar.style.left = "160px";
        bar.style.zIndex = "999";
    }
    else
    {
        var bar = document.getElementById("bar");
        bar.style.position = "";
        bar.style.top = "";
        bar.style.left = "";
        bar.style.zIndex = "";
    }
}
window.onload = function ()
{

    setTime();
    setInterval("setTime()", 1000);

}

function getTime()
{
    var time = new Date();
    var tm = time.getFullYear() + "年" + doubleNum((time.getMonth() + 1)) + "月" + doubleNum(time.getDate()) + "日" + "&nbsp;" + doubleNum(time.getHours()) + ":" + doubleNum(time.getMinutes());
    return tm;
}

function doubleNum(str)
{
    if (str <= 9)
    {
        return "0" + str;
    }
    else
    {
        return str;
    }
}

function setTime()
{
    document.getElementById("schedule_head_time").innerHTML = getTime();
}

document.body.onselectstart = document.body.ondrag = function ()
{
    return false;
}