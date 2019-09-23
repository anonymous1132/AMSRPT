<template>
    <div>
        <div><label>{{ title }}</label></div>
        <slot name="chart"></slot>
    </div>
</template>

<script>
export default {
name:"baseChartContainer",
props:{
    title:{
        type:String,
        default:()=>""
    }
}
}
</script>

<style>

</style>
