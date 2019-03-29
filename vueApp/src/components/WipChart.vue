<template>
  <div>
    <svg ref="wipChart" class="wipChart"></svg>
  </div>
</template>

<script>
import * as d3 from 'd3'
export default {
  name: 'wipChart',
  props: {
    chartData: {},
    options: {
      height: '',
      key:'',
      month:'',
      prod:[]
    }
  },
  data () {
    return {
        barWidth:8,
        barSpace:0,
        marge:{
            top:50,
            left:60,
            right:60,
            bottom:150
        },
        colors:['blue','pink','DarkGreen','gray','LightSalmon','Olive','LawnGreen']
    }
  },
  created () {},
  mounted () {
    this.renderChart()
  },
  methods: {
    renderChart () {
      // 设置图表数据，宽度、高度、边值
      var chartData = this.chartData;
      var height = this.options.height;
      var marge = this.marge;
      var colors=this.colors;
      var prod=this.options.prod;
      var barWidth=this.barWidth;
      var barSpace=this.barSpace;
      var width=chartData.length*(barWidth+barSpace)+marge.left+marge.right;
      width=width<1000?1000:width;
      let yHeight=height-marge.bottom-marge.top
      // 画布
      var chartSvg = d3.select(this.$refs.wipChart)
        .attr('width', width)
        .attr('height', height)
      // 作图的容器
      var svgContainer = chartSvg.append('g')
        .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
      // x轴、y轴比例尺
      var xScale = d3.scaleBand()
        .domain(chartData.map(function (d) {
          return d.stage+'('+d.ct+')'
        }))
        .rangeRound([0, width - marge.left - marge.right])

      var y2Scale = d3.scaleLinear()
        .domain([0, d3.max(chartData.map(function (d) {
          return d.accWip
        }))])
        .rangeRound([height - marge.top - marge.bottom, 0])

      var y1Scale=d3.scaleLinear()
          .domain([0, d3.max(chartData.map(function (d) {
          return d.holdLot+d.superHot+d.hot+eval(d.other.join('+'));
        }))])
        .rangeRound([height - marge.top - marge.bottom, 0])
      // 添加x轴、y轴
      var xAxis = d3.axisBottom(xScale)
      var y1Axis = d3.axisLeft(y1Scale)
          .ticks(10)
      var y2Axis = d3.axisRight(y2Scale)
        .ticks(10)
      svgContainer.append('g')
        .attr('transform', 'translate(0,' + (height - marge.top - marge.bottom) + ')')
        .call(xAxis)
         .selectAll("text")
      .style("text-anchor", "start")
      .attr("dx", ".8em")
      .attr("dy", "-.55em")
      .attr("transform", "rotate(90)" );

      svgContainer.append('g')
        .attr('transform', 'translate(0,0)')
        .call(y1Axis)

        svgContainer.append('text')
        .attr("dy", -10)
        .attr('dx',-10)
        .text("(lots)")
      svgContainer.append('g')
        .attr('transform','translate('+(width-marge.left-marge.right)+',0)')
        .call(y2Axis)
        svgContainer.append('text')
        .attr('dy',-10)
        .attr('dx',width-marge.left-marge.right)
        .text("(wfrs)");
   
   
        //添加图例
        let tuli_x=marge.left+barWidth;
        this.renderTuli(svgContainer,tuli_x,0-marge.top/2,'line','orange','Wip Accum');
        tuli_x+=100+barWidth;
        this.renderTuli(svgContainer,tuli_x,0-marge.top/2,'line','green','StageOut Move');
        tuli_x+=130+barWidth;
        this.renderTuli(svgContainer,tuli_x,0-marge.top/2,'bar','red','Hold Lot');
        tuli_x+=100+barWidth;
        this.renderTuli(svgContainer,tuli_x,0-marge.top/2,'bar','gold','S.Hot Lot');
        tuli_x+=100+barWidth;
        this.renderTuli(svgContainer,tuli_x,0-marge.top/2,'bar','purple','Hot Lot');
        for(let i=0;i<prod.length;i++){
            tuli_x+=100+barWidth;
             this.renderTuli(svgContainer,tuli_x,0-marge.top/2,'bar',colors[i],prod[i]);
        }
        
  
      // 添加矩形
      var gs = svgContainer.selectAll('.rect')
        .data(chartData)
        .enter()
        .append('g')
      gs.append('rect')
        .attr('class', 'myRect')
        .attr('x', function (d, i) {
          return (width - marge.left - marge.right) / chartData.length * i + (barSpace-barWidth+xScale.step() )/ 2
        })
        .attr('y', function (d) {
          return y1Scale(d.holdLot)
        })
        .attr('width', barWidth)
        .attr('height', function (d) {
          return  height-marge.top-marge.bottom-y1Scale(d.holdLot)
        })
        .attr('fill', 'red')
       gs.append('rect')
        .attr('class', 'myRect')
        .attr('x', function (d, i) {
          return (width - marge.left - marge.right) / chartData.length * i + (barSpace-barWidth+xScale.step() )/ 2
        })
        .attr('y', function (d) {
          return  y1Scale(d.holdLot+d.superHot)
        })
        .attr('width', barWidth)
        .attr('height', function (d) {
          return y1Scale(d.holdLot)-y1Scale(d.holdLot+d.superHot)
        })
        .attr('fill', 'gold')
         gs.append('rect')
          .attr('class', 'myRect')
        .attr('x', function (d, i) {
          return (width - marge.left - marge.right) / chartData.length * i + (barSpace-barWidth+xScale.step() )/ 2
        })
        .attr('y', function (d) {
          return  y1Scale(d.holdLot+d.superHot+d.hot)
        })
        .attr('width', barWidth)
        .attr('height', function (d) {
          return y1Scale(d.holdLot+d.superHot) -y1Scale(d.holdLot+d.superHot+d.hot)
        })
        .attr('fill', 'purple')

        for(let i=0;i<prod.length;i++){
        gs.append('rect')
        .attr('class', 'myRect')
        .attr('x', function (d, i) {
          return (width - marge.left - marge.right) / chartData.length * i+ (barSpace-barWidth+xScale.step()) / 2
        })
        
        .attr('y', function (d) {
            let res=d.holdLot+d.superHot+d.hot;
            for(let j=0;j<=i;j++){
                res+=d.other[i];
            }
          return y1Scale(res)
        })
        .attr('width', barWidth)
        .attr('height', function (d) {
         let res=d.holdLot+d.superHot+d.hot;
              for(let j=1;j<=i;j++){
                res+=d.other[i-1];
            }
            return y1Scale(res)-y1Scale(res+d.other[i]);
        })
        .attr('fill', function(d){return i>=colors.length?'black':colors[i]})
        }

                 // 绘制折线
      var line_generator1 = d3.line()
        .x(function (d) {
          return xScale( d.stage+'('+d.ct+')')+(barSpace+xScale.step() )/ 2;
        })
        .y(function (d) {
          return y1Scale(d.stageMove);
        })
     var line_generator2 = d3.line()
        .x(function (d, i) {
          return xScale( d.stage+'('+d.ct+')')+(barSpace+xScale.step() )/ 2;
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
        
      //添加key line+month line
        let key=this.options.key
        let month=this.options.month
        let xk=xScale(key)+(barSpace+xScale.step() )/ 2
        let xm=xScale(month)+(barSpace+xScale.step() )/ 2
        chartSvg.append('line')
         .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
        .attr('x1',xm)
        .attr('x2',xm)
        .attr('y1',0)
        .attr('y2',yHeight)
        .attr('stroke','black')
        .attr('stroke-width',2)

         chartSvg.append('line')
         .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
        .attr('x1',xk)
        .attr('x2',xk)
        .attr('y1',0)
        .attr('y2',yHeight)
        .attr('stroke','hotpink')
        .attr('stroke-width',1)
        .attr('stroke-dasharray','5,2')

        if(xk<xm){
            chartSvg.append('rect')
            .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
            .attr('x',xk)
            .attr('y',0)
            .attr('height',yHeight)
            .attr('width',xm-xk)
            .attr('stroke','orange')
            .attr('stroke-width',2)
            .attr('fill','lightgreen')
            .attr('stroke-opacity',0)
            .attr('fill-opacity',0.2)
        }else if(xk>xm){
              chartSvg.append('rect')
            .attr('transform', 'translate(' + marge.left + ',' + marge.top + ')')
            .attr('x',xm)
            .attr('y',0)
            .attr('height',yHeight)
            .attr('width',xk-xm)
            .attr('stroke','orange')
            .attr('stroke-width',2)
            .attr('fill','pink')
            .attr('stroke-opacity',0)
            .attr('fill-opacity',0.2)
        }
      // 交互事件
      chartSvg.selectAll('.myRect')
        .on('dblclick', function (d) {
            alert(d.stage);
        })
    
    },
    renderTuli(container,x,y,type,color,label){
        if(type==='line'){
            container.append('line')
             .attr("x1",x)
            .attr("y1",y)
            .attr("x2",x+25)
            .attr("y2",y)
            .attr("stroke",color)
            .attr("stroke-width",2);

        }else{
            container.append('rect')
             .attr("x",x)
            .attr("y",y-7.5)
            .attr('height',15)
            .attr('width',20)
            .attr("stroke",'black')
            .attr("stroke-width",1)
            .attr('fill', color)
        }
        let offset=type==='bar'?25:30;
        container.append('text')
        .style('font-size', '11px')
        .attr('x',x+offset)
        .attr('y',y+3.5)
        .text(label)
    }


  }
}
</script>