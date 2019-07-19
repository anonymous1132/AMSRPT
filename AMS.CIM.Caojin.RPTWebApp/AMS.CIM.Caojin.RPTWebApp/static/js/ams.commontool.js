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

function FormExcelContextOfMutiTables(tableData,caption) {
    if (tableData.length < 1 || (!Array.isArray(tableData))) return false;
    let tableHtml = "<tr><td>"+caption+"</td></tr><thead><tr>";
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
    tableHtml = tableHtml + '</tbody>';
    return tableHtml;
}


function getDateStr(date) {
    let y = date.getFullYear();
    let m = date.getMonth() + 1;
    m = m < 10 ? "0" + m : m;
    let d = date.getDate();
    d = d < 10 ? "0" + d : d;
    return y + "-" + m + "-" + d;
  }

  function getCurDate() {
    let date = new Date();
    return getDateStr(date);
  }

  function getCurTime() {
    let date = new Date();
    let y = date.getFullYear();
    let m = date.getMonth() + 1;
    m = m < 10 ? "0" + m : m;
    let d = date.getDate();
    d = d < 10 ? "0" + d : d;
    let h = date.getHours();
    h = h < 10 ? "0" + h : h;
    let min = date.getMinutes();
    min = min < 10 ? "0" + min : min;
    let s = date.getSeconds();
    s = s < 10 ? "0" + s : s;
    return y + "-" + m + "-" + d + " " + h + ":" + min + ":" + s;
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