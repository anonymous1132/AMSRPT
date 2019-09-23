<template>
  <BaseLayout>
    <BaseContainer>
      <el-row slot="header" type="flex" justify="start">
        <el-col :span="20">
          <el-row v-if="showCtx!='detail'" type="flex" justify="center" class="spc-head">
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
          <el-row v-else type="flex" justify="center" class="spc-head">
            <el-button
              icon="el-icon-back"
              type="primary"
              size="small"
              @click="handleReturnClick"
            >Return</el-button>
          </el-row>
        </el-col>
        <el-col :span="4">
          <BaseHeaderCard project="RPT000064" user="林建成（0438）"/>
        </el-col>
      </el-row>
      <div slot="main">
        <BaseTableContainer
          v-if="showCtx=='main'"
          :fileName="fileName"
          :title="tableTitle"
          :tableData="filteredTableData"
        >
          <table
            class="table table-responsive table-bordered table-hover"
            slot="table"
            slot-scope="scope"
          >
            <thead>
              <tr>
               <!-- <th>Group Name</th>-->
                <th>Module</th>
                <th>EQ ID</th>
                <th>Chart Title</th>
                <th>Chart Type</th>
                <th>USL</th>
                <th>UCL</th>
                <th>Target</th>
                <th>LSL</th>
                <th>LCL</th>
                <th>Mean</th>
                <th>Sigma</th>
                <th>Ca</th>
                <th>Cp</th>
                <th>Cpk</th>
                <th>OOCRate</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(entity,idx) in scope.datas.tableEntities" :key="idx">
               <!-- <td v-text="entity.Gname"></td>-->
                <td v-text="entity.Department"></td>
                <td v-text="entity.EqpID"></td>
                <td>
                  <el-button
                    type="text"
                    :loading="scope.datas.loading"
                    @click="handleChartTitleClick(entity,scope.datas.from,scope.datas.to)"
                  >{{entity.ChartTitle}}</el-button>
                </td>
                <td v-text="entity.ChartType"></td>
                <td v-text="entity.Usl"></td>
                <td v-text="entity.Ucl"></td>
                <td v-text="entity.Target"></td>
                <td v-text="entity.Usl!=''&& entity.Lsl==''?'NA':entity.Lsl"></td>
                <td v-text="entity.Ucl!=''&&entity.Lcl==''?'NA': entity.Lcl"></td>
                <td v-text="entity.Mean"></td>
                <td v-text="entity.Sigma"></td>
                <td v-text="entity.Ca"></td>
                <td v-text="entity.Cp"></td>
                <td v-text="entity.Cpk" :style="{color:entity.Cpk<1.33?'red':'blue'}"></td>
                <td v-text="entity.OOSRate"></td>
              </tr>
            </tbody>
          </table>
          <div slot="left" class="spc-filter-div">
            <div class="spc-box">
              <label>Group Name:</label>
               <el-autocomplete
                class="inline-input"
                v-model="filteredProd"
                :fetch-suggestions="querySearchProd"
                placeholder="All"
                size="small"
              ></el-autocomplete>
            </div>
            <div class="spc-box">
              <label>Module:</label>
              <el-select v-model="filteredModule" size="small" placeholder="All" clearable>
                <el-option v-for="(item,idx) in allModules" :key="idx" :label="item" :value="item"></el-option>
              </el-select>
            </div>
            <div class="spc-box">
              <label>EQ ID:</label>
              <el-autocomplete
                class="inline-input"
                v-model="filteredEqpID"
                :fetch-suggestions="querySearchEqp"
                placeholder="All"
                size="small"
              ></el-autocomplete>
            </div>
            <div class="spc-box">
              <label>Chart Title:</label>
              <el-autocomplete
                class="inline-input"
                v-model="filteredChartTitle"
                :fetch-suggestions="querySearchChartTitle"
                placeholder="All"
                size="small"
              ></el-autocomplete>
            </div>
            <div class="spc-box">
              <label>Chart Type:</label>
              <el-select v-model="filteredChartType" size="small" placeholder="All" clearable>
                <el-option v-for="(item,idx) in allTypes" :key="idx" :label="item" :value="item"></el-option>
              </el-select>
            </div>
            <div class="spc-box">
              <label>Cpk:</label>
              <el-select v-model="filteredCpk" size="small" placeholder="All" clearable>
                <el-option label="<1.33" :value="1"></el-option>
                <el-option label=">=1.33" :value="2"></el-option>
              </el-select>
            </div>
            <div class="spc-box">
              <el-button type="primary" size="small"  @click="handleClear">重置</el-button>
            </div>
          </div>
        </BaseTableContainer>
        <div v-if="showCtx=='detail'" class="spc-detail">
          <BaseTableContainer style="width:85%" :useExcel="false" :tableData="detailData">
            <table
              class="table table-responsive table-bordered table-hover"
              slot="table"
              slot-scope="scope"
            >
              <tbody>
                <tr>
                  <td>Chart Title</td>
                  <td colspan="3">{{ scope.datas.SetModel.ChartTitle }}</td>
                </tr>
                <tr>
                  <td colspan="2">Process</td>
                  <td colspan="2">Statistics</td>
                </tr>
                <tr>
                  <td>USL</td>
                  <td>{{ scope.datas.ProcessModel.Usl }}</td>
                  <td>USL</td>
                  <td>{{ scope.datas.StaticModel.Usl }}</td>
                </tr>
                <tr>
                  <td>UCL</td>
                  <td>{{ scope.datas.ProcessModel.Ucl }}</td>
                  <td>UCL</td>
                  <td>{{ scope.datas.StaticModel.Ucl }}</td>
                </tr>
                <tr>
                  <td>Target</td>
                  <td>{{ scope.datas.ProcessModel.Target }}</td>
                  <td>Target</td>
                  <td>{{ scope.datas.StaticModel.Target }}</td>
                </tr>
                <tr>
                  <td>LCL</td>
                  <td>{{ scope.datas.ProcessModel.Lcl }}</td>
                  <td>LCL</td>
                  <td>{{ scope.datas.StaticModel.Lcl }}</td>
                </tr>
                <tr>
                  <td>LSL</td>
                  <td>{{ scope.datas.ProcessModel.Lsl }}</td>
                  <td>LSL</td>
                  <td>{{ scope.datas.StaticModel.Lsl }}</td>
                </tr>
                <tr>
                  <td>Mean</td>
                  <td>{{ scope.datas.ProcessModel.Mean }}</td>
                  <td>Mean</td>
                  <td>{{ scope.datas.StaticModel.Mean }}</td>
                </tr>
                <tr>
                  <td>Sigma</td>
                  <td>{{ scope.datas.ProcessModel.Sigma }}</td>
                  <td>Sigma</td>
                  <td>{{ scope.datas.StaticModel.Sigma }}</td>
                </tr>
                <tr>
                  <td>Ca</td>
                  <td>{{ scope.datas.ProcessModel.Ca }}</td>
                  <td>Ca</td>
                  <td>{{ scope.datas.StaticModel.Ca }}</td>
                </tr>
                <tr>
                  <td>Cp(false)</td>
                  <td>{{ scope.datas.ProcessModel.Cp }}</td>
                  <td>Cp</td>
                  <td>{{ scope.datas.StaticModel.Cp }}</td>
                </tr>
                <tr>
                  <td>Cpk verify(false)</td>
                  <td>{{ scope.datas.ProcessModel.Cpkv }}</td>
                  <td>Cpk verify</td>
                  <td>{{ scope.datas.StaticModel.Cpkv }}</td>
                </tr>
                <tr>
                  <td>Cpk(false)</td>
                  <td>{{ scope.datas.ProcessModel.Cpk }}</td>
                  <td>Cpk</td>
                  <td>{{ scope.datas.StaticModel.Cpk }}</td>
                </tr>
              </tbody>
            </table>
          </BaseTableContainer>
          <BaseTableContainer
            :useExcel="false"
            title="Values"
            style="width:40%;margin-right:5%;"
            :tableData="detailData"
          >
            <table
              class="table table-responsive table-bordered table-hover"
              slot="table"
              slot-scope="scope"
            >
              <thead>
                <tr>
                  <th>Date_Time</th>
                  <th>Chart_Point_Value</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item,idx) in scope.datas.ValueList" :key="idx">
                  <td v-text="scope.datas.TimeList[idx]"></td>
                  <td v-text="item"></td>
                </tr>
              </tbody>
            </table>
          </BaseTableContainer>
          <BaseTableContainer
            :useExcel="false"
            title="Spc Client Settings"
            style="width:40%"
            :tableData="detailData"
          >
            <table
              class="table table-responsive table-bordered table-hover"
              slot="table"
              slot-scope="scope"
            >
              <thead>
                <tr>
                  <th>Item</th>
                  <th>Value</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>Chart Type</td>
                  <td v-text="scope.datas.SetModel.ChartType"></td>
                </tr>
                <tr>
                  <td>DC ID</td>
                  <td v-text="scope.datas.SetModel.DcID"></td>
                </tr>
                <tr>
                  <td>DC Spec ID</td>
                  <td v-text="scope.datas.SetModel.DcSpecID"></td>
                </tr>
                <tr>
                  <td>Item Name</td>
                  <td v-text="scope.datas.SetModel.ItemName"></td>
                </tr>
                <tr>
                  <td>Chart Name</td>
                  <td v-text="scope.datas.SetModel.ChartName"></td>
                </tr>
                <tr>
                  <td>Chart Title</td>
                  <td v-text="scope.datas.SetModel.ChartTitle"></td>
                </tr>
                <tr>
                  <td>Sample_Size</td>
                  <td v-text="scope.datas.SetModel.SampleSize"></td>
                </tr>
                <tr>
                  <td>Max Point</td>
                  <td v-text="scope.datas.SetModel.MaxPoint"></td>
                </tr>
                <tr>
                  <td>From</td>
                  <td v-text="scope.datas.SetModel.From"></td>
                </tr>
                <tr>
                  <td>To</td>
                  <td v-text="scope.datas.SetModel.To"></td>
                </tr>
                <tr>
                  <td>UPL</td>
                  <td v-text="scope.datas.SetModel.Upl"></td>
                </tr>
                <tr>
                  <td>USL</td>
                  <td v-text="scope.datas.SetModel.Usl"></td>
                </tr>
                <tr>
                  <td>UCL</td>
                  <td v-text="scope.datas.SetModel.Ucl"></td>
                </tr>
                <tr>
                  <td>UWL</td>
                  <td v-text="scope.datas.SetModel.Uwl"></td>
                </tr>
                <tr>
                  <td>Target</td>
                  <td v-text="scope.datas.SetModel.Target"></td>
                </tr>
                <tr>
                  <td>Mean</td>
                  <td v-text="scope.datas.SetModel.Mean"></td>
                </tr>
                <tr>
                  <td>LWL</td>
                  <td v-text="scope.datas.SetModel.Lwl"></td>
                </tr>
                <tr>
                  <td>LCL</td>
                  <td v-text="scope.datas.SetModel.Lcl"></td>
                </tr>
                <tr>
                  <td>LSL</td>
                  <td v-text="scope.datas.SetModel.Lsl"></td>
                </tr>
                <tr>
                  <td>LPL</td>
                  <td v-text="scope.datas.SetModel.Lpl"></td>
                </tr>
              </tbody>
            </table>
          </BaseTableContainer>
        </div>
      </div>
    </BaseContainer>
  </BaseLayout>
</template>

<script>
import BaseLayout from "../components/BaseLayout";
import BaseContainer from "../components/BaseContainer";
import BaseHeaderCard from "../components/BaseHeaderCard";
import BaseTableContainer from "../components/BaseTableContainer";
export default {
  name: "SpcReportPage",
  components: { BaseLayout, BaseContainer, BaseHeaderCard, BaseTableContainer },
  data() {
    return {
      from: null,
      to: null,
      loading: false,
      tableData: { from: null, to: null, loading: false, tableEntities: [] },
      showCtx: "",
      detailData: {},
      filteredProd:"",
      filteredModule: "",
      filteredEqpID: "",
      filteredChartTitle: "",
      filteredChartType: "",
      filteredCpk: ""
    };
  },
  computed: {
    fileName: function() {
      let file = "RPT_SPC_";
      let from = this.tableData.from.replace(/[- :]/g, "");
      let to = this.tableData.to.replace(/[- :]/g, "");
      return file + from + "_" + to + ".xls";
    },
    tableTitle: function() {
      let title = "SPC Report:";
      let from = this.tableData.from;
      let to = this.tableData.to;
      return title + from + "~" + to;
    },
    filteredTableData: function() {
      let from = this.tableData.from;
      let to = this.tableData.to;
      let loading = this.tableData.loading;
      let tableEntities = this.tableData.tableEntities;
      if(this.filteredProd) tableEntities = tableEntities.filter(
          f => f.Gname == this.filteredProd
        );
      if (this.filteredModule)
        tableEntities = tableEntities.filter(
          f => f.Department == this.filteredModule
        );
      if (this.filteredEqpID)
        tableEntities = tableEntities.filter(
          f => f.EqpID.indexOf(this.filteredEqpID) === 0
        );
      if (this.filteredChartTitle)
        tableEntities = tableEntities.filter(
          f => f.ChartTitle.indexOf(this.filteredChartTitle) === 0
        );
      if (this.filteredChartType)
        tableEntities = tableEntities.filter(
          f => f.ChartType == this.filteredChartType
        );
      if (this.filteredCpk == 1)
        tableEntities = tableEntities.filter(f => f.Cpk < 1.33);
      if (this.filteredCpk == 2)
        tableEntities = tableEntities.filter(f => !(f.Cpk < 1.33));
      return {
        from: from,
        to: to,
        loading: loading,
        tableEntities: tableEntities
      };
    },
    allModules: function() {
      let arry = this.tableData.tableEntities.map(m => m.Department);
      arry.distinct();
      return arry;
    },
    allTypes: function() {
      let arry = this.tableData.tableEntities.map(m => m.ChartType);
      arry.distinct();
      return arry;
    }
  },
  methods: {
    handleQueryClick() {
      if (!(this.from && this.to))
        return this.$message.error("请选择开始跟结束时间");
      this.loading = true;
      let url = this.URL_PREFIX + "/ReqRpt064/GetMainTable";
      //let from=JSON.parse(JSON.stringify(this.from))
      //let to=JSON.parse(JSON.stringify(this.to))
      let data = {
        startDate: this.getDateStr(this.from),
        endDate: this.getDateStr(this.to)
      };
      let _this = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            _this.tableData.from = data.startDate;
            _this.tableData.to = data.endDate;
            _this.tableData.tableEntities = response.data.tableEntities;
            _this.showCtx = "main";
          } else {
            _this.$message.error("服务器程序发生异常");
            console.log(response.data.msg);
          }
          _this.loading = false;
        })
        .catch(error => {
          _this.$message.error("网络异常");
          console.log(error);
          _this.loading = false;
        });
    },
    handleChartTitleClick(entity, from, to) {
      let url = this.URL_PREFIX + "/ReqRpt064/GetDetail";
      let data = {
        gno: entity.Gno,
        cno: entity.Cno,
        ctype: entity.Ctype,
        startDate: from,
        endDate: to
      };
      this.tableData.loading = true;
      let _this = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            _this.detailData = response.data.detail;
            _this.showCtx = "detail";
          } else {
            _this.$message.error("服务器程序异常");
            console.log(response.data.msg);
          }
          _this.tableData.loading = false;
        })
        .catch(error => {
          _this.$message.error("网络异常");
          console.log(error);
          _this.tableData.loading = false;
        });
    },
    handleReturnClick() {
      this.showCtx = "main";
    },
    handleClear(){
      this.filteredCpk=""
      this.filteredModule=""
      this.filteredEqpID=""
      this.filteredChartTitle=""
      this.filteredChartType=""
    },
    querySearchEqp(queryString, cb) {
      var eqp = this.tableData.tableEntities.map(m => m.EqpID);
      eqp.distinct();
      eqp = eqp.map(m => {
        return { value: m };
      });
      var results = queryString
        ? eqp.filter(this.createFilter(queryString))
        : eqp;
      // 调用 callback 返回建议列表的数据
      cb(results);
    },
    querySearchChartTitle(queryString, cb) {
      var eqp = this.tableData.tableEntities.map(m => m.ChartTitle);
      eqp.distinct();
      eqp = eqp.map(m => {
        return { value: m };
      });
      var results = queryString
        ? eqp.filter(this.createFilter(queryString))
        : eqp;
      // 调用 callback 返回建议列表的数据
      cb(results);
    },
      querySearchProd(queryString, cb) {
          var eqp = this.tableData.tableEntities.map(m => m.Gname);
      eqp.distinct();
      eqp= eqp.filter(f=>f!=null)
      eqp = eqp.map(m => {
        return { value: m };
      });
      var results = queryString
        ? eqp.filter(this.createFilter(queryString))
        : eqp;
      // 调用 callback 返回建议列表的数据
      cb(results);
    },
    getDateStr(date) {
      let y = date.getFullYear();
      let m = date.getMonth() + 1;
      m = m < 10 ? "0" + m : m;
      let d = date.getDate();
      d = d < 10 ? "0" + d : d;
      return y + "-" + m + "-" + d;
    },
    getCurDate() {
      let date = new Date();
      return this.getDateStr(date);
    },
    createFilter(queryString) {
      return temp => {
        return (
          temp.value.toLowerCase().indexOf(queryString.toLowerCase()) === 0
        );
      };
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
.spc-head {
  align-items: center;
  height: 100%;
}
.spc-head label {
  margin-left: 10px;
  padding: 4px;
}
.spc-detail {
  display: flex;
  flex-flow: row wrap;
  align-content: center;
}
.spc-filter-div {
  width: 80%;
  display: flex;
  flex-flow: row wrap;
  justify-content: end;
  align-items: baseline;
  align-content: space-between;
  height: 80px;
}
.spc-filter-div >.spc-box{
  margin-left: 10px;
}
</style>
