$(document).ready(function () {
    $('.dual_select').bootstrapDualListbox({
        selectorMinimalHeight: 80,
        showFilterInputs: false,
        infoText: false,
        nonSelectedListLabel: 'Available Priority:',
        selectedListLabel: 'Selected Priority:',
        });
    query();
});



var tableView0 = new Vue({
    el: '#table-view-0',
    data: {
        Conditions: '',
        FullRows: [],
        dialogFormVisible: false,
        dialogUpdateVisible: false,
        changeKeyButtonVisible:false,
        formLabelWidth: '120px',
        form:{
            key:undefined
        },
        selRow:null,
        update:{
            oldKey:"",
            newKey:""
        },
        loading:false
    },
    computed: {
        display: function () {
            return this.Conditions ? 'block' : 'none';
        },
        Rows:function(){
            var rows=[];
            if(this.Conditions){
            for(var i=0;i<this.FullRows.length;i++){
                if(this.Conditions.indexOf(this.FullRows[i].Priority)>-1){
                    rows.push(this.FullRows[i]);
                }
            }
        }
            return rows;
        }
    },
    methods:{
        handleRowDblClick(row){
            if(this.form.key===undefined){
                this.dialogFormVisible=true;
                this.selRow=row;
            }else{
                this.Rows.map(m=>m.EditState=false);
                this.selRow=row;
                row.EditState=true;
            }
        },
        handleDialogCancel(){
            this.selRow=null;
            this.form.key=undefined;
            this.dialogFormVisible=false;
            modalView.quotaProj.Rows.sel=null;
            modalView.quotaProj.Rows.selRow=null;
            modalView.quotaNormal.Rows.sel=null;
            modalView.quotaNormal.Rows.selRow=null;
        },
        handleDialogOK(){
            if(!this.form.key){ this.dialogFormVisible=false;return;}
            let data={key:this.form.key};
            let url="CheckKey";
            PostAjaxGetJson(data,url,function(data){
                if(!data.successed){
                    alert("口令认证失败！");
                    tableView0.form.key=undefined;
                    tableView0.changeKeyButtonVisible=false;
                }else{
                    tableView0.dialogFormVisible=false;
                    tableView0.changeKeyButtonVisible=true;
                    if(modalView.dialogQuotaVisible){
                        //大框
                        //Proj
                        if(modalView.quotaProj.selRow){
                            //修改
                            modalView.quotaProj.selRow.EditState=true;
                        }else if(modalView.quotaProj.sel&&modalView.quotaProj.sel.id==0){
                            //新增
                            modalView.quotaProj.Rows.push(modalView.quotaProj.sel);
                        }else{
                            //Normal
                            if(modalView.quotaNormal.selRow){
                                //修改
                                modalView.quotaNormal.selRow.EditState=true;
                            }else if(modalView.quotaNormal.sel&&modalView.quotaNormal.sel.id==0){
                                //新增
                                modalView.quotaNormal.Rows.push(modalView.quotaNormal.sel);
                            }else{
                                modalView.$message.success("口令校验成功，请重新执行后续动作！");
                            }
                        }
                    }
                    else if(stage2View.dialogStage2Visible){
                        stage2View.selRow.EditState=true;
                    }
                    else{
                        //MainTable
                        tableView0.selRow.EditState=true;
                    }   
                }
            },function(){alert("认证服务发生错误！");});
        },
        handleChangeKeyButtonClick(){
            this.dialogUpdateVisible=true;
        },
        handleUpdateCancel(){
            this.dialogUpdateVisible=false;
            this.update.oldKey="";
            this.update.newKey="";
        },
        handleUpdateOK(){
            let data={newKey:this.update.newKey,oldKey:this.update.oldKey};
            let url="updateKey";
            PostAjaxGetJson(data,url,function(data){
                if(data.successed){
                    this.$message.success(data.msg);
                    tableView0.dialogUpdateVisible=false;
                    tableView0.update.oldKey="";
                    tableView0.update.newKey="";
                    }else{
                    this.$message.error(data.msg);
                    tableView0.update.oldKey="";
                    tableView0.update.newKey="";
                } 
            },function(){this.$message.error("口令更改服务发生错误！");});
          },
        handleSelectChange(value){
            let url="QuotaProjectLotMappingHandle";
            let post={
                lotid:this.selRow.LotID.split('.')[0]+'.00',
                project:this.selRow.Project
            };
            PostAjaxGetJson(post,url,function(data){
                if(data.Success){
                    tableView0.$message.success("成功修改");
                    tableView0.selRow.EditState=false;
                    if(tableView0.selRow.Project===''){
                        tableView0.selRow.QuotaType=0;
                        tableView0.selRow.Purpose='';
                    }else{
                        tableView0.selRow.QuotaType=1;
                        tableView0.selRow.Purpose=modalView.quotaProj.Rows.find(f=>f.Project==tableView0.selRow.Project).Purpose;
                    }
                    tableView0.selRow=null;
                }else{
                    tableView0.$message.error("更改失败");
                }
              
            },function(data){
                tableView0.$message.error("服务端错误，更改失败");
                console.log(data.Msg);
            });
        },
        handleLotIDClick(row){
            let data={lot:row.LotID,product:row.ProductID};
            let url="Stage2Query";
            this.loading=true;
            PostAjaxGetJson(data,url,function(response){
                if(response.success){
                    response.Entities.map(m=>m.EditState=false);
                    stage2View.LotID=row.LotID;
                    stage2View.Entities=response.Entities;
                    stage2View.ChartModels=response.ChartModels;
                    let end=new Date(JSON.parse(JSON.stringify(stage2View.queryTime)));
                    end.setDate(end.getDate()+1);
                    stage2View.timeInput=[new Date(),end];
                    stage2View.rawTime=new Date();
                    stage2View.dialogStage2Visible=true;
                }else{
                    tableView0.$message.error(response.msg);
                }
                tableView0.loading=false;
            },function(response){
                tableView0.$message.error(response);
                tableView0.loading=false;
            });
        },
        outputExcel(){
            let tableHtml=FormExcelContext(this.Rows);
            if(!tableHtml)return false;
            let ctx= { worksheet: "sheet1" , table: tableHtml };
            let dlink=this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = "SHL.xls";
            dlink.click();
        }
    }
});


var modalView=new Vue({
    el:'#myModal',
    data:{
        quotaProd:{Rows:[]},
        quotaDept:{Rows:[]},
        quotaProj:{Rows:[],sel:null,selRow:null},
        quotaNormal:{Rows:[],sel:null,selRow:null},
        dialogQuotaVisible:false,
        dialogInnerVisible:false
    },
    computed:{
        SHLUsedTotal:function(){
            var total=0;
            for(var i=0; i<this.quotaDept.Rows.length;i++){
                total+=this.quotaDept.Rows[i].NormalSHLUsed+this.quotaDept.Rows[i].ProjectSHLUsed;
            }
            return total;
        },
        SHLRemnantTotal:function(){
            var total=0;
            for(var i=0; i<this.quotaDept.Rows.length;i++){
                total+=this.quotaDept.Rows[i].NormalSHLRemnant+this.quotaDept.Rows[i].ProjectSHLRemnant;
            }
            return total;
        },
        HLUsedTotal:function(){
            var total=0;
            for(var i=0; i<this.quotaDept.Rows.length;i++){
                total+=this.quotaDept.Rows[i].NormalHLUsed+this.quotaDept.Rows[i].ProjectHLUsed;
            }
            return total;
        },
        HLRemnantTotal:function(){
            var total=0;
            for(var i=0; i<this.quotaDept.Rows.length;i++){
                total+=this.quotaDept.Rows[i].NormalHLRemnant+this.quotaDept.Rows[i].ProjectHLRemnant;
            }
            return total;
        },
        Departments:function(){
            return this.quotaDept.Rows.map(item=>item.Department);
        }
    },
    methods:{

        //添加
        addQuota() {
            for (let i of this.quotaProj.Rows) {
            if (i.EditState) return this.$message.warning("请先保存当前编辑项");
            }
            let j = { id: 0, Department: "", Project: "", Purpose: "", QuotaSHL: 0, QuotaHL: 0, UsedSHL: 0, UsedHL:0,RemnantSHL:0,RemnantHL:0,EditState:true};
            this.quotaProj.sel = JSON.parse(JSON.stringify(j));
            this.quotaProj.selRow=null;
            if(tableView0.form.key===undefined){
                tableView0.dialogFormVisible=true;
            }else{
                this.quotaProj.Rows.push(j);
            } 
        },
        //修改、保存、取消
        quotaChange(row, index, cg){

            //点击修改 判断是否已经保存所有操作
            for (let i of this.quotaProj.Rows) {
                if (i.EditState && i.id != row.id) {
                    this.$message.warning("请先保存当前编辑项");
                return false;
                }
            }
            //是否是取消操作
            if (!cg) {
                if (!this.quotaProj.sel.id) this.quotaProj.Rows.splice(index, 1);
                return row.EditState = !row.EditState;
                }
            //提交数据
            if (row.EditState) {
                if(!modalView.quotaProj.sel.Department){
                    return modalView.$message.warning("请选择Department");
                }
                if(!modalView.quotaProj.sel.Project){
                    return modalView.$message.warning("请填写Project");
                }
                (function () {
                let data = JSON.parse(JSON.stringify(modalView.quotaProj.sel));
                //slot 增加一个提交数据的操作
                let url="QuotaHandle";
                let post={
                Department:data['Department'],
                Quota_Type:1,
                Project_Desc:data['Project'],
                Purpose:data["Purpose"],
                Quota_SHL:data['QuotaSHL'],
                Quota_HL:data['QuotaHL']
                };
                PostAjaxGetJson(post,url,function(responseData){
                if(responseData.Success){
                for (let k in data) row[k] = data[k];
                modalView.$message.success(responseData.Msg);
                //然后这边重新读取表格数据
                row.id=responseData.newID;
                row.EditState = false;
                }else{
                modalView.$message.error(responseData.Msg);
                }
                },function(responseData){
                modalView.$message.error(responseData.Msg);
                });
                })();
            }else{

                //点击修改进入编辑状态
                this.quotaProj.sel = JSON.parse(JSON.stringify(row));
                this.quotaProj.selRow=row;
                if(tableView0.form.key===undefined){
                    tableView0.dialogFormVisible=true;
                }else{
                    row.EditState = true;
                } 
            }
        },
        quotaDel(row,index){
            if(tableView0.form.key===undefined){
                this.quotaProj.sel=null;
                this.quotaProj.selRow=null;
                this.$message.info("请输入口令后再次操作");
                tableView0.dialogFormVisible=true;
            }else{
                //slot 增加一个提交数据的操作
                let url="QuotaHandle";
                let post={
                Department:row.Department,
                Quota_Type:1,
                Project_Desc:row.Project,
                Purpose:"DEL_FLAG",
                Quota_SHL:row.QuotaSHL,
                Quota_HL:row.QuotaHL
                };
                PostAjaxGetJson(post,url,function(responseData){
                    if(responseData.Success){
                    modalView.quotaProj.Rows.splice(index,1);
                    modalView.$message.success(responseData.Msg);
                    //然后这边重新读取表格数据
                    }else{
                    modalView.$message.error(responseData.Msg);
                    }
                    },function(responseData){
                    modalView.$message.error(responseData.Msg);
                    });
            }
        },
         //添加
         addNormalQuota() {
            for (let i of this.quotaNormal.Rows) {
            if (i.EditState) return this.$message.warning("请先保存当前编辑项");
            }
            let j = { id: 0, Department: "",  QuotaSHL: 0, QuotaHL: 0, UsedSHL: 0, UsedHL:0,RemnantSHL:0,RemnantHL:0,EditState:true};
            this.quotaNormal.sel = JSON.parse(JSON.stringify(j));
            this.quotaNormal.selRow=null;
            if(tableView0.form.key===undefined){
                tableView0.dialogFormVisible=true;
            }else{
                this.quotaNormal.Rows.push(j);
            } 
        },
        //修改、保存、取消
        quotaNormalChange(row, index, cg){
               //点击修改 判断是否已经保存所有操作
               for (let i of this.quotaNormal.Rows) {
                if (i.EditState && i.id != row.id) {
                    this.$message.warning("请先保存当前编辑项");
                return false;
                }
            }
            //是否是取消操作
            if (!cg) {
                //判断是否取消新增行
                if (!this.quotaNormal.sel.id) this.quotaNormal.Rows.splice(index, 1);
                return row.EditState = !row.EditState;
                }
            //提交数据
            if (row.EditState) {
                if(!modalView.quotaNormal.sel.Department){
                    return modalView.$message.warning("请选择Department");
                }
                if(modalView.quotaNormal.Rows.find(f=>f.EditState==false&&f.Department==modalView.quotaNormal.sel.Department)){
                    return modalView.$message.warning("每个部门只能设定一次");
                }
                (function () {
                let data = JSON.parse(JSON.stringify(modalView.quotaNormal.sel));
                //slot 增加一个提交数据的操作
                let url="QuotaHandle";
                let post={
                Department:data['Department'],
                Quota_Type:0,
                Project_Desc:"",
                Purpose:"",
                Quota_SHL:data['QuotaSHL'],
                Quota_HL:data['QuotaHL']
                };
                PostAjaxGetJson(post,url,function(responseData){
                if(responseData.Success){
                for (let k in data) row[k] = data[k];
                modalView.$message.success(responseData.Msg);
                //然后这边重新读取表格数据
                row.id=responseData.newID;
                row.EditState = false;
                }else{
                modalView.$message.error(responseData.Msg);
                }
                },function(responseData){
                modalView.$message.error(responseData.Msg);
                });
                })();
            }else{
                //点击修改进入编辑状态
                this.quotaNormal.sel = JSON.parse(JSON.stringify(row));
                this.quotaNormal.selRow=row;
                if(tableView0.form.key===undefined){
                    tableView0.dialogFormVisible=true;
                }else{
                    row.EditState = true;
                } 
            }
        },
        quotaNormalDel(row,index){
            if(tableView0.form.key===undefined){
                this.quotaNormal.sel=null;
                this.quotaNormal.selRow=null;
                this.$message.info("请输入口令后再次操作");
                tableView0.dialogFormVisible=true;
            }else{
                //slot 增加一个提交数据的操作
                let url="QuotaHandle";
                let post={
                Department:row.Department,
                Quota_Type:0,
                Project_Desc:"",
                Purpose:"DEL_FLAG",
                Quota_SHL:row.QuotaSHL,
                Quota_HL:row.QuotaHL
                };
                PostAjaxGetJson(post,url,function(responseData){
                    if(responseData.Success){
                    modalView.quotaNormal.Rows.splice(index,1);
                    modalView.$message.success(responseData.Msg);
                    }else{
                    modalView.$message.error(responseData.Msg);
                    }
                    },function(responseData){
                    modalView.$message.error(responseData.Msg);
                    });
            }
        },
        outputExcel(type){
            let data=[];
            let filename="";
            if(type=="prod"){data=this.quotaProd.Rows;filename="SHL_QuotaProduct.xls";}
            else if(type=="dept"){data=this.quotaDept.Rows;filename="SHL_QuotaDepartment.xls";}
            else if(type=="proj"){data=this.quotaProj.Rows;filename="SHL_QuotaProject.xls";}
            else if(type=="normal"){data=this.quotaNormal.Rows;filename="SHL_QuotaNormal.xls";}
            else return false;
            let tableHtml=FormExcelContext(data);
            if(!tableHtml)return false;
            let ctx= { worksheet: "sheet1" , table: tableHtml };
            let dlink=this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = filename;
            dlink.click();
        }
    }
});

var stage2View=new Vue({
    el: '#stage2Modal',
    data:{
        dialogStage2Visible:false,
        Entities:[],
        LotID:'',
        timeInput:[new Date(),new Date()],
        rawTime:new Date(),
        selRow:null,
        chartOptions:{
            //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
            scaleBeginAtZero : true,

            //Boolean - Whether grid lines are shown across the chart
            scaleShowGridLines : true,

            //String - Colour of the grid lines
            scaleGridLineColor : "rgba(0,0,0,.05)",

            //Number - Width of the grid lines
            scaleGridLineWidth : 1,

            //Boolean - Whether to show horizontal lines (except X axis)
            scaleShowHorizontalLines: true,

            //Boolean - Whether to show vertical lines (except Y axis)
            scaleShowVerticalLines: true,

            //Boolean - If there is a stroke on each bar
            barShowStroke : true,

            //Number - Pixel width of the bar stroke
            barStrokeWidth : 2,

            //Number - Spacing between each of the X value sets
            barValueSpacing : 5,

            //Number - Spacing between data sets within X values
            barDatasetSpacing : 1,

            //String - A legend template
            legendTemplate : "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>",

        },
        ctx:null,
        canvas:null,
        ChartModels:[]
    },
    computed:{
        queryTime:function(){         
            return stage2View.timeInput?stage2View.timeInput[0]:stage2View.rawTime;
        },
        pickerOptions:function(){
            return {
            shortcuts: [{
                text:'重置',
                onClick(picker){
                    let start=new Date();
                    let end=new Date();
                    end.setDate(end.getDate()+1);
                    picker.$emit('pick',[start,end]);
                }
            },
            {
              text: '24小时内',
              onClick(picker) {
                  let end=new Date(JSON.parse(JSON.stringify(stage2View.queryTime)));
                  end.setDate(end.getDate()+1);
                picker.$emit('pick', [stage2View.queryTime,end]);
               //stage2View.timeInput=[stage2View.queryTime,end];
              }
            }, {
              text: '36小时内',
              onClick(picker) {
                let end=new Date( JSON.parse( JSON.stringify(stage2View.queryTime)));
                end.setTime(end.getTime()+1.5*24*60*60*1000);
                picker.$emit('pick', [stage2View.queryTime,end]);
                //stage2View.timeInput=[stage2View.queryTime,end];
              }
            }, {
              text: '48小时内',
              onClick(picker) {
                let end=new Date( JSON.parse( JSON.stringify(stage2View.queryTime)));
                end.setDate(end.getDate()+2);
                picker.$emit('pick', [stage2View.queryTime,end]);
              //  stage2View.timeInput=[stage2View.queryTime,end];
              }
            },{
                text: '60小时内',
              onClick(picker) {
                let end=new Date( JSON.parse( JSON.stringify(stage2View.queryTime)));
                end.setTime(end.getTime()+2.5*24*60*60*1000);
                picker.$emit('pick', [stage2View.queryTime,end]);
               // stage2View.timeInput=[stage2View.queryTime,end];
              }
            },{
                text: '72小时内',
                onClick(picker) {
                    let end=new Date( JSON.parse( JSON.stringify(stage2View.queryTime)));
                    end.setDate(end.getDate()+3);
                    picker.$emit('pick', [stage2View.queryTime,end]);
                 //   stage2View.timeInput=[stage2View.queryTime,end];
                }
            }
            ]
          }
        },
        filterdEntities:function(){
            let res=[];
            if(this.timeInput){
             res= this.Entities.filter(f=>{
                if(f.strWFIn){
                    let dt=new Date(f.strWFIn);
                    return dt>stage2View.timeInput[0]&&dt<stage2View.timeInput[1];
                }
                if(f.strForecast){
                    let dt=new Date(f.strForecast);
                    return dt>stage2View.timeInput[0]&&dt<stage2View.timeInput[1];
                }
                return false;
            });
        }
        return res;
        },
        targetWFOut:function(){
            if(this.Entities.length>0){
           let plan= this.Entities[this.Entities.length-1].strPlan;
           let date=new Date(plan);
           let y=date.getFullYear();
           let m=date.getMonth()+1;
           m=m<10?'0'+m:m;
           let d=date.getDate();
           d=d<10?'0'+d:d;
           return y+""+m+""+d;}else return "";
        },
        forecastWFOut:function(){
            if(this.Entities.length>0){
            let fore= this.Entities[this.Entities.length-1].strForecast;
           let date=new Date(fore);
           let y=date.getFullYear();
           let m=date.getMonth()+1;
           m=m<10?'0'+m:m;
           let d=date.getDate();
           d=d<10?'0'+d:d;
           return y+""+m+""+d;}else return "";
        },
        spanArray:function(){
            let arry=[];
            let pos=0;
            for (let i = 0; i < this.filterdEntities.length; i++) {
                    if (i === 0) {
                          arry.push(1);
                    } 
                    else if(!this.filterdEntities[i].Qtime){
                        arry.push(1);
                        pos = i;
                    }
                    else {
                      // 判断当前元素与上一个元素是否相同
                        if (this.filterdEntities[i].Qtime === this.filterdEntities[i-1].Qtime&&this.filterdEntities[i].QtimeType===this.filterdEntities[i-1].QtimeType) {
                            arry[pos] += 1;
                            arry.push(0);
                          } else {
                            arry.push(1);
                            pos = i;
                          }
                    }
                }
            return arry;
        }
    },
    methods:{
        handleRowDblClick(row){
            if(tableView0.form.key==undefined){
                tableView0.dialogFormVisible=true;
                this.selRow=row;
            }else{
                this.filterdEntities.map(m=>m.EditState=false);
                this.selRow=row;
                row.EditState=true;
            }
        },
        handleUpdateRmk(){
            let data={
                LotID:stage2View.LotID,
                ModulePD:stage2View.selRow.ModulePD,
                OpeNo:stage2View.selRow.OpeNO,
                Remark:stage2View.selRow.Remark
            };
            let url="RemarkHandle";
            PostAjaxGetJson(data,url,function(response){
                if(response.success){
                    stage2View.selRow.EditState=false;
                    stage2View.selRow=null;
                    stage2View.$message.success(response.Msg);
                }else{
                    stage2View.$message.error(response.Msg);
                    console.log(response.Error);
                }
            },function(){
                stage2View.$message.error("服务错误");
            });
        },
        show(){
            this.canvas = this.$refs.canvas;//指定canvas
            if(this.canvas){
            this.ctx = this.canvas.getContext("2d");//设置2D渲染区域
                let labels= modalView.Departments;
                let data=[];
                for(let i=0;i<labels.length;i++){
                let j=this.ChartModels.find(f=>f.Department==labels[i]);
                    data.push(j?j.gap:0);
                }
                let datasets=[{
                data:data
                }];
                let chartData= {labels:labels,datasets:datasets};
                var myBarChart= new Chart(this.ctx,{type:'bar',data:chartData,options:this.chartOptions});
         }
        },
        objSpanMethod({ row, column, rowIndex, columnIndex }){
            if(columnIndex===5){
                const _row = this.spanArray[rowIndex];
                const _col = _row > 0 ? 1 : 0;
                return {
                  rowspan: _row,
                  colspan: _col
                }
            }
        },
        tableCellStyle({row,column,rowIndex,columnIndex}){
            return 'border: 1px solid black;'
        },
        outputExcel(){
            if(this.filterdEntities.length==0)return false;
            let data=[];
            let filted=this.filterdEntities;
            for(var i =0;i<filted.length;i++){
                data.push({
                    OpeNo:filted[i].OpeNO,
                    OpeName:filted[i].OpeName,
                    Plan:filted[i].strPlan,
                    Forecast:filted[i].strForecast,
                    Qtime:filted[i].strQtime,
                    Department:filted[i].Department,
                    EqpList:filted[i].EqpList.map(m=>m.EqpID).join('/'),
                    EqpStatus:filted[i].EqpList.map(m=>m.E10Status+'('+m.StateID+')').join('/'),
                    PMS:filted[i].EqpList.map(m=>{if(filted[i].strWFIn)return '';else if(m.Description){return m.PMS_Early_Time+'~'+m.PMS_Late_Time;}else return '待确认'; }).join('/'),
                    ProdTime:filted[i].PRSecond,
                    CT:filted[i].CTSecond,
                    StepIn:filted[i].strWFIn,
                    StepOut:filted[i].strStepComplete,
                    StepGap:filted[i].strStepGap
                });
            }
            let filename="SHL_FLOW_"+this.LotID+".xls";
            let tableHtml=FormExcelContext(data);
            if(!tableHtml)return false;
            let ctx= { worksheet: "sheet1" , table: tableHtml };
            let dlink=this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = filename;
            dlink.click();
        }
    }
});

function query(){
    var url="GetMainTableView";
    var data={Priority:"1,2"};
    $('#circle').css('display', 'block');
    PostAjaxGetJson(data,url,function(responseData){
        if(responseData.successed){
            tableView0.FullRows=responseData.Rows;
            modalView.quotaProd.Rows=responseData.ProductQuota.Rows;
            modalView.quotaDept.Rows=responseData.DepartmentQuota.Rows;
            modalView.quotaProj.Rows=responseData.ProjectQuota.Rows;
            modalView.quotaNormal.Rows=responseData.NormalQuota.Rows;
        }else{
            console.log(responseData.msg);
        }
        $('#circle').css('display', 'none');
    },function(responseData){
        alert(responseData);
        $('#circle').css('display', 'none');
    });
}

function queryClick(){
    tableView0.Conditions=checkselect("bootstrap-duallistbox-selected-list_");
}

function showDialog(){
    modalView.dialogQuotaVisible=true;
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

