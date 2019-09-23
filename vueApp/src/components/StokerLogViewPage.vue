<template>
  <BaseLayout>
    <BaseContainer>
      <template slot="header">
        <el-row type="flex" justify="end">
          <el-button
            type="primary"
            size="mini"
            :disabled="!tinymceHtml"
            v-on:click="handleSubmitClick"
          >Submit</el-button>
        </el-row>
        <editor id="tinymce" v-model="tinymceHtml" :init="init"></editor>
      </template>
      <template slot="main">
        <div ref="targetDiv" v-html="tinymceHtml" class="aio-hidden"></div>
        <BaseTableContainer :tableData="tableData" fileName="StokerEventReport.xls">
          <table
            slot="table"
            slot-scope="scope"
            class="table table-responsive table-bordered table-hover"
          >
            <thead>
              <tr>
                <th>任务开始时间</th>
                <th>任务结束时间</th>
                <th>耗时(s)</th>
                <th>任务类型</th>
                <th>Position</th>
                <th>Tag Data</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item,idx) in scope.datas" :key="idx">
                <td v-text="item.start"></td>
                <td v-text="item.end"></td>
                <td v-text="item.duration"></td>
                <td v-text="item.category"></td>
                <td v-text="item.postion"></td>
                <td v-text="item.tag"></td>
              </tr>
            </tbody>
          </table>
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
import tinymce from "tinymce/tinymce";
import "tinymce/themes/silver/theme";
import Editor from "@tinymce/tinymce-vue";
export default {
  name: "",
  components: {
    BaseLayout,
    BaseContainer,
    BaseHeaderCard,
    BaseTableContainer,
    Editor
  },
  data() {
    return {
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
      tableData: []
    };
  },
  computed: {},
  methods: {
    handleSubmitClick() {
      this.tableData = [];
      let ps = this.$refs.targetDiv.getElementsByTagName("p");
      let arry = [];
      for (let i = 0; i < ps.length; i++) {
        arry.push(ps[i].innerText);
      }
      while (arry.length > 0) {
        this.shiftArryToTargetLine(arry);
        this.getOneRowData(arry);
      }
      console.log(this.tableData);
    },
    shiftArryToTargetLine(arry) {
      while (arry.length > 0) {
        if (arry[0].indexOf("Command Acknowledge : (from Host)") > -1) {
          break;
        } else {
          arry.shift();
        }
      }
    },
    getOneRowData(arry) {
      let temp = [];
      let type = "";
      if (arry.length > 0) {
        if (arry[0].indexOf("Slot Transfer") > -1) {
          type = "Slot Transfer";
        } else {
          type = "Box Check In";
        }
        temp.push(arry[0]);
        arry.shift();
      }
      while (arry.length > 0) {
        if (arry[0].indexOf(type) > -1 && arry[0].indexOf("completed") > -1) {
          temp.push(arry[0]);
          arry.shift();
          break;
        } else {
          temp.push(arry[0]);
          arry.shift();
        }
      }
      if (temp.length > 0) {
        let start = temp[0].split(".")[0];
        let end = temp[temp.length - 1].split(".")[0];
        let duration = this.getDuration(start, end);
        let category = type === "Slot Transfer" ? "out" : "in";
        let posLine = temp.find(
          f =>
            f.indexOf("RFID Reader") > -1 &&
            f.indexOf("Move " + (category === "in" ? "In" : "Out") + " Event") >
              -1 &&
            f.indexOf("Position:F" > -1)
        );
        let postion = "";
        let tag = "";
        if (posLine) {
          let posTmp = posLine.split("Position:")[1].split(" , Tag Data: ");
          postion = posTmp[0];
          tag = posTmp[1].replace(")", "");
        }
        this.tableData.push({ start, end, duration, category, postion, tag });
      }
    },
    getDuration(start, end) {
      let startAry = start.split(":");
      let endAry = end.split(":");
      let deltaHr = parseInt(endAry[0]) - parseInt(startAry[0]);
      let deltaMin = parseInt(endAry[1]) - parseInt(startAry[1]);
      let deltaSec = parseInt(endAry[2]) - parseInt(startAry[2]);
      let total = deltaHr * 3600 + deltaMin * 60 + deltaSec;
      return total;
    }
  },
  mounted() {
    tinymce.init({});
  },
  created() {}
};
</script>

<style>
.aio-hidden {
  display: none;
}
</style>