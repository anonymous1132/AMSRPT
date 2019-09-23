<template>
  <BaseLayout>
    <BaseContainer>
      <template slot="header">
        <el-row type="flex" justify="center">
          <el-col :span="20">
            <el-row type="flex" justify="center" class="scraplist-head">
              <label>From:</label>
              <el-date-picker type="date" placeholder="请选择开始日期" v-model="from"></el-date-picker>
              <label>To:</label>
              <el-date-picker type="date" placeholder="请选择结束日期" v-model="to"></el-date-picker>
              <el-button
                type="primary"
                icon="el-icon-search"
                :loading="loading"
                @click="handleQueryClick"
              ></el-button>
            </el-row>
          </el-col>
          <el-col :span="4">
            <BaseHeaderCard project="RPT000022" user="李冬（0490）"/>
          </el-col>
        </el-row>
      </template>
      <template slot="main">
        <BaseTableContainer
          v-if="showTable"
          :tableData="filteredRowEntities"
          :fileName="fileName"
          :title="tableTitle"
        >
          <div slot="table" slot-scope="scope" class="scraplist-table-div">
            <table class="table table-responsive table-bordered table-hover">
              <thead>
                <tr>
                  <th>No</th>
                  <th>LotID</th>
                  <th>Owner</th>
                  <th>LotType</th>
                  <th>ScrapType</th>
                  <th>ScrapDate</th>
                  <th>EventDate</th>
                  <th>MainPD</th>
                  <th>ModulePD</th>
                  <th>OpeNo</th>
                  <th>Qty</th>
                  <th>EqpType</th>
                  <th>User</th>
                  <th>Code</th>
                  <th>Reason</th>
                  <th>Claim Memo</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item,idx) in scope.datas" :key="idx">
                  <th v-text="idx+1"></th>
                  <th v-text="item.LotID"></th>
                  <th v-text="item.Owner"></th>
                  <th v-text="item.LotType"></th>
                  <th v-text="item.ScrapType"></th>
                  <th v-text="item.ScrapTime"></th>
                  <th v-text="item.EventTime"></th>
                  <th v-text="item.MainPD"></th>
                  <th v-text="item.ModulePD"></th>
                  <th v-text="item.OpeNo"></th>
                  <th v-text="item.Qty"></th>
                  <th v-text="item.EqpType"></th>
                  <th v-text="item.User"></th>
                  <th v-text="item.Code"></th>
                  <th v-text="item.CodeDesc"></th>
                  <th v-text="item.ClaimMemo"></th>
                </tr>
              </tbody>
            </table>
          </div>
          <div slot="left" class="scraplist-filter-div">
            <div class="scraplist-box">
              <label>LotType:</label>
              <el-select v-model="selectedLotType" size="small" placeholder="All" multiple>
                <el-option v-for="(item,idx) in allLotTypes" :key="idx" :label="item" :value="item"></el-option>
              </el-select>
            </div>
            <div class="scraplist-box">
              <label>Module:</label>
              <el-select v-model="selectedModule" size="small" placeholder="All" multiple>
                <el-option v-for="(item,idx) in allModules" :key="idx" :label="item.Description" :value="item.Description"></el-option>
              </el-select>
            </div>
            <div class="scraplist-box">
              <el-button type="primary" size="small" @click="handleClear">重置</el-button>
            </div>
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
  name: "ScrapList",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      loading: false,
      from: null,
      to: null,
      showTable: false,
      tableData: {
        from: null,
        to: null,
        rowEntities: []
      },
      selectedModule: [],
      selectedLotType: [],
      allModules: [],
      allLotTypes: []
    };
  },
  computed: {
    fileName: function() {
      let file = "RPT_ScrapLotList_";
      let from = this.tableData.from.replace(/[- :]/g, "");
      let to = this.tableData.to.replace(/[- :]/g, "");
      return file + from + "_" + to + ".xls";
    },
    tableTitle: function() {
      let title = "Scrap Lot List Report:";
      let from = this.tableData.from;
      let to = this.tableData.to;
      return title + from + "~" + to;
    },
    filteredRowEntities: function() {
      let tableEntities = this.tableData.rowEntities;
      if (this.selectedLotType.length > 0) {
        tableEntities = tableEntities.filter(
          f => this.selectedLotType.indexOf(f.LotType) > -1
        );
      }
      if (this.selectedModule.length > 0) {
        tableEntities = tableEntities.filter(
          f => this.selectedModule.indexOf(f.Module) > -1
        );
      }
      return tableEntities;
    }
  },
  methods: {
    handleQueryClick() {
      if (!(this.from && this.to))
        return this.$message.error("请选择开始跟结束时间");
      this.loading = true;
      let url = this.URL_PREFIX + "/ReqRpt022/GetTableData";
      let data = {
        startTime: this.getDateStr(this.from) + " 00:00:00",
        endTime: this.getDateStr(this.to) + " 23:59:59"
      };
      let _this = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            (_this.tableData.from = data.startTime),
              (_this.tableData.to = data.endTime),
              (_this.tableData.rowEntities = response.data.RowEntities);
            _this.showTable = true;
          } else {
            _this.$message.error("服务端程序异常");
            console.log(response.data.msg);
          }
          _this.loading = false;
        })
        .catch(error => {
          console.log(error);
          _this.$message.error("网络异常");
          _this.loading = false;
        });
    },
    handleClear(){
      this.selectedModule=[]
      this.selectedLotType=[]
    },
    getDateStr(date) {
      let y = date.getFullYear();
      let m = date.getMonth() + 1;
      m = m < 10 ? "0" + m : m;
      let d = date.getDate();
      d = d < 10 ? "0" + d : d;
      return y + "-" + m + "-" + d;
    },
    getAllModules() {
      let url = this.URL_PREFIX + "/Common/GetAllModule";
      let data = { type: 1 };
      let _this = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            _this.allModules = response.data.modules;
          } else {
            console.log(response.data.msg);
          }
        })
        .catch(error => {
          console.log(error);
        });
    },
    getAllLotTypes() {
      let url = this.URL_PREFIX + "/Common/GetAllLotType";
      let _this = this;
      this.$http
        .post(url, {})
        .then(response => {
          if (response.data.success) {
            _this.allLotTypes = response.data.values;
          } else {
            console.log(response.data.msg);
          }
        })
        .catch(error => {
          console.log(error);
        });
    }
  },
  created() {
    this.getAllModules();
    this.getAllLotTypes();
  }
};
</script>

<style>
.scraplist-head {
  align-items: center;
  height: 100%;
}
.scraplist-head label {
  margin-left: 10px;
  padding: 4px;
}
.scraplist-table-div {
  max-height: 600px;
  white-space: nowrap;
  overflow: auto;
}

.scraplist-filter-div {
  width: 80%;
  display: flex;
  flex-flow: row wrap;
  justify-content: end;
  align-items: baseline;
  align-content: space-between;
  height: 40px;
}
.scraplist-filter-div >.scraplist-box{
  margin-left: 10px;
  margin-bottom: 5px;
}
</style>