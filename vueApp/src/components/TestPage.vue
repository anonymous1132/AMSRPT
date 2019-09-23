<template>
  <BaseLayout>
    <BaseContainer>
      <el-row type="flex" slot="header">
        <el-col :lg="4" :offset="18" :sm="12">
          <BaseHeaderCard project="RPT000036" />
        </el-col>
      </el-row>
      <div slot="main">
        <BaseTableContainer :tableData="tableData"  :useExcel="false">
          <table
            slot="table"
            class="table table-responsive table-bordered table-striped table-hover"
            slot-scope="scope"
          >
            <template v-for="(ship,idx ) in scope.datas.shipData">
              <thead :key="'h'+idx">
                <tr>
                  <th>Prod ID</th>
                  <th>WF Out(PCS)</th>
                  <th v-for="item in scope.datas.items" :key="item" v-text="item"></th>
                </tr>
              </thead>
              <tbody :key="'b'+idx">
                <tr>
                  <td rowspan="3">{{ ship.ProductID }}</td>
                  <td>Target</td>
                  <td v-for="(data,index) in ship.Plans" :key="index" v-text="data.Target"></td>
                </tr>
                <tr>
                  <td>Actual</td>
                  <td v-for="(data,index) in ship.Plans" :key="index">
                    <el-button type="text" size="small">{{ data.Actual }}</el-button>
                  </td>
                </tr>
                <tr>
                  <td>Acc.Gap</td>
                  <td v-for="(data,index) in ship.Plans" :key="index" v-text="data.Gap"></td>
                </tr>
              </tbody>
            </template>
          </table>
        </BaseTableContainer>
        <BaseTransfer :titleLeft="'123'" :titleRight="'4561111111111111111111'" :data="['caojin','yangbt','zyj','ld']"></BaseTransfer>
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

export default {
  name: "TestPage",
  components: {
    BaseLayout,
    BaseContainer,
    BaseHeaderCard,
    BaseTableContainer,
    BaseTransfer
  },
  data() {
    return {};
  },
  computed: {
    tableData: function() {
      return {};
    }
  },
  mounted() {}
};
</script>
