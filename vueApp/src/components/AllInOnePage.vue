<template>
  <BaseLayout>
    <BaseContainer>
      <div slot="header">
        <el-row type="flex" justify="center">
          <el-col :lg="5" :md="12">
            <div class="aio-nowrap" v-show="showCtx=='main'">
              <el-switch
                v-model="useCur"
                active-text="Current"
                inactive-text="History"
                active-color="#13ce66"
                inactive-color="#ff4949"
              ></el-switch>
              <el-date-picker
                v-model="selHistDate"
                type="date"
                placeholder="选择日期"
                v-show="!useCur"
                @change="handleDatePickerChg"
                :loading="loading"
                :picker-options="pickerOptions"
              ></el-date-picker>
            </div>
          </el-col>
          <el-col :lg="12" :md="12">
            <div class="aio-nowrap">
              <el-col>
                <el-button
                  type="primary"
                  :loading="loading"
                  @click="handleViewTgtClick"
                >{{ wBtnCtx }}</el-button>
                <template v-if="keyTrue">
                  <el-button
                    type="primary"
                    :loading="loading"
                    @click="handleSetTgtClick"
                  >{{ mBtnCtx }}</el-button>
                  <el-button type="primary" :loading="loading" @click="handleLogOutClick">Log Out</el-button>
                  <el-button
                    type="primary"
                    :loading="loading"
                    @click="handleChgKeyClick"
                  >{{kBtnCtx}}</el-button>
                </template>
                <template v-else>
                  <el-input type="password" v-model="key" placeholder="请输入口令"></el-input>
                  <el-button type="primary" :loading="loading" @click="handleLogInClick">Log In</el-button>
                </template>
              </el-col>
              <el-col v-if="false">
                <el-button
                  type="primary"
                  :loading="loading"
                  @click="handleStopTimingClick"
                >{{sBtnCtx}} Timing</el-button>
                <el-button type="primary" :loading="loading" @click="handleReTimingClick">ReTiming</el-button>
              </el-col>
            </div>
          </el-col>
          <el-col :lg="4" :offset="2" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000002">
              <div>{{'Update Period:'+ period+'min'}}</div>
              <div>{{'Time Remaining:'+remSec+'sec' }}</div>
              <div>{{'Last Update Time:'+updateTime }}</div>
            </BaseHeaderCard>
          </el-col>
        </el-row>
      </div>
      <div slot="main">
        <template v-if="showCtx=='main'">
          <BaseHtml2Image  :fileName="fileName"   v-if="!loading">
          <BaseTableContainer
            :tableData="showTableDatas"
            :useExcel="false"
          >
            <template slot="table" slot-scope="scope" >
              <table class="table table-responsive table-bordered table-hover">
                <thead>
                  <tr>
                    <th rowspan="2" class="right-borded">Module</th>
                    <th colspan="3" class="right-borded">WIP</th>
                    <th colspan="3" class="right-borded">Hold</th>
                    <th colspan="4" class="right-borded">Yesterday Move</th>
                    <th colspan="3">Today Move</th>
                  </tr>
                  <tr>
                    <th>Lots</th>
                    <th>Wafer</th>
                    <th class="right-borded">Dev%</th>
                    <th>Lots</th>
                    <th>>24Hrs</th>
                    <th class="right-borded">Rate</th>
                    <th>Target</th>
                    <th>Act.</th>
                    <th>A/T(%)</th>
                    <th class="right-borded">Turn Rate</th>
                    <th>Target</th>
                    <th>Act.</th>
                    <th>A/T(%)</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(entity,idx) in scope.datas.moduleEntities" :key="idx">
                    <td
                      class="right-borded"
                      :class="idx==0?'top-borded':''"
                      v-text="entity.Department"
                    ></td>
                    <td v-text="entity.WipLot" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.WipWafer" :class="idx==0?'top-borded':''"></td>
                    <td class="right-borded" v-text="entity.WipDev" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.HoldLot" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.HoldLotOverTime" :class="idx==0?'top-borded':''"></td>
                    <td
                      class="right-borded"
                      v-text="entity.HoldRate"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td v-text="entity.YstdMoveTarget" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.YstdMoveActual" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.YstdMovePercentage" :class="idx==0?'top-borded':''"></td>
                    <td
                      class="right-borded"
                      v-text="entity.YstdMoveTurnRate"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td v-text="entity.TdMoveTarget" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.TdMoveActual" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.TdMovePercentage" :class="idx==0?'top-borded':''"></td>
                  </tr>
                </tbody>
                <tbody>
                  <tr class="aio-total">
                    <td class="right-borded">Module Total</td>
                    <td v-text="scope.datas.moduleTotal.WipLot"></td>
                    <td v-text="scope.datas.moduleTotal.WipWafer"></td>
                    <td class="right-borded" v-text="scope.datas.moduleTotal.WipDev"></td>
                    <td v-text="scope.datas.moduleTotal.HoldLot"></td>
                    <td v-text="scope.datas.moduleTotal.HoldLotOverTime"></td>
                    <td class="right-borded" v-text="scope.datas.moduleTotal.HoldRate"></td>
                    <td v-text="scope.datas.moduleTotal.YstdMoveTarget"></td>
                    <td v-text="scope.datas.moduleTotal.YstdMoveActual"></td>
                    <td v-text="scope.datas.moduleTotal.YstdMovePercentage"></td>
                    <td class="right-borded" v-text="scope.datas.moduleTotal.YstdMoveTurnRate"></td>
                    <td v-text="scope.datas.moduleTotal.TdMoveTarget"></td>
                    <td v-text="scope.datas.moduleTotal.TdMoveActual"></td>
                    <td v-text="scope.datas.moduleTotal.TdMovePercentage"></td>
                  </tr>
                </tbody>
                <tbody>
                  <tr v-for="(entity,idx) in scope.datas.testEntities" :key="idx">
                    <td
                      class="right-borded"
                      v-text="entity.Department"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td v-text="entity.WipLot" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.WipWafer" :class="idx==0?'top-borded':''"></td>
                    <td class="right-borded" v-text="entity.WipDev" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.HoldLot" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.HoldLotOverTime" :class="idx==0?'top-borded':''"></td>
                    <td
                      class="right-borded"
                      v-text="entity.HoldRate"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td v-text="entity.YstdMoveTarget" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.YstdMoveActual" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.YstdMovePercentage" :class="idx==0?'top-borded':''"></td>
                    <td
                      class="right-borded"
                      v-text="entity.YstdMoveTurnRate"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td v-text="entity.TdMoveTarget" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.TdMoveActual" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.TdMovePercentage" :class="idx==0?'top-borded':''"></td>
                  </tr>
                </tbody>
                <tbody>
                  <tr class="aio-total">
                    <td class="right-borded">Test Total</td>
                    <td v-text="scope.datas.testTotal.WipLot"></td>
                    <td v-text="scope.datas.testTotal.WipWafer"></td>
                    <td class="right-borded" v-text="scope.datas.testTotal.WipDev"></td>
                    <td v-text="scope.datas.testTotal.HoldLot"></td>
                    <td v-text="scope.datas.testTotal.HoldLotOverTime"></td>
                    <td class="right-borded" v-text="scope.datas.testTotal.HoldRate"></td>
                    <td v-text="scope.datas.testTotal.YstdMoveTarget"></td>
                    <td v-text="scope.datas.testTotal.YstdMoveActual"></td>
                    <td v-text="scope.datas.testTotal.YstdMovePercentage"></td>
                    <td class="right-borded" v-text="scope.datas.testTotal.YstdMoveTurnRate"></td>
                    <td v-text="scope.datas.testTotal.TdMoveTarget"></td>
                    <td v-text="scope.datas.testTotal.TdMoveActual"></td>
                    <td v-text="scope.datas.testTotal.TdMovePercentage"></td>
                  </tr>
                </tbody>
                <tbody>
                  <tr v-for="(entity,idx) in scope.datas.bankEntities" :key="idx">
                    <td
                      class="right-borded"
                      v-text="entity.Department"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td v-text="entity.WipLot" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.WipWafer" :class="idx==0?'top-borded':''"></td>
                    <td class="right-borded" v-text="entity.WipDev" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.HoldLot" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.HoldLotOverTime" :class="idx==0?'top-borded':''"></td>
                    <td
                      class="right-borded"
                      v-text="entity.HoldRate"
                      :class="idx==0?'top-borded':''"
                    ></td>
                    <td :class="idx==0?'top-borded':''"></td>
                    <td :class="idx==0?'top-borded':''"></td>
                    <td :class="idx==0?'top-borded':''"></td>
                    <td class="right-borded" :class="idx==0?'top-borded':''"></td>
                    <td :class="idx==0?'top-borded':''"></td>
                    <td :class="idx==0?'top-borded':''"></td>
                    <td :class="idx==0?'top-borded':''"></td>
                  </tr>
                </tbody>
                <tfoot>
                  <tr class="aio-total">
                    <td class="right-borded">Bank Total</td>
                    <td v-text="scope.datas.bankTotal.WipLot"></td>
                    <td v-text="scope.datas.bankTotal.WipWafer"></td>
                    <td class="right-borded" v-text="scope.datas.bankTotal.WipDev"></td>
                    <td v-text="scope.datas.bankTotal.HoldLot"></td>
                    <td v-text="scope.datas.bankTotal.HoldLotOverTime"></td>
                    <td class="right-borded" v-text="scope.datas.bankTotal.HoldRate"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td class="right-borded"></td>
                    <td></td>
                    <td></td>
                    <td></td>
                  </tr>
                  <tr class="aio-total">
                    <td class="right-borded top-borded">Fab Total</td>
                    <td v-text="scope.datas.fabTotal.WipLot" class="top-borded"></td>
                    <td v-text="scope.datas.fabTotal.WipWafer" class="top-borded"></td>
                    <td class="right-borded top-borded" v-text="scope.datas.fabTotal.WipDev"></td>
                    <td v-text="scope.datas.fabTotal.HoldLot" class="top-borded"></td>
                    <td v-text="scope.datas.fabTotal.HoldLotOverTime" class="top-borded"></td>
                    <td class="right-borded top-borded" v-text="scope.datas.fabTotal.HoldRate"></td>
                    <td v-text="scope.datas.fabTotal.YstdMoveTarget" class="top-borded"></td>
                    <td v-text="scope.datas.fabTotal.YstdMoveActual" class="top-borded"></td>
                    <td v-text="scope.datas.fabTotal.YstdMovePercentage" class="top-borded"></td>
                    <td
                      class="right-borded top-borded"
                      v-text="scope.datas.fabTotal.YstdMoveTurnRate"
                    ></td>
                    <td v-text="scope.datas.fabTotal.TdMoveTarget" class="top-borded"></td>
                    <td v-text="scope.datas.fabTotal.TdMoveActual" class="top-borded"></td>
                    <td v-text="scope.datas.fabTotal.TdMovePercentage" class="top-borded"></td>
                  </tr>
                </tfoot>
              </table>
              <table class="table table-responsive table-bordered table-hover">
                <thead>
                  <tr>
                    <th class="right-borded" rowspan="2">Product</th>
                    <th class="right-borded" colspan="3">OutSourcing</th>
                    <th colspan="3">WF Out</th>
                  </tr>
                  <tr>
                    <th>Acc.Target</th>
                    <th>Acc.Actual</th>
                    <th class="right-borded">Acc.Gap</th>
                    <th>Target</th>
                    <th>Actual</th>
                    <th>Yield</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(entity,idx) in scope.datas.waferOutEntities" :key="idx">
                    <td v-text="entity.Product" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.OutSourceAccTarget" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.OutSourceAccActual" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.OutSourceGap" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.WFOutAccTarget" :class="idx==0?'top-borded':''"></td>
                    <td v-text="entity.WFOutAccActual" :class="idx==0?'top-borded':''"></td>
                    <td
                      v-text="(entity.WFOutYield*100).toFixed(2)+'%'"
                      :class="idx==0?'top-borded':''"
                    ></td>
                  </tr>
                </tbody>
                <tfoot>
                  <tr class="aio-total">
                    <td class="top-borded">Total</td>
                    <td
                      class="top-borded"
                      v-text="scope.datas.waferOutTotalEntity.OutSourceAccTarget"
                    ></td>
                    <td
                      class="top-borded"
                      v-text="scope.datas.waferOutTotalEntity.OutSourceAccActual"
                    ></td>
                    <td class="top-borded" v-text="scope.datas.waferOutTotalEntity.OutSourceGap"></td>
                    <td class="top-borded" v-text="scope.datas.waferOutTotalEntity.WFOutAccTarget"></td>
                    <td class="top-borded" v-text="scope.datas.waferOutTotalEntity.WFOutAccActual"></td>
                    <td
                      class="top-borded"
                      v-text="(scope.datas.waferOutTotalEntity.WFOutYield*100).toFixed(2)+'%'"
                    ></td>
                  </tr>
                </tfoot>
              </table>
            </template>
          </BaseTableContainer>
          </BaseHtml2Image>
        </template>
        <template v-if="showCtx=='wafer'">
          <BaseTableContainer
            :useExcel="true"
            :fileName="dialogWFOut.fileName"
            :tableData="dialogWFOut.datas"
            title="View WFOut Target"
          >
            <table
              class="table table-responsive table-bordered table-hover"
              slot="table"
              slot-scope="scope"
              v-loading="dialogWFOut.loading"
            >
              <thead>
                <tr>
                  <th>ProductID</th>
                  <th>Category</th>
                  <th v-for="(item,index) in scope.datas.items" :key="index" v-text="item"></th>
                </tr>
              </thead>
              <tbody>
                <template v-for="(wip,index) in dialogWFOut.datas.wipData">
                  <tr :key="'wip'+index">
                    <td rowspan="2">{{ wip.ProductID }}</td>
                    <td>WIP</td>
                    <td v-for="(data,idx) in wip.Plans" :key="idx" v-text="data.Target"></td>
                  </tr>
                  <tr :key="'ship'+index">
                    <td>Finished</td>
                    <td
                      v-for="(data,idx) in dialogWFOut.datas.shipData[index].Plans"
                      :key="idx"
                      v-text="data.Target"
                    ></td>
                  </tr>
                </template>
              </tbody>
            </table>
          </BaseTableContainer>
        </template>
        <template v-if="showCtx=='move'">
          <div
            class="tinymce"
            v-loading="dialogMoveTgt.loading"
            element-loading-text="数据交互中，请稍后……"
            element-loading-spinner="el-icon-loading"
            element-loading-background="rgba(0, 0, 0, 0.8)"
          >
            <h4>Set Move Target</h4>

            <p>请下载模板文件，设定完成后复制粘贴至以下文本框</p>
            <BaseTableContainer
              excelBtnLabel="Demo File"
              :excelStyle="dialogMoveTgt.excelStyle"
              fileName="RPT_SetMoveTarget_Demo.xls"
              :tableData="dialogMoveTgt.targetData"
            >
              <el-date-picker
                slot="left"
                type="month"
                size="small"
                placeholder="请选择月份"
                v-model="dialogMoveTgt.selMonth"
                @change="handleMoveMonthChange"
              ></el-date-picker>
              <table
                v-if="dialogMoveTgt.selMonth"
                slot="table"
                slot-scope="scope"
                class="table table-responsive table-bordered table-hover"
              >
                <thead>
                  <tr>
                    <td>{{scope.datas.month}}</td>
                    <td v-for="i in scope.datas.days" :key="i" v-text="i"></td>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="tgt in scope.datas.targetList" :key="tgt.DeptCode">
                    <td v-text="tgt.Department"></td>
                    <td v-for="(val,idx) in tgt.TargetList" :key="tgt.DeptCode+idx" v-text="val"></td>
                  </tr>
                </tbody>
              </table>
            </BaseTableContainer>
            <el-row type="flex" justify="end">
              <el-button
                type="primary"
                size="mini"
                :disabled="!tinymceHtml"
                v-on:click="handleSubmitClick"
              >Submit</el-button>
            </el-row>
            <editor id="tinymce" v-model="tinymceHtml" :init="init"></editor>
            <div ref="targetDiv" v-html="tinymceHtml" class="aio-hidden"></div>
          </div>
        </template>
        <template v-if="showCtx=='key'">
          <BaseChangeKeyView :project="project" @submited="handleChangeKeySubmited"></BaseChangeKeyView>
        </template>
      </div>
    </BaseContainer>
  </BaseLayout>
</template>

<script>
import BaseLayout from "../components/BaseLayout";
import BaseContainer from "../components/BaseContainer";
import BaseHeaderCard from "../components/BaseHeaderCard";
import BaseTableContainer from "../components/BaseTableContainer";
import tinymce from "tinymce/tinymce";
import "tinymce/themes/silver/theme";
import Editor from "@tinymce/tinymce-vue";
import BaseChangeKeyView from "../components/BaseChangeKeyView";
import BaseHtml2Image from "../components/BaseHtml2Image";

export default {
  name: "allInOnePage",
  components: {
    BaseLayout,
    BaseContainer,
    BaseHeaderCard,
    BaseTableContainer,
    Editor,
    BaseChangeKeyView,
    BaseHtml2Image
  },
  data() {
    return {
      useCur: true,
      selHistDate: "",
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() > Date.now();
        }
      },
      loading: true,
      key: "",
      project: "ReqRpt00002",
      keyTrue: false,
      curTableDatas: {},
      histTableDatas: {},
      period: 5,
      remSec: 300,
      lastUpdateTime: "",
      timer: null,
      sBtnCtx: "Start",
      showCtx: "main",
      dialogWFOut: {
        datas: {},
        fileName: "RPT_WaferOutTarget.xls",
        loading: true
      },
      tinymceHtml: "",
      init: {
        language_url: "/static/tinymce/zh_CN.js",
        language: "zh_CN",
        skin_url: "/static/tinymce/skins/ui/oxide",
        height: 300,
        toolbar: " undo redo",
        menubar: false,
        branding: false
      },
      dialogMoveTgt: {
        excelStyle:
          '<style type="text/css">table td {border: 1px solid #000000;width: 100px;}</style>',
        selMonth: "",
        loading: false,
        deptList: [
          "Photo",
          "Diffusion",
          "CVD",
          "PVD",
          "ETCH",
          "WET",
          "CMP",
          "DCM",
          "PIE",
          "Production",
          "Device(WAT)",
          "QRA"
        ],
        targetData: {
          month: "",
          days: 0,
          targetList: []
        }
      }
    };
  },
  computed: {
    showTableDatas: function() {
      if (this.useCur) return this.curTableDatas;
      else return this.histTableDatas;
    },
    updateTime: function() {
      let arry = this.lastUpdateTime.split(" ");
      if (arry.length == 2) return arry[1];
      else return "";
    },
    fileName: function() {
      if (!this.selHistDate && !this.useCur) return undefined;
      let file = "RPT_AllInOne_";
      let date = this.useCur
        ? this.lastUpdateTime.replace(/[- :]/g, "")
        : this.getDateStr(new Date(this.selHistDate)).replace(/[- :]/g, "");
      return file + date;
    },
    mBtnCtx: function() {
      return this.showCtx == "move" ? "Return" : "Set Move Target";
    },
    wBtnCtx: function() {
      return this.showCtx == "wafer" ? "Return" : "View WFOut Target";
    },
    kBtnCtx: function() {
      return this.showCtx == "key" ? "Return" : "Change Key";
    }
  },
  watch: {
    remSec(newValue, oldValue) {
      if (newValue === 0) {
        clearInterval(this.timer);
        this.updateCurTableData();
        this.remSec = this.period * 60;
        this.lastUpdateTime = this.getCurTime();
        this.setTimer();
      }
    }
  },
  methods: {
    handleViewTgtClick() {
      if (!this.useCur && !this.selHistDate)
        return this.$message.error("查询历史记录必须选择一个日期");
      if (this.showCtx != "wafer") {
        this.showCtx = "wafer";
        this.getWFOutTarget();
      } else {
        this.showCtx = "main";
      }
    },
    handleSetTgtClick() {
      if (this.showCtx != "move") {
        if (!this.dialogMoveTgt.selMonth) {
          this.dialogMoveTgt.selMonth = new Date();
          this.handleMoveMonthChange(this.dialogMoveTgt.selMonth);
        }
        this.showCtx = "move";
      } else {
        this.showCtx = "main";
      }
    },
    handleLogInClick() {
      let url = this.URL_PREFIX + "/Common/CheckKey";
      let data = {
        project: this.project,
        key: this.key
      };
      this.loading = true;
      let aio = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            aio.$message.success("验证成功");
            aio.keyTrue=true
          } else {
            aio.$message.error("验证失败");
            aio.key=""
          }
          aio.loading = false;
        })
        .catch(error => {
          console.log(error);
          aio.$message.error("网络故障");
          aio.loading = false;
        });
    },
    handleLogOutClick() {
      this.keyTrue = false;
      this.key = "";
      if (this.showCtx == "move" || this.showCtx == "key")
        this.showCtx = "main";
    },
    handleChgKeyClick() {
      if (this.showCtx != "key") {
        this.showCtx = "key";
      } else {
        this.showCtx = "main";
      }
    },
    handleDatePickerChg(val) {
      if (!val) return;
      let url = this.URL_PREFIX + "/ReqRpt002/GetAllDatas";
      let data = { date: this.getDateStr(val) };
      this.loading = true;
      let aio = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            aio.histTableDatas = response.data.tableData;
          } else {
            console.log(response.data.msg);
          }
          aio.loading = false;
        })
        .catch(error => {
          console.log(error);
          aio.loading = false;
        });
    },
    handleStopTimingClick() {
      if (this.sBtnCtx === "Stop") {
        clearInterval(this.timer);
        this.sBtnCtx = "Start";
      } else {
        this.setTimer();
        this.sBtnCtx = "Stop";
      }
    },
    handleReTimingClick() {
      this.remSec = 0;
      this.sBtnCtx = "Stop";
    },
    handleMoveMonthChange(val) {
      if (!val) return;
      let month = this.getDateStr(val);
      let url = this.URL_PREFIX + "/ReqRpt002/GetMoveTargetByMonth";
      let data = { month: month };
      this.dialogMoveTgt.loading = true;
      let aio = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            aio.dialogMoveTgt.targetData.days = response.data.Days;
            aio.dialogMoveTgt.targetData.month = response.data.Month;
            let targets = response.data.TargetList;
            aio.dialogMoveTgt.targetData.targetList = [];
            aio.dialogMoveTgt.deptList.forEach(dept => {
              let target = targets.find(f => f.Department == dept);
              if (target) {
                aio.dialogMoveTgt.targetData.targetList.push(target);
              } else {
                let arry = new Array(aio.dialogMoveTgt.targetData.days).fill(0);
                let target_0 = {
                  Department: dept,
                  DeptCode: dept,
                  TargetList: arry
                };
                aio.dialogMoveTgt.targetData.targetList.push(target_0);
              }
            });
          } else {
            console.log(response.data.msg);
            aio.$message.error("服务端查询错误");
          }
          aio.dialogMoveTgt.loading = false;
        })
        .catch(error => {
          console.log(error);
          aio.$message.error("发生异常");
          aio.dialogMoveTgt.loading = false;
        });
    },
    handleSubmitClick() {
      let tableData = [];
      let trs = this.$refs.targetDiv.getElementsByTagName("tr");
      for (let i = 0; i < trs.length; i++) {
        let tdData = [];
        let tds = trs[i].getElementsByTagName("td");
        for (let j = 0; j < tds.length; j++) {
          //console.log(tds[j].innerText)
          tdData.push(tds[j].innerText);
        }
        tableData.push(tdData);
      }
      if (tableData.length < 2) {
        this.$message.error("提交的数据至少包含2行excel复制的内容！");
        return (this.tinymceHtml = "");
      }
      let len = tableData[0].length;
      if (tableData.every(e => e.length == len)) {
        let y = parseInt(tableData[0][0].split("年")[0]);
        let m = parseInt(tableData[0][0].split("年")[1]) - 1;
        if (y < 0 || m < 0 || m > 11)
          return this.$message.error("第一行第一列内容不是某年某月");
        //检查通过，开始上传
        this.dialogMoveTgt.loading = true;
        let url = this.URL_PREFIX + "/ReqRpt002/SetMoveTarget";
        let da = new Date(y, m);
        let dayArray = tableData[0].slice(1);
        let deptData = [];
        tableData.slice(1).forEach(element => {
          let dept = element[0];
          let targetArray = element.slice(1).map(m => parseInt(m));
          deptData.push({ dept, targetArray });
        });
        let data = { date: da, dayArray, deptData };
        let aio = this;
        this.$http
          .post(url, data)
          .then(response => {
            if (response.data.success) {
              aio.$message.success("操作成功");
              aio.handleMoveMonthChange(data.date);
              aio.dialogMoveTgt.selMonth = data.date;
            } else {
              console.log(response.data.msg);
              aio.$message.error("操作失败，服务端后台发生错误");
            }
            aio.dialogMoveTgt.loading = false;
          })
          .catch(error => {
            console.log(error);
            aio.$message.error("发生异常，请查询确认导入数据是否生效");
            aio.dialogMoveTgt.loading = false;
          });
      } else {
        this.$message.error("数据格式不合法！");
        return (this.tinymceHtml = "");
      }
    },
    handleChangeKeySubmited(val) {
      if (val) {
        this.showCtx = "main";
        this.keyTrue = false;
      }
    },
    updateCurTableData() {
      let url = this.URL_PREFIX + "/ReqRpt002/GetCurWipAndHoldAndMoveDatas";
      let data = { lastUpdateTime: this.lastUpdateTime };
      this.loading = true;
      let aio = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            if (response.data.type == "all") {
              aio.curTableDatas = response.data.tableData;
            } else if (response.data.type == "cur") {
              let tableData = response.data.tableData;
              for (let i = 0; i < tableData.moduleEntities.length; i++) {
                tableData.moduleEntities[i].YstdMoveTarget =
                  aio.curTableDatas.moduleEntities[i].YstdMoveTarget;
                tableData.moduleEntities[i].YstdMoveActual =
                  aio.curTableDatas.moduleEntities[i].YstdMoveActual;
                tableData.moduleEntities[i].YstdMovePercentage =
                  aio.curTableDatas.moduleEntities[i].YstdMovePercentage;
                tableData.moduleEntities[i].YstdMoveTurnRate =
                  aio.curTableDatas.moduleEntities[i].YstdMoveTurnRate;
              }
              for (let i = 0; i < tableData.testEntities.length; i++) {
                tableData.testEntities[i].YstdMoveTarget =
                  aio.curTableDatas.testEntities[i].YstdMoveTarget;
                tableData.testEntities[i].YstdMoveActual =
                  aio.curTableDatas.testEntities[i].YstdMoveActual;
                tableData.testEntities[i].YstdMovePercentage =
                  aio.curTableDatas.testEntities[i].YstdMovePercentage;
                tableData.testEntities[i].YstdMoveTurnRate =
                  aio.curTableDatas.testEntities[i].YstdMoveTurnRate;
              }
              tableData.moduleTotal.YstdMoveTarget =
                aio.curTableDatas.moduleTotal.YstdMoveTarget;
              tableData.moduleTotal.YstdMoveActual =
                aio.curTableDatas.moduleTotal.YstdMoveActual;
              tableData.moduleTotal.YstdMovePercentage =
                aio.curTableDatas.moduleTotal.YstdMovePercentage;
              tableData.moduleTotal.YstdMoveTurnRate =
                aio.curTableDatas.moduleTotal.YstdMoveTurnRate;
              tableData.testTotal.YstdMoveTarget =
                aio.curTableDatas.testTotal.YstdMoveTarget;
              tableData.testTotal.YstdMoveActual =
                aio.curTableDatas.testTotal.YstdMoveActual;
              tableData.testTotal.YstdMovePercentage =
                aio.curTableDatas.testTotal.YstdMovePercentage;
              tableData.testTotal.YstdMoveTurnRate =
                aio.curTableDatas.testTotal.YstdMoveTurnRate;
              tableData.waferOutEntities = aio.curTableDatas.waferOutEntities;
              tableData.waferOutTotalEntity =
                aio.curTableDatas.waferOutTotalEntity;
              tableData.fabTotal.YstdMoveTarget =
                aio.curTableDatas.fabTotal.YstdMoveTarget;
              tableData.fabTotal.YstdMoveActual =
                aio.curTableDatas.fabTotal.YstdMoveActual;
              tableData.fabTotal.YstdMovePercentage =
                aio.curTableDatas.fabTotal.YstdMovePercentage;
              tableData.fabTotal.YstdMoveTurnRate =
                aio.curTableDatas.fabTotal.YstdMoveTurnRate;
              aio.curTableDatas = tableData;
            } else {
              console.log("未定义的type:" + response.data.type);
            }
            aio.loading = false;
          } else {
            console.log(response.data.msg);
            aio.loading = false;
          }
        })
        .catch(error => {
          console.log(error);
        });
    },
    getWFOutTarget() {
      let prod = this.showTableDatas.waferOutEntities.map(m => m.Product);
      let month = "";
      if (this.useCur) {
        let ary = this.lastUpdateTime.split("-");
        month = ary[0] + "-" + ary[1];
      } else {
        let ary = this.getDateStr(new Date(this.selHistDate)).split("-");
        month = ary[0] + "-" + ary[1];
      }
      let data = { prods: prod, month: month };
      let aio = this;
      let url = this.URL_PREFIX + "/ReqRpt011/GetTableData";
      this.dialogWFOut.loading = true;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            aio.dialogWFOut.datas.items = response.data.items;
            aio.dialogWFOut.datas.wipData = response.data.wipData;
            aio.dialogWFOut.datas.shipData = response.data.shipData;
          } else {
            aio.$message.error("服务端程序发生异常");
            console.log(response.data.msg);
          }
          aio.dialogWFOut.loading = false;
        })
        .catch(error => {
          aio.$message.error("发生异常");
          console.log(error);
          aio.dialogWFOut.loading = false;
        });
    },
    setTimer() {
      this.timer = setInterval(() => {
        if (this.remSec > 0) this.remSec--;
      }, 1000);
    },
    getDateStr(date) {
      let y = date.getFullYear();
      let m = date.getMonth() + 1;
      m = m < 10 ? "0" + m : m;
      let d = date.getDate();
      d = d < 10 ? "0" + d : d;
      return y + "-" + m + "-" + d;
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
    },
    getCurDate() {
      let date = new Date();
      return this.getDateStr(date);
    }
  },
  mounted() {
    this.handleStopTimingClick();
    tinymce.init({});
  },
  created() {
    let url = this.URL_PREFIX + "/ReqRpt002/GetAllDatas";
    let date = this.getCurDate();
    let queryTime = this.getCurTime();
    let data = { date: date };
    let aio = this;
    this.$http
      .post(url, data)
      .then(response => {
        if (response.data.success) {
          aio.curTableDatas = response.data.tableData;
          aio.histTableDatas = aio.curTableDatas;
          aio.lastUpdateTime = queryTime;
        } else {
          console.log(response.data.msg);
        }
        aio.loading = false;
      })
      .catch(error => {
        console.log(error);
        aio.loading = false;
      });
  },
  destroyed() {
    clearInterval(this.timer);
  }
};
</script>

<style>
.aio-nowrap {
  margin-top: 10px;
  display: flex;
  flex-flow: column nowrap;
  justify-content: center;
  align-items: baseline;
  height: 80%;
}
.aio-nowrap .el-switch {
  margin-bottom: 20px;
}
.aio-nowrap .el-button,
.aio-nowrap .el-input {
  width: 170px;
  margin: 10px;
}
.aio-hidden {
  display: none;
}
.tinymce .el-row {
  margin-bottom: 10px;
}
.aio-total {
  align-content: center;
  font-weight: bold;
}
.table-div table {
  border: 2px solid purple;
  border-collapse: separate;
}

.table-div > .table-bordered > tbody > tr > td,
.table-bordered > tbody > tr > th,
.table-bordered > tfoot > tr > td,
.table-bordered > tfoot > tr > th,
.table-bordered > thead > tr > td,
.table-bordered > thead > tr > th {
  border-top: 1px solid #ddd;
}

.table-div .right-borded {
  border-right: 1px solid purple;
}

.table-div > table > tbody > tr > .top-borded,
.table-div > table > tfoot > tr > .top-borded {
  border-top: 1px solid purple;
}
.table-div > .table-bordered > thead > tr > td,
.table-bordered > thead > tr > th {
  border-bottom-width: 1px;
}
</style>
