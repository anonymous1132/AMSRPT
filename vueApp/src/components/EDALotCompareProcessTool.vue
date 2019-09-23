<template>
  <BaseLayout>
    <BaseContainer>
      <template slot="header">
        <el-row type="flex" justify="start">
          <el-col :lg="12" :offset="1" :md="12">
            <div class="eda-inline-auto">
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
                <el-select placeholder="请选择Product Category" clearable v-model="prodCategory">
                  <el-option
                    v-for="item in allProdCategory"
                    :key="item"
                    :label="item"
                    :value="item"
                  ></el-option>
                </el-select>
                <el-autocomplete
                  placeholder="请输入Product"
                  v-model="prod"
                  :fetch-suggestions="querySearch"
                ></el-autocomplete>
              <!--  <el-select multiple placeholder="请选择Spec Item" v-model="specItem">
                  <el-option v-for="(item,idx) in allSpecItem" :key="idx" :value="item"></el-option>
                </el-select> -->
                <el-select multiple placeholder="请选择LotID" v-model="lotID">
                  <el-option v-for="(item,idx) in allLotId" :key="idx" :value="item"></el-option>
                </el-select>
                <el-select multiple placeholder="请选择WaferID" v-model="waferID">
                  <el-option v-for="(item,idx) in allWaferID" :key="idx" :value="idx" :label="item"></el-option>
                </el-select>
              </div>
            </div>
          </el-col>
          <el-col :lg="4" :offset="5" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000206" user="姜兆涛（0102）"></BaseHeaderCard>
          </el-col>
        </el-row>
      </template>
      <template slot="main">
        <BaseTableContainer
          v-if="showTable"
          :title="title"
          :tableData="filteredAutoTableData"
          fileName="EDA_LotCompareProcessTool.xls"
        >
          <div slot="table" slot-scope="scope" class="eda-table-div">
            <table class="table table-responsive table-bordered table-hover">
              <thead>
                <tr>
                  <th>No</th>
                  <th>Route ID</th>
                  <th>Oper ID</th>
                  <th>Oper No.</th>
                  <th>Oper Name</th>
                  <th>Lot ID</th>
                  <th>Process EQP</th>
                  <th>Oper Time</th>
                  <th
                    v-for="wafer in viewWaferID"
                    :key="'h'+allWaferID[wafer]"
                    v-text="allWaferID[wafer]"
                  ></th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row,idx) in scope.datas" :key="idx">
                  <td v-text="idx+1"></td>
                  <td v-text="row.RouteID"></td>
                  <td v-text="row.OperID"></td>
                  <td v-text="row.OpeNo"></td>
                  <td v-text="row.OpeName"></td>
                  <td v-text="row.LotID"></td>
                  <td v-text="row.EQP"></td>
                  <td v-text="row.OperTime"></td>
                  <td
                    v-for="wafer in viewWaferID"
                    :key="'s'+allWaferID[wafer]"
                    v-text="row.ChamberArray[wafer]"
                  ></td>
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
  name: "EDALotCompareProcessTool",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      autoPeriod: "",
      loading: false,
      prodCategory: "",
      prod: "",
      specItem: [],
      measItem: [],
      lotID: [],
      waferID: [],
      tableData: [],
      title: "",
      allProdCategory: [
        "Production",
        "Dummy",
        "Process Monitor",
        "Equipment Monitor"
      ],
      allProds: [],
      allWaferID: []
    };
  },
  computed: {
    avaProds: function() {
      if (!this.prodCategory) return this.allProds;
      let arry = this.allProds.filter(
        f => f.Prod_Category_ID === this.prodCategory
      );
      // if (!arry.find(f => f.ProdSpec_ID === this.prod)) this.prod = "";
      return arry;
    },
    prodFilteredData: function() {
      let ary = this.tableData;
      if (this.prod !== null && this.prod !== "") {
        ary = ary.filter(f => f.ProdID === this.prod);
      }
      return ary;
    },
    specFilteredData: function() {
      let ary = this.prodFilteredData;
      if (this.specItem.length > 0) {
        ary = ary.filter(f => this.specItem.indexOf(f.SpecItem) > -1);
      }
      return ary;
    },
    allSpecItem: function() {
      let data = this.prodFilteredData.map(m => m.SpecItem);
      data.distinct();
      data.sort();
      this.specItem = [];
      return data;
    },
    allLotId: function() {
      let ary = this.specFilteredData.map(m => m.LotID);
      ary.distinct();
      ary.sort();
      this.lotID = [];
      return ary;
    },
    filteredAutoTableData: function() {
      let ary = this.specFilteredData;
      if (this.lotID.length > 0) {
        ary = ary.filter(f => this.lotID.indexOf(f.LotID) > -1);
      }
      return ary;
    },
    viewWaferID: function() {
      return this.waferID.length == 0
        ? this.allWaferID.map((m, idx) => idx)
        : this.waferID.sort();
    },
    showTable:function(){
      return this.title !== "";
    }
  },
  methods: {
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
      let data = { from: startDate, to: endDate };
      this.queryFun(data, response => {
        this.title =
          "From:" + response.data.StartDate + " To:" + response.data.EndDate;
        this.tableData = response.data.RowEntities;
      });
    },
    queryFun(data, cb) {
      let url = this.URL_PREFIX + "/ReqRpt212/GetTableData";
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
    handleClear() {
      this.prodCategory = "";
      this.prod = "";
      this.specItem = [];
      this.lotID = [];
      this.waferID = [];
    },
    querySearch(queryString, cb) {
      let avaProds = this.avaProds;
      let results = queryString
        ? avaProds.filter(
            f =>
              f.ProdSpec_ID.toLowerCase().indexOf(queryString.toLowerCase()) ===
              0
          )
        : avaProds;
      results.map(m => (m.value = m.ProdSpec_ID));
      // 调用 callback 返回建议列表的数据
      cb(results);
    }
  },
  created() {
    let url = this.URL_PREFIX + "/ReqRpt209/GetProdList";
    this.loading = true;
    //this.allWaferID = ["#*"];
    for (let i = 1; i < 26; i++) {
      this.allWaferID.push(i < 10 ? "#0" + i : "#" + i);
    }
    let _this = this;
    this.$http
      .post(url)
      .then(response => {
        if (response.data.success) {
          _this.allProds = response.data.prodList;
        } else {
          _this.$message.error("网页初始化失败");
          console.log(response.data.msg);
        }
        _this.loading = false;
      })
      .catch(error => {
        _this.$message.error("网页初始化失败");
        console.log(error);
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
  flex-flow: row nowrap;
  margin-top: 10px;
}
.eda-inline-select-group .el-select,.el-autocomplete {
  min-width: 200px;
}

.eda-table-div {
  max-height: 600px;
  white-space: nowrap;
  overflow: auto;
}

.eda-red {
  color: red;
}

.el-autocomplete{
  width: 217px;
}
</style>