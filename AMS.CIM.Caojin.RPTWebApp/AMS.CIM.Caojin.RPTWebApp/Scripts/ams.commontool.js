//移动所有的到右边
function moveAll(selectfrom,selectto) {
    //得到第一个select对象
    var selectElement = document.getElementById(selectfrom);
    var optionElements = selectElement.getElementsByTagName("option");
    var len = optionElements.length;
    //alert(len);

    //将第一个selected中的数组翻转
    var firstOption = new Array();
    for (var k = len - 1; k >= 0; k--) {
        firstOption.push(optionElements[k]);

    }
    var lens = firstOption.length;
    //得到第二个select对象
    var selectElement2 = document.getElementById(selectto);
    for (var j = lens - 1; j >= 0; j--) {
        selectElement2.appendChild(firstOption[j]);
    }
}

//全部向左移
function moveAllLeft(selectfrom,selectto) {
    var selectElement = document.getElementById(selectfrom);
    var optionElements = selectElement.getElementsByTagName("option");
    var len = optionElements.length;

    var optionEls = new Array();
    for (var i = len - 1; i >= 0; i--) {
        optionEls.push(optionElements[i]);
    }
    var lens = optionEls.length;

    var firstSelectElement = document.getElementById(selectto);
    for (var j = lens - 1; j >= 0; j--) {
        firstSelectElement.appendChild(optionEls[j]);
    }
}

//获取select所有项
function checkselect(objname) {
    o = document.getElementById(objname);
    var intvalue = "";
    for (i = 0; i < o.options.length; i++) {
        intvalue += o.options[i].value + ",";
    }
    //alert(intvalue);
    return intvalue.substr(0, intvalue.length - 1);
}

//获取select所有选中项
function checkselectoption(objname) {
    o = document.getElementById(objname);
    var intvalue = "";
    for (i = 0; i < o.options.length; i++) {
        if (o.options[i].selected) {
            intvalue += o.options[i].value + ",";
        }
    }
    return intvalue.substr(0, intvalue.length - 1);
}

function PopUp() {
    document.getElementById('light').style.display = 'block'; document.getElementById('fade').style.display = 'block';
}

function MoveItem(fromId, toId) {
    $("#" + fromId + " option:selected").each(function () {
        $(this).appendTo($("#" + toId + ":not(:has(option[value=" + $(this).val() + "]))"));
    });
    $("#" + fromId + " option:selected").remove();
}

function PostAjax(data,url,elementObj) {
    document.getElementById('loading').style.display = "block";
    $.ajax({
        type: 'POST', 
        url: url,
        dataType: 'html', 
        contentType: 'application/json',
        data: JSON.stringify(data), 
        success: function (responseData) {
            elementObj.html("");
            elementObj.html(responseData);
            document.getElementById('loading').style.display = "none";
        },
        error: function () {
            alert(arguments[1]);
            document.getElementById('loading').style.display = "none";
        }
    });

}

//当前日期
function today(){
    var today=new Date();//new 出当前时间
    var h=today.getFullYear();//获取年
    var m=today.getMonth()+1;//获取月
    var d=today.getDate();//获取日
    m=m<10?'0'+m:m;
    d=d<10?'0'+d:d;
    return h+"-"+m+"-"+d; //返回 年-月-日
}

function currentTime(){
    var today=new Date();//new 出当前时间
    var H = today.getHours();//获取时
    var M = today.getMinutes();//获取分
    H=H<10?'0'+H:H;
    M=M<10?'0'+M:M;
    return H+":"+M;//返回时:分:秒
}

var base64 = function (s) {
    return window.btoa(unescape(encodeURIComponent(s)));
};
//替换table数据和worksheet名字
var format = function (s, c) {
    return s.replace(/{(\w+)}/g,
        function (m, p) {
            return c[p];
        });
}

function tableToExcel(tableid, sheetName, fileName, linkId) {
    var uri = 'data:application/vnd.ms-excel;base64,';
    var template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"' +
        'xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>'
        + '<x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>'
        + '</x:ExcelWorkbook></xml><![endif]-->' +
        ' <style type="text/css">' +
        'table th td{' +
        'border-right: 1px solid #F00;' +
        'border - bottom: 1px solid #F00;' +
        'font - size: smaller;' +
        '}' +
        '</style>' +
        '</head><body ><table class="excelTable">{table}</table></body></html>';
    if (!tableid.nodeType) tableid = document.getElementById(tableid);
    var ctx = { worksheet: sheetName || 'Worksheet', table: tableid.innerHTML };
    var dlinkInfo = document.getElementById(linkId);
    dlinkInfo.href = uri + base64(format(template, ctx));
    dlinkInfo.download = fileName;
    dlinkInfo.click();
}