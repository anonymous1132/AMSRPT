<template>
  <BaseLayout>
    <BaseContainer>
      <template slot="header">
        <el-row type="flex" justify="center">
          <el-col :lg="5" :md="12">
            <div>
              <el-switch
                v-model="manualInput"
                active-text="Manual"
                inactive-text="Auto"
                active-color="#13ce66"
                inactive-color="#ff4949"
              ></el-switch>
            </div>
          </el-col>
          <el-col :lg="12" :md="12">
            <div v-if="manualInput" class="eda-inline-date-query">
              <el-input placeholder="请输入LotID，支持逗号隔开，*模糊匹配" v-model="manualLotQueryStr"></el-input>
              <el-button
                type="primary"
                icon="el-icon-search"
                :loading="loading"
                @click="handleManualQueryClick"
              ></el-button>
            </div>
            <div v-else class="eda-head-auto">
              <div class="eda-inline-select-group">
                <el-select placeholder="请选择Product Category" clearable v-model="prodCategory">
                  <el-option
                    v-for="item in allProdCategory"
                    :key="item"
                    :label="item"
                    :value="item"
                  ></el-option>
                </el-select>
                <el-select placeholder="请选择Product" clearable v-model="prod">
                  <el-option
                    v-for="item in avaProds"
                    :key="item"
                    :value="item"
                  ></el-option>
                </el-select>
                <el-select multiple placeholder="请选择LotID" v-model="lotID">
                  <el-option v-for="(item,idx) in allLotId" :key="idx" :value="item"></el-option>
                </el-select>
                <el-button
                  type="primary"
                  icon="el-icon-search"
                  :loading="loading"
                  @click="handleAutoQueryClick"
                ></el-button>
              </div>
            </div>
          </el-col>
          <el-col :lg="4" :offset="2" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000211" user="姜兆涛（0102）"></BaseHeaderCard>
          </el-col>
        </el-row>
      </template>
      <template slot="main">
        <BaseTableContainer :tableData="tableData" :fileName="'EDA_ParentChildSplitRecord.xls'">
          <div slot="table" slot-scope="scope" class="eda-table-div">
            <table class="table table-responsive table-bordered table-hover">
              <thead>
                <tr>
                  <th>No.</th>
                  <th>Lot ID</th>
                  <th>Foup ID</th>
                  <th>Qty</th>
                  <th>Route ID</th>
                  <th>Oper ID</th>
                  <th>Oper No</th>
                  <th>Oper Name</th>
                  <th>Oper Category</th>
                  <th v-for="i in 25" :key="i" v-text="'#'+i"></th>
                  <th>Oper Time</th>
                  <th>Claim Memo</th>
                  <th>User ID</th>
                  <th>User Full Name</th>
                  <th>User Department</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row,idx) in scope.datas" :key="'r'+idx">
                  <td v-text="idx+1"></td>
                  <td v-text="row.LotID"></td>
                  <td v-text="row.FoupID"></td>
                  <td v-text="row.Qty"></td>
                  <td v-text="row.RouteID"></td>
                  <td v-text="row.OperID"></td>
                  <td v-text="row.OperNo"></td>
                  <td v-text="row.OperName"></td>
                  <td v-text="row.OperCategory"></td>
                  <td v-for="i in 25" :key="'c'+i" v-text="row.WaferList[i-1]?i:''"></td>
                  <td v-text="row.OperTime"></td>
                  <td v-text="row.ClaimMemo"></td>
                  <td v-text="row.UserID"></td>
                  <td v-text="row.UserFullName"></td>
                  <td v-text="row.UserDept"></td>
                </tr>
              </tbody>
            </table>
          </div>
        </BaseTableContainer>
      </template>
    </BaseContainer>
  </BaseLayout>
</template>

<script>
import BaseLayout from "../components/BaseLayout";
import BaseContainer from "../components/BaseContainer";
import BaseHeaderCard from "../components/BaseHeaderCard";
import BaseTableContainer from "../components/BaseTableContainer";
export default {
  name: "EDAParentChildSplitRecord",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      manualInput: true,
      manualLotQueryStr: "",
      prodCategory: "",
      prod: "",
      lotID: [],
      prodLotMap: [],
      loading: false,
      tableData:[]
    };
  },
  computed: {
    allProdCategory: function() {
      let arry = this.prodLotMap.map(m => m.Prod_Category_ID);
      arry.distinct();
      return arry;
    },
    prodCategoryFilteredData: function() {
      if (this.prodCategory == "") {
        return this.prodLotMap;
      } else {
        return this.prodLotMap.filter(
          f => f.Prod_Category_ID === this.prodCategory
        );
      }
    },
    avaProds: function() {
      let arry = this.prodCategoryFilteredData.map(m => m.ProdSpec_ID);
      arry.distinct();
      this.prod=''
      return arry;
    },
    prodFilteredData:function(){
      if(this.prod==="")return this.prodCategoryFilteredData
      return this.prodCategoryFilteredData.filter(f=>f.ProdSpec_ID==this.prod)
    },
    allLotId:function(){
      let arry=this.prodFilteredData.map(m=>m.Lot_ID)
      arry.distinct();
      this.lotID=[]
      return arry;
    }
  },
  methods: {
    handleManualQueryClick(){
      if(this.manualLotQueryStr){
        this.handleQueryClick(this.manualLotQueryStr)
      }else{
        this.$message.error("请输入LotID")
      }
    },
    handleAutoQueryClick(){
      if(this.lotID.length>0){
        this.handleQueryClick(this.lotID.join(","))
      }else{
        this.$message.error("请选择LotID")
      }
    },
    handleQueryClick(lot) {
      let url = this.URL_PREFIX + "/ReqRpt211/GetTableData";
      let data = { lot };
      this.loading = true;
      let _this = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            _this.tableData=response.data.tableData
          } else {
            console.log(response.data.msg);
            _this.$message.error("服务端程序异常");
          }
          _this.loading = false;
        })
        .catch(error => {
          console.log(error);
          _this.$message.error("网络故障");
          _this.loading = false;
        });
    }
  },
  created() {
    let url = this.URL_PREFIX + "/ReqRpt211/GetLotProdMapping";
    this.loading = true;
    let _this = this;
    this.$http
      .post(url)
      .then(response => {
        if (response.data.success) {
          _this.prodLotMap = response.data.prodLotMap;
        } else {
          console.log(response.data.msg);
          _this.$message.error("服务端程序异常");
        }
        _this.loading = false;
      })
      .catch(error => {
        console.log(error);
        _this.$message.error("网络故障");
        _this.loading = false;
      });
    Array.prototype.distinct = function() {
      var ret = [];
      for (var i = 0; i < this.length; i++) {
        for (var j = i + 1; j < this.length; ) {
          if (this[i] === this[j]) {
            ret.push(this.splice(j, 1)[0]);
          } else {
            j++;
          }
        }
      }
      return ret;
    };
  }
};
</script>

<style>
.eda-inline-date-query {
  display: flex;
  width: 406px;
}
.eda-table-div {
  max-height: 600px;
  white-space: nowrap;
  overflow: auto;
}
</style>