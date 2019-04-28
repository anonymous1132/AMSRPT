var app=new Vue({
    el:'#app',
    data:{
        prods:[],
        outType:'wafer',
        monthValue:new Date(),
        activeNames:['1'],
        loading:true,
        tableData:{
            items:[],
            shipData:[],
            wipData:[]
        },
        dialogSetData:{
            visible:false,
            loading:false,
            viewMonth:new Date(),
            items:[],
            shipData:[],
            wipData:[],
            selProd:'',
            setMonth:new Date(),
            planType:'wip',
            fromDate:1,
            toDate:31,
            setValue:0,
            btnLoading:false
        },
        keyData: {
            dialogFormVisible: false,
            dialogUpdateVisible: false,
            changeKeyBtnVisible:false,
            formLabelWidth: '120px',
            form: {
                key: undefined,
                inFlag:false
            },
            update: {
                oldKey: "",
                newKey: ""
            },
            loading:false
        }
    },
    computed:{
        tableShow:function(){
            return this.tableData.items.length>0
         }
    },
    watch:{

    },
    methods:{
        handleSetClick() {
            if (this.keyData.form.inFlag) {
                this.dialogSetData.visible = true
            } else {
                this.keyData.dialogFormVisible = true
            }
        },
        handleQueryClick(){
            if(!this.monthValue)return this.$message.error('请选择月份')
            let prods=this.prods
            if(prods.length==0)return this.$message.error('没有获取到product列表')
            let month=this.fixDateValue(this.monthValue)
            this.loading=true
            let vue=this
            let data={
                prods:prods,
                month:month,
            }
            let url="GetTableData"
            PostAjaxGetJson(data,url,response=>{
                if(response.success){
                    vue.tableData.shipData=response.shipData
                    vue.tableData.wipData=response.wipData
                    vue.tableData.items=response.items
                    if(vue.dialogSetData.items.length==0){
                        vue.dialogSetData.viewMonth=vue.monthValue
                        vue.dialogSetData.items=vue.tableData.items
                        vue.dialogSetData.wipData=vue.tableData.wipData
                        vue.dialogSetData.shipData=vue.tableData.shipData
                    }
                }else{
                    vue.$message.error('服务端程序发生异常')
                    console.log(response.msg)
                }
                vue.loading=false
            },()=>{
                vue.$message.error('发生异常')
                console.log('前端解析错误')
                vue.loading=false
            })
        },
        handleExportClick(){
            let html='<tr><td>WIP</td></tr>'
            html+=this.$refs.wipOutTable.innerHTML
            html+='<tr><td>Finished</td></tr>'
            html+=this.$refs.shipOutTable.innerHTML
            let ctx = { worksheet: "sheet1", table: html };
            let dlink = this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = 'WaferOutSummary'+'_'+this.fixDateValue(this.monthValue)+'.xls';
            dlink.click();
        },
        fixDateValue(date){
            let y=date.getFullYear()
            let m=date.getMonth()+1
            m=m<10?'0'+m:m
            return y+'-'+m
        },
        diaDatePickerChanged(){
            if(!this.dialogSetData.viewMonth)return this.$message.error('请选择月份')
            let prods=this.prods
            if(prods.length==0)return this.$message.error('没有获取到product列表')
            this.dialogSetData.loading=true
            let month=this.fixDateValue(this.dialogSetData.viewMonth)
            let vue=this
            let data={
                prods:prods,
                month:month,
            }
            let url="GetTableData"
            PostAjaxGetJson(data,url,response=>{
                if(response.success){
                    vue.dialogSetData.items=response.items
                    vue.dialogSetData.wipData=response.wipData
                    vue.dialogSetData.shipData=response.shipData
                }else{
                    vue.$message.error('服务端程序发生异常')
                    console.log(response.msg)
                }
                vue.dialogSetData.loading=false
            },()=>{
                vue.$message.error('发生异常')
                console.log('前端解析错误')
                vue.dialogSetData.loading=false
            })
        },
        handleSubmitByCmd(){
            if(!this.dialogSetData.setMonth)return this.$message.error("请选择月份")
            let url='HandlePlanTableByCmd'
            let data={
                prod:this.dialogSetData.selProd,
                month:this.fixDateValue(this.dialogSetData.setMonth),
                fromDate:this.dialogSetData.fromDate,
                toDate:this.dialogSetData.toDate,
                value:this.dialogSetData.setValue,
                planType:this.dialogSetData.planType
            }
            let vue=this
            if(!data.prod)return this.$message.error("请选择产品")
            this.dialogSetData.btnLoading=true
            PostAjaxGetJson(data,url,response=>{
                if(response.success){
                    vue.$message.success('提交设定成功');
                }else{
                    vue.$message.error('提交失败')
                    console.log(response.msg)
                }
                vue.dialogSetData.btnLoading=false
            },()=>{
                vue.$message.error('提交过程出现异常')
                console.log('前端解析错误')
                vue.dialogSetData.btnLoading=false
            })
        },
        handleDiaSetOpened(){
            if(this.dialogSetData.items.length==0){
                this.diaDatePickerChanged();
            }
        },
        handleQueryLot(data,type){
            let lots=data.Lots
            if(lots.length==0)return this.$message.info("No lot list")
            this.loading=true
            let post={lots:lots,type:type}
            let uri="GetLotDetail"
            let vue=this
            PostAjaxGetJson(post,uri,response=>{
                if(response.success){
                    //显示lot detail 代码
                    vue.$message.info("OK")
                }else{
                vue.$message.error("发生异常")
                console.log(response.msg)
                }
                vue.loading=false
            },()=>{
                vue.$message.error("发生异常")
                console.log('前端解析错误')
                vue.loading=false
            })
        },
        handleDialogCancel() {
          this.keyData.form.key = undefined;
          this.keyData.dialogFormVisible = false;
        },
        handleDialogOK() {
            if (!this.keyData.form.key) { this.keyData.dialogFormVisible = false; return; }
            let data = { key: this.keyData.form.key };
            let url = "CheckKey"
            this.keyData.loading=true
            let vue=this
            PostAjaxGetJson(data, url, response => {
                if (response.success) {
                    vue.keyData.dialogFormVisible = false;
                    vue.keyData.changeKeyBtnVisible = true;
                    vue.dialogSetData.visible = true;
                    vue.keyData.form.inFlag=true
                } else {
                    vue.$message.error(response.msg)
                }
                vue.keyData.loading=false
            }, () => {
                vue.$message.error("交互异常")
                console.log("前端异常")
                vue.keyData.loading=false
            })
        },
        handleChangeKeyBtnClick() {
            this.keyData.dialogUpdateVisible = true;
        },
        handleUpdateCancel() {
            this.keyData.dialogUpdateVisible = false;
            this.keyData.update.oldKey = "";
            this.keyData.update.newKey = "";
        },
        handleUpdateOK() {
            let data = { newKey: this.keyData.update.newKey, oldKey: this.keyData.update.oldKey };
            let url = "updateKey";
            this.keyData.loading=true
            let vue = this
            PostAjaxGetJson(data, url, response => {
                if (response.success) {
                    vue.$message.success(response.msg);
                    vue.keyData.dialogUpdateVisible = false;
                    vue.keyData.update.oldKey = "";
                    vue.keyData.update.newKey = "";
                } else {
                    vue.$message.error(response.msg);
                    vue.keyData.update.oldKey = "";
                    vue.keyData.update.newKey = "";
                }
                vue.keyData.loading=false
            }, () => {
                vue.$message.error("交互异常")
                console.log("前端错误")
                vue.keyData.loading=false
                })
        }
    },
    mounted(){
        let vue=this
        let url="../Common/GetAllProduct"
        let post={type:6}
        PostAjaxGetJson(post,url,response=>{
            if(response.success){
                vue.prods=response.prods
                vue.loading=false
            }else{
                vue.$message.error('获取Product列表失败')
                console.log(response.msg)
            }
        },()=>{
            vue.$message.error('获取Product列表失败')
            console.log('前端解析错误')
        })
    }
})