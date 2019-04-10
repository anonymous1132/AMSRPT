Array.prototype.distinct = function () {

    var ret = [];

    for (var i = 0; i < this.length; i++) {

        for (var j = i + 1; j < this.length;) {

            if (this[i] === this[j]) {

                ret.push(this.splice(j, 1)[0]);

            } else {

                j++;

            }

        }

    }

    return ret;

}

$(document).ready(function () {
    let url = 'GetSelectOptions'
    PostAjaxGetJson(null, url, response => {
        if (response.success) {
            nonLot.DepartmentEntities = response.DepartmentEntities
            nonLot.EqpTypeEntities = response.EqpTypeEntities
            nonLot.loading = false
        } else {
            nonLot.$message.error(response.msg)
            nonLot.loading = false
        }
    }, () => { nonLot.$message.error('初始化错误') });
});

var nonLot = new Vue({
    el: '#app',
    data: {
        activeNames: ['1'],
        selDepts: [],
        selEqpTypes: [],
        selEqps: [],
        loading: true,
        DepartmentEntities: [],
        EqpTypeEntities: [],
        RowEntities: [],
        EdcSetEntities: { Rows: [], sel: null, selRow: null },
        dialogVisible: false,
        DcItems: [],
        filtedEqpId: '',
        filtedEdcPlan: '',
        diaTableLoading:true,
        diaDcItemLoading:true
    },
    computed: {
        avaEqpTypes: function () {
            let result = []
            let selDepts = this.selDepts
            if (selDepts.length == 0) {
                result = this.EqpTypeEntities.map(m => m.EqpType)
            } else {
                this.DepartmentEntities.forEach(element => {
                    if (selDepts.find(f => f == element.Department)) {
                        element.EqpTypes.forEach(fe => {
                            result.push(fe)
                        })
                    }
                });
            }
            result.distinct()
            return result
        },
        avaEqps: function () {
            let result = []
            let selEqpTypes = this.selEqpTypes
            let avaEqpTypes = this.avaEqpTypes
            if (selEqpTypes.length == 0) {
                this.EqpTypeEntities.forEach(element => {
                    if (avaEqpTypes.find(f => f == element.EqpType)) {
                        element.Eqps.forEach(fe => { result.push({ eqp: fe }) })
                    }
                });
            } else {
                this.EqpTypeEntities.forEach(element => {
                    if (selEqpTypes.find(f => f == element.EqpType)) {
                        element.Eqps.forEach(fe => { result.push({ eqp: fe }) })
                    }
                })
            }
            result.distinct()
            return result
        },
        allEqps: function () {
            let result = []
            this.EqpTypeEntities.forEach(element => {
                element.Eqps.forEach(fe => { result.push(fe) })
            })
            result.sort()
            return result
        },
        filtedEdcSetEntities: function () {
            return this.EdcSetEntities.Rows.filter(f => f.EditState == true || (f.EqpID.search(this.filtedEqpId) > -1 && f.EdcPlan.search(this.filtedEdcPlan) > -1));
        },
        diaLoading:function(){
            return this.diaTableLoading||this.diaDcItemLoading;
        }
    },
    methods: {
        handleQueryClick() {
            this.loading = true
            let url = 'GetTableViewModel'
            let data = this.selEqps
            PostAjaxGetJson(data, url, response => {
                if (response.success) {
                    let redata=response.RowEntities
                    nonLot.RowEntities=[]
                    for(let i=0;i<redata.length;i++){
                        let d={}
                        d.EqpID=redata[i].EqpID
                        d.EdcEntities=[]
                        for(let j=0;j<redata[i].EdcEntities.length;j++){
                            d.EdcEntities.push({
                                EdcPlan:redata[i].EdcEntities[j].EdcPlan,
                                Period:redata[i].EdcEntities[j].Period+'('+redata[i].EdcEntities[j].PeriodType+')',
                                TestTime:redata[i].EdcEntities[j].TestTime,
                                SpecResult:redata[i].EdcEntities[j].SpecResult?'OK':'NG',
                                Count:redata[i].EdcEntities[j].TestTime===''?'':redata[i].EdcEntities[j].Count
                            })
                            d.EdcEntities[j].SpecResult=d.EdcEntities[j].Count===''?'':d.EdcEntities[j].SpecResult
                        }
                        nonLot.RowEntities.push(d)
                    }
                } else {
                    nonLot.$message.error(response.msg)
                }
                nonLot.loading = false
            }, () => {
                nonLot.$message.error('发生异常错误')
            })
        },
        handleSetClick() {
            this.dialogVisible = true
        },
        outputExcel() {
            let data = []
            let rawData = this.RowEntities
            for (let i = 0; i < rawData.length; i++) {
                for (let j = 0; j < rawData[i].EdcEntities.length; j++) {
                    data.push({
                        EqpID: rawData[i].EqpID,
                        EdcPlan: rawData[i].EdcEntities[j].EdcPlan,
                        Period: rawData[i].EdcEntities[j].Period,
                        TestTime: rawData[i].EdcEntities[j].TestTime,
                        Result: rawData[i].EdcEntities[j].SpecResult,
                        Count: rawData[i].EdcEntities[j].Count
                    })
                }
            }
            let filename = "Non_Lot General Report.xls";
            let tableHtml = FormExcelContext(data);
            if (!tableHtml) return false;
            let ctx = { worksheet: "sheet1", table: tableHtml };
            let dlink = this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = filename;
            dlink.click();
        },
        show() {
            if(!this.diaLoading)return;
                let url = 'GetDcitemList'
                PostAjaxGetJson(null, url, response => {
                    if (response.success) {
                        nonLot.DcItems = response.DcItems
                    } else {
                        nonLot.$message.warning('服务端查询异常')
                        console.log(response.msg)
                    }
                    nonLot.diaDcItemLoading=false
                }, () => {
                    nonLot.$message.warning('出现异常')
                    nonLot.diaDcItemLoading=false
                });
                url2='GetEDCTable'
                PostAjaxGetJson(null,url2,response=>{
                    if(response.success){
                        let data=response.EdcTable
                        data.map(m=>m.EditState=false)
                        nonLot.EdcSetEntities.Rows=data
                    }else{
                        nonLot.$message.warning('服务端查询异常')
                        console.log(response.msg)
                    }
                    nonLot.diaTableLoading=false
                },()=>{
                    nonLot.$message.warning('出现异常')
                    nonLot.diaTableLoading=false
                })
            
        },
        edcAdd() {
            for (let i of this.EdcSetEntities.Rows) {
                if (i.EditState) return this.$message.warning("请先保存当前编辑项");
            }
            let j = { id: 0, EqpID: '', EdcPlan: '', Period: 1, PeriodType: 'D', EditState: true }
            this.EdcSetEntities.sel = JSON.parse(JSON.stringify(j))
            this.EdcSetEntities.selRow = null
            this.EdcSetEntities.Rows.push(j)
        },
        edcChange(row, index, flag) {
            for (let i of this.EdcSetEntities.Rows) {
                if (i.EditState&&i.id!=row.id) return this.$message.warning("请先保存当前编辑项");
            }
            //是否是取消操作
            if (!flag) {
                //如果取消新增行，则删除新增行
                if (!this.EdcSetEntities.sel.id) {
                    let i = this.EdcSetEntities.Rows.indexOf(row)
                    this.EdcSetEntities.Rows.splice(i, 1)
                };
                return row.EditState = !row.EditState;
            }
            //提交数据
            if (row.EditState) {
                if (!this.EdcSetEntities.sel.EqpID) {
                    return this.$message.warning("EqpID不能为空");
                }
                if (!this.EdcSetEntities.sel.EdcPlan) {
                    return this.$message.warning("EdcPlan不能为空");
                }
                (function () {
                    let data = JSON.parse(JSON.stringify(nonLot.EdcSetEntities.sel));
                    //增加一个提交数据的操作
                    let url = "MaintainEDCTable";
                    let post = {
                        OpeType: nonLot.EdcSetEntities.sel.id === 0 ? 'insert' : 'update',
                        EqpID: nonLot.EdcSetEntities.sel.EqpID,
                        EdcPlan: nonLot.EdcSetEntities.sel.EdcPlan,
                        Period: nonLot.EdcSetEntities.sel.Period,
                        PeriodType: nonLot.EdcSetEntities.sel.PeriodType
                    };
                    PostAjaxGetJson(post, url, response => {
                        if (response.success) {
                            for (let k in data) row[k] = data[k];
                            nonLot.$message.success(response.msg);
                            //然后这边重新读取表格数据
                            row.id = response.newID;
                            row.EditState = false;
                        } else {
                            nonLot.$message.error(response.msg);
                        }
                    }, () => {
                        nonLot.$message.error('操作发生异常');
                    });
                })();
            } else {
                //点击修改进入编辑状态
                this.EdcSetEntities.sel = JSON.parse(JSON.stringify(row));
                this.EdcSetEntities.selRow = row;
                row.EditState = true
            }
        },
        edcDel(row, index) {
            let url = "MaintainEDCTable";
            let post = {
                OpeType:'delete',
                EqpID: row.EqpID,
                EdcPlan: row.EdcPlan,
                Period: 0,
                PeriodType: ''
            };
            PostAjaxGetJson(post, url, response => {
                if (response.success) {
                    let i = nonLot.EdcSetEntities.Rows.indexOf(row)
                    nonLot.EdcSetEntities.Rows.splice(i, 1)
                    nonLot.$message.success(response.msg);
                } else {
                    nonLot.$message.error(response.msg);
                }
            }, () => {
                nonLot.$message.error("操作发生异常");
            });
        }
    },

});

