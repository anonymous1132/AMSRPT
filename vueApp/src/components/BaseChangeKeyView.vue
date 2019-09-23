<template>
  <div class="chg-key" v-loading="loading">
    <table class="table-responsive">
      <tr>
        <td>
          <label>Old Key:</label>
        </td>
        <td>
          <el-input type="password" v-model="oldKey"></el-input>
        </td>
        <td>
          <label v-if="!oldKey" class="keyPoint">*必填</label>
        </td>
      </tr>
      <tr>
        <td>
          <label>New Key:</label>
        </td>
        <td>
          <el-input type="password" v-model="newKey"></el-input>
        </td>
        <td>
          <label v-if="!newKey" class="keyPoint">*必填</label>
        </td>
      </tr>
      <tr>
        <td>
          <label>Confirm New Key:</label>
        </td>
        <td>
          <el-input type="password" v-model="confirmNewKey"></el-input>
        </td>
        <td>
          <label v-if="newKey!=confirmNewKey" class="keyPoint">*输入的口令不一致</label>
        </td>
      </tr>
      <tr>
          <td></td>
          <td><el-button type="primary" @click="submit">Submit</el-button></td>
          <td></td>
      </tr>
    </table>
  </div>
</template>

<script>
export default {
  name: "baseChangeKeyView",
  props: {
    project: {
      type: String,
      required: true,
      default: () => ""
    }
  },
  data() {
    return {
      oldKey: "",
      newKey: "",
      confirmNewKey: "",
      checkPass: null,
      loading: false
    };
  },
  methods: {
    submit() {
      if(!(this.oldKey&&this.newKey))return this.$message.error('必填项未填')
      if(this.newKey!=this.confirmNewKey)return this.$message.error('两次输入的新口令不一致')
      let url = this.URL_PREFIX + "/Common/UpdateKey";
      let data = {
        project: this.project,
        newKey: this.newKey,
        oldKey: this.oldKey
      };
      this.loading = true;
      let chk = this;
      this.$http
        .post(url, data)
        .then(response => {
          if (response.data.success) {
            chk.checkPass = true;
            chk.$message.success(response.data.msg);
          } else {
            chk.checkPass = false;
            chk.$message.error(response.data.msg);
          }
          chk.loading = false;
          chk.$emit("submited", chk.checkPass);
        })
        .catch(error => {
          console.log(error);
          chk.$message.error("网络故障");
          chk.loading = false;
          chk.checkPass = false;
          chk.$emit("submited", chk.checkPass);
        });
    }
  }
};
</script>

<style>

.chg-key{
    display: flex;
    justify-content: center;
}
.chg-key table td{
  align-content: left;
  min-width: 150px;
  white-space: nowrap;
  padding-top: 20px;
}
.chg-key .el-input {
  width: 200px;
}

.chg-key .el-button {
  width: 120px;
}

.chg-key label {
  width: 100px;
}

.chg-key .keyPoint {
  color: red;
  font-size: smaller;
}
</style>
