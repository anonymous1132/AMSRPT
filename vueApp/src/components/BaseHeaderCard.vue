<template>
  <el-card class="box-card" shadow="hover">
    <div>开发者：{{ coder }}</div>
    <div>需求者：{{ user }}</div>
    <div>Cilck Count: {{ count }}</div>
    <slot></slot>
  </el-card>
</template>

<script>
export default {
    name:"baseHeaderCard",
    props:{
        coder:{
            type:String,
            default:()=>"曹晋（0279）"
        },
        user:{
            type:String,
            default:()=>"陈舒（0353）"
        },
        project:{
            type:String,
            required:true
        }
    },
    data(){
        return {
            count:0
        }
    },
    mounted(){
        let url=this.URL_PREFIX+ "/Common/GetClickCount"
        let data={
            title:this.project
        }
        let card=this
        this.$http.post(url,data)
            .then(response=>{
            if(response.data.success){
                card.count=response.data.count
            }else{
                console.log(response.data.msg)
            }
        })
            .catch(error=>{console.log(error)})
    }
}
</script>


