<template>
  <el-container>
    <el-header :height="headerHeight">
      <el-collapse v-model="activeNames">
        <el-collapse-item title="查询条件" name="1">
            <slot name="header"></slot>
        </el-collapse-item>
      </el-collapse>
    </el-header>
    <el-main>
      <slot name="main"></slot>
    </el-main>
  </el-container>
</template>

<script>
export default {
  name: "baseContainer",
  props: {
    headerHeight: {
      type: String,
      default: () => "600"
    }
  },
  data() {
    return {
        activeNames:['1']
    };
  }
};
</script>
