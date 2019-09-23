<template>
  <div class="h2Img-div">
    <el-row type="flex" justify="end">
    <el-button type="primary" icon="el-icon-download" size="mini" v-on:click="toImage">PNG</el-button>
    </el-row>
    <div ref="img">
      <slot></slot>
    </div>
    <a style="display:none" ref="dlink"/>
  </div>
</template>

<script>
import html2canvas from "html2canvas";
import { Canvas2Image } from "../../static/js/canvas2image";
export default {
  name: "baseHtml2Image",
  props: {
    fileName: {
      type: String,
      required: true,
      default: () => "下载文件"
    }
  },
  data() {
    return {};
  },
  methods: {
    toImage() {
      let _this = this;
      html2canvas(this.$refs.img, {
        backgroundColor: null
      }).then(canvas => {
        let _dataURL = canvas.toDataURL("image/png");
        let data = Canvas2Image.convertToPNG(canvas).getAttribute("src");
        let dlink = _this.$refs.dlink;
        dlink.href = data;
        dlink.download = _this.fileName + ".png";
        dlink.click();
      });
    }
  }
};
</script>

<style>

</style>
