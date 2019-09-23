<template>
<div>
     <el-row :gutter="20">
  <el-col :span="6"><div class="grid-content bg-purple">
    Recipe ID :<el-input id="in1" v-model="rname"></el-input> 
    </div></el-col>
  <el-col :span="6"><div class="grid-content bg-purple">
    Site No: <el-input id="in2" v-model="count"></el-input>
    </div></el-col>
    <el-col :span="6"><div class="grid-content bg-purple">
    Owner: <el-input id="in3" v-model="owner"></el-input>
    </div></el-col>
</el-row>
<el-row :gutter="20"  v-loading="loading">
  <el-col :span="6"><div class="grid-content  "></div></el-col>
  <el-col :span="2"><div class="grid-content  ">
    <el-button type="info" @click="createtable">add</el-button>
    </div></el-col>
    <el-col :span="2"><div class="grid-content  ">
    <el-button type="info" @click="querytable">query</el-button>
    </div></el-col>
  <el-col :span="2"><div class="grid-content  ">
    <el-button type="info" v-on:click="clear">clear</el-button>   
    </div></el-col>
</el-row>
<!-- <div v-if="addvisable"> -->
<div>
<el-table id="savetable" 
      :data="tableData"
      style="width: 65%">
      <el-table-column
        label="Recipe Name"
        width="200">
        <template slot-scope="scope">
        <span style="margin-left: 10px">{{ scope.row.RECIPENAME }}</span>
      </template>
      </el-table-column>
      <el-table-column
        label="Site"
        width="100">
        <template slot-scope="scope">
        <span style="margin-left: 10px">{{ scope.row.SITENAME }}</span>
      </template>
      </el-table-column>
       <el-table-column
        label="OWNER"
        width="120">
        <template slot-scope="scope">
        <el-input  placeholder="请输入工号" v-model="scope.row.OWNER"></el-input>
      </template>
      </el-table-column>
      <el-table-column width="200" label="X">
        
        <template slot-scope="scope">
        <el-input  placeholder="请输入X坐标" v-model="scope.row.COORDINATEX"></el-input>
      </template>
      </el-table-column>
      <el-table-column width="200" label="Y">
        <template slot-scope="scope">
        <el-input  placeholder="请输入X坐标" v-model="scope.row.COORDINATEY"></el-input>
      </template>
      </el-table-column>
    </el-table>
    <el-row :gutter="20">
  <el-col :span="6"><div class="grid-content  "></div></el-col>
  <el-col :span="2"><div class="grid-content  ">
    <el-button type="info" @click="savedata">save</el-button>
    </div></el-col>
  <el-col :span="2"><div class="grid-content  ">
    <el-button type="info" v-on:click="clear2">clear</el-button>   
    </div></el-col>
</el-row>
</div>


<div>
<!-- <div v-if="queryvisable"> -->
    <el-table id="queryData" 
      :data="queryData"
      style="width: 80%">
      <el-table-column
        label="Recipe Name"
        width="200"
        prop="RECIPENAME">
        
      </el-table-column>
      <el-table-column
        label="Site"
        width="100"
        prop="SITENAME">
      </el-table-column>
      <el-table-column
        label="COORDINATE"
        width="120"
        prop="COORDINATE">
      </el-table-column>
      <el-table-column width="120" label="Owner">
      <template slot-scope="scope">
        <el-input  placeholder="请输入工号" v-model="scope.row.OWNER"></el-input>
      </template>
      </el-table-column>
      <el-table-column width="200" label="Time"
      prop="CREATETIME">
      </el-table-column>
      <el-table-column width="200" label="X">
      <template slot-scope="scope">
        <el-input  placeholder="请输入X坐标" v-model="scope.row.COORDINATEX"></el-input>
      </template>
      </el-table-column>
      <el-table-column width="200" label="Y">
      <template slot-scope="scope">
        <el-input  placeholder="请输入y坐标" v-model="scope.row.COORDINATEY"></el-input>         
      </template>
      </el-table-column>
      <el-table-column label="操作">
      <template slot-scope="scope">
        <el-button
          size="mini"
          type="danger"
          @click="handleDelete(scope.$index, scope.row)">删除</el-button>
      </template>
    </el-table-column>
    </el-table>
    <el-row :gutter="20">
        <el-col :span="6"><div class="grid-content  "></div></el-col>
        <el-col :span="2"><div class="grid-content  ">
          <el-button type="info" @click="changedata">change</el-button>
          </div></el-col>
      </el-row>
</div>
</div>

</template>
<script>
 export default {
   data() {
        return {
          tableData: [],
          count:'',
          rname:'',
          isvisable:false,
          queryData:[],
          loading:false,
          addvisable:false,
          queryvisable:false,
          owner:''
        }
      },
    methods:{
      clear:function(){
          this.rname='';
          this.count='';
      },
      clear2:function()
      {
        let _this=this;
        for(var i=0;i<_this.tableData.length;i++)
        {
          _this.tableData[i]["COORDINATEX"]="";
          _this.tableData[i]["COORDINATEY"]="";
        }
      },
      createtable:function(){
          let _this=this;
          var reg = /^[1-9]\d*$/;
          if(_this.rname==""||_this.rname==null)
          {
            alert("请先输入RecipeName再查询");
            return;
          }
          if(!reg.test(_this.count))
          {
            alert("Site No 请输入数字");
            return;
          }
          if(_this.owner=="")
          {
            alert("Owner请输入工号");
            return;
          }
          var a=new Array(Number( _this.count));
          for(var i=0;i<Number( _this.count);i++)
          {
            var o=new Object();
            o.RECIPENAME=_this.rname;
            o.SITENAME="Site"+(i+1);
            o.OWNER=_this.owner;
            o.CREATETIME='';
            o.COORDINATEX='';
            o.COORDINATEY='';
            a[i]=o;
          }
          _this.tableData=a;
          _this.addvisable=true;
          _this.queryvisable=false;
      },
      savedata:function(){
        let _this=this;
        if(_this.tableData.length==0)
        {
          alert("请先添加数据再进行保存！");
          return;
        }
        let url = this.URL_PREFIX + "/ReqRpt216/savecoordinate";
        var date=new Date();

        	var year=date.getFullYear();
        	var month=date.getMonth();
        	var day=date.getDate();

            var hour=date.getHours();
            var minute=date.getMinutes();
            var second=date.getSeconds();

            if (hour<10) {
            	hour='0'+hour;
            }
            if (minute<10) {
            	minute='0'+minute;
            }
            if(second<10)
            {
              second='0'+second;
            }
            var nowtime=year+'-'+(month+1)+'-'+day+' '+hour+':'+minute+':'+second;
        for(var i=0;i<_this.tableData.length;i++)
        {
          if(_this.tableData[i]["OWNER"]==""||_this.tableData[i]["COORDINATEX"]==""||_this.tableData[i]["COORDINATEY"]=="")
          {
            alert("有栏位的值未输入");
            return;
          }
          _this.tableData[i]["CREATETIME"]=nowtime;
          _this.tableData[i]["COORDINATEX"]=Number(_this.tableData[i]["COORDINATEX"]);
          _this.tableData[i]["COORDINATEY"]=Number(_this.tableData[i]["COORDINATEY"]);
        }
        

        let savedata={savelist:_this.tableData};
          $.ajax({
                    url: url,
                    type: "post",
                    contentType: "application/x-www-form-urlencoded; ", //默认，表单提交格式
                    dataType: "json",
                    data: savedata ,
                    success: function (data,status) {
                        if(data=="exist")
                          {
                            alert("该RecipeName已经存在，请重新输入");
                          }else
                          {
                            alert("添加成功");
                          }
                        
                    },
                    error:function (data,status)
                    {
                        alert("系统错误，请重新输入或联系IT");
                    },
                    async: false
            });
      },
      querytable:function(){
        let _this=this;
       if(_this.rname=="")
       {
         alert("请先输入RecipeName再查询");
         return;
       }

      _this.loading=true;
        let url = this.URL_PREFIX + "/ReqRpt216/querycoordinate";
        $.ajax({
                    url: url,
                    type: "post",
                    contentType: "application/x-www-form-urlencoded; ", //默认，表单提交格式
                    dataType: "json",
                    data: {RecipeName:_this.rname} ,
                    success: function (data,status) {
                        if(data=='nodata')
                        {
                          alert("该RecipeName没有Coordinate，请先添加再查询");
                        }else
                        {
                          _this.queryData=JSON.parse(data);
                          _this.queryvisable=true;
                        }
                    },
                    error:function (data,status)
                    {
                        alert("系统错误，请重新输入或联系IT");
                    },
                    async: false
            });
         _this.addvisable=false;
      _this.loading=false;
      },
      changedata:function(){

            var date=new Date();

        	var year=date.getFullYear();
        	var month=date.getMonth();
        	var day=date.getDate();

            var hour=date.getHours();
            var minute=date.getMinutes();
            var second=date.getSeconds();

            if (hour<10) {
            	hour='0'+hour;
            }
            if (minute<10) {
            	minute='0'+minute;
            }
            if(second<10)
            {
              second='0'+second;
            }
            var nowtime=year+'-'+(month+1)+'-'+day+' '+hour+':'+minute+':'+second;

            let _this=this;
            let url = this.URL_PREFIX + "/ReqRpt216/changecoordinate";
            var needchangearray=new Array();
            //queryData
            for(var i=0;i<_this.queryData.length;i++)
            {
              if((_this.queryData[i]["COORDINATEX"]==null&&_this.queryData[i]["COORDINATEY"]!=""&&_this.queryData[i]["COORDINATEY"]!=null)||(_this.queryData[i]["COORDINATEY"]==null&&(_this.queryData[i]["COORDINATEX"]!=""&&_this.queryData[i]["COORDINATEX"]!=null)) )
              {
                  alert("x坐标和y坐标同时需要输入或者不输入");
                        return;
              }
              if((_this.queryData[i]["COORDINATEX"]==""&&_this.queryData[i]["COORDINATEY"]!=""&&_this.queryData[i]["COORDINATEY"]!=null)||(_this.queryData[i]["COORDINATEY"]==""&&(_this.queryData[i]["COORDINATEX"]!=""&&_this.queryData[i]["COORDINATEX"]!=null)) )
              {
                  alert("x坐标和y坐标同时需要输入或者不输入");
                        return;
              }
              if(_this.queryData[i]["COORDINATEX"]!=""&&_this.queryData[i]["COORDINATEX"]!=null)
              {
                _this.queryData[i]["CREATETIME"]=nowtime;
                needchangearray.push(_this.queryData[i]);
              }
                
            }
            if(needchangearray.length==0)
            {
              alert("请先输入想要修改的值，再修改！");
              return;
            }
            let changedata={changelist:needchangearray};
            $.ajax({
                    url: url,
                    type: "post",
                    contentType: "application/x-www-form-urlencoded; ", //默认，表单提交格式
                    dataType: "json",
                    data: changedata ,
                    success: function (data,status) {
                        if(data=="success")
                        {
                          alert("修改成功");
                        }else
                        {
                          alert("操作失败");
                        }
                    },
                    error:function (data,status)
                    {
                        alert("系统错误，请重新输入或联系IT");
                    }
            });

      },
      handleDelete:function(index, row) {
        let _this=this;
         let url = this.URL_PREFIX + "/ReqRpt216/deletecoordinate";
         $.ajax({
                    url: url,
                    type: "post",
                    contentType: "application/x-www-form-urlencoded; ", //默认，表单提交格式
                    dataType: "json",
                    data: {RECIPENAME:row.RECIPENAME,SITENAME:row.SITENAME} ,
                    success: function (data,status) {
                        if(data=='success')
                        {
                          alert("删除成功");
                        }else
                        {
                         alert("操作失败");
                        }
                    },
                    error:function (data,status)
                    {
                        alert("系统错误，请重新操作或联系IT");
                    },
                    async: false
            });
            for(var i=_this.queryData.length;i>0;i--)
            {
              if(_this.queryData[i-1]["SITENAME"]==row.SITENAME)
              {
                _this.queryData.splice(i-1,1);
                return;
              }
            }
      }
      
    }
}

</script>
<style>
 .el-row {
    margin-bottom: 10px;
   
  }
  .el-col {
    border-radius: 4px;
  }
  .bg-purple-dark {
    background: #99a9bf;
  }
  .bg-purple {
    background: #d3dce6;
  }
  .bg-purple-light {
    background: #e5e9f2;
  }
  .grid-content {
    border-radius: 4px;
    min-height: 36px;
    margin-top:5px;
    padding-top: 5px; 
  }
  .row-bg {
    padding: 5px 0;
    background-color: #f9fafc;
  }
</style>