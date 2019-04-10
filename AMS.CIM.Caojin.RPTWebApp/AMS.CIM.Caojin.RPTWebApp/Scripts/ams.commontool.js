//移动所有的到右边
function moveAll(selectfrom, selectto, callback) {
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

    if (typeof callback == 'function') {
        callback();
    }
}

//全部向左移
function moveAllLeft(selectfrom, selectto) {
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

function MoveItem(fromId, toId, callback) {
    $("#" + fromId + " option:selected").each(function () {
        $(this).appendTo($("#" + toId + ":not(:has(option[value=" + $(this).val() + "]))"));
    });
    $("#" + fromId + " option:selected").remove();
    if (typeof callback == 'function') {
        callback();
    }
}

function PostAjax(data, url, elementObj) {
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
            //alert(arguments[1]);
            document.getElementById('loading').style.display = "none";
        }
    });
}

function PostAjaxGetHtml(data, url, callback, errorBack) {
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'html',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (responseData) {
            if (typeof callback == 'function') {
                callback(responseData);
            }
        },
        error: function (responseData) {
            if (typeof errorBack == 'function') {
                errorBack(responseData);
            }
        }
    });
}

function PostAjaxGetJson(data, url, callback, errorBack) {

    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (responseData) {
            if (typeof callback == 'function') {
                callback(responseData);
            }
        },
        error: function (responseData) {
            if (typeof errorBack == 'function') {
                errorBack(responseData);
            }
        }
    });
}


//当前日期
function today() {
    var today = new Date();//new 出当前时间
    var h = today.getFullYear();//获取年
    var m = today.getMonth() + 1;//获取月
    var d = today.getDate();//获取日
    m = m < 10 ? '0' + m : m;
    d = d < 10 ? '0' + d : d;
    return h + "-" + m + "-" + d; //返回 年-月-日
}

function fixDatePickerValue(someDate) {
    var h = someDate.getFullYear();//获取年
    var m = someDate.getMonth() + 1;//获取月
    var d = someDate.getDate();//获取日
    m = m < 10 ? '0' + m : m;
    d = d < 10 ? '0' + d : d;
    return h + "-" + m + "-" + d; //返回 年-月-日
}

function fixCommonDateTimeValue(someDate) {
    var date = fixDatePickerValue(someDate);
    var H = someDate.getHours();
    var M = someDate.getMinutes();
    var S = someDate.getSeconds();
    H = H < 10 ? '0' + H : H;
    M = M < 10 ? '0' + M : M;
    S = S < 10 ? '0' + S : S;
    return date + " " + H + ":" + M + ":" + S;
}

//获取某日是每年的第几周
function getWeekOfYear(someDay) {
    var firstDay = new Date(someDay.getFullYear(), 0, 1);
    var dayOfWeek = firstDay.getDay();
    var spendDay = 1;
    if (dayOfWeek != 0) {
        spendDay = 7 - dayOfWeek + 1;
    }
    firstDay = new Date(someDay.getFullYear(), 0, 1 + spendDay);
    var d = Math.ceil((someDay.valueOf() - firstDay.valueOf()) / 86400000);
    var result = Math.ceil(d / 7);
    return result + 1;
};
//Format日期至input date格式
//function formatDateValue(someDay) {
//    var h = someDay.getFullYear(),
//    var m = someDay.getMonth() + 1,
//    var d = someDay.getDate(),
//    m = m < 10 ? '0' + m : m;
//    d = d < 10 ? '0' + d : d;
//    return h + "-" + m + "-" + d; //返回 年-月-日
//}

function currentTime() {
    var today = new Date();//new 出当前时间
    var H = today.getHours();//获取时
    var M = today.getMinutes();//获取分
    H = H < 10 ? '0' + H : H;
    M = M < 10 ? '0' + M : M;
    return H + ":" + M;//返回时:分:秒
}
function currentTimeToSecond() {
    var today = new Date();//new 出当前时间
    var H = today.getHours();//获取时
    var M = today.getMinutes();//获取分
    var S = today.getSeconds();
    H = H < 10 ? '0' + H : H;
    M = M < 10 ? '0' + M : M;
    S = S < 10 ? '0' + S : S;
    return H + ":" + M + ':' + S;//返回时:分:秒
}
//日期加减
function addDate(date, days) {
    var d = new Date(date);
    d.setDate(d.getDate() + days);
    var m = d.getMonth() + 1;
    m = m < 10 ? '0' + m : m;
    var da = d.getDate() < 10 ? '0' + d.getDate() : d.getDate();
    return d.getFullYear() + '-' + m + '-' + da;
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

var tableSort = function () {
    this.initialize.apply(this, arguments);
}

tableSort.prototype = {
    initialize: function (tableId, clickRow, startRow, endRow, classUp, classDown, selectClass) {
        this.Table = document.getElementById(tableId);
        this.rows = this.Table.rows;//所有行 
        this.Tags = this.rows[clickRow - 1].cells;//标签td 
        this.up = classUp;
        this.down = classDown;
        this.startRow = startRow;
        this.selectClass = selectClass;
        this.endRow = (endRow == 999 ? this.rows.length : endRow);
        this.T2Arr = this._td2Array();//所有受影响的td的二维数组 
        this.setShow();
    },
    //设置标签切换 
    setShow: function () {
        var defaultClass = this.Tags[0].className;
        for (var Tag, i = 0; Tag = this.Tags[i]; i++) {
            Tag.index = i;
            addEventListener(Tag, 'click', Bind(Tag, statu));
        }
        var _this = this;
        var turn = 0;
        function statu() {
            for (var i = 0; i < _this.Tags.length; i++) {
                _this.Tags[i].className = defaultClass;
            }
            if (turn == 0) {
                addClass(this, _this.down)
                _this.startArray(0, this.index);
                turn = 1;
            } else {
                addClass(this, _this.up)
                _this.startArray(1, this.index);
                turn = 0;
            }
        }
    },
    //设置选中列样式 
    colClassSet: function (num, cla) {
        //得到关联到的td 
        for (var i = (this.startRow - 1); i < (this.endRow); i++) {
            for (var n = 0; n < this.rows[i].cells.length; n++) {
                removeClass(this.rows[i].cells[n], cla);
            }
            addClass(this.rows[i].cells[num], cla);
        }
    },
    //开始排序 num 根据第几列排序 aord 逆序还是顺序 
    startArray: function (aord, num) {
        var afterSort = this.sortMethod(this.T2Arr, aord, num);//排序后的二维数组传到排序方法中去 
        this.array2Td(num, afterSort);//输出 
    },
    //将受影响的行和列转换成二维数组 
    _td2Array: function () {
        var arr = [];
        for (var i = (this.startRow - 1), l = 0; i < (this.endRow); i++ , l++) {
            arr[l] = [];
            for (var n = 0; n < this.rows[i].cells.length; n++) {
                arr[l].push(this.rows[i].cells[n].innerHTML);
            }
        }
        return arr;
    },
    //根据排序后的二维数组来输出相应的行和列的 innerHTML 
    array2Td: function (num, arr) {
        this.colClassSet(num, this.selectClass);
        for (var i = (this.startRow - 1), l = 0; i < (this.endRow); i++ , l++) {
            for (var n = 0; n < this.Tags.length; n++) {
                this.rows[i].cells[n].innerHTML = arr[l][n];
            }
        }
    },
    //传进来一个二维数组，根据二维数组的子项中的w项排序，再返回排序后的二维数组 
    sortMethod: function (arr, aord, w) {
        //var effectCol = this.getColByNum(whichCol); 
        arr.sort(function (a, b) {
            x = killHTML(a[w]);
            y = killHTML(b[w]);
            x = x.replace(/,/g, '');
            y = y.replace(/,/g, '');
            switch (isNaN(x)) {
                case false:
                    return Number(x) - Number(y);
                    break;
                case true:
                    return x.localeCompare(y);
                    break;
            }
        });
        arr = aord == 0 ? arr : arr.reverse();
        return arr;
    }
}
/*-----------------------------------*/
function addEventListener(o, type, fn) {
    if (o.attachEvent) { o.attachEvent('on' + type, fn) }
    else if (o.addEventListener) { o.addEventListener(type, fn, false) }
    else { o['on' + type] = fn; }
}

function hasClass(element, className) {
    var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
    return element.className.match(reg);
}

function addClass(element, className) {
    if (!this.hasClass(element, className)) {
        element.className += " " + className;
    }
}

function removeClass(element, className) {
    if (hasClass(element, className)) {
        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)');
        element.className = element.className.replace(reg, ' ');
    }
}

var Bind = function (object, fun) {
    return function () {
        return fun.apply(object, arguments);
    }
}
//去掉所有的html标记 
function killHTML(str) {
    return str.replace(/<[^>]+>/g, "");
}

//仅适用于简单表格
function FormExcelContext(tableData) {
    if (tableData.length < 1 || (!Array.isArray(tableData))) return false;
    let tableHtml = "<table><thead><tr>";
    let thead = Object.keys(tableData[0]);
    for (var i = 0; i < thead.length; i++) {
        tableHtml = tableHtml + '<th>' + thead[i] + '</th>';
    }
    tableHtml = tableHtml + '</tr></thead><tbody>';
    for (var i = 0; i < tableData.length; i++) {
        tableHtml = tableHtml + '<tr>';
        for (var j = 0; j < thead.length; j++) {
            tableHtml = tableHtml + '<td>' + tableData[i][thead[j]] + '</td>';
        }
        tableHtml = tableHtml + '</tr>';
    }
    tableHtml = tableHtml + '</tbody></table>';
    return tableHtml;
}
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