<template>
  <BaseLayout>
    <BaseContainer>
      <template slot="header">
        <el-row type="flex" justify="center">
          <el-col :lg="7" :md="12">
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
          <el-col :lg="10" :md="12">
            <div v-if="manualInput" class="eda-inline-manual">
              <el-input placeholder="请输入LotID，支持逗号隔开，*模糊匹配" v-model="manualLotQueryStr"></el-input>
              <div class="eda-inline-date-query">
                <el-date-picker
                  v-model="manualPeriod"
                  type="daterange"
                  range-separator="~"
                  start-placeholder="开始日期,00:00:00"
                  end-placeholder="结束日期,23:59:59"
                ></el-date-picker>
                <el-button
                  type="primary"
                  icon="el-icon-search"
                  :loading="loading"
                  @click="handleManualQueryClick"
                ></el-button>
              </div>
            </div>
            <div v-else class="eda-inline-auto">
              <div class="eda-inline-date-query">
                <el-date-picker
                  v-model="autoPeriod"
                  type="daterange"
                  range-separator="~"
                  start-placeholder="开始日期,00:00:00"
                  end-placeholder="结束日期,23:59:59"
                ></el-date-picker>
                <el-button
                  type="primary"
                  icon="el-icon-search"
                  :loading="loading"
                  @click="handleAutoQueryClick"
                ></el-button>
                <el-button type="primary" @click="handleClear">重置</el-button>
              </div>
              <div class="eda-inline-select-group">
                <el-autocomplete
                  placeholder="请输入Product"
                  v-model="prod"
                  :fetch-suggestions="querySearch"
                ></el-autocomplete>
                <el-select multiple placeholder="请选择Oper Name" v-model="operName">
                  <el-option v-for="(item,idx) in allOperName" :key="idx" :value="item"></el-option>
                </el-select>
                <el-select multiple placeholder="请选择Position" v-model="position">
                  <el-option v-for="(item,idx) in allPosition" :key="idx" :value="item"></el-option>
                </el-select>
                <el-select multiple placeholder="请选择LotID" v-model="lotID">
                  <el-option v-for="(item,idx) in allLotId" :key="idx" :value="item"></el-option>
                </el-select>
              </div>
            </div>
          </el-col>
          <el-col :lg="4" :offset="2" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000213" user="姜兆涛（0102）"></BaseHeaderCard>
          </el-col>
        </el-row>
      </template>
      <template slot="main">
        <BaseTableContainer
          v-if="manualInput&&showTable"
          :title="manualTitle"
          :tableData="manualTableData"
          fileName="EDA_FuranceBatchInfo.xls"
        >
          <div slot="table" slot-scope="scope" class="eda-table-div">
            <table class="table table-responsive table-bordered table-hover">
              <thead>
                <tr>
                  <th>No</th>
                  <th>Lot ID</th>
                  <th>Foup ID</th>
                  <th>Qty</th>
                  <th>Route ID</th>
                  <th>Oper ID</th>
                  <th>Oper No.</th>
                  <th>Oper Name</th>
                  <th>Eqp Type</th>
                  <th>Eqp ID</th>
                  <th>Recipe ID</th>
                  <th v-for="wafer in allWaferID" :key="'h'+wafer" v-text="wafer"></th>
                  <th>Furance Position</th>
                  <th>Oper Start Time</th>
                  <th>Oper Complete Time</th>
                  <th>Run Hrs</th>
                  <th>User ID</th>
                  <th>User Full Name</th>
                  <th>User Department</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row,idx) in scope.datas" :key="idx">
                  <td v-text="idx+1"></td>
                  <td v-text="row.LotID"></td>
                  <td v-text="row.FoupID"></td>
                  <td v-text="row.Qty"></td>
                  <td v-text="row.RouteID"></td>
                  <td v-text="row.OperID"></td>
                  <td v-text="row.OperNo"></td>
                  <td v-text="row.OperName"></td>
                  <td v-text="row.EqpType"></td>
                  <td v-text="row.EqpID"></td>
                  <td v-text="row.RecipeID"></td>
                  <td
                    v-for="(wafer,idx) in allWaferID"
                    :key="'s'+wafer"
                    v-text="row.WaferValue[idx]"
                  ></td>
                  <!-- <td 
                    v-for="(pos,idx) in allWaferID"
                    :key="'c'+pos"
                    v-text="row.Position[idx]"
                    ></td> -->
                  <td v-text="row.Position"></td>
                  <td v-text="row.OpeStartTime"></td>
                  <td v-text="row.OpeCompleteTime"></td>
                  <td v-text="row.RunHrs"></td>
                  <td v-text="row.UserID"></td>
                  <td v-text="row.UserFullName"></td>
                  <td v-text="row.UserDept"></td>
                </tr>
              </tbody>
            </table>
          </div>
        </BaseTableContainer>
        <BaseTableContainer
          v-if="(!manualInput)&&showTable"
          :title="autoTitle"
          :tableData="filteredAutoTableData"
          fileName="EDA_FuranceBatchInfo.xls"
        >
          <div slot="table" slot-scope="scope" class="eda-table-div">
            <table class="table table-responsive table-bordered table-hover">
              <thead>
                <tr>
                  <th>No</th>
                  <th>Lot ID</th>
                  <th>Foup ID</th>
                  <th>Qty</th>
                  <th>Route ID</th>
                  <th>Oper ID</th>
                  <th>Oper No.</th>
                  <th>Oper Name</th>
                  <th>Eqp Type</th>
                  <th>Eqp ID</th>
                  <th>Recipe ID</th>
                  <th
                    v-for="wafer in viewWaferID"
                    :key="'h'+allWaferID[wafer]"
                    v-text="allWaferID[wafer]"
                  ></th>
                  <th>Furance Position</th>
                  <th>Oper Start Time</th>
                  <th>Oper Complete Time</th>
                  <th>Run Hrs</th>
                  <th>User ID</th>
                  <th>User Full Name</th>
                  <th>User Department</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row,idx) in scope.datas" :key="idx">
                  <td v-text="idx+1"></td>
                  <td v-text="row.LotID"></td>
                  <td v-text="row.FoupID"></td>
                  <td v-text="row.Qty"></td>
                  <td v-text="row.RouteID"></td>
                  <td v-text="row.OperID"></td>
                  <td v-text="row.OperNo"></td>
                  <td v-text="row.OperName"></td>
                  <td v-text="row.EqpType"></td>
                  <td v-text="row.EqpID"></td>
                  <td v-text="row.RecipeID"></td>
                  <td
                    v-for="wafer in viewWaferID"
                    :key="'s'+allWaferID[wafer]"
                    v-text="row.WaferValue[wafer]"
                  ></td>
                  <!-- <td 
                    v-for="pos in allPosition"
                    :key="allPosition[pos]"
                    v-text="row.Position[idx]" 
                    ></td> -->
                  <td v-text="row.Position"></td>
                  <td v-text="row.OpeStartTime"></td>
                  <td v-text="row.OpeCompleteTime"></td>
                  <td v-text="row.RunHrs"></td>
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
  name: "EDAFuranceBatchInfo",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      manualInput: true,
      manualPeriod: "",
      manualLotQueryStr: "",
      autoPeriod: "",
      loading: false,
      prod: "",
      operName: [],
      position: [],
      //allPosition: [],
      lotID: [],
      manualTableData: [],
      manualTitle: "",
      autoTableData: [],
      autoTitle: "",
      allWaferID: [],
      waferID: [],
      
    };
  },
  computed: {
    allProds: function() {
      let data = this.autoTableData.map(m => m.Prod);
      if (this.autoTableData.length === 0) return [];
      data.distinct();
      data.sort(function(a, b) {
        return a.length - b.length;
      });
      return data;
      // return this.autoTableData.map(m => m.Prod);
    },
    prodFilteredData: function() {
      let ary = this.autoTableData;
      if (this.prod !== null && this.prod !== "") {
        ary = ary.filter(f => f.Prod === this.prod);
      }
      return ary;
    },
    operNameFilteredData: function() {
      let ary = this.prodFilteredData;
      if (this.operName.length > 0) {
        ary = ary.filter(f => this.operName.indexOf(f.OperName) > -1);
      }
      return ary;
    },
    positionFilteredData: function() {
      let ary = this.operNameFilteredData;
      if (this.position.length > 0) {
        ary = ary.filter(f => this.position.indexOf(f.Position) > -1);
      }
      return ary;
    },
    allOperName: function() {
      let data = this.prodFilteredData.map(m => m.OperName);
      data.distinct();
      data.sort();
      this.operName = [];
      return data;
    },
    allPosition:function(){
      let data = this.operNameFilteredData.map(m => m.Position);
      data.distinct();
      data.sort();
      this.position = [];
      return data;
    },
    // pp:function (Position) {
    //   var temp=[];
    //   for(var i = 0; i < Position.length; i++){
    //     if(temp.indexOf(Position[i]) == -1){
    //         temp.push(Position[i]);
    //     }
    // }
    // return temp; 
    // },
    allLotId: function() {
      let data = this.positionFilteredData.map(m => m.LotID);
      data.distinct();
      data.sort();
      this.lotID = [];
      return data;
    },
    filteredAutoTableData: function() {
      let ary = this.positionFilteredData;
      if (this.lotID.length > 0) {
        ary = ary.filter(f => this.lotID.indexOf(f.LotID) > -1);
      }
      return ary;
    },
    showTable: function() {
      if (this.manualInput) return this.manualTitle !== "";
      else return this.autoTitle !== "";
    },
    viewWaferID: function() {
      return this.waferID.length == 0
        ? this.allWaferID.map((m, idx) => idx)
        : this.waferID.sort();
    },
   
  },
  methods: {
    querySearch(queryString, cb) {
      let avaProds = [];
      this.allProds.forEach(Element => {
        avaProds.push({ value: Element });
      });
      let results = queryString
        ? avaProds.filter(
            f => f.value.toLowerCase().indexOf(queryString.toLowerCase()) === 0
          )
        : avaProds;
      //results.map(m => (m.value = m.Prod));
      // 调用 callback 返回建议列表的数据
      cb(results);
    },
    queryFun(data, cb) {
      let url = this.URL_PREFIX + "/ReqRpt213/ManualQuery";
      let _this = this;
      this.loading = true;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            cb(response);
          } else {
            if (response.data.msg == "没有数据")
              return _this.$message.error("没有查询到符合条件的数据");
            console.log(response.data.msg);
            _this.$message.error("服务端程序异常");
          }
          _this.loading = false;
        })
        .catch(error => {
          console.log(error);
          _this.$message.error("网络异常");
          _this.loading = false;
        });
    },
    handleManualQueryClick() {
      if (!this.manualLotQueryStr)
        return this.$message.error("请输入需要查询的LotID");
      let period = this.manualPeriod;
      let startDate;
      let endDate;
      if (period) {
        startDate = getDateStr(period[0]);
        endDate = getDateStr(period[1]);
      } else {
        endDate = getCurDate();
        let date = new Date();
        date.setDate(date.getDate() - 10);
        startDate = getDateStr(date);
      }
      let data = { lot: this.manualLotQueryStr, startDate, endDate };
      //let _this = this;
      this.queryFun(data, response => {
        this.manualTitle =
          "Lot:" +
          response.data.LotStr +
          ";From:" +
          response.data.StartDate +
          " To:" +
          response.data.EndDate;
        this.manualTableData = response.data.RowEntities;
      });
    },
    handleAutoQueryClick() {
      let period = this.autoPeriod;
      let startDate;
      let endDate;
      if (period) {
        startDate = getDateStr(period[0]);
        endDate = getDateStr(period[1]);
      } else {
        endDate = getCurDate();
        let date = new Date();
        date.setDate(date.getDate() - 10);
        startDate = getDateStr(date);
      }
      let data = { lot: "*", startDate, endDate };
      this.queryFun(data, response => {
        this.autoTitle =
          "From:" + response.data.StartDate + " To:" + response.data.EndDate;
        this.autoTableData = response.data.RowEntities;
      });
      console.log(this.autoTableData)
     // console.log(this.allPosition)
    },
    handleClear() {
      this.lotID = [];
      this.position = [];
      this.operName = [];
      this.prod = "";
    }
  },
  created() {
    for (let i = 1; i < 26; i++) {
      this.allWaferID.push(i < 10 ? "#0" + i : "#" + i);
    }
    // for (let j = 1; j <4; j++) {
    //   this.allPosition.push("");
    // }
    
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
.eda-inline-manual {
  display: flex;
  flex-flow: column;
}
.eda-inline-auto {
  display: flex;
  flex-flow: column;
}
.eda-inline-date-query {
  display: flex;
  width: 406px;
}
.eda-inline-manual .el-input {
  width: 406px;
  margin-bottom: 20px;
}
.eda-inline-select-group {
  display: flex;
  flex-flow: row wrap;
  margin-top: 10px;
}

.eda-inline-select-group .el-select,
.el-autocomplete {
  min-width: 220px;
}
.eda-table-div {
  max-height: 600px;
  white-space: nowrap;
  overflow: auto;
}

.eda-red {
  color: red;
}
</style>