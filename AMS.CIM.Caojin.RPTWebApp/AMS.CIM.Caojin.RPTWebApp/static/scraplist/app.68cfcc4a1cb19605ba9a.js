webpackJsonp([1],{"5rzY":function(t,e){},C5bi:function(t,e){},NHnr:function(t,e,a){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var l=a("7+uW"),s=a("zL8q"),o=a.n(s),n=(a("tvR6"),{render:function(){var t=this.$createElement,e=this._self._c||t;return e("div",{attrs:{id:"app"}},[e("router-view")],1)},staticRenderFns:[]});var r=a("VU/8")({name:"App"},n,!1,function(t){a("C5bi")},null,null).exports,i=a("/ocq"),c={name:"baseLayout",data:function(){return{date:new Date}},computed:{dateStr:function(){var t=this.date.getFullYear(),e=this.date.getMonth()+1;e=e<10?"0"+e:e;var a=this.date.getDate();a=a<10?"0"+a:a;var l=this.date.getDay();switch(l){case 1:l="星期一";break;case 2:l="星期二";break;case 3:l="星期三";break;case 4:l="星期四";break;case 5:l="星期五";break;case 6:l="星期六";break;case 0:l="星期日"}var s=this.date.getHours(),o=this.date.getMinutes(),n=this.date.getSeconds();return t+"年"+e+"月"+a+"日 "+l+" "+(s=s<10?"0"+s:s)+":"+(o=o<10?"0"+o:o)+":"+(n=n<10?"0"+n:n)}}},d={render:function(){var t=this.$createElement,e=this._self._c||t;return e("div",{staticClass:"gray-bg",staticStyle:{"min-height":"100%"}},[e("div",{staticClass:"wrapper wrapper-content animated fadeInRight"},[this._t("default")],2),this._v(" "),e("div",{staticClass:"footer fixed"},[this._m(0),this._v(" "),e("div",[e("strong",[this._v(this._s(this.dateStr))])])])])},staticRenderFns:[function(){var t=this.$createElement,e=this._self._c||t;return e("div",{staticClass:"pull-right"},[e("strong",[this._v("@AMS Report")])])}]},u=a("VU/8")(c,d,!1,null,null,null).exports,p={name:"baseContainer",props:{headerHeight:{type:String,default:function(){return"600"}}},data:function(){return{activeNames:["1"]}}},h={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("el-container",[a("el-header",{attrs:{height:t.headerHeight}},[a("el-collapse",{model:{value:t.activeNames,callback:function(e){t.activeNames=e},expression:"activeNames"}},[a("el-collapse-item",{attrs:{title:"查询条件",name:"1"}},[t._t("header")],2)],1)],1),t._v(" "),a("el-main",[t._t("main")],2)],1)},staticRenderFns:[]},v=a("VU/8")(p,h,!1,null,null,null).exports,_={name:"baseHeaderCard",props:{coder:{type:String,default:function(){return"曹晋（0279）"}},user:{type:String,default:function(){return"陈舒（0353）"}},project:{type:String,required:!0}},data:function(){return{count:0}},mounted:function(){var t=this.URL_PREFIX+"/Common/GetClickCount",e={title:this.project},a=this;this.$http.post(t,e).then(function(t){t.data.success?a.count=t.data.count:console.log(t.data.msg)}).catch(function(t){console.log(t)})}},f={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("el-card",{staticClass:"box-card"},[a("div",[t._v("开发者："+t._s(t.coder))]),t._v(" "),a("div",[t._v("需求者："+t._s(t.user))]),t._v(" "),a("div",[t._v("Cilck Count: "+t._s(t.count))]),t._v(" "),t._t("default")],2)},staticRenderFns:[]},m=a("VU/8")(_,f,!1,null,null,null).exports,b=this,x={name:"baseTableContainer",props:{title:{type:String,default:function(){return""}},useExcel:{type:Boolean,default:function(){return!0}},excelStyle:{type:String},tableData:{},fileName:{type:String,default:"RPT.xls"},excelBtnLabel:{type:String,default:"Excel"}},data:function(){return{}},computed:{show:function(){return""!=b.title}},methods:{table2Excel:function(){var t="";this.excelStyle&&(t=this.excelStyle);var e='<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"xmlns="http://www.w3.org/TR/REC-html40"><head>\x3c!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--\x3e'+t+"</head><body>{table}</body></html>",a=this.$refs.dlink,l={worksheet:"Worksheet",table:this.$refs.table.innerHTML};a.href="data:application/vnd.ms-excel;base64,"+base64(format(e,l)),a.download=this.fileName,a.click()},table2Html:function(){var t=this.$refs.table,e=(window.getComputedStyle(t),document.createElement("div"));e.innerHTML=t.outerHTML,console.log(e)}}},y={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"ibox"},[t.show?a("div",{staticClass:"ibox-title"},[a("label",[t._v(t._s(t.title))])]):t._e(),t._v(" "),a("div",{staticClass:"ibox-content"},[a("el-row",{attrs:{type:"flex",justify:"end"}},[t._t("left"),t._v(" "),t.useExcel?a("div",{staticClass:"downloadBtn"},[a("el-button",{attrs:{type:"primary",icon:"el-icon-download",size:"mini"},on:{click:t.table2Excel}},[t._v(t._s(t.excelBtnLabel))])],1):t._e(),t._v(" "),t._t("right")],2),t._v(" "),a("div",{ref:"table",staticClass:"table-div"},[t._t("table",null,{datas:t.tableData})],2),t._v(" "),a("a",{ref:"dlink",staticStyle:{display:"none"}})],1)])},staticRenderFns:[]};var g={name:"ScrapList",components:{BaseLayout:u,BaseContainer:v,BaseHeaderCard:m,BaseTableContainer:a("VU/8")(x,y,!1,function(t){a("5rzY")},null,null).exports},data:function(){return{loading:!1,from:null,to:null,showTable:!1,tableData:{from:null,to:null,rowEntities:[]},selectedModule:[],selectedLotType:[],allModules:[],allLotTypes:[]}},computed:{fileName:function(){return"RPT_ScrapLotList_"+this.tableData.from.replace(/[- :]/g,"")+"_"+this.tableData.to.replace(/[- :]/g,"")+".xls"},tableTitle:function(){return"Scrap Lot List Report:"+this.tableData.from+"~"+this.tableData.to},filteredRowEntities:function(){var t=this,e=this.tableData.rowEntities;return this.selectedLotType.length>0&&(e=e.filter(function(e){return t.selectedLotType.indexOf(e.LotType)>-1})),this.selectedModule.length>0&&(e=e.filter(function(e){return t.selectedModule.indexOf(e.Module)>-1})),e}},methods:{handleQueryClick:function(){if(!this.from||!this.to)return this.$message.error("请选择开始跟结束时间");this.loading=!0;var t=this.URL_PREFIX+"/ReqRpt022/GetTableData",e={startTime:this.getDateStr(this.from)+" 00:00:00",endTime:this.getDateStr(this.to)+" 23:59:59"},a=this;this.$http.post(t,e).then(function(t){t.data.success?(a.tableData.from=e.startTime,a.tableData.to=e.endTime,a.tableData.rowEntities=t.data.RowEntities,a.showTable=!0):(a.$message.error("服务端程序异常"),console.log(t.data.msg)),a.loading=!1}).catch(function(t){console.log(t),a.$message.error("网络异常"),a.loading=!1})},handleClear:function(){this.selectedModule=[],this.selectedLotType=[]},getDateStr:function(t){var e=t.getFullYear(),a=t.getMonth()+1;a=a<10?"0"+a:a;var l=t.getDate();return e+"-"+a+"-"+(l=l<10?"0"+l:l)},getAllModules:function(){var t=this.URL_PREFIX+"/Common/GetAllModule",e=this;this.$http.post(t,{type:1}).then(function(t){t.data.success?e.allModules=t.data.modules:console.log(t.data.msg)}).catch(function(t){console.log(t)})},getAllLotTypes:function(){var t=this.URL_PREFIX+"/Common/GetAllLotType",e=this;this.$http.post(t,{}).then(function(t){t.data.success?e.allLotTypes=t.data.values:console.log(t.data.msg)}).catch(function(t){console.log(t)})}},created:function(){this.getAllModules(),this.getAllLotTypes()}},C={render:function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("BaseLayout",[a("BaseContainer",[a("template",{slot:"header"},[a("el-row",{attrs:{type:"flex",justify:"center"}},[a("el-col",{attrs:{span:20}},[a("el-row",{staticClass:"scraplist-head",attrs:{type:"flex",justify:"center"}},[a("label",[t._v("From:")]),t._v(" "),a("el-date-picker",{attrs:{type:"date",placeholder:"请选择开始日期"},model:{value:t.from,callback:function(e){t.from=e},expression:"from"}}),t._v(" "),a("label",[t._v("To:")]),t._v(" "),a("el-date-picker",{attrs:{type:"date",placeholder:"请选择结束日期"},model:{value:t.to,callback:function(e){t.to=e},expression:"to"}}),t._v(" "),a("el-button",{attrs:{type:"primary",icon:"el-icon-search",loading:t.loading},on:{click:t.handleQueryClick}})],1)],1),t._v(" "),a("el-col",{attrs:{span:4}},[a("BaseHeaderCard",{attrs:{project:"RPT000022",user:"李冬（0490）"}})],1)],1)],1),t._v(" "),a("template",{slot:"main"},[t.showTable?a("BaseTableContainer",{attrs:{tableData:t.filteredRowEntities,fileName:t.fileName,title:t.tableTitle},scopedSlots:t._u([{key:"table",fn:function(e){return a("div",{staticClass:"scraplist-table-div"},[a("table",{staticClass:"table table-responsive table-bordered table-hover"},[a("thead",[a("tr",[a("th",[t._v("No")]),t._v(" "),a("th",[t._v("LotID")]),t._v(" "),a("th",[t._v("Owner")]),t._v(" "),a("th",[t._v("LotType")]),t._v(" "),a("th",[t._v("ScrapType")]),t._v(" "),a("th",[t._v("ScrapDate")]),t._v(" "),a("th",[t._v("EventDate")]),t._v(" "),a("th",[t._v("MainPD")]),t._v(" "),a("th",[t._v("ModulePD")]),t._v(" "),a("th",[t._v("OpeNo")]),t._v(" "),a("th",[t._v("Qty")]),t._v(" "),a("th",[t._v("EqpType")]),t._v(" "),a("th",[t._v("User")]),t._v(" "),a("th",[t._v("Code")]),t._v(" "),a("th",[t._v("Reason")]),t._v(" "),a("th",[t._v("Claim Memo")])])]),t._v(" "),a("tbody",t._l(e.datas,function(e,l){return a("tr",{key:l},[a("th",{domProps:{textContent:t._s(l+1)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.LotID)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.Owner)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.LotType)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.ScrapType)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.ScrapTime)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.EventTime)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.MainPD)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.ModulePD)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.OpeNo)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.Qty)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.EqpType)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.User)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.Code)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.CodeDesc)}}),t._v(" "),a("th",{domProps:{textContent:t._s(e.ClaimMemo)}})])}),0)])])}}],null,!1,3950485358)},[t._v(" "),a("div",{staticClass:"scraplist-filter-div",attrs:{slot:"left"},slot:"left"},[a("div",{staticClass:"scraplist-box"},[a("label",[t._v("LotType:")]),t._v(" "),a("el-select",{attrs:{size:"small",placeholder:"All",multiple:""},model:{value:t.selectedLotType,callback:function(e){t.selectedLotType=e},expression:"selectedLotType"}},t._l(t.allLotTypes,function(t,e){return a("el-option",{key:e,attrs:{label:t,value:t}})}),1)],1),t._v(" "),a("div",{staticClass:"scraplist-box"},[a("label",[t._v("Module:")]),t._v(" "),a("el-select",{attrs:{size:"small",placeholder:"All",multiple:""},model:{value:t.selectedModule,callback:function(e){t.selectedModule=e},expression:"selectedModule"}},t._l(t.allModules,function(t,e){return a("el-option",{key:e,attrs:{label:t.Description,value:t.Description}})}),1)],1),t._v(" "),a("div",{staticClass:"scraplist-box"},[a("el-button",{attrs:{type:"primary",size:"small"},on:{click:t.handleClear}},[t._v("重置")])],1)])]):t._e()],1)],2)],1)},staticRenderFns:[]};var T=a("VU/8")(g,C,!1,function(t){a("jx0T")},null,null).exports;l.default.use(i.a);var k=new i.a({routes:[{path:"/",name:"HelloWorld",component:T}]}),L=a("mtWM"),w=a.n(L);l.default.config.productionTip=!1,l.default.use(o.a),l.default.prototype.$http=w.a,l.default.prototype.URL_PREFIX="..",new l.default({el:"#app",router:k,components:{App:r},template:"<App/>"})},jx0T:function(t,e){},tvR6:function(t,e){}},["NHnr"]);
//# sourceMappingURL=app.68cfcc4a1cb19605ba9a.js.map