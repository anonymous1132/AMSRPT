webpackJsonp([1],{"5fxX":function(t,e){},"5rzY":function(t,e){},C5bi:function(t,e){},NHnr:function(t,e,a){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var r=a("7+uW"),o=a("zL8q"),n=a.n(o),s=(a("tvR6"),{render:function(){var t=this.$createElement,e=this._self._c||t;return e("div",{attrs:{id:"app"}},[e("router-view")],1)},staticRenderFns:[]});var l=a("VU/8")({name:"App"},s,!1,function(t){a("C5bi")},null,null).exports,i=a("/ocq"),c={name:"baseLayout",data:function(){return{date:new Date}},computed:{dateStr:function(){var t=this.date.getFullYear(),e=this.date.getMonth()+1;e=e<10?"0"+e:e;var a=this.date.getDate();a=a<10?"0"+a:a;var r=this.date.getDay();switch(r){case 1:r="星期一";break;case 2:r="星期二";break;case 3:r="星期三";break;case 4:r="星期四";break;case 5:r="星期五";break;case 6:r="星期六";break;case 0:r="星期日"}var o=this.date.getHours(),n=this.date.getMinutes(),s=this.date.getSeconds();return t+"年"+e+"月"+a+"日 "+r+" "+(o=o<10?"0"+o:o)+":"+(n=n<10?"0"+n:n)+":"+(s=s<10?"0"+s:s)}}},d={render:function(){var t=this.$createElement,e=this._self._c||t;return e("div",{staticClass:"gray-bg",staticStyle:{"min-height":"100%"}},[e("div",{staticClass:"wrapper wrapper-content animated fadeInRight"},[this._t("default")],2),this._v(" "),e("div",{staticClass:"footer fixed"},[this._m(0),this._v(" "),e("div",[e("strong",[this._v(this._s(this.dateStr))])])])])},staticRenderFns:[function(){var t=this.$createElement,e=this._self._c||t;return e("div",{staticClass:"pull-right"},[e("strong",[this._v("@AMS Report")])])}]},u=a("VU/8")(c,d,!1,null,null,null).exports,p={name:"baseContainer",props:{headerHeight:{type:String,default:function(){return"600"}}},data:function(){return{activeNames:["1"]}}},f={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("el-container",[a("el-header",{attrs:{height:t.headerHeight}},[a("el-collapse",{model:{value:t.activeNames,callback:function(e){t.activeNames=e},expression:"activeNames"}},[a("el-collapse-item",{attrs:{title:"查询条件",name:"1"}},[t._t("header")],2)],1)],1),t._v(" "),a("el-main",[t._t("main")],2)],1)},staticRenderFns:[]},h=a("VU/8")(p,f,!1,null,null,null).exports,v={name:"baseHeaderCard",props:{coder:{type:String,default:function(){return"曹晋（0279）"}},user:{type:String,default:function(){return"陈舒（0353）"}},project:{type:String,required:!0}},data:function(){return{count:0}},mounted:function(){var t=this.URL_PREFIX+"/Common/GetClickCount",e={title:this.project},a=this;this.$http.post(t,e).then(function(t){t.data.success?a.count=t.data.count:console.log(t.data.msg)}).catch(function(t){console.log(t)})}},m={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("el-card",{staticClass:"box-card",attrs:{shadow:"hover"}},[a("div",[t._v("开发者："+t._s(t.coder))]),t._v(" "),a("div",[t._v("需求者："+t._s(t.user))]),t._v(" "),a("div",[t._v("Cilck Count: "+t._s(t.count))]),t._v(" "),t._t("default")],2)},staticRenderFns:[]},_=a("VU/8")(v,m,!1,null,null,null).exports,g=this,b={name:"baseTableContainer",props:{title:{type:String,default:function(){return""}},useExcel:{type:Boolean,default:function(){return!0}},excelStyle:{type:String},tableData:{},fileName:{type:String,default:"RPT.xls"},excelBtnLabel:{type:String,default:"Excel"}},data:function(){return{}},computed:{show:function(){return""!=g.title}},methods:{table2Excel:function(){var t="";this.excelStyle&&(t=this.excelStyle);var e='<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"xmlns="http://www.w3.org/TR/REC-html40"><head>\x3c!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--\x3e'+t+"</head><body>{table}</body></html>",a=this.$refs.dlink,r={worksheet:"Worksheet",table:this.$refs.table.innerHTML};a.href="data:application/vnd.ms-excel;base64,"+base64(format(e,r)),a.download=this.fileName,a.click()},table2Html:function(){var t=this.$refs.table,e=(window.getComputedStyle(t),document.createElement("div"));e.innerHTML=t.outerHTML,console.log(e)}}},x={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"ibox"},[t.show?a("div",{staticClass:"ibox-title"},[a("label",[t._v(t._s(t.title))])]):t._e(),t._v(" "),a("div",{staticClass:"ibox-content"},[a("el-row",{attrs:{type:"flex",justify:"end"}},[t._t("left"),t._v(" "),t.useExcel?a("div",{staticClass:"downloadBtn"},[a("el-button",{attrs:{type:"primary",icon:"el-icon-download",size:"mini"},on:{click:t.table2Excel}},[t._v(t._s(t.excelBtnLabel))])],1):t._e(),t._v(" "),t._t("right")],2),t._v(" "),a("div",{ref:"table",staticClass:"table-div"},[t._t("table",null,{datas:t.tableData})],2),t._v(" "),a("a",{ref:"dlink",staticStyle:{display:"none"}})],1)])},staticRenderFns:[]};var D={name:"EDALotCompareProcessTool",components:{BaseLayout:u,BaseContainer:h,BaseHeaderCard:_,BaseTableContainer:a("VU/8")(b,x,!1,function(t){a("5rzY")},null,null).exports},data:function(){return{autoPeriod:"",loading:!1,prodCategory:"",prod:"",specItem:[],measItem:[],lotID:[],waferID:[],tableData:[],title:"",allProdCategory:["Production","Dummy","Process Monitor","Equipment Monitor"],allProds:[],allWaferID:[]}},computed:{avaProds:function(){var t=this;return this.prodCategory?this.allProds.filter(function(e){return e.Prod_Category_ID===t.prodCategory}):this.allProds},prodFilteredData:function(){var t=this,e=this.tableData;return null!==this.prod&&""!==this.prod&&(e=e.filter(function(e){return e.ProdID===t.prod})),e},specFilteredData:function(){var t=this,e=this.prodFilteredData;return this.specItem.length>0&&(e=e.filter(function(e){return t.specItem.indexOf(e.SpecItem)>-1})),e},allSpecItem:function(){var t=this.prodFilteredData.map(function(t){return t.SpecItem});return t.distinct(),t.sort(),this.specItem=[],t},allLotId:function(){var t=this.specFilteredData.map(function(t){return t.LotID});return t.distinct(),t.sort(),this.lotID=[],t},filteredAutoTableData:function(){var t=this,e=this.specFilteredData;return this.lotID.length>0&&(e=e.filter(function(e){return t.lotID.indexOf(e.LotID)>-1})),e},viewWaferID:function(){return 0==this.waferID.length?this.allWaferID.map(function(t,e){return e}):this.waferID.sort()},showTable:function(){return""!==this.title}},methods:{handleAutoQueryClick:function(){var t=this,e=this.autoPeriod,a=void 0,r=void 0;if(e)a=getDateStr(e[0]),r=getDateStr(e[1]);else{r=getCurDate();var o=new Date;o.setDate(o.getDate()-10),a=getDateStr(o)}var n={from:a,to:r};this.queryFun(n,function(e){t.title="From:"+e.data.StartDate+" To:"+e.data.EndDate,t.tableData=e.data.RowEntities})},queryFun:function(t,e){var a=this.URL_PREFIX+"/ReqRpt212/GetTableData",r=this;this.loading=!0,this.$http.post(a,t).then(function(t){if(t.data.success)e(t);else{if("没有数据"==t.data.msg)return r.$message.error("没有查询到符合条件的数据");console.log(t.data.msg),r.$message.error("服务端程序异常")}r.loading=!1}).catch(function(t){console.log(t),r.$message.error("网络异常"),r.loading=!1})},handleClear:function(){this.prodCategory="",this.prod="",this.specItem=[],this.lotID=[],this.waferID=[]},querySearch:function(t,e){var a=this.avaProds,r=t?a.filter(function(e){return 0===e.ProdSpec_ID.toLowerCase().indexOf(t.toLowerCase())}):a;r.map(function(t){return t.value=t.ProdSpec_ID}),e(r)}},created:function(){var t=this.URL_PREFIX+"/ReqRpt209/GetProdList";this.loading=!0;for(var e=1;e<26;e++)this.allWaferID.push(e<10?"#0"+e:"#"+e);var a=this;this.$http.post(t).then(function(t){t.data.success?a.allProds=t.data.prodList:(a.$message.error("网页初始化失败"),console.log(t.data.msg)),a.loading=!1}).catch(function(t){a.$message.error("网页初始化失败"),console.log(t),a.loading=!1}),Array.prototype.distinct=function(){for(var t=[],e=0;e<this.length;e++)for(var a=e+1;a<this.length;)this[e]===this[a]?t.push(this.splice(a,1)[0]):a++;return t}}},y={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("BaseLayout",[a("BaseContainer",[a("template",{slot:"header"},[a("el-row",{attrs:{type:"flex",justify:"start"}},[a("el-col",{attrs:{lg:12,offset:1,md:12}},[a("div",{staticClass:"eda-inline-auto"},[a("div",{staticClass:"eda-inline-date-query"},[a("el-date-picker",{attrs:{type:"daterange","range-separator":"~","start-placeholder":"开始日期,00:00:00","end-placeholder":"结束日期,23:59:59"},model:{value:t.autoPeriod,callback:function(e){t.autoPeriod=e},expression:"autoPeriod"}}),t._v(" "),a("el-button",{attrs:{type:"primary",icon:"el-icon-search",loading:t.loading},on:{click:t.handleAutoQueryClick}}),t._v(" "),a("el-button",{attrs:{type:"primary"},on:{click:t.handleClear}},[t._v("重置")])],1),t._v(" "),a("div",{staticClass:"eda-inline-select-group"},[a("el-select",{attrs:{placeholder:"请选择Product Category",clearable:""},model:{value:t.prodCategory,callback:function(e){t.prodCategory=e},expression:"prodCategory"}},t._l(t.allProdCategory,function(t){return a("el-option",{key:t,attrs:{label:t,value:t}})}),1),t._v(" "),a("el-autocomplete",{attrs:{placeholder:"请输入Product","fetch-suggestions":t.querySearch},model:{value:t.prod,callback:function(e){t.prod=e},expression:"prod"}}),t._v(" "),a("el-select",{attrs:{multiple:"",placeholder:"请选择LotID"},model:{value:t.lotID,callback:function(e){t.lotID=e},expression:"lotID"}},t._l(t.allLotId,function(t,e){return a("el-option",{key:e,attrs:{value:t}})}),1),t._v(" "),a("el-select",{attrs:{multiple:"",placeholder:"请选择WaferID"},model:{value:t.waferID,callback:function(e){t.waferID=e},expression:"waferID"}},t._l(t.allWaferID,function(t,e){return a("el-option",{key:e,attrs:{value:e,label:t}})}),1)],1)])]),t._v(" "),a("el-col",{attrs:{lg:4,offset:5,md:8,sm:12}},[a("BaseHeaderCard",{attrs:{project:"RPT000206",user:"姜兆涛（0102）"}})],1)],1)],1),t._v(" "),a("template",{slot:"main"},[t.showTable?a("BaseTableContainer",{attrs:{title:t.title,tableData:t.filteredAutoTableData,fileName:"EDA_LotCompareProcessTool.xls"},scopedSlots:t._u([{key:"table",fn:function(e){return a("div",{staticClass:"eda-table-div"},[a("table",{staticClass:"table table-responsive table-bordered table-hover"},[a("thead",[a("tr",[a("th",[t._v("No")]),t._v(" "),a("th",[t._v("Route ID")]),t._v(" "),a("th",[t._v("Oper ID")]),t._v(" "),a("th",[t._v("Oper No.")]),t._v(" "),a("th",[t._v("Oper Name")]),t._v(" "),a("th",[t._v("Lot ID")]),t._v(" "),a("th",[t._v("Process EQP")]),t._v(" "),a("th",[t._v("Oper Time")]),t._v(" "),t._l(t.viewWaferID,function(e){return a("th",{key:"h"+t.allWaferID[e],domProps:{textContent:t._s(t.allWaferID[e])}})})],2)]),t._v(" "),a("tbody",t._l(e.datas,function(e,r){return a("tr",{key:r},[a("td",{domProps:{textContent:t._s(r+1)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.RouteID)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.OperID)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.OpeNo)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.OpeName)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.LotID)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.EQP)}}),t._v(" "),a("td",{domProps:{textContent:t._s(e.OperTime)}}),t._v(" "),t._l(t.viewWaferID,function(r){return a("td",{key:"s"+t.allWaferID[r],domProps:{textContent:t._s(e.ChamberArray[r])}})})],2)}),0)])])}}],null,!1,2809910929)}):t._e()],1)],2)],1)},staticRenderFns:[]};var C=a("VU/8")(D,y,!1,function(t){a("5fxX")},null,null).exports;r.default.use(i.a);var I=new i.a({routes:[{path:"/",name:"HelloWorld",component:C}]}),k=a("mtWM"),P=a.n(k);r.default.config.productionTip=!1,r.default.use(n.a),r.default.prototype.$http=P.a,r.default.prototype.URL_PREFIX="..",new r.default({el:"#app",router:I,components:{App:l},template:"<App/>"})},tvR6:function(t,e){}},["NHnr"]);
//# sourceMappingURL=app.fc8ba635178b3a7c04a2.js.map