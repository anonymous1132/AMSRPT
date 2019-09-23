<template>
  <BaseLayout>
    <BaseContainer>
      <el-row slot="header" type="flex" justify="start">
        <el-col :span="20">
          <div class="lhr-head-div">
            <div class="lhr-box">
              <label>From:</label>
              <el-date-picker
                type="date"
                placeholder="请选择开始日期,00:00:00"
                v-model="from"
                :picker-options="pickerOptions"
              ></el-date-picker>
              <label>To:</label>
              <el-date-picker
                type="date"
                placeholder="请选择结束日期,23:59:59"
                v-model="to"
                :picker-options="pickerOptions"
              ></el-date-picker>
              <el-button
                type="primary"
                icon="el-icon-search"
                :loading="loading"
                @click="handleQueryClick"
              ></el-button>
            </div>
            <template v-if="showCtx">
              <div class="lhr-box">
                <label>Modules:</label>
                <el-select v-model="selDepts" multiple placeholder="Default All">
                  <el-option
                    v-for="item in depts"
                    :key="item.Code_ID"
                    :label="item.Description"
                    :value="item.Code_ID"
                  ></el-option>
                </el-select>
              </div>
              <div class="lhr-box">
                <label>ResonCodes:</label>
                <el-select v-model="selReCodes" filterable multiple placeholder="Default All">
                  <el-option v-for="(item,idx) in reCodes" :key="idx" :label="item" :value="item"></el-option>
                  <el-option label="System" value="System"></el-option>
                </el-select>
              </div>
              <div class="lhr-box">
                <el-switch v-model="showProd" active-text="Product" inactive-text="Non-Product"></el-switch>
              </div>
              <!--
              <div class="lhr-box">
                <el-switch v-model="showDetail" active-text="Detail" inactive-text="Summary"></el-switch>
              </div>-->
              <div class="lhr-box">
                <el-button type="primary" @click="handleClearClick">重置</el-button>
              </div>
            </template>
          </div>
        </el-col>
        <el-col :span="4">
          <BaseHeaderCard project="RPT000016" user="李冬（0490）"/>
        </el-col>
      </el-row>
      <div slot="main" class="lhr-main">
        <BaseTableContainer
          v-if="showCtx"
          :title="tableTitle"
          :fileName="tableFileName"
          :tableData="showTableData"
        >
          <div slot="table" slot-scope="scope" class="lhr-table-div">
            <table class="table table-responsive table-bordered table-hover" v-if="showDetail">
              <thead>
                <tr>
                  <th>Item</th>
                  <th>Prod ID</th>
                  <th>LotID</th>
                  <th>LotType</th>
                  <th>Qty</th>
                  <th>CurQty</th>
                  <th>Ope_NO</th>
                  <th>Ope Name</th>
                  <th>Ope.Dept.</th>
                  <th>Ope.Eqp Type</th>
                  <th>Hold User ID</th>
                  <th>Hold User Name</th>
                  <th>Hold User Dept.</th>
                  <th>Hold Time</th>
                  <th>Release Time</th>
                  <th>Duration(H)</th>
                  <th>Hold Code</th>
                  <th>Hold Comment</th>
                  <th>Re User ID</th>
                  <th>Re User Name</th>
                  <th>Re User Dept.</th>
                  <th>Release Comment</th>
                  <th>Hold Code Dept</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(item,idx) in scope.datas.lotHoldList" :key="idx">
                  <td v-text="idx+1"></td>
                  <td v-text="item.Prod"></td>
                  <td v-text="item.LotID"></td>
                  <td v-text="item.LotType"></td>
                  <td v-text="item.Qty"></td>
                  <td v-text="item.CurQty"></td>
                  <td v-text="item.OpeNO"></td>
                  <td v-text="item.PDName"></td>
                  <td v-text="item.HoldPDModule"></td>
                  <td v-text="item.EqpType"></td>
                  <td v-text="item.HoldUserID"></td>
                  <td v-text="item.HoldUserName"></td>
                  <td v-text="item.HoldUserDept"></td>
                  <td v-text="item.HoldTime"></td>
                  <td v-text="item.ReleaseTime"></td>
                  <td v-text="item.Duration"></td>
                  <td v-text="item.ReasonCode"></td>
                  <td v-text="item.HoldComment"></td>
                  <td v-text="item.ReleaseUserID"></td>
                  <td v-text="item.ReleaseUserName"></td>
                  <td v-text="item.ReleaseUserDept"></td>
                  <td v-text="item.ReleaseComment"></td>
                  <td v-text="item.ReCodeDept"></td>
                </tr>
              </tbody>
            </table>
            <table class="table table-responsive table-bordered table-hover" v-else>
              <thead>
                <tr>
                  <th>Item</th>
                  <th>Department</th>
                  <th>HoldCode</th>
                  <th>Qty</th>
                  <th v-for="(item,idx) in scope.datas.items" :key="idx" v-text="item"></th>
                </tr>
              </thead>
            </table>
          </div>
        </BaseTableContainer>
      </div>
    </BaseContainer>
  </BaseLayout>
</template>
</BaseTableContainer>
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
  name: "LotHoldReviewPage",
  components: {
    BaseLayout,
    BaseContainer,
    BaseHeaderCard,
    BaseTableContainer
  },
  data() {
    return {
      loading: false,
      from: null,
      to: null,
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() > Date.now();
        }
      },
      showCtx: "",
      selDepts: [],
      depts: [],
      selReCodes: [],
      allReCodes: [],
      showDetail: true,
      showProd: true,
      tableData: {
        from: null,
        to: null,
        items: [],
        lotHoldList: [],
        nonProdSummaryEntities: [],
        prodSummaryEntities: []
      }
    };
  },
  computed: {
    reCodes: function() {
      this.selReCodes = [];
      let depts = this.selDepts;
      if (depts.length === 0) return this.allReCodes;
      let arry = depts.map(m => m);
      return this.allReCodes.filter(f => arry.indexOf(f.substring(0, 1)) > -1);
    },
    tableFileName: function() {
      let head = "RPT_HoldLotReview_";
      let from = this.tableData.from.substring(0, 10).replace(/[-]/g, "");
      let to = this.tableData.to.substring(0, 10).replace(/[-]/g, "");
      let prod = this.showProd ? "Product" : "NonProduct";
      let detail = this.showDetail ? "Detail" : "Summary";
      return head + prod + "_" + detail + "_" + from + "~" + to + ".xls";
    },
    tableTitle: function() {
      let head = "Lot Hold Review:";
      let from = this.tableData.from;
      let to = this.tableData.to;
      let prod = this.showProd ? "Product" : "NonProduct";
      let detail = this.showDetail ? "Detail" : "Summary";
      return head + from + "~" + to + ";" + prod + ";" + detail;
    },
    showTableData: function() {
      let items = this.tableData.items;
      let lotHoldList = [];
      let summaryEntities = [];
      if (this.showProd) {
        lotHoldList = this.tableData.lotHoldList.filter(
          f => f.LotType === "Production"
        );
        summaryEntities = this.tableData.prodSummaryEntities;
      } else {
        lotHoldList = this.tableData.lotHoldList.filter(
          f => f.LotType !== "Production"
        );
        summaryEntities = this.tableData.nonProdSummaryEntities;
      }

      if (this.selDepts.length > 0) {
        lotHoldList = lotHoldList.filter(
          f => this.selDepts.indexOf(f.FinalDept) > -1
        );
      }

      if (this.selReCodes.length > 0) {
        let list1 = lotHoldList.filter(
          f => this.selReCodes.indexOf(f.ReasonCode) > -1
        );
        if (this.selReCodes.indexOf("System") > -1) {
          let list2 = lotHoldList.filter(f => f.ReasonDept == "system");
          lotHoldList = list1.concat(list2);
        } else {
          lotHoldList = list1;
        }
      }
      lotHoldList.sort((a, b) => {
        if (a.HoldTime < b.HoldTime) return 1;
        if (a.HoldTime == b.HoldTime) return 0;
        return -1;
      });
      let data = {
        items: items,
        lotHoldList: lotHoldList,
        summaryEntities: summaryEntities
      };
      return data;
    }
  },
  watch: {
    showDetail(val) {
      this.showCtx = val ? "detail" : "summary";
    }
  },
  methods: {
    handleQueryClick() {
      if (!(this.from && this.to))
        return this.$message.error("请选择开始跟结束时间");
      this.loading = true;
      let url = this.URL_PREFIX + "/ReqRpt016/GetLotDetail";
      let data = {
        startTime: this.getDateStr(this.from) + " 00:00:00",
        endTime: this.getDateStr(this.to) + " 23:59:59"
      };
      let _this = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            _this.tableData = {
              from: data.startTime,
              to: data.endTime,
              items: response.data.Items,
              lotHoldList: response.data.LotHoldList
              //nonProdSummaryEntities: response.data.NonProdSummaryEntities,
              //prodSummaryEntities: response.data.ProdSummaryEntities
            };
            _this.tableData.lotHoldList.map(m => {
              let temp = _this.depts.find(f => f.Code_ID == m.ReasonDept);
              m.ReCodeDept = temp ? temp.Description : m.ReasonDept;
              let temp2 = _this.depts.find(f => f.Code_ID == m.HoldPDDept);
              m.HoldPDModule = temp2 ? temp2.Description : m.HoldPDDept;
            });
            _this.showCtx = _this.showDetail ? "detail" : "summary";
          } else {
            console.log(response.data.msg);
            _this.$message.error("服务端运行异常");
          }
          _this.loading = false;
        })
        .catch(error => {
          console.log(error);
          _this.$message.error("网络异常");
          _this.loading = false;
        });
    },
    handleClearClick() {
      this.selDepts = [];
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
    getCurTime() {
      let date = new Date();
      let y = date.getFullYear();
      let m = date.getMonth() + 1;
      m = m < 10 ? "0" + m : m;
      let d = date.getDate();
      d = d < 10 ? "0" + d : d;
      let h = date.getHours();
      h = h < 10 ? "0" + h : h;
      let min = date.getMinutes();
      min = min < 10 ? "0" + min : min;
      let s = date.getSeconds();
      s = s < 10 ? "0" + s : s;
      return y + "-" + m + "-" + d + " " + h + ":" + min + ":" + s;
    }
  },
  created() {
    let url = this.URL_PREFIX + "/ReqRpt016/GetModuleAndReasonList";
    let _this = this;
    this.$http
      .post(url)
      .then(response => {
        if (response.data.success) {
          _this.depts = response.data.ModuleList;
          _this.allReCodes = response.data.HoldReasonCodeList;
        } else {
          console.log(response.data.msg);
          _this.$message.error("服务端程序出现异常");
        }
      })
      .catch(error => {
        console.log(error);
        _this.$message.error("服务器网络异常，初始化失败");
      });
  }
};
</script>

<style>
.lhr-head-div {
  display: flex;
  flex-flow: row wrap;
  justify-content: end;
  align-items: baseline;
  align-content: space-between;
  height: 100%;
  padding-top: 10px;
}
.lhr-head-div > .lhr-box {
  margin-left: 30px;
}
.lhr-table-div {
  max-height: 600px;
  white-space: nowrap;
  overflow: auto;
}
</style>
