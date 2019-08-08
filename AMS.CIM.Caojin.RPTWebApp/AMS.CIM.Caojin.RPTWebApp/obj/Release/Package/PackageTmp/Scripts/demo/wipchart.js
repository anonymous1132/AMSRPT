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

var wipChart = new Vue({
    el: '#app',
    data: {
        activeNames: ['1'],
        queryClicked: false,
        selGroups: [],
        remSec: 300,
        loading: false,
        subLoading:false,
        curChart: [],
        curTable: [],
        ystdChart: [],
        ystdTable: [],
        prods: [],
        timer: null,
        updateTime: "",
        sBtnCtx: "Stop",
        queryProds: [],
        wipType: "out",
        timeType: "cur",
        barWidth: 8,
        subBarWidth:15,
        barSpace: 0,
        marge: {
            top: 50,
            left: 60,
            right: 60,
            bottom: 160
        },
        options: {
            height: 600,
            key: '',
            month: '',
            day:'',
            prod: []
        },
        colors: ['blue', 'pink', 'DarkGreen', 'gray', 'LightSalmon', 'Olive', 'LawnGreen', 'Teal','	Indigo'],
        dialogSubChartVisible:false,
        wipSvg:null,
        subSvg:null,
        subRenderPara:'',
        lotInfoEntities:[]
    },
    methods: {
        handleQueryClick() {
            this.queryClicked = true;
            //this.renderChart();
        },
        handleUpdtClick() {
            this.remSec = 0;
            this.sBtnCtx = "Stop";
        },
        handleStopClick() {
            if (this.sBtnCtx === "Stop") {
                clearInterval(this.timer);
                this.sBtnCtx = "Start";
            } else {
                this.setTimer();
                this.sBtnCtx = "Stop";
            }
        },
        setTimer() {
            this.timer = setInterval(() => {
                if (this.remSec > 0) this.remSec--;
            }, 1000)
        },
        getWipByLots(lotEntities) {
            let wip = 0
            for (let i = 0; i < lotEntities.length; i++) {
                wip+=lotEntities[i].Qty
            }
            return wip
        },
        update() {
            var url = "GetChartData";
            wipChart.loading = true;
            PostAjaxGetJson(null, url, function (data) {
                if (data.success) {
                    wipChart.curChart = data.CurChart;
                    wipChart.ystdChart = data.YstdChart;
                    wipChart.curTable = data.CurTable;
                    wipChart.ystdTable = data.YstdTable;
                    wipChart.prods = data.Prods;
                } else {
                    wipChart.$message.error(data.msg);
                }
                wipChart.loading = false;
            }, function (data) {
                wipChart.loading = false;
                wipChart.$message.error(data);
            });
        },
        renderChart() {
            var chartSvg=this.wipSvg;
            this.removeChart(this.wipSvg);
            var chartData = this.chartData;
           // console.log(chartData);
            if ((!chartData)|| chartData.length==0) return;
           // 设置图表数据，宽度、高度、边值
            var height = this.options.height;
            var marge = this.marge;
            var colors = this.colors;
            var prod = this.options.prod;
            var barWidth = this.barWidth;
            var barSpace = this.barSpace;
            var width = chartData.length * (barWidth + barSpace) + marge.left + marge.right;
            width = width < 1400 ? 1400 : width;
            let yHeight = height - marge.bottom - marge.top
            let xWidth=width - marge.left - marge.right
            // 画布
               chartSvg
                .attr('width', width)
                .attr('height', height)
  
            var svgContainer = chartSvg.append('g')
                // 作图的容器
                .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
            // x轴、y轴比例尺
            var xScale = d3.scaleBand()
                .domain(chartData.map(function (d) {
                    return d.stage + '(' + d.ct + ')'
                }))
                .rangeRound([0, xWidth])
            let max = d3.max(chartData.map(function (d) {
                return d.accWip
            }))
            max=Math.ceil(max/10)*10
            var y2Scale = d3.scaleLinear()
                .domain([0, max])
                .rangeRound([yHeight, 0])
            max = d3.max(chartData.map(function (d) {
                //return d.holdLot + d.superHot + d.hot + d.holdSuperLot + eval(d.other.join('+'));
                return d.holdLot + d.superHot + d.hot + eval(d.other.join('+'));
            }))
            max = Math.ceil(max/10)*10 
            var y1Scale = d3.scaleLinear()
                .domain([0,max])
                .rangeRound([yHeight, 0])
            // 添加x轴、y轴
            var xAxis = d3.axisBottom(xScale)
            var y1Axis = d3.axisLeft(y1Scale)
                .ticks(10)
            var y2Axis = d3.axisRight(y2Scale)
                .ticks(10)
            svgContainer.append('g')
                .attr('transform', 'translate(0,' + (yHeight) + ')')
                .call(xAxis)
                .selectAll("text")
                .style("text-anchor", "start")
                .attr("dx", ".8em")
                .attr("dy", "-.25em")
                .attr("font-size", "1.3rem")
                .attr("stroke", "black")
                .attr("stroke-width", ".4")
                //.attr("textLength", marge.bottom - 20)
               // .attr("lengthAdjust","spacingAndGlyphs")
                .attr("transform", "rotate(90)");

            svgContainer.append('g')
                .attr('transform', 'translate(0,0)')
                .call(y1Axis)

            svgContainer.append('text')
                .attr("dy", -10)
                .attr('dx', -10)
                .text("(lots)")
            svgContainer.append('g')
                .attr('transform', 'translate(' + xWidth + ',0)')
                .call(y2Axis)
            svgContainer.append('text')
                .attr('dy', -10)
                .attr('dx', xWidth)
                .style("fill", "orange")
                .text("(pcs)");


            //添加图例
            let tuli_x = marge.left + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'line', 'orange', 'Wip Accum');
            tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'line', 'green', 'StageOut Move');
            tuli_x += 130 + barWidth;
            //this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', 'black', 'Hold Super Lot');
            //tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'dashBar', 'red', 'Hold Lot');
            tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', 'gold', 'S.Hot Lot');
            tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', 'purple', 'Hot Lot');
            for (let i = 0; i < prod.length; i++) {
                tuli_x += 140 + barWidth;
                this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', colors[i], prod[i]);
            }


            //添加key line+month line
            let key = this.options.key
            let month = this.options.month
            let xk =key=='wafer out'? xWidth:xScale(key) +  xScale.step() /2
            let xm =month=='wafer out'?xWidth: xScale(month) + xScale.step() /2
           //绘制两线之间颜色块
            if (xk < xm) {
                svgContainer.append('rect')
                //    .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
                    .attr('x', xk)
                    .attr('y', 0)
                    .attr('height', yHeight)
                    .attr('width', xm - xk)
                    .attr('stroke', 'orange')
                    .attr('stroke-width', 2)
                    .attr('fill', 'lightgreen')
                    .attr('stroke-opacity', 0)
                    .attr('fill-opacity', 0.2)
            } else if (xk > xm) {
                svgContainer.append('rect')
                  //  .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
                    .attr('x', xm)
                    .attr('y', 0)
                    .attr('height', yHeight)
                    .attr('width', xk - xm)
                    .attr('stroke', 'orange')
                    .attr('stroke-width', 2)
                    .attr('fill', 'pink')
                    .attr('stroke-opacity', 0)
                    .attr('fill-opacity', 0.2)
            }

            // 添加矩形
            var gs = svgContainer.selectAll('.rect')
                .data(chartData)
                .enter()
                .append('g')
          /*  
                //hold super bar
                gs.append('rect')
                .attr('class', 'myRect')
                .attr('x', function (d, i) {
                    return xScale(d.stage + '(' + d.ct + ')') + (xScale.step() - barWidth) / 2
                })
                .attr('y', function (d) {
                    return y1Scale(d.holdSuperLot)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                    return height - marge.top - marge.bottom - y1Scale(d.holdSuperLot)
                })
                .attr('fill', 'black') */

            //hold bar
            gs.append('rect')
               // .attr('class', 'myRect')
                .attr('x', function (d, i) {
                    return xScale(d.stage+'('+d.ct+')')+(xScale.step()-barWidth)/2
                })
                .attr('y', function (d) {
                    //return y1Scale(d.holdLot+d.holdSuperLot)
                    return y1Scale(d.holdLot)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                    //return y1Scale(d.holdSuperLot) - y1Scale(d.holdLot+d.holdSuperLot)
                    return yHeight - y1Scale(d.holdLot)
                })
                .attr('stroke', 'red')
                .attr("stroke-width", 1.5)
                .attr('stroke-dasharray', '4,2')
                .attr('fill', 'none')


            //super hot bar
            gs.append('rect')
              //  .attr('class', 'myRect')
                .attr('x', function (d, i) {
                    return xScale(d.stage+'('+d.ct+')')+(xScale.step()-barWidth)/2
                })
                .attr('y', function (d) {
                   // return y1Scale(d.holdLot + d.superHot+d.holdSuperLot)
                    return y1Scale(d.holdLot + d.superHot)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                   // return y1Scale(d.holdLot+d.holdSuperLot) - y1Scale(d.holdLot + d.superHot+d.holdSuperLot)
                    return y1Scale(d.holdLot) - y1Scale(d.holdLot + d.superHot)
                })
                .attr('fill', 'gold')

            //hot bar
            gs.append('rect')
               // .attr('class', 'myRect')
                .attr('x', function (d, i) {
                    return xScale(d.stage+'('+d.ct+')')+(xScale.step()-barWidth)/2
                })
                .attr('y', function (d) {
                    //return y1Scale(d.holdLot + d.superHot + d.hot+d.holdSuperLot)
                    return y1Scale(d.holdLot + d.superHot + d.hot)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                   // return y1Scale(d.holdLot + d.superHot+d.holdSuperLot) - y1Scale(d.holdLot + d.superHot + d.hot+d.holdSuperLot)
                    return y1Scale(d.holdLot + d.superHot ) - y1Scale(d.holdLot + d.superHot + d.hot)
                })
                .attr('fill', 'purple')

            //other prod bar
            for (let i = 0; i < prod.length; i++) {
                gs.append('rect')
                   // .attr('class', 'myRect')
                    .attr('x', function (d, i) {
                        return xScale(d.stage+'('+d.ct+')')+(xScale.step()-barWidth)/2
                    })

                    .attr('y', function (d) {
                      //  let res = d.holdLot + d.superHot + d.hot+d.holdSuperLot;
                        let res = d.holdLot + d.superHot + d.hot;
                        for (let j = 0; j <= i; j++) {
                            res += d.other[i];
                        }
                        return y1Scale(res)
                    })
                    .attr('width', barWidth)
                    .attr('height', function (d) {
                        //let res = d.holdLot + d.superHot + d.hot+d.holdSuperLot;
                        let res = d.holdLot + d.superHot + d.hot ;
                        for (let j = 1; j <= i; j++) {
                            res += d.other[i - 1];
                        }
                        return y1Scale(res) - y1Scale(res + d.other[i]);
                    })
                    .attr('fill', function (d) { return i >= colors.length ? 'black' : colors[i] })
            }

            //透明层，用于move on时显示边框
            gs.append('rect')
                .attr('class', 'myRect')
                .attr('x', function (d, i) {
                    return xScale(d.stage + '(' + d.ct + ')') + (xScale.step() - barWidth) / 2
                })
                .attr('y', function (d) {
                    let res = d.holdLot + d.superHot + d.hot;
                    for (let j = 0; j < d.other.length; j++) {
                        res += d.other[j];
                    }
                    return y1Scale(res)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                    let res = d.holdLot + d.superHot + d.hot;
                    for (let j = 0; j < d.other.length; j++) {
                        res += d.other[j];
                    }
                    return yHeight - y1Scale(res)
                })
                .attr('fill', 'Aqua')
                .attr('fill-opacity', 0)

            // 绘制折线
            var line_generator1 = d3.line()
                .x(function (d) {
                    return xScale(d.stage + '(' + d.ct + ')') + (barSpace+xScale.step()) / 2;
                })
                .y(function (d) {
                    return y1Scale(d.stageMove);
                })
            var line_generator2 = d3.line()
                .x(function (d, i) {
                    return xScale(d.stage + '(' + d.ct + ')') + (barSpace+xScale.step()) / 2;
                })
                .y(function (d) {
                    return y2Scale(d.accWip);
                })

            svgContainer.append('path')
                .attr('d', line_generator1(chartData))
                .style('fill', 'none')
                .style('stroke', 'green')

            svgContainer.append('path')
                .attr('d', line_generator2(chartData))
                .style('fill', 'none')
                .style('stroke', 'orange')

                //绘制month线
                svgContainer.append('line')
                // .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
                 .attr('x1', xm)
                 .attr('x2', xm)
                 .attr('y1', 0)
                 .attr('y2', yHeight)
                 .attr('stroke', 'black')
                 .attr('stroke-width', 2)
            //绘制key线
             svgContainer.append('line')
               //  .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
                 .attr('x1', xk)
                 .attr('x2', xk)
                 .attr('y1', 0)
                 .attr('y2', yHeight)
                 .attr('stroke', 'hotpink')
                 .attr('stroke-width', 1)
                 .attr('stroke-dasharray', '5,2')
            // 交互事件
            chartSvg.selectAll('.myRect')
                .on('dblclick', function (d) {
                    wipChart.subRenderPara=d;
                    wipChart.dialogSubChartVisible = true;
                })
                .on('mouseover', function (d) {
                    this.setAttribute('stroke', 'red')
                    this.setAttribute('stroke-opacity', 0.5)
                    this.setAttribute('fill-opacity', 0.2)
                })
                .on('mouseout', function (d) {
                    this.setAttribute('stroke-opacity', 0)
                    this.setAttribute('fill-opacity', 0)
                })
        },
        renderTuli(container, x, y, type, color, label) {
            if (type === 'line') {
                container.append('line')
                    .attr("x1", x)
                    .attr("y1", y)
                    .attr("x2", x + 25)
                    .attr("y2", y)
                    .attr("stroke", color)
                    .attr("stroke-width", 2);

            } else if (type === 'bar') {
                container.append('rect')
                    .attr("x", x)
                    .attr("y", y - 7.5)
                    .attr('height', 15)
                    .attr('width', 20)
                    .attr("stroke", 'black')
                    .attr("stroke-width", 1)
                    .attr('fill', color)
            } else {
                //dashBar
                container.append('rect')
                    .attr("x", x)
                    .attr("y", y - 7.5)
                    .attr('height', 15)
                    .attr('width', 20)
                    .attr("stroke", color)
                    .attr("stroke-width", 1)
                    .attr('stroke-dasharray', '3,3')
                    .attr('fill', "none")
            }
            let offset = type === 'line' ? 30 : 25;
            container.append('text')
                .style('font-size', '11px')
                .attr('x', x + offset)
                .attr('y', y + 3.5)
                .text(label)
        },
        removeChart(chartSvg){
            //清除原来的图
            chartSvg.selectAll('g').remove()
            chartSvg.selectAll('rect').remove()
            chartSvg.selectAll('line').remove()
        },
        arryMerage(arry){
            let first=arry[0];
            first.stageMove=eval(arry.map(m=>m.stageMove).join('+'));
            first.holdLot=eval(arry.map(m=>m.holdLot).join('+'));
            first.superHot=eval(arry.map(m=>m.superHot).join('+'));
            first.hot=eval(arry.map(m=>m.hot).join('+'));
            first.accWip=eval(arry.map(m=>m.accWip).join('+'));
           for(let i=1;i<arry.length;i++){
              for(let j=0;j<first.other.length;j++){
                  first.other[j]+=arry[i].other[j];
              }
           }
        },
        renderSubChart(entity){
           var chartSvg=this.subSvg;
           this.removeChart(chartSvg);
            let chartData = this.formSubChartData(entity);
           // console.log(chartData);
           let stage=entity.stage;
             // 设置图表数据，宽度、高度、边值
            var height = this.options.height;
            var marge = this.marge;
            var colors = this.colors;
            var prod = chartData.prodList;
            var barWidth = this.subBarWidth;
            var width = chartData.ChartEntities.length * barWidth + marge.left + marge.right;
            width = width < 1400 ? 1400 : width;
            let yHeight = height - marge.bottom - marge.top
            let xWidth=width - marge.left - marge.right
            // 画布
               chartSvg
                .attr('width', width)
                .attr('height', height)
            // 作图的容器
            var svgContainer = chartSvg.append('g') 
            .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
            // x轴、y轴比例尺
            var xScale = d3.scaleBand()
                .domain(chartData.ChartEntities.map(function (d) {
                    return d.step + '(' + d.ct + ')'
                }))
                .rangeRound([0, xWidth])

            let max = d3.max(chartData.ChartEntities.map(function (d) {
                return d.lotEntities.length * 25
            }))
            max=Math.ceil(max/10)*10
            var y2Scale = d3.scaleLinear()
                .domain([0, max])
                .rangeRound([yHeight, 0])
            max = d3.max(chartData.ChartEntities.map(function (d) {
                return d.lotEntities.length;
            }))
            max=Math.ceil(max/10)*10
            var y1Scale = d3.scaleLinear()
                .domain([0, max])
                .rangeRound([yHeight, 0])
            // 添加x轴、y轴
            var xAxis = d3.axisBottom(xScale)
            var y1Axis = d3.axisLeft(y1Scale)
                .ticks(10)
            var y2Axis = d3.axisRight(y2Scale)
                .ticks(10)
            svgContainer.append('g')
                .attr('transform', 'translate(0,' + yHeight + ')')
                .call(xAxis)
                .selectAll("text")
                .style("text-anchor", "start")
                .attr("dx", ".8em")
                .attr("dy", "-.25em")
                .attr("font-size", "1.3rem")
                .attr("stroke", "black")
                .attr("stroke-width", ".4")
                //.attr("textLength", marge.bottom - 20)
                // .attr("lengthAdjust","spacingAndGlyphs")
                .attr("transform", "rotate(90)");

            svgContainer.append('g')
                .attr('transform', 'translate(0,0)')
                .call(y1Axis)

            svgContainer.append('text')
                .attr("dy", -10)
                .attr('dx', -10)
                .text("(lots)")
            svgContainer.append('g')
                .attr('transform', 'translate(' + xWidth + ',0)')
                .call(y2Axis)
            svgContainer.append('text')
                .attr('dy', -10)
                .attr('dx', xWidth)
                .text("(wfrs)");

                 //添加图例
            let tuli_x = marge.left + barWidth;
           // this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', 'black', 'Hold Super Lot');
           // tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'dashBar', 'red', 'Hold Lot');
            tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', 'gold', 'S.Hot Lot');
            tuli_x += 100 + barWidth;
            this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', 'purple', 'Hot Lot');
            for (let i = 0; i < prod.length; i++) {
                tuli_x += 100 + barWidth;
                this.renderTuli(svgContainer, tuli_x, 0 - marge.top / 2, 'bar', colors[i], prod[i]);
            }
              // 添加矩形
              var gs = svgContainer.selectAll('.rect')
                  .data(chartData.ChartEntities)
                  .enter()
                .append('g')
           /* gs.append('rect')
                .attr('class', 'myRect')
                .attr('x', function (d) {
                    return xScale(d.step + '(' + d.ct + ')') + (xScale.step() - barWidth) / 2
                })
                .attr('y', function (d) {
                    return y1Scale(d.holdSuperLot)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                    return yHeight - y1Scale(d.holdSuperLot)
                })
                .attr('fill', 'black') */
              gs.append('rect')
                //  .attr('class', 'myRect')
                  .attr('x', function (d) {
                      return xScale(d.step+'('+d.ct+')')+(xScale.step()-barWidth)/2
                  })
                  .attr('y', function (d) {
                      //return y1Scale(d.holdLot+d.holdSuperLot)
                      return y1Scale(d.holdLot)
                  })
                  .attr('width', barWidth)
                  .attr('height', function (d) {
                      //return y1Scale(d.holdSuperLot)- y1Scale(d.holdLot+d.holdSuperLot)
                      return yHeight - y1Scale(d.holdLot)
                  })
                  .attr('stroke', 'red')
                  .attr("stroke-width", 1.5)
                  .attr('stroke-dasharray', '4,2')
                  .attr('fill', 'none')

              gs.append('rect')
                 // .attr('class', 'myRect')
                  .attr('x', function (d) {
                      return xScale(d.step+'('+d.ct+')')+(xScale.step()-barWidth)/2
                  })
                  .attr('y', function (d) {
                      //return y1Scale(d.holdLot + d.superHot+d.holdSuperLot)
                      return y1Scale(d.holdLot + d.superHot)
                  })
                  .attr('width', barWidth)
                  .attr('height', function (d) {
                      //return y1Scale(d.holdLot+d.holdSuperLot) - y1Scale(d.holdLot + d.superHot+d.holdSuperLot)
                      return y1Scale(d.holdLot ) - y1Scale(d.holdLot + d.superHot)
                  })
                  .attr('fill', 'gold')
              gs.append('rect')
                 // .attr('class', 'myRect')
                  .attr('x', function (d, i) {
                      return xScale(d.step+'('+d.ct+')')+(xScale.step()-barWidth)/2
                  })
                  .attr('y', function (d) {
                      //return y1Scale(d.holdLot + d.superHot + d.hot+d.holdSuperLot)
                      return y1Scale(d.holdLot + d.superHot + d.hot )
                  })
                  .attr('width', barWidth)
                  .attr('height', function (d) {
                    //  return y1Scale(d.holdLot + d.superHot+d.holdSuperLot) - y1Scale(d.holdLot + d.superHot + d.hot+d.holdSuperLot)
                      return y1Scale(d.holdLot + d.superHot ) - y1Scale(d.holdLot + d.superHot + d.hot )
                  })
                  .attr('fill', 'purple')
              for (let i = 0; i < prod.length; i++) {
                  gs.append('rect')
                     // .attr('class', 'myRect')
                      .attr('x', function (d) {
                          return xScale(d.step+'('+d.ct+')')+(xScale.step()-barWidth)/2
                      })
                      .attr('y', function (d) {
                          //let res = d.holdLot + d.superHot + d.hot+d.holdSuperLot;
                          let res = d.holdLot + d.superHot + d.hot ;
                          for (let j = 0; j <= i; j++) {
                              res += d.other[i];
                          }
                          return y1Scale(res)
                      })
                      .attr('width', barWidth)
                      .attr('height', function (d) {
                          //let res = d.holdLot + d.superHot + d.hot+d.holdSuperLot;
                          let res = d.holdLot + d.superHot + d.hot;
                          for (let j = 1; j <= i; j++) {
                              res += d.other[i - 1];
                          }
                          return y1Scale(res) - y1Scale(res + d.other[i]);
                      })
                      .attr('fill', function (d) { return i >= colors.length ? 'black' : colors[i] })
            }

            //透明层，用于move on时显示边框
            gs.append('rect')
                .attr('class', 'myRect')
                .attr('x', function (d, i) {
                    return xScale(d.step + '(' + d.ct + ')') + (xScale.step() - barWidth) / 2
                })
                .attr('y', function (d) {
                    let res = d.holdLot + d.superHot + d.hot;
                    for (let j = 0; j < d.other.length; j++) {
                        res += d.other[j];
                    }
                    return y1Scale(res)
                })
                .attr('width', barWidth)
                .attr('height', function (d) {
                    let res = d.holdLot + d.superHot + d.hot;
                    for (let j = 0; j < d.other.length; j++) {
                        res += d.other[j];
                    }
                    return yHeight - y1Scale(res)
                })
                .attr('fill', 'Aqua')
                .attr('fill-opacity', 0)

            svgContainer.append('text')
                .attr('x',xWidth/2)
                .attr('y',5)
                .style('font-size', '14px')
                .attr('fill', 'red')
                .attr('text-anchor',"middle")
                .text(stage)
               // 交互事件
            chartSvg.selectAll('.myRect')
            .on('dblclick', function (d) {
                let data={
                    OpeNo:d.opeNo,
                    LotList:d.lotEntities.map(m=>m.LotID)
                }
                wipChart.handleSubChartBarDbl(data);
            })
            .on('mouseover', function () {
                this.setAttribute('stroke', 'red')
                this.setAttribute('stroke-opacity', 0.5)
                this.setAttribute('fill-opacity', 0.2)
            })
            .on('mouseout', function () {
                this.setAttribute('stroke-opacity', 0)
                this.setAttribute('fill-opacity', 0)
            })

        },
        formSubChartData(entity){
            let stage=entity.stage;
            let ct=entity.ct;
            let data=[];
            let ChartData={prodList:[],ChartEntities:[]};
            this.queryResault.chart.forEach(element => {
                let arry= element.ChartEntities.filter(f=>f.Stage==stage);
                if(arry.length>0){
                    let obj={product:element.Product,ChartEntities:[]}
                    arry.forEach(f => {                       
                        obj.ChartEntities.push({
                            OpeNo:f.OpeNo,
                            Ct:f.RemainCT,
                            Step:f.Step,
                            Wip:f.Wip,
                            LotEntities:f.LotEntities,
                            //HoldLot:f.LotEntities.filter(fi=>fi.HoldState=='ONHOLD'&&fi.Priority>1).length,
                            HoldLot: f.LotEntities.filter(fi => fi.HoldState == 'ONHOLD').length,
                            SuperHot:f.LotEntities.filter(fi=>fi.Priority==1&&fi.HoldState!='ONHOLD').length,
                            Hot: f.LotEntities.filter(fi => fi.Priority == 2 && fi.HoldState != 'ONHOLD').length,
                           // HoldSuperLot: f.LotEntities.filter(fi => fi.HoldState == 'ONHOLD'&&fi.Priority===1).length
                        });
                    });
                     //obj.ChartEntities.map(m=>{m.Other=m.LotEntities.length-m.HoldLot-m.SuperHot-m.Hot-m.HoldSuperLot});
                    obj.ChartEntities.map(m => { m.Other = m.LotEntities.length - m.HoldLot - m.SuperHot - m.Hot  });
                    data.push(obj);
                }
            });
            ChartData.prodList = data.map(m => m.product);
            if (data.length === 0) return ChartData;
            //console.log(data);
          data[0].ChartEntities.forEach(element => {
            let arry=[];
            data.forEach(f=>{
               arry.push(f.ChartEntities.filter(fi=>fi.OpeNo===element.OpeNo)[0]) 
              });
            let model={
                opeNo:element.OpeNo,
                ct:element.Ct,
                step:element.Step,
                wip:eval(arry.map(m=>m.Wip).join('+')),
                holdLot:eval(arry.map(m=>m.HoldLot).join('+')),
                superHot:eval(arry.map(m=>m.SuperHot).join('+')),
                hot: eval(arry.map(m => m.Hot).join('+')),
               // holdSuperLot: eval(arry.map(m => m.HoldSuperLot).join('+')),
                other:[],
                lotEntities:[]
            };  
            for(let i=0;i<arry.length;i++){
                model.other.push(arry[i].Other);
                arry[i].LotEntities.forEach(element => {
                    model.lotEntities.push(element)
                });
            }
            ChartData.ChartEntities.push(model);
          });
          return ChartData;
        },
        show(){
            this.subSvg=d3.select(this.$refs.subChart);
            this.renderSubChart(this.subRenderPara);
            this.lotInfoEntities = [];
        },
        handleSubChartBarDbl(data){
            let url = 'GetLotDetail';
            this.subLoading = true;
            PostAjaxGetJson(data,url,function(response){
                if(response.success){
                    wipChart.lotInfoEntities=response.lotInfoEntities;
                }else{
                    wipChart.$message.error(response.msg);
                }
                wipChart.subLoading = false;
            },function(response){
                wipChart.$message.error(response);
                wipChart.subLoading = false;
            });
        },
        outputExcel(){
            let data=[];
            let rawData=this.lotInfoEntities;
            for(var i =0;i<rawData.length;i++){
                data.push({
                   NO:i+1,
                   EqpType:rawData[i].EqpType,
                   Pri:rawData[i].Priority,
                   OpeNO:'="'+rawData[i].OpeNo+'"',
                   Ope$sName:rawData[i].OpeName,
                   Prod$sID: rawData[i].ProductID,
                   LotID:rawData[i].LotID,
                   Foup:rawData[i].Foup,
                   Location:rawData[i].Location,
                   Status:rawData[i].Status,
                   Lot$sProc$sStatus:rawData[i].LotHoldStatus+'/'+rawData[i].LotProcStatus,
                   HoldReason:rawData[i].HoldReason,
                   HoldClaimMemo:rawData[i].HoldReasonDesc,
                   Qty:rawData[i].Qty,
                   OpeStartTime:rawData[i].OpeStartTime,
                    ChgUserID:rawData[i].ChgUserID,
                    ChgUserName:rawData[i].ChgUserName
                });
            }
            let filename="WIP_Chart.xls";
            let tableHtml = FormExcelContext(data);
            tableHtml = tableHtml.replace(/\$s/g, " ");
            if(!tableHtml)return false;
            let ctx= { worksheet: "sheet1" , table: tableHtml };
            let dlink=this.$refs.dlink;
            dlink.href = uri + base64(format(template, ctx));
            dlink.download = filename;
            dlink.click();
        }
    },
    computed: {
        avaGroups: function () {
            let arry = this.prods.map(m => m.substr(5, 2));
            arry.distinct();
            return arry;
        },
        avaProds: function () {
            let list = [];
            if (this.selGroups.length > 0) {
                for (let i = 0; i < this.selGroups.length; i++) {
                    this.prods.map(m => { if (m.substr(5, 2) === this.selGroups[i]) list.push(m); });
                }
            } else {
                list = this.prods;
            }
            return list.map(m => { return { prod: m } });
        },
        queryResault: function () {
            let chart = this.timeType === 'cur' ? this.curChart : this.ystdChart;
            let table = this.timeType === 'cur' ? this.curTable : this.ystdTable;
            chart = chart.filter(f => this.options.prod.find(s => s === f.Product));
            table = table.filter(f => this.options.prod.find(s => s === f.Product));
            if (this.wipType === "all") return { chart: chart, table: table };
            chart = JSON.parse(JSON.stringify(chart));
            if (this.wipType === 'out') {
                for (let i = 0; i < chart.length; i++) {
                    for (let j = 0; j < chart[i].ChartEntities.length; j++) {
                        chart[i].ChartEntities[j].LotEntities = chart[i].ChartEntities[j].LotEntities.filter(f => f.InBank === false);
                        chart[i].ChartEntities[j].Wip = this.getWipByLots(chart[i].ChartEntities[j].LotEntities)
                    }
                }
                return { chart: chart, table: table };
            } else {
                for (let i = 0; i < chart.length; i++) {
                    for (let j = 0; j < chart[i].ChartEntities.length; j++) {
                        chart[i].ChartEntities[j].LotEntities = chart[i].ChartEntities[j].LotEntities.filter(f => f.InBank === true);
                        chart[i].ChartEntities[j].Wip = this.getWipByLots(chart[i].ChartEntities[j].LotEntities)
                    }
                }
                return { chart: chart, table: table };
            }
        },
        chartData(){
            if (!this.queryClicked) return [];
            if (this.options.prod.length == 0) {this.removeChart(this.wipSvg); return [];}
            let data = [];
            let chart = this.queryResault.chart;
            for (let i = 0; i < chart.length; i++) {
                for (let j = 0; j < chart[i].ChartEntities.length; j++) {
                    let entity = chart[i].ChartEntities[j];
                    if (data.length > 0 && data[data.length - 1].stage == entity.Stage) {
                        let d = data[data.length - 1];
                        d.stageMove = entity.LotEntities.length;
                        d.holdLot += entity.LotEntities.filter(f => f.HoldState == 'ONHOLD').length;
                        //d.holdLot += entity.LotEntities.filter(f => f.HoldState == 'ONHOLD' && f.Priority > 1).length;
                       // d.holdSuperLot += entity.LotEntities.filter(f => f.HoldState == 'ONHOLD' && f.Priority == 1).length;
                        d.superHot += entity.LotEntities.filter(f => f.Priority == 1 && f.HoldState != 'ONHOLD').length;
                        d.hot += entity.LotEntities.filter(f => f.Priority == 2 && f.HoldState != 'ONHOLD').length;
                        d.other[i] += entity.LotEntities.filter(f => f.Priority != 1 && f.Priority != 2 && f.HoldState != 'ONHOLD').length;
                        d.accWip+=entity.Wip;
                    } else {
                        let d = {
                            stage: entity.Stage,
                            ct: entity.RemainCT,
                            stageMove: entity.LotEntities.length,
                            holdLot: entity.LotEntities.filter(f => f.HoldState == 'ONHOLD').length,
                            //holdLot : entity.LotEntities.filter(f => f.HoldState == 'ONHOLD' && f.Priority > 1).length,
                            //holdSuperLot : entity.LotEntities.filter(f => f.HoldState == 'ONHOLD' && f.Priority == 1).length,
                            superHot: entity.LotEntities.filter(f => f.Priority == 1 && f.HoldState != 'ONHOLD').length,
                            hot: entity.LotEntities.filter(f => f.Priority == 2 && f.HoldState != 'ONHOLD').length,
                            other: new Array(chart.length).fill(0),
                            accWip: entity.Wip
                        };
                        //d.other[i] = entity.LotEntities.length - d.holdLot - d.hot - d.superHot-d.holdSuperLot;
                        d.other[i] = entity.LotEntities.length - d.holdLot - d.hot - d.superHot;
                        data.push(d);
                    }

                }
            }
            let data0=data.filter(f=>f.ct==0);
            let data1 = data.filter(f => f.ct > 0).sort((a, b) => { if (a.ct > b.ct) return -1; if (a.ct == b.ct) return 0; else return 1;});
            data0.forEach(element => {
                data1.push(element)
            });
            let repeat=data1.map(m=> m.stage+'|'+m.ct).distinct();
            if(repeat.length>0){
                repeat.forEach(f=>{
                    let rawArr=f.split('|');
                    let arry=data1.filter(fi=>fi.stage==rawArr[0]&&fi.ct==rawArr[1]);
                    if(arry.length>1){
                        this.arryMerage(arry);
                        for(let i=1;i<arry.length;i++){
                             data1.splice(data1.lastIndexOf(arry[i]),1);
                        }
                    }
                })
            }
           for(let i=data1.length-1;i>0;i--){
                //计算累计wip
                data1[i-1].accWip+=data1[i].accWip
           }
            return data1;
        },
        wipTable(){
            let data=[];
            let all={};
            for(let i=0;i<this.queryResault.table.length;i++){
                let d={};
                let raw=this.queryResault.table[i];
                d.product=raw.Product;
                d.targetOut=raw.TargetOut;
                d.quantity=raw.CanOut-raw.TargetOut;
                d.keyStage=raw.KeyStage;
                d.day=raw.Day;
                data.push(d);
            }
            all.targetOut=eval(data.map(m=>m.targetOut).join('+'));
            all.quantity=eval(data.map(m=>m.quantity).join('+'));
            all.canOut=all.targetOut+all.quantity;
            return {all,data}
        }
    },
    watch: {
        remSec(newValue, oldValue) {
            if (newValue === 0) {
                clearInterval(this.timer);
                this.update();
                this.remSec = 300;
                let d = new Date();
                this.updateTime = d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
                this.setTimer();
                if (this.queryClicked) {
                    this.renderChart();
                }
            }
        },
        chartData(newValue, oldValue) {
            //计算离下个月1号的天数
            if (newValue.length > 0) {
                let date = new Date();
                let rawDate = this.timeType == 'cur' ? new Date() : new Date(date.getFullYear(), date.getMonth(), date.getDate(), 08);
                date.setMonth(date.getMonth() + 1);
                let NexMonFirDay = new Date(date.getFullYear(), date.getMonth());
                let remainDays = (NexMonFirDay - rawDate) / (24 * 60 * 60 * 1000);
                let remainKey=-1;
                let remainMon=-1;
                //计算keyline
                for (let i = 0; i < newValue.length; i++) {
                    if (remainDays > newValue[i].ct) {
                        this.options.key = newValue[i].stage + '(' + newValue[i].ct + ')';
                        remainKey=newValue[i].ct;
                        break;
                    }
                }

                //计算monthline
                let remainOut = this.queryResault.table.map(m => m.AlreadyOut - m.TargetOut);
                let total = remainOut ? eval(remainOut.join('+')) : 0;
                for (let i = 0; i < newValue.length; i++) {
                    if (total + newValue[i].accWip < 0) {
                        this.options.month = i == 0 ? newValue[i].stage + '(' + newValue[i].ct + ')' : newValue[i - 1].stage + '(' + newValue[i - 1].ct + ')';
                        remainMon=i==0? newValue[i].ct:newValue[i - 1].ct;
                        break;
                    }
                }
                if (remainMon == -1) {
                    this.options.month = 'wafer out';
                    remainMon=0;
                }
                if(remainKey==-1){
                    remainKey=0;
                    this.options.key ='wafer out';
                }
                this.options.day=remainKey-remainMon;
                this.renderChart();
            }
        }
    },
    mounted() {
        clearInterval(this.timer);
       this.wipSvg= d3.select(this.$refs.wipChart);
    },

    distroyed: function () {
        clearInterval(this.timer)
    }
});

$(document).ready(function () {
    wipChart.handleUpdtClick();
});

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
