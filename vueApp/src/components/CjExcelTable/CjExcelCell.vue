<template>
  <div class="cj-excel-cell" @dblclick="handleDblClick" :style="styleOptions">
    <input v-if="editState" v-model="value" class="cj-input" ref="textbox" @blur="handleBlur">
    <div v-else v-text="value" class="cj-div"></div>
  </div>
</template>

<script>
export default {
  name: "CjExcelCell",
  props: {
    value: {
      type: String,
      default: () => ""
    },
    editable: {
      type: Boolean,
      default: () => false
    },
    styleOptions: {
      type: Object,
      default: () => {
        return {
        };
      }
    }
  },
  data() {
    return {
      editState: false
    };
  },
  methods: {
    handleDblClick() {
      if (!this.editable) return;
      this.editState = true;
      this.$nextTick(() => {
        this.$refs.textbox.focus();
      });
    },
    handleBlur() {
      this.editState = false;
    }
  },
  mounted() {},
  created() {}
};
</script>

<style>
  .cj-excel-cell:active{
      color:red;
  }
</style>


