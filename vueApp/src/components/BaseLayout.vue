<template>
  <div class="gray-bg" style="min-height:100%;">
    <div class="wrapper wrapper-content animated fadeInRight">
      <slot></slot>
    </div>
    <div class="footer fixed">
      <div class="pull-right">
        <strong>@AMS Report</strong>
      </div>
      <div>
        <strong>{{ dateStr }}</strong>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "baseLayout",
  data() {
    return {
      date: new Date()
    };
  },
  computed: {
    dateStr: function() {
      let y = this.date.getFullYear();
      let m = this.date.getMonth() + 1;
      m=m<10?'0'+m:m
      let d = this.date.getDate();
      d=d<10?'0'+d:d
      let xq = this.date.getDay();
      switch (xq) {
        case 1:
          xq = "星期一";
          break;
        case 2:
          xq = "星期二";
          break;
        case 3:
          xq = "星期三";
          break;
        case 4:
          xq = "星期四";
          break;
        case 5:
          xq = "星期五";
          break;
        case 6:
          xq = "星期六";
          break;
        case 0:
          xq = "星期日";
          break;
      }
      let h=this.date.getHours()
      let min=this.date.getMinutes()
      let s=this.date.getSeconds()
      h=h<10?'0'+h:h
      min=min<10?'0'+min:min
      s=s<10?'0'+s:s
      return y+'年'+m+'月'+d+'日 '+xq+' '+h+':'+min+':'+s
    }
  }
};
</script>


