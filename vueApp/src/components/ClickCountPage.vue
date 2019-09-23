<template>
  <BaseLayout>
    <BaseContainer>
      <el-row slot="header" type="flex" justify="start">
        <el-col :span="20">
          <el-row type="flex" justify="center">

              <BaseTransfer
                :titleLeft="'RPT Type'"
                :titleRight="'Selected RPT Type'"
                :data="rptTypeItems"
                width="200px"
                ref="rptType"
                @selectedChanged="handleRptTypeSelectedChanged"
              ></BaseTransfer>

              <BaseTransfer
                :titleLeft="'RPT Name'"
                :titleRight="'Selected RPT Name'"
                :data="rptNameItems"
                width="270px"
                ref="rptName"
              ></BaseTransfer>
          </el-row>
        </el-col>
        <el-col :span="4">
          <BaseHeaderCard project="RPT000063"/>
          <div class="clkct-lbl"><label>Month:</label></div>
          <div class="clkct-btn">
            <el-date-picker type="month" size="small" placeholder="default by year" v-model="selMonth"></el-date-picker>
            <el-button type="primary" size="small" icon="el-icon-search" :loading="loading" @click="handleQueryClick"></el-button>
          </div>
        </el-col>
      </el-row>
      <div slot="main">
        <BaseTableContainer
          :tableData="tableData"
          :fileName="fileName"
          v-if="tableData.items.length>0"
        >
          <table
            class="table table-responsive table-bordered table-hover"
            slot="table"
            slot-scope="scope"
          >
            <caption>{{scope.datas.caption}}</caption>
            <thead>
              <tr>
                <th>RPT Type</th>
                <th>RPT Name</th>
                <th v-for="(item,idx) in scope.datas.items" :key="idx" v-text="item"></th>
                <th>Total</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="row in scope.datas.clkctEntities" :key="row.PrivilegeID">
                <td v-text="row.RptType"></td>
                <td v-text="row.RptName"></td>
                <td
                  v-for="(val,idx) in row.ClickCountValues"
                  :key="row.PrivilegeID+idx"
                  v-text="val"
                ></td>
                <td v-text="row.Total"></td>
              </tr>
            </tbody>
          </table>
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
import BaseTransfer from "../components/BaseTransfer";

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

export default {
  name: "ClickCountPage",
  components: {
    BaseLayout,
    BaseContainer,
    BaseHeaderCard,
    BaseTableContainer,
    BaseTransfer
  },
  data() {
    return {
      selMonth: "",
      categoryList: [],
      rptNameItems: [],
      loading: true,
      tableData: { items: [], clkctEntities: [], caption: "" }
    };
  },
  computed: {
    rptTypeItems: function() {
      let arry = this.categoryList.map(m => m.PrivilegeCategory);
      arry.distinct();
      return arry;
    },
    fileName: function() {
      return this.tableData.caption + ".xls";
    }
  },
  methods: {
    handleQueryClick() {
      let items = this.$refs.rptName.selectedItems;
      let idList = this.categoryList
        .filter(f => items.indexOf(f.PrivilegeName) > -1)
        .map(m => m.PrivilegeID);
      if (idList.length == 0)
        return this.$message.error("Please select one RPT Name Item at least");
      if (!this.selMonth) this.getTableDataOfYear(idList);
      else this.getTableDataOfMonth(idList);
    },
    handleRptTypeSelectedChanged(items) {
      this.rptNameItems = this.categoryList
        .filter(f => items.indexOf(f.PrivilegeCategory) > -1)
        .map(m => m.PrivilegeName);
      this.$refs.rptName.selectedItems = [];
    },
    getTableDataOfMonth(idList) {
      let url = this.URL_PREFIX + "/ReqRpt063/GetTableDataOfMonth";
      let data = {
        year: this.selMonth.getFullYear(),
        month: this.selMonth.getMonth() + 1,
        privilegeIdList: idList
      };
      this.tableData.caption =
        "Click Count-" +
        data.year +
        (data.month < 10 ? "0" + data.month : data.month);
      this.postRequestTableData(url, data);
    },
    getTableDataOfYear(idList) {
      let url = this.URL_PREFIX + "/ReqRpt063/GetTableDataOfYear";
      let data = { privilegeIdList: idList };
      this.tableData.caption = "Click Count-" + new Date().getFullYear() + "年";
      this.postRequestTableData(url, data);
    },
    postRequestTableData(url, data) {
      this.loading = true;
      let clkct = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            clkct.tableData.items = response.data.Items;
            clkct.tableData.clkctEntities = response.data.ClickCountEntities;
            clkct.tableData.clkctEntities.map(m => {
              let category = clkct.categoryList.find(
                f => f.PrivilegeID == m.PrivilegeID
              );
              m.RptType = category.PrivilegeCategory;
              m.RptName = category.PrivilegeName;
              m.Total = m.ClickCountValues.reduce((prev, next) => {
                return prev + next;
              });
            });
            clkct.tableData.clkctEntities.sort((a,b)=>{
              if(a.Total>b.Total)return -1;
              if(a.Total<b.Total)return 1;
              return 0;
              })
          } else {
            console.log(response.data.msg);
            clkct.$message.error("服务器发生异常，请求失败");
          }
          this.loading = false;
        })
        .catch(error => {
          console.log(error);
          clkct.$message.error("请求异常，获取数据更新页面失败");
          this.loading = false;
        });
    }
  },
  mounted() {
    let url = this.URL_PREFIX + "/ReqRpt063/GetCategoryList";
    let clkct = this;
    this.$http
      .post(url)
      .then(response => {
        if (response.data.success) {
          clkct.categoryList = response.data.BrprivilegeList;
        } else {
          console.log(response.data.msg);
          clkct.$message.error("服务端运行异常，初始化失败");
        }
        clkct.loading = false;
      })
      .catch(error => {
        console.log(error);
        clkct.$message.error("服务器网络异常，初始化失败");
        clkct.loading = false;
      });
  }
};
</script>

<style>
.clkct-label-left {
  text-align: left;
  overflow: hidden;
}

.table-div > table {
  white-space: nowrap;
}
.table-div {
  max-height: 600px;
  overflow: auto;
}
.clkct-btn{
  display: flex;
  flex-flow: row nowrap;
  justify-content: center;
  align-items: baseline;
  margin-bottom: 10px;
}
.clkct-lbl{
  text-align: left;
  margin-top: 20px;
}
</style>

