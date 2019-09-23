<template>
  <div class="tinymce">
    <h1>tinymce</h1>
    <editor id="tinymce" v-model="tinymceHtml" :init="init"></editor>
  </div>
</template>

<script>
import tinymce from "tinymce/tinymce";
import "tinymce/themes/silver/theme";
import Editor from "@tinymce/tinymce-vue";


export default {
  name: "tinymce",
  data() {
    return {
      tinymceHtml: "",
      init: {
        language_url: "/static/tinymce/zh_CN.js",
        language: "zh_CN",
        skin_url: "/static/tinymce/skins/ui/oxide",
        height: 300,
        toolbar:
          ' undo redo',
        branding: false
      }
    };
  },
  computed:{

  },
  mounted() {
    tinymce.init({});
  },
  components: { Editor }
};
</script>

<style>
   .tox .tox-editor-container .tox-menubar{
        display: none;
    }
</style>
