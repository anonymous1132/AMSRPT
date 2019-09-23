<template></template>

<script>
export default {
  name: "baseCanvasRender",
  props: {
    chartType: {
      type: String,
      required: true,
      validator: function(value) {
        return ['line','bar','stackedBar'].indexOf(value)>-1;
      }
    }
  }
};
</script>

<style>
</style>
