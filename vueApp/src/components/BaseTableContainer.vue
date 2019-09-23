<template>
  <div class="ibox">
    <div class="ibox-title" v-if="show">
      <label>{{ title }}</label>
    </div>
    <div class="ibox-content">
      <el-row type="flex" justify="end">
        <slot name="left"></slot>
      <div v-if="useExcel" class="downloadBtn">
        <el-button type="primary" icon="el-icon-download" size="mini" v-on:click="table2Excel">{{excelBtnLabel}}</el-button>
      </div>
      <slot name="right"></slot>
      </el-row>
      <div ref="table" class="table-div">
          <slot name="table" :datas="tableData"></slot>
      </div>
      <a ref="dlink" style="display: none;"></a>
    </div>
  </div>
</template>

<script>
export default {
  name: "baseTableContainer",
  props: {
    title: {
      type: String,
      default: () => ""
    },
    useExcel: {
      type: Boolean,
      default: () => true
    },
    excelStyle: {
      type: String
    },
    tableData:{
    },
    fileName:{
      type:String,
      default:'RPT.xls'
    },
    excelBtnLabel:{
      type:String,
      default:'Excel'
    }
  },
  data() {
    return {};
  },
  computed: {
    show: () => {
      return this.title=="" ? false : true;
    }
  },
  methods: {
    table2Excel() {
      let style = '';
      if (this.excelStyle) style = this.excelStyle;
      let uri = "data:application/vnd.ms-excel;base64,";
      let template =
        '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"' +
        'xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>' +
        "<x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>" +
        "</x:ExcelWorkbook></xml><![endif]-->" +style+
        "</head><body>{table}</body></html>";
      let dlink=this.$refs.dlink
      let table=this.$refs.table
      let ctx = {worksheet: 'Worksheet', table: table.innerHTML};
      dlink.href = uri + base64(format(template, ctx));
      dlink.download =this.fileName;
      dlink.click();
    },
    table2Html(){
        let table=this.$refs.table
        let style=window.getComputedStyle(table)
        let newDiv=document.createElement('div')
        newDiv.innerHTML=table.outerHTML
        
        console.log(newDiv)
    }
  }
};
</script>

<style>
    .ibox-content{
        text-align: right;
    }

    .ibox-title{
        text-align: left;
    }

    .ibox-content >.el-row{
      margin-bottom: 10px;
      align-items: baseline;
    }
  
    .ibox-content > .table{
      max-width: 100%;
      overflow: auto;
    }


</style>
