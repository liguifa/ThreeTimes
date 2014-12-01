
$(document).ready(function () {
    $("#panel").layout({
        fit: true
    });
    $("#south").panel({
        collapsible: false
    });
    $("#east").panel({
        collapsible: false
    });
    $("#north").panel({
        collapsible: false,
    });
    $("#class").accordion({

        fit: true,
        selected: 3
    });
    $("#window").tabs({
        fit: true,
        tools: '#search',
        toolPosition: 'right'

    });

});
$("#company_list").ready(function () {
    $("#company_list").click(function () {
        var parameters = new Array(2);
        parameters[0] = 1;
        parameters[1] = 30;
        parameters[2] = "";
        parameters[3] = "all";
        openWindow("公司列表", "/Admin/PartChart", parameters);
    });
})
$("#add_company").ready(function () {
    $("#add_company").click(function () {
        openWindow("添加公司", "/Admin/CompanyAdd", new Array(0))
    });
});
$("#item_class").ready(function () {
    $("#item_class").click(function () {
        var parameters = new Array(2);
        parameters[0] = 1;
        parameters[1] = 30;
        openWindow("题库类别管理", "/Admin/QuestionClass", parameters)
    }); 
});
$("#questionM").ready(function () {
    $("#questionM").click(function () {
       
        openWindow("题库问题管理", "/Admin/QusetionManage", new Array(0))
    });
});
$("#standby_data").ready(function () {
    $("#standby_data").click(function () {
        openWindow("备份数据")
    });
});
$("#clear_data").ready(function () {
    $("#clear_data").click(function () {
        //openWindow("清除数据")
        $.messager.confirm('提示', '您确定要清除缓存(导入导出遗留的数据)吗？', function (r)
        {
            if (r)
            {
                $.messager.progress({
                    title: "正在清除.....",
                    msg: "清除缓存中.....这需要一些时间",
                    text: "请稍后....."
                });
                $.ajax({
                    type: "post",
                    url: "/Admin/ClearData",
                    data: {
                        data:"clear"
                    },
                    success: function (data)
                    {
                        $.messager.progress('close');
                        var data = eval("(" + data + ")");
                        if (data.data == "ok")
                        {
                            $.messager.alert("提示", "清除成功！", "info");
                        }
                        else
                        {
                            $.messager.alert("错误", data.data, "error");
                        }
                    }
                })
            }
        });
        


    });
});
$("#modify_pwd").ready(function () {
    $("#modify_pwd").click(function () {
        openWindow("修改密码", "/Admin/ChangePassword", new Array(0))
    });
});
$("#admin_administration").ready(function () {
    $("#admin_administration").click(function () {
        var parameters = new Array(2);
        parameters[0] = 1;
        parameters[1] = 30;
        openWindow("管理员管理", "/Admin/admin", parameters)
    });
});


$(".panel-tool:eq(0)").ready(function () {
    $(".panel-tool:eq(0)").prepend("<div style=\"float:right\">" + getTime() + "</div>");
    setInterval("$('.panel-tool:eq(0)').empty();$('.panel-tool:eq(0)').prepend('<div style=\"float:right\">' + getTime() + '</div>')", 1000);
});
var window_id = 1;
function openWindow(title, funname, parameters) {
    getContextHtml(funname, "#window" + window_id, parameters);
    $('#window').tabs('add', {
        title: title,
        closable: true,
        content: '<div  class="loader"><div class="dot"></div><div class="dot"></div><div class="dot"></div> <div class="dot"></div><div class="dot"></div></div>',
        id: "window" + window_id++
    });
}
function getContextHtml(funname, id, parameters) {
    $.ajax({
        type: "post",
        url: funname,
        data: {
            dataId: id,
            parameter0: parameters[0],
            parameter1: parameters[1],
            parameter2: parameters[2],
            parameter3: parameters[3],
            parameter4: parameters[4],
            parameter5: parameters[5],
            parameter6: parameters[6],
            parameter7: parameters[7],
            parameter8: parameters[8],
            parameter9: parameters[9],
        },
        dataType: "html",
        success: function (data) {
            $(id).empty();
            $(id).prepend(data);

        }
    });
}
function getTime() {
    var time = new Date();
    return time.getFullYear() + "年" + doubleNum((time.getMonth() + 1)) + "月" + doubleNum(time.getDate()) + "日" + doubleNum(time.getHours()) + "：" + doubleNum(time.getMinutes());
}
function doubleNum(str) {
    if (str <= 9) {
        return "0" + str;
    }
    else {
        return str;
    }
}
