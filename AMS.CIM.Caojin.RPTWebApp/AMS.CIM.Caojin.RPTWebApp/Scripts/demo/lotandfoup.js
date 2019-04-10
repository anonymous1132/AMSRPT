var app = new Vue({
    el: '#app',
    data: {
        inputUserId:'',
        userId: '',
        userName: '',
        dept:'',
        visiable: false,
        loading:false,
        lotEntities: [],
        foupEntities: [],
        activeNames:['1']
    },
    methods: {
        getUserInfo() {
            let url = "GetUserInfoByUserID";
            let data = { userid: this.inputUserId };
            this.visiable = false
            PostAjaxGetJson(data, url, response => {
                if (response.success) {
                    app.userId = app.inputUserId;
                    app.userName = response.UserName;
                    app.dept = response.Department;
                    app.visiable=true
                } else {
                    app.visable = false;
                    app.userId = ""
                    app.userName = ""
                    app.dept = ""
                    app.$message.error(response.msg );
                }
            }, () => { app.$message.error("服务器发生错误");});
        },
        handleQueryClick() {
            let url = "GetTableEntities";
            let data = { userid: this.userId };
            this.loading = true;
            PostAjaxGetJson(data, url, response => {
                if (response.success) {
                    let lots = response.LotEntities
                    let foups = response.CastEntities
                    lots.forEach(f => { f.lotOwnerId = app.userId; f.lotOwnerName = app.userName; })
                    foups.forEach(f => { f.foupOwnerId = app.userId, f.foupOwnerName = app.userName; })
                    app.lotEntities = lots
                    app.foupEntities = foups
                    if (lots.length==0 && foups.length == 0) app.$message.error("没有查询到lot&foup咨询");
                } else {
                    app.$message.error(response.msg);
                }
                app.loading = false;
            }, () => { app.$message.error("服务器发生错误");app.loading=false });
        },
        outputExcel() {
            let data = [];
            let rawData = this.lotEntities;
            for (var i = 0; i < rawData.length; i++) {
                data.push({
                    No: i + 1,
                    LotID: rawData[i].Lot_ID,
                    Qty: rawData[i].Qty,
                    LotOwner: rawData[i].lotOwnerId,
                    LotOwnerName: rawData[i].lotOwnerName,
                    FoupID: rawData[i].Cast_ID,
                    FoupOwner: rawData[i].Foup_Owner,
                    FoupOwnerName: rawData[i].Foup_Owner_Name,
                    Reciver: '',
                    ReciverName:''
                });
            }
            let tableHtml1 = FormExcelContext(data,'Lot Infomation');
            let data2 = [];
            let rawData2 = this.foupEntities;
            for (var i = 0; i < rawData2.length; i++) {
                data2.push({
                    No: i + 1,
                    FoupID: rawData2[i].Cast_ID,
                    FoupOwner: rawData2[i].foupOwnerId,
                    FoupOwnerName: rawData2[i].foupOwnerName,
                    Reciver: '',
                    ReciverName: ''
                });
            }
            let tableHtml2 = FormExcelContext(data2,'Foup Infomation');
            let filename = "Lot&Foup移交清单(" + this.userId + ").xls";
            if (!(tableHtml1 + tableHtml2)) return false;
            tableHtml1 =tableHtml1?tableHtml1: "<tr><td>Lot Infomation</td></tr>"
            tableHtml2 = tableHtml2 ? tableHtml2 : "<tr><td>Foup Infomation</td></tr>"
            let ctx = { worksheet: "sheet1", table: tableHtml1+"<tr></tr>"+tableHtml2 };
            let dlink = this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = filename;
            dlink.click();
        }
    },
    computed: {

    }
});


//仅适用于简单表格
function FormExcelContext(tableData,caption) {
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