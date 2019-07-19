

var app=new Vue({
    el:'#app',
    data:{
        prods:[],
        selProd:[],
        dateType:'month',
        dateFromValue:new Date(),
        dateToValue:new Date(),
        activeNames:['1'],
        svgChart:null,
        chartOptions:{
            height:500,
            minWidth:800,
            colors: ['gold','green','purple','blue', 'pink', 'DarkGreen', 'gray', 'LightSalmon', 'Olive', 'LawnGreen'],
            marge: {
                top: 50,
                left: 60,
                right: 60,
                bottom: 50
            },
            barWidth:30,
            minXinterval:40,
        },
        tableData:{
            tableTitle:'', 
            prods:[],
            items:[],
            showTarget:false,
            actTotals:[],
            planTotals:[],
            gapTotals:[],
            originalTarget:0,
            currentTarget:0
        },
        loading:false,
        dialogData:{
            prods:[],
            items:[],
        },
        dialogVisible:false,
        dialogDate:new Date(),
        setType:'cmd',
        setEntity:{
            selProd:'',
            month:new Date(),
            fromDate:1,
            toDate:31,
            value:0,
            loading:false,
        },
        lotInfoEntities:[],
        lotDetailVisible: false,
        keyData: {
            form: {
                key: undefined,
                inFlag:false
            },
            dialogFormVisible: false,
            dialogUpdateVisible: false,
            changeKeyBtnVisible: false,
            formLabelWidth: '120px',
            update: {
                oldKey: "",
                newKey: ""
            },
            loading: false
        }
    },
    computed:{
        dpPlaceholder:function(){
            return this.dateType==='month'?'请选择月':'请选择年'
        },
        tableShow:function(){
           return this.tableData.tableTitle!==''
        }
    },
    watch:{

    },
    methods:{
        handleSetClick() {
            if (!this.keyData.inFlag) {
               this.keyData.dialogFormVisible=true
            } else {
                this.dialogVisible = true
            }
        },
        handleQueryClick(){
            if(!this.dateFromValue&&this.dateToValue)return this.$message.error('请选择正确的时间范围')
            let from=this.fixDateValue(this.dateFromValue)
            let to=this.fixDateValue(this.dateToValue)
            let prodList=this.selProd.length>0?this.selProd:this.prods
            this.loading=true
            let vue=this
            let data={
                ProdList:prodList,
                DateType:this.dateType,
                DateFromValue:from,
                DateToValue:to
            }
            let url="GetTableData"
            PostAjaxGetJson(data,url,response=>{
                if(response.success){
                    vue.tableData.tableTitle=response.Title
                    vue.tableData.showTarget=response.ShowTarget
                    vue.tableData.items=response.Items 
                    vue.tableData.prods = response.ProductEntities.sort((a, b) => { if (a.ProductID === b.ProductID) return 0; if (a.ProductID < b.ProductID) return -1; return 1 })
                    vue.tableData.actTotals=[]
                    vue.tableData.planTotals=[]
                    vue.tableData.gapTotals=[]
                    let prods=vue.tableData.prods

                  //适配item
                  if(data.DateType=='month'){
                    if(from.split('-')[0]==to.split('-')[0]){
                        vue.tableData.items=vue.tableData.items.map(m=>{
                            let temp=m.split('-')
                            return temp[1]+'-'+temp[2]
                        })
                    }
                  }
                  vue.tableData.items.push('total')

                    //为每一项添加footer数据
                    for(var i=0;i<vue.tableData.items.length;i++){
                        let planTotal=0
                        let actTotal=0
                        for(var j=0;j<prods.length;j++){
                            planTotal += prods[j].Plans[i].Plan
                            actTotal += prods[j].Plans[i].Act
                        }
                        vue.tableData.actTotals.push(actTotal)
                        vue.tableData.planTotals.push(planTotal)
                        vue.tableData.gapTotals.push(actTotal-planTotal)
                    }
                    vue.tableData.originalTarget=eval(prods.map(m=>m.OriginalTarget).join('+'))
                    vue.tableData.currentTarget=eval(prods.map(m=>m.CurrentTarget).join('+'))
                    vue.renderChart()
                }else{
                    vue.$message.error('查询发生异常,error code:1')
                    console.log(response.msg)
                }
                vue.loading=false
            },()=>{
                 vue.$message.error('查询发生异常,error code:2')
                 console.log('前端解析错误')
                 vue.loading=false
                })
        },
        handleExportClick(){
            let uri = 'data:application/vnd.ms-excel;base64,'
            let template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"' +
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
        '</head><body ><table class="excelTable">{table}</table></body></html>'
         let table=this.$refs.waferStartTable
         let dlink= this.$refs.dlink
         let ctx = { worksheet: 'Sheet1', table: table.innerHTML }
         dlink.href = uri + base64(format(template, ctx))
         dlink.download = 'WaferStartReport.xls'
         dlink.click()
        },
        fixDateValue(date){
            let y=date.getFullYear()
            let m=date.getMonth()+1
            m=m<10?'0'+m:m
            return y+'-'+m
        },
        show(){
            let date=this.fixDateValue(this.dialogDate)
            let data={
                ProdList:this.prods,
                DateType:'month',
                DateFromValue:date,
                DateToValue:date
            }
            let url="GetTableData"
            let vue=this
            PostAjaxGetJson(data,url,response=>{
                if(response.success){
                    vue.dialogData.items=response.Items 
                    vue.dialogData.prods=response.ProductEntities
                    vue.dialogData.prods.forEach(element => {
                        let idx=element.Plans.length-1
                        element.Plans.splice(idx,1)
                    });
                }else{
                    console.log(response.msg)
                }
            },()=>{
                console.log('前端解析错误,error code 4')
            })
        },
        diaDatePickerChanged(){
            if(!this.dialogDate)return
            this.show()
        },
        handleSubmitByCmd(){
            if(!this.setEntity.month)return this.$message.error("请选择月份")
            let url='HandlePlanTableByCmd'
            let data={
                prod:this.setEntity.selProd,
                month:this.fixDateValue(this.setEntity.month),
                fromDate:this.setEntity.fromDate,
                toDate:this.setEntity.toDate,
                value:this.setEntity.value
            }
            let vue=this
            if(!data.prod)return this.$message.error("请选择产品")
            this.setEntity.loading=true
            PostAjaxGetJson(data,url,response=>{
                if(response.success){
                    vue.$message.success('提交设定成功');
                }else{
                    vue.$message.error('提交失败，error code 5')
                    console.log(response.msg)
                }
                vue.setEntity.loading=false
            },()=>{
                vue.$message.error('提交过程出现异常,error code 6')
                console.log('前端解析错误')
                vue.setEntity.loading=false
            })
        },
        handleProdClick(prod){
           let url='GetLotDetailOfProd'
           let data={prod:prod}
           let vue=this
           this.loading=true
           PostAjaxGetJson(data,url,response=>{
               if(response.success){
                    vue.lotInfoEntities=response.data
                    lotDetailVisiable=true
               }else{
                   vue.$message.error(response.msg)
               }
               vue.loading=false
           },()=>{
            vue.$message.error('前端解析错误')
            vue.loading=false
           })
        },
        renderChart(){
            let chart=this.svgChart
            if(!chart)return
            this.removeChart(chart)
            //数据源
            let items=this.tableData.items.filter(f=>f!='total')
            let prods=this.tableData.prods.map(m=>m.ProductID)
            let datas=this.tableData.prods.map(m=>m.Plans.map(ma=>ma.Act))
            let actTotals=this.tableData.actTotals
            let planTotals=this.tableData.planTotals
            actTotals=actTotals.slice(0,actTotals.length-1)
            planTotals=planTotals.slice(0,planTotals.length-1)
            // 设置图表数据，宽度、高度、边值
            let height = this.chartOptions.height
            let marge = this.chartOptions.marge
            let colors = this.chartOptions.colors
            let barWidth = this.chartOptions.barWidth
            let width = items.length * this.chartOptions.minXinterval + marge.left + marge.right
            width = width < this.chartOptions.minWidth ? this.chartOptions.minWidth : width
            let yHeight = height - marge.bottom - marge.top
            let xWidth=width - marge.left - marge.right
            // 画布
            chart
                .attr('width', width)
                .attr('height', height)
            //作图容器
            let svgContainer = chart.append('g')
             .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')

             // x轴、y轴比例尺
             let xScale = d3.scaleBand()
                .domain(items)
                .rangeRound([0, xWidth])
            let yMax=d3.max(actTotals.concat(planTotals))+30
            var yScale = d3.scaleLinear()
                .domain([0, yMax])
                .rangeRound([height - marge.top - marge.bottom, 0])
              // 添加x轴、y轴
            var xAxis = d3.axisBottom(xScale)
            var yAxis = d3.axisLeft(yScale)
                .ticks(15)

            svgContainer.append('g')
                .attr('transform', 'translate(0,' + (height - marge.top - marge.bottom) + ')')
                .call(xAxis)
                .selectAll("text")
                .style("text-anchor", "middle") //start\ middle\ end
                //.attr("dx", ".8em")
                //.attr("dy", "-.55em")
               // .attr("transform", "rotate(90)");

            svgContainer.append('g')
                .attr('transform', 'translate(0,0)')
                .call(yAxis)

            svgContainer.append('text')
                .attr("dy", -10)
                .attr('dx', -10)
                .text("pcs")

               //添加图例
               let tuli_x = marge.left + barWidth;
               this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'line', 'red', 'Plan Total');
               for (let i = 0; i < prods.length; i++) {
                   tuli_x += 150 + barWidth;
                   this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', colors[i], prods[i]);
               }
               
                //添加矩形
               for(let i=0;i<items.length;i++){
                  let arry=[]
                  let sum=0
                  datas.forEach(data => {
                      arry.push({data:data[i],sum:sum})
                      sum+=data[i]
                  });
                  let gs = svgContainer.selectAll('.rect')
                  .data(arry)
                  .enter()
                  .append('g')
                    gs.append('rect')
                    .attr('x', function () {
                        return xScale(items[i])+(xScale.step()-barWidth)/2
                    })
                        .attr('y', function (d, index) {
                        return yScale(d.sum+d.data)
                    })
                    .attr('width', barWidth)
                    .attr('height', function (d) {
                        return  yScale(d.sum)- yScale(d.sum+d.data)
                    })
                    .attr('fill', function (d, index) {return colors[index]})
               }
               let gt=svgContainer.selectAll('.text')
                    .data(items)
                    .enter()
                    .append('g')
                    gt.append('text')
                    .attr('x', function (d) {
                        return xScale(d)+(xScale.step()-barWidth)/2
                    })
                    .attr('y', function (d,index) {
                        return yScale(actTotals[index])
                    })
                    .attr("dy", -2)
                    .attr('dx', barWidth/2)
                    .style("text-anchor", "middle")
                    .text(function (d,index) {
                        return actTotals[index]
                    })


               //

                //添加折线
                var line_generator = d3.line()
                .x(function (d,index) {
                    return xScale(items[index]) + xScale.step()/ 2;
                })
                .y(function (d) {
                    return yScale(d);
                })
                svgContainer.append('path')
                .attr('d', line_generator(planTotals))
                .style('fill', 'none')
                .style('stroke', 'red')

        },
        removeChart(chart){
            chart.selectAll('g').remove()
        },
        renderTuli(container, x, y, type, color, label){
            if (type === 'line') {
                container.append('line')
                    .attr("x1", x)
                    .attr("y1", y)
                    .attr("x2", x + 25)
                    .attr("y2", y)
                    .attr("stroke", color)
                    .attr("stroke-width", 2);

            } else {
                container.append('rect')
                    .attr("x", x)
                    .attr("y", y - 7.5)
                    .attr('height', 15)
                    .attr('width', 20)
                    .attr("stroke", 'black')
                    .attr("stroke-width", 1)
                    .attr('fill', color)
            }
            let offset = type === 'bar' ? 25 : 30;
            container.append('text')
                .style('font-size', '11px')
                .attr('x', x + offset)
                .attr('y', y + 3.5)
                .text(label)
        },
        handleDialogCancel() {
            this.keyData.form.key = undefined;
            this.keyData.dialogFormVisible = false;
        },
        handleDialogOK() {
            if (!this.keyData.form.key) { this.keyData.dialogFormVisible = false; return; }
            let data = { key: this.keyData.form.key };
            let url = "CheckKey"
            this.keyData.loading = true
            let vue = this
            PostAjaxGetJson(data, url, response => {
                if (response.success) {
                    vue.keyData.dialogFormVisible = false;
                    vue.keyData.changeKeyBtnVisible = true;
                    vue.dialogVisible = true;
                    vue.keyData.form.inFlag = true
                } else {
                    vue.$message.error(response.msg)
                }
                vue.keyData.loading = false
            }, () => {
                vue.$message.error("交互异常")
                console.log("前端异常")
                vue.keyData.loading = false
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
            this.keyData.loading = true
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
                vue.keyData.loading = false
            }, () => {
                vue.$message.error("交互异常")
                console.log("前端错误")
                vue.keyData.loading = false
            })
        }
    },
    mounted(){
        let vue=this
        vue.svgChart=d3.select(this.$refs.svgChart)
        let url="../Common/GetAllProduct"
        let post={type:6}
        PostAjaxGetJson(post,url,response=>{
            if(response.success){
                vue.prods=response.prods.sort()
            }else{
                vue.$message.error('获取Product列表失败,error code 7')
                console.log(response.msg)
            }
        },()=>{
            vue.$message.error('获取Product列表失败,error code 8')
            console.log('前端解析错误')
        })
    }
});

