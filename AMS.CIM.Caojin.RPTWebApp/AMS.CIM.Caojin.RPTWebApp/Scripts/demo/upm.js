
    var prColor='green';
    var sbColor='yellow';
    var enColor='blue';
    var sdColor='pink';
    var udColor='red';
    var nsColor='lightgray';
    var upColor='skyblue';
    var uuColor='orange';
    var bgColor='white';
    var textColor='gray';
    var lineColor='gray';
    var targetColor='red';
    var xAxisWidth=25;  //x轴旁边周显示
    var yAxisHeight=60;//y轴下面显示文字预留高度
    var titleHeight=30;//chart标题预留高度
    var sampleHeight=20;//图例部分预留高度
    var canvasHeight=400;//canvas总高度，预留300画图部分
    var brokenLineWidth=1.5;//折线宽度
    var TotalHeight=canvasHeight-titleHeight-sampleHeight-yAxisHeight;//画布总高度

function drawChart(canvasObj,data){
    var columnlarWidth=40;//柱状体宽度
    var canvasWidth=600;
    var xSpaceWidth=80;
    var count=data.Entities.length;
    var ctx=canvasObj.getContext("2d");
    if(count<6){
    xSpaceWidth=(canvasWidth-2*xAxisWidth)/(count+1);
    }else{
        canvasWidth=2*xAxisWidth+(count+1)*xSpaceWidth;
    }
    canvasObj.width=canvasWidth;
    canvasObj.height=canvasHeight;

    var drawFrame=function(){
        ctx.fillStyle=bgColor;
        ctx.fillRect(0,0,canvasWidth,canvasHeight);
        unitHeight=(canvasHeight-titleHeight-yAxisHeight-sampleHeight)/10;
        var x=xAxisWidth;//x坐标
        var rx=canvasWidth-xAxisWidth;//x次坐标
        for(var i=0;i<=10;i++){
            var text=(100-10*i)+'%';
            var y=titleHeight+i*unitHeight;
            ctx.fillStyle=textColor;
            ctx.textAlign='right';
            ctx.textBaseline='middle';
            ctx.fillText(text,x,y);
            ctx.beginPath();
            ctx.moveTo(x+5,y);
            ctx.lineTo(rx,y);
            ctx.strokeStyle=lineColor;
            ctx.lineWidth=0.5;
            ctx.stroke();
            ctx.fillStyle=textColor;
            ctx.textAlign='right';
            ctx.textBaseline='middle';
            ctx.fillText(text,rx+10,y);
        }

        //画图例
        x=xAxisWidth+5;
        var y=canvasHeight-sampleHeight;
        var axisObj={x,y};
        if(data.hasPR)
        {
            drawSampleBlock(prColor,axisObj,'PR');

        }
        if(data.hasSB)
        {
            drawSampleBlock(sbColor,axisObj,'SBY');
        }
        if(data.hasEN)
        {
           drawSampleBlock(enColor,axisObj,'EN');
        }
        if(data.hasSD)
        {
            drawSampleBlock(sdColor,axisObj,'SD');
        }
        if(data.hasUD)
        {
            drawSampleBlock(udColor,axisObj,'UD');
        }
        if(data.hasNS)
        {
           drawSampleBlock(nsColor,axisObj,'NST');
        }
        drawSampleLine(upColor,axisObj,"UPm"); 
        drawSampleLine(uuColor,axisObj,"UUm"); 
    }

    var drawSampleBlock=function(fillStyle,axisObj,label){
        var blockWidth=40;
        var blockHeight=25;
        var textMaxWidth=25;
        var textOffsetWidth=1;
        ctx.fillStyle=fillStyle;
        ctx.fillRect(axisObj.x,axisObj.y,blockWidth,blockHeight);
        axisObj.x+=blockWidth+textOffsetWidth;
        ctx.fillStyle=textColor;
        ctx.textAlign='left';
        ctx.textBaseline='middle';
        ctx.fillText(label,axisObj.x,axisObj.y+blockHeight/2,textMaxWidth);
        axisObj.x+=textMaxWidth;
    }

    var drawSampleLine=function(strokeStyle,axisObj,label){
        var blockWidth=40;
        var blockHeight=25;
        var textMaxWidth=25;
        var textOffsetWidth=1;
        ctx.strokeStyle=strokeStyle;
        ctx.beginPath();
        ctx.moveTo(axisObj.x,axisObj.y+blockHeight/2);
        ctx.lineTo(axisObj.x+blockWidth,axisObj.y+blockHeight/2);
        ctx.lineWidth=1;
        ctx.stroke();
        ctx.fillStyle=textColor;
        ctx.textAlign='left';
        ctx.textBaseline='middle';
        axisObj.x+=blockWidth+textOffsetWidth;
        ctx.fillText(label,axisObj.x,axisObj.y+blockHeight/2,textMaxWidth);
        axisObj.x+=textMaxWidth;
    }

    var drawStack=function(){
        for(var i=0;i<count;i++){
            var prHeight=data.Entities[i].PR*TotalHeight;
            var sbHeight=data.Entities[i].SB*TotalHeight;
            var enHeight=data.Entities[i].EN*TotalHeight;
            var sdHeight=data.Entities[i].SD*TotalHeight;
            var udHeight=data.Entities[i].UD*TotalHeight;
            var nsHeight=data.Entities[i].NS*TotalHeight;
            prHeight=fixHeight(prHeight);
            sbHeight=fixHeight(sbHeight);
            enHeight=fixHeight(enHeight);
            sdHeight=fixHeight(sdHeight);
            udHeight=fixHeight(udHeight);
            nsHeight=fixHeight(nsHeight);

            var x=xAxisWidth+(i+1)*xSpaceWidth-columnlarWidth/2;
            var y=canvasHeight-sampleHeight-yAxisHeight;
            if(data.hasPR&&prHeight){
                y-=prHeight;
                ctx.fillStyle=prColor;
                ctx.fillRect(x,y,columnlarWidth,prHeight);
            }
            if(data.hasSB&&sbHeight){
                y-=sbHeight;
                ctx.fillStyle=sbColor;
                ctx.fillRect(x,y,columnlarWidth,sbHeight);
            }
            if(data.hasEN&&enHeight){
                y-=enHeight;
                ctx.fillStyle=enColor;
                ctx.fillRect(x,y,columnlarWidth,enHeight);
            }
            if(data.hasSD&&sdHeight){
                y-=sdHeight;
                ctx.fillStyle=sdColor;
                ctx.fillRect(x,y,columnlarWidth,sdHeight);
            }
            if(data.hasUD&&udHeight){
                y-=udHeight;
                ctx.fillStyle=udColor;
                ctx.fillRect(x,y,columnlarWidth,udHeight);
            }
            if(data.hasNS&&nsHeight){
                y-=nsHeight;
                ctx.fillStyle=nsColor;
                ctx.fillRect(x,y,columnlarWidth,nsHeight);
            }

        }
    }

    var drawBrokenLine=function(){

        //UPm
        for(var i=0;i<count;i++){
            var upHeight=data.Entities[i].UP*TotalHeight;
            upHeight=fixHeight(upHeight);
            var x=xAxisWidth+(i+1)*xSpaceWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight-upHeight;
            if(i===0){
                ctx.beginPath()
                ctx.moveTo(x,y);
            }else{
                ctx.lineTo(x,y)
            }
        }
        ctx.strokeStyle=upColor;
        ctx.lineWidth=brokenLineWidth;
        ctx.stroke();
        //UUm
        for(var i=0;i<count;i++){
            var uuHeight=data.Entities[i].UU*TotalHeight;
            uuHeight=fixHeight(uuHeight);
            var x=xAxisWidth+(i+1)*xSpaceWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight-uuHeight;
            if(i===0){
                ctx.beginPath();
                ctx.moveTo(x,y);
            }else{
                ctx.lineTo(x,y)
            }
        }
        ctx.strokeStyle=uuColor;
        ctx.lineWidth=brokenLineWidth;
        ctx.stroke();
    }

    var drawXAxisValue=function(){
        var yOffersetHeight=3;
        for(var i=0;i<count;i++){
            var x=xAxisWidth+(i+1)*xSpaceWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight+yOffersetHeight;
            var text=data.Entities[i].EqpID;
            ctx.save();
            ctx.translate(x,y);
            ctx.fillStyle=textColor;
            ctx.rotate(90*Math.PI/180);
            ctx.fillText(text,0,3,yAxisHeight);
            ctx.restore();
        }
    }

   
    drawFrame();
    drawStack();
    drawBrokenLine();
    drawXAxisValue();
}


function drawMonthlyChart(canvasObj,data){
    var columnlarWidth=20;//柱状体宽度
    var canvasWidth=600;
    var xSpaceWidth=40;
    var xLittleSpaceWidth=23;
    var count=data.length;
    var ctx=canvasObj.getContext("2d");
    canvasWidth=2*xAxisWidth+(count+1)*(xSpaceWidth+2*xLittleSpaceWidth);
    canvasObj.width=canvasWidth;
    canvasObj.height=canvasHeight;

    var drawFrame=function(){
        ctx.fillStyle=bgColor;
        ctx.fillRect(0,0,canvasWidth,canvasHeight);
        unitHeight=(canvasHeight-titleHeight-yAxisHeight-sampleHeight)/10;
        var x=xAxisWidth;//x坐标
        var rx=canvasWidth-xAxisWidth;//x次坐标
        for(var i=0;i<=10;i++){
            var text=(100-10*i)+'%';
            var y=titleHeight+i*unitHeight;
            ctx.fillStyle=textColor;
            ctx.textAlign='right';
            ctx.textBaseline='middle';
            ctx.fillText(text,x,y);
            ctx.beginPath();
            ctx.moveTo(x+5,y);
            ctx.lineTo(rx,y);
            ctx.strokeStyle=lineColor;
            ctx.lineWidth=0.5;
            ctx.stroke();
            ctx.fillStyle=textColor;
            ctx.textAlign='right';
            ctx.textBaseline='middle';
            ctx.fillText(text,rx+10,y);
        }
        //画图例
        x=xAxisWidth+5;
        var y=canvasHeight-sampleHeight;
        var axisObj={x,y};
        drawSampleBlock(sdColor,axisObj,'SD(%)');
        drawSampleBlock(udColor,axisObj,'UD(%)');
        drawSampleBlock(enColor,axisObj,'EN(%)');
        drawSampleLine(upColor,axisObj,'UPm(%)');
        drawSampleLine(uuColor,axisObj,'UUm(%)');
        drawSampleLine(targetColor,axisObj,'UPM Target',true,true);
    }

    var drawSampleBlock=function(fillStyle,axisObj,label){
        var blockWidth=40;
        var blockHeight=25;
        var textMaxWidth=25;
        var textOffsetWidth=1;
        ctx.fillStyle=fillStyle;
        ctx.fillRect(axisObj.x,axisObj.y,blockWidth,blockHeight);
        axisObj.x+=blockWidth+textOffsetWidth;
        ctx.fillStyle=textColor;
        ctx.textAlign='left';
        ctx.textBaseline='middle';
        ctx.fillText(label,axisObj.x,axisObj.y+blockHeight/2,textMaxWidth);
        axisObj.x+=textMaxWidth;
    }

    var drawSampleLine=function(strokeStyle,axisObj,label,useDash,notUseMaxWidth){
        var blockWidth=40;
        var blockHeight=25;
        var textMaxWidth=25;
        var textOffsetWidth=1;
        ctx.save();
        ctx.strokeStyle=strokeStyle;
        ctx.beginPath();
        ctx.moveTo(axisObj.x,axisObj.y+blockHeight/2);
        ctx.lineTo(axisObj.x+blockWidth,axisObj.y+blockHeight/2);
        ctx.lineWidth=1;
        if(useDash){ctx.setLineDash([4,2]);}
        ctx.stroke();
        ctx.fillStyle=textColor;
        ctx.textAlign='left';
        ctx.textBaseline='middle';
        axisObj.x+=blockWidth+textOffsetWidth;
        if(notUseMaxWidth){
            ctx.fillText(label,axisObj.x,axisObj.y+blockHeight/2);
        }else{
            ctx.fillText(label,axisObj.x,axisObj.y+blockHeight/2,textMaxWidth);
        }
        ctx.restore();
        axisObj.x+=textMaxWidth;
    }

    var drawCluster=function(){

        for(var i=0;i<count;i++){
            var x=xAxisWidth+(i+1)*xSpaceWidth+(i*xLittleSpaceWidth*2)-columnlarWidth/2;
            var y=canvasHeight-sampleHeight-yAxisHeight;
            var sdHeight=data[i].SD*TotalHeight;
            var udHeight=data[i].UD*TotalHeight;
            var enHeight=data[i].EN*TotalHeight;

            sdHeight=fixHeight(sdHeight);
            udHeight=fixHeight(udHeight);
            enHeight=fixHeight(enHeight);
            drawRect(sdColor,x,y,columnlarWidth,sdHeight);
            drawRect(udColor,x+xLittleSpaceWidth,y,columnlarWidth,udHeight);
            drawRect(enColor,x+2*xLittleSpaceWidth,y,columnlarWidth,enHeight);
        }
        
    }

    var drawBrokenLine=function(){
        var lineXOffsetWidth=xLittleSpaceWidth;
         //UPm
         for(var i=0;i<count;i++){
            var upHeight=data[i].UPm*TotalHeight;
            upHeight=fixHeight(upHeight);
            var x=xAxisWidth+(i+1)*xSpaceWidth+i*2*xLittleSpaceWidth+lineXOffsetWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight-upHeight;
            if(i===0){
                ctx.beginPath()
                ctx.moveTo(x,y);
            }else{
                ctx.lineTo(x,y)
            }
        }
        ctx.strokeStyle=upColor;
        ctx.lineWidth=brokenLineWidth;
        ctx.stroke();

        //UUm
        for(var i=0;i<count;i++){
            var uuHeight=data[i].UUm*TotalHeight;
            uuHeight=fixHeight(uuHeight);
            var x=xAxisWidth+(i+1)*xSpaceWidth+i*2*xLittleSpaceWidth+lineXOffsetWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight-uuHeight;
            if(i===0){
                ctx.beginPath();
                ctx.moveTo(x,y);
            }else{
                ctx.lineTo(x,y)
            }
        }
        ctx.strokeStyle=uuColor;
        ctx.lineWidth=brokenLineWidth;
        ctx.stroke();

        //UPm Target
        var targetHeight=0.9*TotalHeight;
        ctx.save();
        for(var i=0;i<count;i++){
            var x=xAxisWidth+(i+1)*xSpaceWidth+i*2*xLittleSpaceWidth+lineXOffsetWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight-targetHeight;
            if(i===0){
                ctx.beginPath();
                ctx.moveTo(x,y);
            }else{
                ctx.lineTo(x,y)
            }
        }
        ctx.strokeStyle=targetColor;
        ctx.lineWidth=brokenLineWidth;
        ctx.setLineDash([4,2]);
        ctx.stroke();
        ctx.restore();
    }

    var drawRect=function(fillStyle,x,y,width,height){
        if(!height){ return;}
        ctx.fillStyle=fillStyle;
        ctx.fillRect(x,y-height,width,height);
    }

    var drawXAxisValue=function(){
        var yOffersetHeight=10;
        for(var i=0;i<count;i++){
            var x=xAxisWidth+(i+1)*xSpaceWidth+i*2*xLittleSpaceWidth;
            var y=canvasHeight-sampleHeight-yAxisHeight+yOffersetHeight;
            var text=data[i].Date;
            ctx.save();
            ctx.translate(x,y);
            ctx.fillStyle=textColor;
            ctx.textAlign="center";
            //ctx.rotate(90*Math.PI/180);
            ctx.fillText(text,xLittleSpaceWidth+columnlarWidth/2,3);
            ctx.restore();
        }
    }

    drawFrame();
    drawBrokenLine();
    drawCluster();
    drawXAxisValue();
}

 //适配高度，如果高度小数部分在0~0.5之间，那么补足至0.5，如果在0.5至1之间，则减少至0.5。0，0.5,1不变。
 var fixHeight=function(height){
    var res=height;
    if(res%0.5==0)return res;
    var f=Math.floor(height/0.5);
    //如果是奇数表示大于0.5
    if(Math%2==1){
        res=f*0.5;
    }else{
        res=(f+1)*0.5;
    }
    return res;



}
