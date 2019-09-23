<template>
  <BaseLayout>
    <BaseContainer>
      <div slot="header">
        <el-menu
          default-active="1-0"
          mode="horizontal"
          class="el-menu-demo wat-menu"
          background-color="#00CED1"
          text-color="#fff"
          active-text-color="#fff"
          @select="handleMenuSelect"
        >
          <el-submenu index="1">
            <template slot="title">
              <div class="wat-title">{{ menuTitle }}</div>
            </template>
            <el-menu-item
              v-for="(item,index) in menuItems"
              :key="index"
              :index="'1-'+index"
              v-text="item"
            ></el-menu-item>
          </el-submenu>
        </el-menu>
        <el-row type="flex" justify="center">
          <el-col :span="24">
            <h4>{{headerTitle}}</h4>
          </el-col>
        </el-row>
        <el-row type="flex" justify="center">
          <el-col :lg="18" :md="24">
            <div class="wat-nowrap">
              <label>Lot ID</label>
              <el-input
                class="wat-input"
                v-model="lotID"
                placeholder="请输入Lot ID"
                @keyup.enter.native="handleLotNext"
              ></el-input>
              <el-button
                class="wat-btn-next"
                icon="el-icon-d-arrow-right"
                :loading="loading"
                :disabled="!lotID"
                @click="handleLotNext"
              ></el-button>
              <label>Recipe</label>
              <el-select class="wat-input" v-model="selectedRcp" placeholder="请选择">
                <el-option v-for="(item,idx) in rcpList" :key="idx" :label="item" :value="item"></el-option>
              </el-select>
              <el-button
                class="wat-btn-next"
                icon="el-icon-d-arrow-right"
                :loading="loading"
                :disabled="!selectedRcp"
                @click="handleRcpNext"
              ></el-button>
              <label>Version</label>
              <el-select class="wat-input" v-model="selectedVersion" placeholder="请选择">
                <el-option v-for="(item,idx) in versionList" :key="idx" :label="item" :value="item"></el-option>
              </el-select>
            </div>
            <el-row type="flex" justify="center">
              <el-col>
                <el-button
                  class="primary-btn"
                  type="primary"
                  :disabled="queryBtnDisabled"
                  @click="handleQueryClick"
                  size="small"
                >Query</el-button>
                <el-button
                  class="primary-btn"
                  type="primary"
                  :disabled="clearBtnDisabled"
                  @click="handleClearClick"
                  size="small"
                >Clear</el-button>
              </el-col>
            </el-row>
          </el-col>
          <el-col :lg="4" :md="8" :sm="12">
            <BaseHeaderCard project="RPT000063" user="李承翰（0190）"/>
          </el-col>
        </el-row>
      </div>
      <div slot="main">
        <table class="table table-responsive table-bordered">
          <caption>
            <div>
              <i>Test Infomation</i>
            </div>
          </caption>
          <tbody>
            <tr>
              <th>Recipe ID</th>
              <td>{{tableEntity.rcp}}</td>
              <th>Foup ID</th>
              <td>{{tableEntity.cast}}</td>
              <th>Tester ID</th>
              <td>{{tableEntity.eqp}}</td>
              <th>Operator ID</th>
              <td>{{tableEntity.operator}}</td>
            </tr>
            <tr>
              <th>测试刻号数</th>
              <td>{{tableEntity.pcs}}</td>
              <th>测试sites</th>
              <td>{{tableEntity.sites}}</td>
              <th>Track in time</th>
              <td>{{tableEntity.inTime}}</td>
              <th>Track out time</th>
              <td>{{tableEntity.outTime}}</td>
            </tr>
          </tbody>
        </table>

        <table class="table table-responsive table-bordered">
          <caption>
            <div>
              <i>Wafer ID</i>
            </div>
          </caption>
          <tbody>
            <tr>
              <td v-for="i in 40" :key="i" v-text="tableEntity.wafers[i-1].id"></td>
            </tr>
            <tr>
              <td v-for="i in 40" :key="i" v-text="tableEntity.wafers[i-1].test"></td>
            </tr>
          </tbody>
        </table>

        <table class="table table-responsive table-bordered">
          <thead>
            <tr>
              <th rowspan="2">Wafer</th>
              <th rowspan="2">No.</th>
              <th rowspan="2">Item</th>
              <th v-for="site in tableEntity.siteItems" :key="site.id" v-text="site.label"></th>
              <th rowspan="2">SPEC HIGH</th>
              <th rowspan="2">SPEC LOW</th>
            </tr>
            <tr>
              <th v-for="site in tableEntity.siteItems" :key="site.id" v-text="site.coordinate"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row,idx) in tableEntity.testData" :key="idx">
                <td v-text="row.wafer"></td>
                <td v-text="row.no"></td>
                <td v-text="row.item"></td>
                <td v-for="(site,index) in row.sites" :key="'site'+index" v-text="site" :class="site>row.specHigh||site<row.specLow?'wat-red':'wat-black'">
                </td>
                <td v-text="row.specHigh"></td>
                <td v-text="row.specLow"></td>
            </tr>
          </tbody>
        </table>
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
  name: "watPage",
  components: {
    BaseLayout,
    BaseContainer,
    BaseHeaderCard,
    BaseTableContainer
  },
  data() {
    return {
      menuItems: ["WAT all data", "WAT out of Spec"],
      menuTitle: "WAT data analisys",
      selectedIndex: ["1", "1-0"],
      lotID: "",
      loading: false,
      selectedRcp: "",
      rcpList: [],
      selectedVersion: "",
      versionList: [],
      tableEntity: {
        lotID: "",
        rcp: "",
        version: "",
        cast: "",
        eqp: "",
        operator: "",
        pcs: "",
        sites: "",
        inTime: "",
        outTime: "",
        wafers: [],
        testData: [],
        siteItems:[]
      }
    };
  },
  computed: {
    subMenuIdx: function() {
      return this.selectedIndex[1].split("-")[1];
    },
    headerTitle: function() {
      let i = this.subMenuIdx;
      let str = "WAT data analisys";
      let para = i === "0" ? "ALL" : "Out of SPEC";
      return str + "(" + para + ")";
    },
    queryBtnDisabled: function() {
      return !(this.lotID && this.selectedRcp && this.selectedVersion);
    },
    clearBtnDisabled: function() {
      return !(this.lotID || this.selectedRcp || this.selectedVersion);
    }
  },
  methods: {
    handleMenuSelect(key, keyPath) {
      this.selectedIndex = keyPath;
    },
    handleLotNext() {},
    handleRcpNext() {},
    handleQueryClick() {},
    handleClearClick() {
      this.lotID = "";
      this.selectedRcp = "";
      this.selectedVersion = "";
    }
  },
  mounted() {},
  created() {
    this.tableEntity.wafers = new Array(40);
    this.tableEntity.wafers.fill({ id: "", test: "" });
    for (let i = 0; i < 25; i++) {
      this.tableEntity.wafers[i] = { id: i + 1, test: i % 2 === 1 ? "p" : "f" };
    }
    for(let i=0;i<9;i++){
      this.tableEntity.siteItems.push({id:i,label:'site'+(i+1),coordinate:i+','+(i*-1)})
    }
    for(let i=0;i<24;i++){
      let row={wafer:'1',no:i+1,item:'item'+(i+1),specHigh:9999,specLow:12,sites:[]}
      row.sites=new Array(9)
      row.sites.fill(1)
      this.tableEntity.testData.push(row)
    }
  }
};
</script>

<style>
.wat-input {
  width: 200px;
  margin-bottom: 30px;
}
.wat-btn-next {
  margin-left: 0;
  margin-right: 30px;
}
.wat-title {
  font-weight: bold;
  font-size: larger;
}
.wat-menu {
  margin-bottom: 50px;
}
.wat-nowrap {
  margin-top: 30px;
  display: flex;
  flex-flow: center nowrap;
  align-items: baseline;
}
.wat-nowrap label {
  margin-right: 5px;
}
.el-main .table th {
  text-align: center;
  vertical-align: middle;
}

.el-main .table caption div {
  font-weight: bold;
  font-size: larger;
  text-align: center;
}

.wat-red{
  color: red
}
</style>
