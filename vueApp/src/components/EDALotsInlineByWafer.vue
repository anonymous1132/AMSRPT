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
                    :key="item.ProdSpec_ID"
                    :value="item.ProdSpec_ID"
                  ></el-option>
                </el-select>
                <el-select multiple placeholder="请选择Spec Item" v-model="specItem">
                  <el-option v-for="(item,idx) in allSpecItem" :key="idx" :value="item"></el-option>
                </el-select>
                <el-select multiple placeholder="请选择Meas Item" v-model="measItem">
                  <el-option v-for="(item,idx) in allMeasItem" :key="idx" :value="item"></el-option>
                </el-select>
                <el-select multiple placeholder="请选择LotID" v-model="lotID">
                  <el-option v-for="(item,idx) in allLotId" :key="idx" :value="item"></el-option>
                </el-select>
                <el-select multiple placeholder="请选择WaferID" v-model="waferID">
                  <el-option v-for="item in allWaferID" :key="item" :value="item"></el-option>
                </el-select>
              </div>
            </div>
          </el-col>
          <el-col :lg="4" :offset="2" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000209" user="姜兆涛（0102）"></BaseHeaderCard>
          </el-col>
        </el-row>
      </template>
      <template slot="main">
        <BaseTableContainer
          v-if="manualInput&&showTable"
          :title="manualTitle"
          :tableData="manualTableData"
          fileName="EDA_LotsInlineByWafer.xls"
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
                  <th>Eqp Type</th>
                  <th>Eqp ID</th>
                  <th>Recipe ID</th>
                  <th>Oper No.</th>
                  <th>Spec Item</th>
                  <th>Meas Item.</th>
                  <th>Wafer ID</th>
                  <th v-for="site in mSites" :key="'h'+site" v-text="'site'+site"></th>
                  <th>Wafer Mean</th>
                  <th>LS</th>
                  <th>LC</th>
                  <th>Target</th>
                  <th>UC</th>
                  <th>US</th>
                  <th>Mean Range</th>
                  <th>Range UC</th>
                  <th>Oper Time</th>
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
                  <td v-text="row.EqpType"></td>
                  <td v-text="row.EqpID"></td>
                  <td v-text="row.RecipeID"></td>
                  <td v-text="row.OpeNo"></td>
                  <td v-text="row.SpecItem"></td>
                  <td v-text="row.MeasItem"></td>
                  <td v-text="row.WaferID"></td>
                  <td
                    v-for="site in mSites"
                    :key="'s'+site"
                    v-text="row.SiteValue[site-1]"
                    :class="(row.SiteValue[site-1]<row.LS&&row.LS!==null )|| (row.SiteValue[site-1]>row.US&&row.US!==null)?'eda-red':''"
                  ></td>
                  <td
                    v-text="row.WaferMean"
                    :class="(row.WaferMean<row.LC&&row.LC!==null)||(row.WaferMean>row.UC&&row.UC!==null)?'eda-red':''"
                  ></td>
                  <td v-text="row.LS"></td>
                  <td v-text="row.LC"></td>
                  <td v-text="row.Target"></td>
                  <td v-text="row.UC"></td>
                  <td v-text="row.US"></td>
                  <td v-text="row.MeanRange"></td>
                  <td v-text="row.RangeUC"></td>
                  <td v-text="row.OpeTime"></td>
                </tr>
              </tbody>
            </table>
          </div>
        </BaseTableContainer>
        <BaseTableContainer
          v-if="(!manualInput)&&showTable"
          :title="autoTitle"
          :tableData="filteredAutoTableData"
          fileName="EDA_LotsInlineByWafer.xls"
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
                  <th>Eqp Type</th>
                  <th>Eqp ID</th>
                  <th>Recipe ID</th>
                  <th>Oper No.</th>
                  <th>Spec Item</th>
                  <th>Meas Item.</th>
                  <th>Wafer ID</th>
                  <th v-for="site in aSites" :key="'h'+site" v-text="'site'+site"></th>
                  <th>Wafer Mean</th>
                  <th>LS</th>
                  <th>LC</th>
                  <th>Target</th>
                  <th>UC</th>
                  <th>US</th>
                  <!--<th>Mean Range</th>
                  <th>Range UC</th>-->
                  <th>Oper Time</th>
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
                  <td v-text="row.EqpType"></td>
                  <td v-text="row.EqpID"></td>
                  <td v-text="row.RecipeID"></td>
                  <td v-text="row.OpeNo"></td>
                  <td v-text="row.SpecItem"></td>
                  <td v-text="row.MeasItem"></td>
                  <td v-text="row.WaferID"></td>
                  <td
                    v-for="site in aSites"
                    :key="'s'+site"
                    v-text="row.SiteValue[site-1]"
                    :class="(row.SiteValue[site-1]<row.LS &&row.LS!==null)||(row.SiteValue[site-1]>row.US&&row.US!==null)?'eda-red':''"
                  ></td>
                  <td
                    v-text="row.WaferMean"
                    :class="(row.WaferMean<row.LC&&row.LC!==null)||(row.WaferMean>row.UC&&row.UC!==null)?'eda-red':''"
                  ></td>
                  <td v-text="row.LS"></td>
                  <td v-text="row.LC"></td>
                  <td v-text="row.Target"></td>
                  <td v-text="row.UC"></td>
                  <td v-text="row.US"></td>
                  <!--td v-text="row.MeanRange"></td>
                  <td v-text="row.RangeUC"></td>-->
                  <td v-text="row.OpeTime"></td>
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
  name: "EDALotsInlineByWafer",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      manualInput: true,
      manualPeriod: "",
      manualLotQueryStr: "",
      autoPeriod: "",
      loading: false,
      prodCategory: "",
      prod: "",
      specItem: [],
      measItem: [],
      lotID: [],
      waferID: [],
      manualTableData: [],
      manualTitle: "",
      mSites: [],
      aSites: [],
      autoTableData: [],
      autoTitle: "",
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
      if (!arry.find(f => f.ProdSpec_ID === this.prod)) this.prod = "";
      return arry;
    },
    showTable: function() {
      if (this.manualInput) return this.manualTitle !== "";
      else return this.autoTitle !== "";
    },
    prodFilteredData: function() {
      let ary = this.autoTableData;
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
    measFilteredData: function() {
      let ary = this.specFilteredData;
      if (this.measItem.length > 0) {
        ary = ary.filter(f => this.measItem.indexOf(f.MeasItem) > -1);
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
    allMeasItem: function() {
      let data = this.specFilteredData.map(m => m.MeasItem);
      data.distinct();
      data.sort();
      this.measItem = [];
      return data;
    },
    allLotId: function() {
      let ary = this.measFilteredData.map(m => m.LotID);
      ary.distinct();
      ary.sort();
      this.lotID = [];
      return ary;
    },
    filteredAutoTableData: function() {
      let ary = this.measFilteredData;
      if (this.lotID.length > 0) {
        ary = ary.filter(f => this.lotID.indexOf(f.LotID) > -1);
      }
      if (this.waferID.length > 0) {
        ary = ary.filter(f => this.waferID.indexOf(f.WaferID) > -1);
      }
      if (ary.length > 0) {
        this.aSites = [];
        let len = ary[0].SiteValue.length;
        for (let i = 0; i < len; i++) {
          let arry = ary.map(m => m.SiteValue[i]);
          if (arry.find(f => f !== null)) this.aSites.push(i + 1);
        }
      }
      return ary;
    }
  },
  methods: {
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
        this.mSites = [];
        this.manualTableData.map(m => {
          let ary = m.WaferID.split(".");
          let len = ary.length;
          if (ary[len - 1].length == 2) m.WaferID = "#" + ary[len - 1];
        });
        let len = response.data.RowEntities[0].SiteValue.length;
        for (let i = 0; i < len; i++) {
          let arry = response.data.RowEntities.map(m => m.SiteValue[i]);
          if (arry.find(f => f !== null)) this.mSites.push(i + 1);
        }
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
        this.autoTableData.map(m => {
          let ary = m.WaferID.split(".");
          let len = ary.length;
          if (ary[len - 1].length == 2) m.WaferID = "#" + ary[len - 1];
        });
      });
    },
    queryFun(data, cb) {
      let url = this.URL_PREFIX + "/ReqRpt209/ManualQuery";
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
      this.measItem = [];
      this.lotID = [];
      this.waferID = [];
    }
  },
  created() {
    let url = this.URL_PREFIX + "/ReqRpt209/GetProdList";
    this.loading = true;
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
  flex-flow: row wrap;
  margin-top: 10px;
}
.eda-inline-select-group .el-select {
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
</style>