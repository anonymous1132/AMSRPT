var Qtime=new Vue({
    el:'#app',
    data:{
        queryQtime:0,
        options:[{value:1.3,label:'1.3'},{value:1.6,label:'1.6'},{value:2,label:'2.0'}],
        queryFactor:'',
        activeNames: ['1'],
        tableEntities:[],
        conditions:'',
        condShow:'none',
        filteredEntites:[],
        loading:false
    },
    methods:{
        handleQueryClick(){
            this.loading=true;
            if(!this.queryFactor){
                this.$message.warning("请选择Factor");
            }else{
                this.filteredEntites=this.tableEntities.filter(f=>f.Qtime<this.queryQtime&&f.FlowFactor<=this.queryFactor);
                this.conditions='Qtime:'+this.queryQtime+';Factor:'+this.queryFactor;
                this.condShow='block';
            }
            this.loading=false;
        },
        outputExcel(){
            if(this.filteredEntites.length==0)return false;
            let data=[];
            let filted=this.filteredEntites;
            for(var i =0;i<filted.length;i++){
                data.push({
                    LotID:filted[i].LotID,
                    FoupID:filted[i].FoupID,
                    Location:filted[i].Location,
                    Status:filted[i].Status,
                    Qtime:filted[i].Qtime,
                    RemainQtime:filted[i].RemainQt,
                    FlowFactor:filted[i].StrFlowFactor,
                    Department:filted[i].Dept,
                    OperationNo:filted[i].OpeNo,
                    Step:filted[i].Step,
                    Priority:filted[i].Priority,
                    Qty:filted[i].Qty,
                    EqpType:filted[i].EqpType,
                    LotStates:filted[i].LotStates,
                    HoldCode:filted[i].HoldCode,
                    HoldComment:filted[i].HoldComment,
                    ToDepartment:filted[i].ToDept,
                    ToOperationNo:filted[i].ToOpeNo,
                    ToStep:filted[i].ToStep,
                    ToEqpType:filted[i].ToEqpType
                });
                let filename="QtimeConstraint"+(new Date()).valueOf()+".xls";
                let tableHtml=FormExcelContext(data);
                if(!tableHtml)return false;
                let ctx= { worksheet: "sheet1" , table: tableHtml };
                let dlink=this.$refs.dlink;
                dlink.href = uri + base64(format(template, ctx));
                dlink.download = filename;
                dlink.click();
            }
        }
    }
});

$(document).ready(function () {
    query();
});

function query(){
    var url="GetTableEntities";
    Qtime.loading=true;
    PostAjaxGetJson(null,url,function(data){
        if(data.success){
            Qtime.tableEntities=data.Entities;
        }else{
          //  console.log(data.Msg);
          Qtime.$message.error(data.Msg);
        }
    Qtime.loading=false;
    },function(data){
        alert(data);
    Qtime.loading=false;
    });
}

//仅适用于简单表格
function FormExcelContext(tableData) {
    if(tableData.length<1||(!Array.isArray(tableData)))return false;
    let tableHtml = "<table><thead><tr>";
    let thead=Object.keys(tableData[0]);
    for (var i = 0; i < thead.length; i++) {
        tableHtml = tableHtml + '<th>' + thead[i] + '</th>';
    }
    tableHtml = tableHtml + '</tr></thead><tbody>';
    for (var i = 0; i < tableData.length; i++) {
        tableHtml = tableHtml + '<tr>';
        for (var j = 0; j < thead.length; j++) {
            tableHtml = tableHtml + '<td>' + tableData[i][thead[j]]+'</td>';
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