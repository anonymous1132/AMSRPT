
  var offsetHead=15;
  var offsetTail=10;
  var width=0;
  var rowHeight=20;
  //canvasObj使用原生js
function drawTh(startTime,endTime,scale,canvasObj){
    //单位长度设置
    var unitWidth=10;
    var startTime=startTime;
    var endTime=endTime;

    //Canvas高度设置
    var height=40;
    var dateTimeList=[];
    var canvas=canvasObj;
    var context=canvas.getContext("2d");
    var scale=scale;
    this.draw=function(){
        if(scale==1){
            getListByHour();    
        }else if(scale==60){
            getListByMinute();
        }
       // console.log(dateTimeList);
        width=dateTimeList.length*unitWidth+offsetHead+offsetTail;
        canvas.width=width;
        canvas.height=height;
        context.fillStyle='black';
        context.fillRect(0,0,width,height);
        if(scale==1){
            drawRulerByHour();
        }else if(scale==60){
           drawRulerByMinute();
        }
    }
    
     var getListByHour=function(){
        var dt=new Date(startTime.getFullYear()+'-'+(startTime.getMonth()+1)+'-'+startTime.getDate()+' '+startTime.getHours()+':00:00');
        while (dt<endTime) {
            dateTimeList.push(new Date(dt.toString()));
            dt.setHours(dt.getHours()+1);
        }
        dateTimeList.push(dt);
    }

    var getListByMinute=function(){
        var dt=new Date(startTime.getFullYear()+'-'+(startTime.getMonth()+1)+'-'+startTime.getDate()+' '+startTime.getHours()+':'+startTime.getMinutes() +':00');
        while (dt<endTime) {
            dateTimeList.push(new Date(dt.toString()));
            dt.setMinutes(dt.getMinutes()+1);
        }
        dateTimeList.push(dt);
    }

    var drawRulerByHour=function(){
        var tipHeight=15;
        var tipY=height-tipHeight;
        for(var i=0;i<dateTimeList.length;i++){
            var lineX=i*unitWidth+offsetHead;
            if(dateTimeList[i].getHours()==0){
                drawWhiteLine(lineX,10);
                context.fillStyle='white';
                var tip=dateTimeList[i].getMonth()+1+'/'+dateTimeList[i].getDate();
                context.fillText(tip,lineX,tipY);
            }else{
                drawWhiteLine(lineX,5);
            }
        }
    }


    var drawRulerByMinute=function(){
        var tipHeight=15;
        var tipY=height-tipHeight;
        for(var i=0;i<dateTimeList.length;i++){
            var lineX=i*unitWidth+offsetHead;
            if((dateTimeList[i].getHours()==0)&&(dateTimeList[i].getMinutes()==0)){
                drawWhiteLine(lineX,10);
                context.fillStyle='white';
                context.textAlign='center';
                var tip=dateTimeList[i].getMonth()+1+'/'+FixMinute(dateTimeList[i].getDate());
                context.fillText('00:00',lineX,tipY);
                context.fillText(tip,lineX,tipY-10);
            }else if(dateTimeList[i].getMinutes()%10==0){
                drawWhiteLine(lineX,8);
                context.fillStyle='white';
                context.textAlign='center';
                var tip=dateTimeList[i].getHours()+':'+FixMinute(dateTimeList[i].getMinutes());
                context.fillText(tip,lineX,tipY);
            }else{
                drawWhiteLine(lineX,3);
            }
        }
    }

    var drawWhiteLine=function(x,lineHeight){
        context.moveTo(x,height);
        context.lineTo(x,height-lineHeight);
        context.strokeStyle="white";
        context.lineWidth=1;
        context.stroke();
    }

    var FixMinute=function(minute){
        return minute<10?'0'+minute:minute;
    }

    
}

function drawRow(rowData,canvasObj){
    var canvas=canvasObj;
    var ctx=canvas.getContext("2d");
    canvas.width=width;
    var delta=rowData.totalSeconds;
    var xWidth=width-offsetHead-offsetTail;
    var stateArray=JSON.parse(JSON.stringify(rowData.historyStates));
    stateArray.sort(sortData);
    for(var i=0;i<stateArray.length;i++){
        var rowStart=stateArray[i].StartTime;
        var rowDelta=stateArray[i].DurationSecond;
        var dur=(rowStart-rowData.startTime)/1000;
        dur=dur>0?dur:0;
        var x=offsetHead+dur*xWidth/delta;
        var rectWidth=rowDelta*xWidth/delta;
        //最小像素为0.5px
        rectWidth=rectWidth>0.5?rectWidth:0.5;
        ctx.fillStyle=stateArray[i].Style;
        ctx.fillRect(x,0,rectWidth,rowHeight);  
    }

    var sortData=function(a,b){
        return a.DurationSecond-b.DurationSecond;
    }

}


function drawPercent(rowData,canvasObj){
    var canvas=canvasObj;
    var ctx=canvas.getContext("2d");
    var xWidth=200;
    var array=[];
    var pr_x=0;
    var totalSeconds=rowData.totalSeconds;
    var pr_w=(rowData.PR*xWidth)/totalSeconds;
    array.push({x:pr_x,w:pr_w,color:'green'});

    var sb_x=pr_x+pr_w;
    var sb_w=(rowData.SB*xWidth)/totalSeconds;
    array.push({x:sb_x,w:sb_w,color:'yellow'});
    var en_x=sb_x+sb_w;
    var en_w=(rowData.EN*xWidth)/totalSeconds;
    array.push({x:en_x,w:en_w,color:'blue'});
    var sd_x=en_x+en_w;
    var sd_w=(rowData.SD*xWidth)/totalSeconds;
    array.push({x:sd_x,w:sd_w,color:'pink'});
    var ud_x=sd_x+sd_w;
    var ud_w=(rowData.UD*xWidth)/totalSeconds;
    array.push({x:ud_x,w:ud_w,color:'red'});
    var ns_x=ud_x+ud_w;
    var ns_w=(rowData.NS*xWidth)/totalSeconds;
    array.push({x:ns_x,w:ns_w,color:'lightgray'});
    array.sort(sortData);

    for(var i=0;i<array.length;i++){
        var item=array[i];
        var viewW=item.w>0.5?item.w:0.5;
        viewW=item.w===0?0:viewW;
        //drawSingleBlock(item.color,item.x,viewW);
        ctx.fillStyle=item.color;
        ctx.fillRect(item.x,0,viewW,20);
    }

 

    var sortData=function(a,b){
        return a.w-b.w;
    }
}