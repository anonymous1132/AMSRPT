<template>
  <BaseLayout>
    <BaseContainer>
      <template slot="header">
        <el-row type="flex" justify="center">
          <el-col :lg="17" :md="12">
            <div class="eda-manual">
              <el-input placeholder="请输入EqpID，支持逗号隔开，*模糊匹配" v-model="eqpQueryStr"></el-input>
              <div class="eda-date-query">
                <el-date-picker
                  v-model="period"
                  type="daterange"
                  range-separator="~"
                  start-placeholder="开始日期,00:00:00"
                  end-placeholder="结束日期,23:59:59"
                ></el-date-picker>
                <el-button
                  type="primary"
                  icon="el-icon-search"
                  :loading="loading"
                  @click="handleQueryClick"
                ></el-button>
              </div>
            </div>
          </el-col>
          <el-col :lg="4" :offset="2" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000207" user="姜兆涛（0102）"></BaseHeaderCard>
          </el-col>
        </el-row>
      </template>
      <template slot="main">
        <BaseTableContainer
          v-if="showTable"
          :title="tableTitle"
          :tableData="filteredTableData"
          fileName="EDA_ToolRunningHistory.xls"
        >
          <template slot="left">
            <el-select v-model="dept" size="small" placeholder="Department" clearable>
              <el-option v-for="(item,idx) in allDepts" :key="idx" :label="item" :value="item"></el-option>
            </el-select>
            <el-select v-model="eqpType" size="small" placeholder="EqpType" clearable>
              <el-option v-for="(item,idx) in allEqpTypes" :key="idx" :label="item" :value="item"></el-option>
            </el-select>
            <el-select v-model="eqpId" size="small" placeholder="EqpID" multiple>
              <el-option v-for="(item,idx) in allEqpIds" :key="idx" :label="item" :value="item"></el-option>
            </el-select>
          </template>
          <div slot="table" slot-scope="scope" class="eda-table-div">
            <table class="table table-responsive table-bordered table-hover">
              <thead>
                <tr>
                  <th>No.</th>
                  <th>EQP ID</th>
                  <th>Lot ID</th>
                  <th>Foup ID</th>
                  <th>Qty</th>
                  <th>Route ID</th>
                  <th>Oper ID</th>
                  <th>Oper No.</th>
                  <th>Oper Name</th>
                  <th>Recipe ID</th>
                  <th>Oper Start Time</th>
                  <th>Oper CompleteTime</th>
                  <th>Run Hrs</th>
                  <th>User ID</th>
                  <th>User full name</th>
                  <th>User Department</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row,idx) in scope.datas" :key="idx">
                  <td v-text="idx+1"></td>
                  <td v-text="row.EqpID"></td>
                  <td v-text="row.LotID"></td>
                  <td v-text="row.CastID"></td>
                  <td v-text="row.Qty"></td>
                  <td v-text="row.MainPDID"></td>
                  <td v-text="row.PDID"></td>
                  <td v-text="row.OpeNo"></td>
                  <td v-text="row.PDName"></td>
                  <td v-text="row.RecipeID"></td>
                  <td v-text="row.StartTime"></td>
                  <td v-text="row.EndTime"></td>
                  <td v-text="row.RunDur"></td>
                  <td v-text="row.UserID"></td>
                  <td v-text="row.UserFullName"></td>
                  <td v-text="row.Dept"></td>
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
  name: "EDAToolRunningHistory",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      tableTitle: "",
      eqpQueryStr: "",
      period: "",
      loading: false,
      tableData: [],
      dept: "",
      eqpType: "",
      eqpId: []
    };
  },
  computed: {
    showTable: function() {
      return this.tableTitle !== "";
    },
    allDepts: function() {
      let arry = this.tableData.map(m => m.Dept);
      arry.distinct();
      if (!arry.find(f => f === this.dept)) this.dept = "";
      return arry.filter(f => f);
    },
    deptFilteredData: function() {
      if (!this.dept) return this.tableData;
      return this.tableData.filter(f => f.Dept === this.dept);
    },
    allEqpTypes: function() {
      let arry = this.deptFilteredData.map(m => m.EqpType);
      arry.distinct();
      if (!arry.find(f => f === this.eqpType)) this.eqpType = "";
      return arry.filter(f => f);
    },
    eqpTypeFilteredData: function() {
      if (!this.eqpType) return this.deptFilteredData;
      return this.deptFilteredData.filter(f => f.EqpType === this.eqpType);
    },
    allEqpIds: function() {
      let arry = this.eqpTypeFilteredData.map(m => m.EqpID);
      arry.distinct();
      this.eqpId = [];
      return arry.filter(f => f);
    },
    filteredTableData: function() {
      if (this.eqpId.length == 0) return this.eqpTypeFilteredData;
      return this.eqpTypeFilteredData.filter(
        f => this.eqpId.indexOf(f.EqpID) > -1
      );
    }
  },
  methods: {
    handleQueryClick() {
      let eqp = this.eqpQueryStr;
      let period = this.period;
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
      if (eqp == "") eqp = "*";
      let data = {
        eqp,
        startDate,
        endDate
      };
      let url = this.URL_PREFIX + "/ReqRpt207/GetTableData";
      let _this = this;
      this.loading = true;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            _this.tableData = response.data.RowEntities;
            _this.tableTitle =
              "Eqp:" +
              response.data.EqpStr +
              ";From:" +
              response.data.StartDate +
              " 00:00:00~" +
              response.data.EndDate +
              " 23:59:59;";
          } else {
            _this.$message.error("服务端程序发生异常");
            console.log(response.data.msg);
          }
          _this.loading = false;
        })
        .catch(error => {
          _this.$message.error("网络异常");
          console.log(error);
          _this.loading = false;
        });
    }
  },
  created() {
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
.eda-date-query {
  display: flex;
  width: 406px;
}
.eda-manual {
  display: flex;
  flex-flow: column;
}
.eda-manual .el-input {
  width: 406px;
  margin-bottom: 20px;
}
.eda-table-div {
  max-height: 600px;
  white-space: nowrap;
  overflow: auto;
}
</style>