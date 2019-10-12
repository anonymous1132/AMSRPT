<template>
    <div class="c1">
        <div style="background-color: #00B0F0;height:50px;">
            <el-row >
                    <el-col :span="4">
                        <div >
                            <el-dropdown trigger="click"  class="dropdown1">
                                <span class="el-dropdown-link" style="font-size: 25px;" width="300px">
                                    {{datatypename}}<i class="el-icon-arrow-down el-icon--right"></i>
                                </span>
                                <el-dropdown-menu slot="dropdown" class="dropdown1">
                                    <el-dropdown-item @click.native="funcalldata">
                                        <span style="font-size: 10px;">
                                        WAT all data
                                        </span>
                                    </el-dropdown-item>
                                    <el-dropdown-item @click.native="funcoutspecdata">
                                        <span>WAT out of Spec</span></el-dropdown-item>
                                </el-dropdown-menu>
                             </el-dropdown>
                        </div>
                    </el-col>
                </el-row>
        </div>
         
                <div style="height:5px;background-color:#F3F3F4;"></div>
                <div style="background-color:#D78D3C;height:30px;"></div>
                <div style="height:50px; margin-top:10px;">
                    <el-row :gutter="10">
                        <el-col :span="2" style="padding-top:12px;">
                                    Lot ID :
                        </el-col>
                        <el-col :span="3" >
                                    <el-select v-model="value" placeholder="请选择">
                                        <el-option
                                        v-for="item in lotnameitems"
                                        :key="item.name"
                                        :label="item.name"
                                        :value="item.name">
                                        </el-option>
                                    </el-select>
                        </el-col>
                         <el-col :span="2" style="padding-top:12px;">
                                    Recipe :
                        </el-col>
                        <el-col :span="6">
                               <el-select v-model="value" placeholder="请选择">
                                        <el-option
                                        v-for="item in lotnameitems"
                                        :key="item.name"
                                        :label="item.name"
                                        :value="item.name">
                                        </el-option>
                                    </el-select> 
                        </el-col>
                        <el-col :span="2" style="padding-top:12px;">
                                    Version :
                        </el-col>
                        <el-col :span="6">
                                <el-input id="in1" ></el-input> 
                        </el-col>
                        </el-row>
                </div>
                <div>
                    <el-row :gutter="20">
                        <el-col :span="10"><div class="grid-content"> </div></el-col>
                        <el-col :span="2"><el-button type="info" @click="queryData">query</el-button></el-col>
                        <el-col :span="2"><el-button type="info">clear</el-button></el-col>
                        <el-col :span="10"><div class="grid-content"></div></el-col>
                  </el-row>
                </div>
                <div>
                    <el-table
                        :data="showdata"
                        style="width: 100%">
                        <el-table-column
                            label="RecipeID"
                            width="180">
                            <template slot-scope="scope">
                                <span style="margin-left: 10px">{{ scope.row.date }}</span>
                            </template>
                                                </el-table-column>
                                                <el-table-column
                                                    label="LotID"
                                                    width="180">
                                                    <template slot-scope="scope">
                                <span style="margin-left: 10px">{{ scope.row.name }}</span>
                            </template>
                                                </el-table-column>
                                                <el-table-column
                                                    label="site1"
                                                    width="180">
                                                    <template slot-scope="scope">
                                <span style="margin-left: 10px" :class="{red:!(scope.row.site1>=scope.row.SPECLOW&&scope.row.site1<=scope.row.SPECHIGH)}">{{ scope.row.site1 }}</span>
                            </template>
                                                </el-table-column>
                                                <el-table-column
                                                    label="site2"
                                                    width="180">
                                                    <template slot-scope="scope">
                                <span style="margin-left: 10px" :class="{red:!(scope.row.site2>=scope.row.SPECLOW&&scope.row.site2<=scope.row.SPECHIGH)}">{{ scope.row.site2 }}</span>
                            </template>
                                                </el-table-column>
                                                <el-table-column
                                                    label="SPEC HIGH">
                                                    <template slot-scope="scope">
                                <span style="margin-left: 10px" >{{ scope.row.SPECHIGH }}</span>
                            </template>
                                                </el-table-column>
                                                <el-table-column
                                                    label="SPEC LOW">
                                                    <template slot-scope="scope">
                                <span style="margin-left: 10px" >{{ scope.row.SPECLOW }}</span>
                            </template>
                                                </el-table-column>
                                                </el-table>
                </div>
    </div>
    
</template>

<script>
export default {
    data(){
        return {
            showdata:[],
            alldata:[],
            outspecdata:[],
            initialdata: [{
            date: 'IMPFI_DCT_V01',
            name: 'APAA00000.01',
            site1:111,
            site2:44,
            SPECHIGH:45,
            SPECLOW:40,
            isvisable:''
          }, {
           date: 'IMPFI_DCT_V01',
            name: 'APAA00000.01',
            site1:1,
            site2:32,
            SPECHIGH:35,
            SPECLOW:30,
            isvisable:''

          }, {
           date: 'IMPFI_DCT_V01',
            name: 'APAA00000.01',
            site1:22,
            site2:12,
            SPECHIGH:25,
            SPECLOW:20,
            isvisable:''

          }, {
           date: 'IMPFI_DCT_V01',
            name: 'APAA00000.01',
            site1:12,
            site2:13,
            SPECHIGH:15,
            SPECLOW:10,
            isvisable:''

          }, {
              date: 'IMPFI_DCT_V01',
            name: 'APAA00000.01',
            site1:2,
            site2:4,
            SPECHIGH:10,
            SPECLOW:5,
            isvisable:''

          }],
          datatype:true,//alldata或者out of spec data,默认时alldata
          datatypename:'WAT all data',
          lotnameitems:[],
          value: ''
        }
    },
     methods:{
        funcalldata:function(){
            let _this=this;
            _this.alldata=_this.initialdata;
            _this.showdata=_this.alldata;
            _this.datatype=true;
            _this.datatypename="WAT all data";
        },
        funcoutspecdata:function(){
            let _this=this;
            _this.outspecdata=_this.initialdata;
            _this.datatype=true;
            _this.datatypename="WAT out of Spec";
            if(_this.outspecdata.length>0)
            {

            }

            _this.showdata=_this.outspecdata;

        },queryData:function(){
            let url = this.URL_PREFIX + "/ReqRpt217/analysisdata";
            $.ajax({
                        url: url,
                        type: "post",
                        contentType: "application/x-www-form-urlencoded; ", //默认，表单提交格式
                        dataType: "json",
                        data: {RecipeName:''} ,
                        success: function (data,status) {
                            console.log(data);
                        },
                        error:function (data,status)
                        {
                            alert("系统错误，请重新输入或联系IT");
                        },
                        async: false
                });
        }
    },
    beforeMount:function(){
        var _this=this;
        let url = this.URL_PREFIX + "/ReqRpt217/analysisdata";
        $.ajax({
                    url: url,
                    type: "post",
                    contentType: "application/x-www-form-urlencoded; ", //默认，表单提交格式
                    dataType: "json",
                    data: '' ,
                    success: function (data,status) {
                        // var array=new Array();
                        // for(var i=0;i<data.length;i++)
                        // {
                        //     var o=new Object();
                        //     o.name=data[i];
                        //     array.push(o);
                        // }
                        // _this.lotnameitems=JSON.parse(JSON.stringify(array));
                        console.log(data);
                    },
                    error:function (data,status)
                    {
                        alert("系统错误，请重新输入或联系IT");
                    },
                    async: false
            });

    },
    created:function(){
        // for(var i=0;i<this.tableData.length;i++)
        // {
        //     if(this.tableData[i]["site1"]>this.tableData[i]["SPECHIGH"]||this.tableData[i]["site1"]<this.tableData[i]["SPECLOW"])
        //     {
        //         this.tableData[i]["color1"]="red";
        //     }
        //     if(this.tableData[i]["site2"]>this.tableData[i]["SPECHIGH"]||this.tableData[i]["site2"]<this.tableData[i]["SPECLOW"])
        //     {
        //         this.tableData[i]["color2"]="red";
        //     }

        // }
    }
    
}
</script>

<style>
  .el-dropdown-link{
      width:100px;
  }
  .red{
      color:red;
  }
  .bg-purple-dark {
    background: #99a9bf;
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