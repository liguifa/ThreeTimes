
$.extend($.fn.validatebox.defaults.rules, {
    Companyid: {
        validator: function (value) {
            return value.search(/^[A-Za-z0-9]{0,25}$/) != -1;
        },
        message: '请输入少于25个数字或字母！'
    }
});
$.extend($.fn.validatebox.defaults.rules, {
    Companyname: {
        validator: function (value) {
            return value.length<32;
        },
        message: '请输入少于32个汉字或字母数字！'
    }
});
$.extend($.fn.validatebox.defaults.rules, {
    Companyphone: {
        validator: function (value) {
            return value.search(/^[0-9]{7,11}$/)!=-1;
        },
        message: '请输入正确的电话号码！'
    }
});
$.extend($.fn.validatebox.defaults.rules, {
    equals: {
        validator: function (value, param) {
            var id = "#" + param[0];
            return value == $(id).val();
        },
        message: '两次输入的密码不一致'
    }
});





