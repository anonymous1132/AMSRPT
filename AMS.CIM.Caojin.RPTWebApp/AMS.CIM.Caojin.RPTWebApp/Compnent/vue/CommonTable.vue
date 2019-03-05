<template>
  <div class="table">
 
    <table border="1px" v-dragform>
      <!-- 表格标题 -->
        <thead>
            <tr>
                <th> </th>
                <th><input type="checkbox" v-if="checkboxs"  v-model='checked' v-on:click='checkedAll' class="chestyle"></th>
                <th v-for="(item,key) in tableTop" :key="key"><span class="formtitle" :title="item">{{item}}</span></th>
            </tr>
        </thead>
      <!-- 表格内容 -->
        <tbody>
            <tr v-for="(item1 ,key1) in tableDate" :key="key1"   @dblclick.stop="tableEdit(key1,item1)">
                  <td>{{key1+1}}</td>
                   <td><input type="checkbox" v-if="checkboxs"   :value="item1"    v-model="tdPicth" class="chestyle"></td>
 
                  <td v-for="(item2 ,key2) in tableDate[key1]" :key="key2"   >
                    <span  v-if="key1==i && Editchent ||(!item2.editable)? !thshow:true" class="formtitle" :title="item2.title">{{(item2.title?item2.title:'')}}</span>
 
                    <span v-if="key1==i&& Editchent&& item2.editable ? thshow:false" >
                      <input type="text"  v-model="itemModel[key2].title" v-if="(item2.data==null)&&(item2.time==null)">
 
                     <select v-model="item2.title " v-if="item2.data" @change ="dbselect(item2)">
 
                        <option :value= item3.value?item3.value:item3.prodCode v-for="(item3,key3) in item2.data" :key="key3">{{item3.text?item3.text:item3.prodName}}</option>
                      </select>
                      <input type="date" v-model="itemModel[key2].title" v-if="item2.time!=null">
                    </span>
                  </td>
            </tr>
        </tbody>
    </table>
    <div class="block">
        <!-- 分页 -->
        <el-pagination
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page="currentPage4"
          :page-sizes="[10, 20, 30, 40]"
          :page-size="10"
          layout="total, sizes, prev, pager, next, jumper"
          :total=pageTotal>
        </el-pagination>
        <button @click="export2Excelshow=true" class="excelBut"><i></i>导出</button>
        <export2Excel v-if="export2Excelshow" @closeExce = "export2Excelshow=false" :tableDateAll= "tableDateAll" :tableDate= "tableDate" :tableTop="tableTop"></export2Excel>
 
  </div>
 
<el-dialog
  title="提示"
  :visible.sync="dialogVisible"
  width="30%"
  >
  <span>{{dialogVisibleTitle}}</span>
  <span slot="footer" class="dialog-footer">
    <el-button @click="tabremoveBut(false)">取 消</el-button>
    <el-button type="primary" @click="tabremoveBut(true)">确 定</el-button>
  </span>
</el-dialog>
</div>
</template>
<script>
import export2Excel from "./exporExcel"
export default {
  data() {
    return {
      currentPage4: 1,
      // Edit:false,
      dialogVisible: false, //提示面板
      dialogVisibleTitle: "", // 面板提示信息
      dialogVisibleType: "", //提示面板触发的事件
      tableDate: [], //   某一页的值
      thshow: false, // 显示编辑
      i: Number, // 行的下标
      checkboxs: true,
      export2Excelshow:false,
      checked: false, //
      tdPicth: [], // 点击选中
      itemModel: null, // 点击行的值
      tabkey: Number, // 编辑的下标
      tableDateSize: Array, // 获取到的列表数据
      pagesize: 10, // 每页显示的条数
      pageTotal: 0, //总共多少条数据
 
    };
  },
 
  components:{export2Excel},
  props: ["tableDateAll", "addTableValue", "tableTop", "Editchent"], // Editchent:是否需要编辑功能，tableDateAll：格单内容，tableTop：表格标题,dbselectShow:打开编码转换
  watch: {
    // 监听从父级传来的 表格数据变化
    tableDateAll(val) {
       this.tablesave()
      this.tableDateSize = val;
      this.tableDate = val.slice(0, 10); // 页面默认显示10条数据
      this.pageTotal = val.length;
    }
  },
 
  methods: {
 
    dbselect(s) {  // 转换编码
      function convert(all, val) {
        var title;
        for (var i in all) {
          if (all[i].prodCode == val) {
            title = all[i].prodName;
          }
          if (all[i].value == val) {
            title = all[i].text;
          }
        }
        return title;
      }
      var that = this
      function correlation(value) { // 关联数据
        for (var i in value) {
          console.log();
          if (value[i].relevance) {
            for (var j in value) {
              if (
                value[i].relevance &&
                value[i].relevance == value[j].relevance
              ) {
                if (i != j) {
                    if(that.dbselectShow){
                      if (convert(s.data, value[j].title)) {
                            value[i].title = convert(s.data, value[j].title);
                      }
                    }else{
                      value[i].title = value[j].title
                    }
 
                }
                break;
              }
            }
          }
        }
      }
 
      for (var i in this.tableDate) {
        correlation(this.tableDate[i]);
      }
    },
    handleSizeChange(val) {
      // 选择每页显示的数据
      console.log(`每页 ${val} 条`);
      this.pagesize = val;
 
      function sliceArr(array, size) {
        var result = [];
        for (var x = 0; x < Math.ceil(array.length / size); x++) {
          var start = x * size;
          var end = start + size;
          result.push(array.slice(start, end));
        }
        return result;
      }
      this.tableDate = sliceArr(this.tableDateSize, val)[0];
    },
    handleCurrentChange(val) {
      // 选择显示第几页
      function sliceArr(array, size) {
        var result = [];
        for (var x = 0; x < Math.ceil(array.length / size); x++) {
          var start = x * size;
          var end = start + size;
          result.push(array.slice(start, end));
        }
        return result;
      }
      this.tableDate = sliceArr(this.tableDateSize, this.pagesize)[val - 1];
    },
 
    getTableVlue() {
      //刷新
 
      this.$store.dispatch("Warranty/getWarranty");
 
    },
 
    tableEdit(key, item) {
      // 可编辑
      console.log(this.Editchent);
      this.itemModel = item; // 点击时行的值写入到输入默认
      this.i = key; // 行的下标
      this.thshow = true; // 显示可以输入
      this.tabkey = key; // 输入行的下标
    },
    tablesave(title) {
      //保存
      if (title) {
        this.dialogVisibleTitle = title;
        this.dialogVisible = true;
        this.dialogVisibleType = "tablesave";
      } else {
        if (this.tableDate == "") {
          // 判断添加时的值不能为空
          return;
        }
        for (var i in this.tableDate) {
          for (var j in this.tableDate[i]) {
            if (this.tableDate[i][j].title == "") {
              console.log("输入不能为空");
              return;
            }
          }
        }
        if (this.itemModel) {
          console.log(this.itemModel);
 
          this.tableDate[this.tabkey] = this.itemModel;
          this.thshow = false;
          console.log(this.tableDate);
        }
        this.tabkey = null;
      }
    },
 
    tabSaveSub(url) {
      // 提交和保存
      this.tablesave();
      var that = this;
      this.$store.dispatch(url, that);
    },
 
    checkedAll() {
      // 全选和反选
      var _this = this;
      console.log(_this.checked);
      if (this.checked) {
        _this.tdPicth = [];
      } else {
        _this.tdPicth = [];
        _this.tableDate.forEach(function(item, key) {
          _this.tdPicth.push(item);
        });
      }
    },
 
    tabremoveBut(v) {
      // 提示面板事件
      if (v) {
        switch (this.dialogVisibleType) {
          case "tabremove":
            this.tabremove();
            this.dialogVisible = false;
            break;
          case "tablesave":
            this.tablesave();
            this.dialogVisible = false;
            break;
          default:
        }
      } else {
        this.dialogVisible = false;
      }
    },
 
    tabremove(title) {
      // 删除选中
 
      if (title) {
        this.dialogVisibleTitle = title;
        this.dialogVisible = true;
        this.dialogVisibleType = "tabremove";
      } else {
        console.log(this.tdPicth);
        function remove(ob, val) {
          var ob = ob;
          var val = val;
          var index = ob.indexOf(val);
          if (index > -1) {
            ob.splice(index, 1);
          }
          return ob;
        }
 
        var tdPicth = this.tdPicth;
        for (var j in tdPicth) {
          console.log(j);
          remove(this.tableDate, this.tdPicth[j]);
          //  remove(this.tableDateAll, this.tdPicth[j]);
        }
 
        this.tdPicth = [];
      }
    },
    addTable() {
      //添加 table
      this.itemModel = this.addTableValue;
      this.i = this.tableDate.length;
      this.tableDate.push(this.addTableValue);
      this.tableDateAll.push(this.addTableValue)
      this.thshow = true;
    },
 
 
  }
};
</script>
<style scoped>
.table{
  width:100%;
  overflow-x: auto
}
th {
  border: 1px solid #dcdfe6;
  padding: 5px;
  white-space: nowrap;
  text-align: center;
  box-sizing: border-box;
}
 
td {
  white-space: nowrap;
  text-align: center;
  border-width: 1px;
  border-style: solid;
  border: 1px solid #dcdfe6;
  border-image: initial;
  padding: 5px;
  overflow: hidden;
  box-sizing: border-box;
}
.formtitle {
  display: block;
  width: 150px;
  overflow: hidden;
  white-space: nowrap;
  word-wrap: normal;
  text-overflow: ellipsis;
 
 
    overflow: hidden;
}
.chestyle {
  display: inline-block;
  padding: 3px;
}
input {
  display: inline-block;
  max-width: 150px;
  text-align: center;
  border: 1px solid #dcdfe6;
  padding: 5px;
}
table {
  width: 200%;
  display: block;
  height: 430px;
  overflow: auto;
  cursor: default;
  border-collapse:collapse
}
 
select {
  display: inline-block;
  height: 30px;
  line-height: 30px;
  text-align: center;
  padding-left: 10px;
  border: 1px solid #dcdfe6;
}
.excelBut{
position: absolute;bottom: 5px;right: 25%;
border: 0;background: #fff;width: 70px;
background: url("../../assets/images/export.png")no-repeat 10% 53%;
color: #606266;
}
/* .excelBut i{width: 14px;height: 14px;display: inline-block;background: url("../../assets/images/export.png")no-repeat;} */
.block{position: relative}
</style>