$(function () {
    //定义清除错误信息的函数
    function clearMsg() {
        $(".msg").html("");
    }

    //定义获取表单数据的函数,注意返回json对象
    function formData() {
        return {
            selectedeqpid: checkselect("selectedeqpid_select"),
            type: $("input[name='radioDateCategory']:checked").val(),
            from: $("#datepicker_from").val(),
            to: $("#datepicker_to").val(),
        };
    }

    //获取select所有选中项
    function checkselect(objname) {
        o = document.getElementById(objname);
        var intvalue = "";
        for (i = 0; i < o.length; i++) {
            if (o.options[i].selected) {
                intvalue += o.options[i].value + ",";
            }
        }
        return intvalue.substr(0, intvalue.length - 1);
    } 

    //定义注册功能的函数
    function query() {
        var url = "Rpt/Query";
        var data = formData();
        //clearMsg();
        $.ajax({
            type: 'POST', //自动会把json对象转换为查询字符串附在url后面如：http://localhost:49521/Register.ashx?id=a&pwd=b&name=c
            url: url,
            dataType: 'json', //要求服务器返回一个json类型的数据,如：{"success":true,"message":"注册成功"}
            contentType: 'application/json',//发送信息给服务器时，内容编码的类型
            data: data, //提交给服务器的数据,直接使用json对象的数据,如:{"id":"a","pwd":"b","name":"c"}　（如果要求json格式的字符串，可使用用JSON.stringify(data)）
            success: function (responseData) {//如果响应成功（即200）
                if (responseData.success == true) {//responseData也是json格式，如：{"success":true,"message":"注册成功"}


                } else {
                    var msgs = responseData.msgs;//msgs对象是一个数组，如下所示：
                    //{"success":false,"message":"注册失败","msgs":[{"id":"pwdMsg","message":"密码不能为空."},{"id":"nameMsg","message":"姓名不能为空."}]}
                    for (var i = 0; i < msgs.length; i++) {
                        $('#' + msgs[i].id).html(msgs[i].message);
                    }
                }
            },
            error: function () {
                //要求为Function类型的参数，请求失败时被调用的函数。该函数有3个参数，即XMLHttpRequest对象、错误信息、捕获的错误对象(可选)。ajax事件函数如下：
                //function(XMLHttpRequest, textStatus, errorThrown){
                //通常情况下textStatus和errorThrown只有其中一个包含信息
                //this;   //调用本次ajax请求时传递的options参数
                //alert(arguments[1]);
            }
        });//ajax
    }

    //定义一个初始化函数
    function init() {
        $("#query").click(function () {
            query();
        });
    }

    //调用初始化函数
    init();



});