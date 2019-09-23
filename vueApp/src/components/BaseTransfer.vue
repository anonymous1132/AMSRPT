<template>
  <div class="transfer">
    <div :style="{width:width}" class="transfer-box">
      <div class="transfer-control">
        <label>{{ titleLeft }}</label>
      </div>
      <div class="transfer-control">
        <el-button size="small" icon="el-icon-d-arrow-right" @click="handleAllSelectClick">全选</el-button>
      </div>
      <div class="transfer-control">
        <select v-model="leftValue" multiple :style="{height:height}">
          <option v-for="(item,index) in availableItems" :key="index" v-text="item" :value="index"></option>
        </select>
      </div>
    </div>

    <div :style="{width:width}" class="transfer-box">
      <div class="transfer-control">
        <label>{{ titleRight }}</label>
      </div>

      <div class="transfer-control">
        <el-button size="small" icon="el-icon-d-arrow-left" @click="handleCancelClick">取消</el-button>
      </div>

      <div class="transfer-control">
        <select v-model="rightValue" multiple :style="{height:height}">
          <option v-for="(item,index) in selectedItems" :key="index" v-text="item" :value="index"></option>
        </select>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "baseTransfer",
  props: {
    titleLeft: {
      type: String,
      default: () => ""
    },
    titleRight: {
      type: String,
      default: () => ""
    },
    width: {
      type: String,
      default: () => "135px"
    },
    height: {
      type: String,
      default: () => "150px"
    },
    data: {
      type: Array,
      default: [],
      required: true
    }
  },
  data() {
    return {
      selectedItems: [],
      leftValue: [],
      rightValue: []
    };
  },
  computed:{
    availableItems:function(){
      return this.data.map(m => m).sort();
    }
  },
  watch: {
    leftValue(val) {
        if(val.length==0)return
        for(let i=val.length-1;i>=0;i--){
            this.selectedItems.push(this.availableItems[val[i]])
            this.availableItems.splice(val[i],1)
            val.splice(i,1)
        }
      this.selectedItems.sort();
    },
    rightValue(val){
        if(val.length==0)return
         for(let i=val.length-1;i>=0;i--){
            this.availableItems.push(this.selectedItems[val[i]])
            this.selectedItems.splice(val[i],1)
            val.splice(i,1)
        }
      this.availableItems.sort();
    },
    selectedItems(val){
      this.$emit('selectedChanged', val)
    }
  },
  methods:{
      handleAllSelectClick(){
          this.leftValue=this.availableItems.map((m,index)=>index)
      },
      handleCancelClick(){
          this.rightValue=this.selectedItems.map((m,index)=>index)
      }
  },
  mounted() {

  }
};
</script>

<style>
.transfer-box {
  display: inline-block;
  padding-left: 15px;
  padding-right: 15px;
}
.transfer-control {
  width: 100%;
  overflow: hidden;
}

.transfer-control > * {
  width: 100%;
}

.transfer-control > select > option {
  font-weight: normal;
  display: block;
  white-space: pre;
  min-height: 1.2em;
  padding: 0px 2px 1px;
  font-size: 14px;
}


</style>
