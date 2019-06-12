<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>Essential Links</h2>
     <WipChart :chartData="wipChartData.data" :options="wipChartData.options"></WipChart>
    <ul>
      <li>
        <a
          href="https://vuejs.org"
          target="_blank"
        >
          Core Docs
        </a>
      </li>
      <li>
        <a
          href="https://forum.vuejs.org"
          target="_blank"
        >
          Forum
        </a>
      </li>
      <li>
        <a
          href="https://chat.vuejs.org"
          target="_blank"
        >
          Community Chat
        </a>
      </li>
      <li>
        <a
          href="https://twitter.com/vuejs"
          target="_blank"
        >
          Twitter
        </a>
      </li>
      <br>
      <li>
        <a
          href="http://vuejs-templates.github.io/webpack/"
          target="_blank"
        >
          Docs for This Template
        </a>
      </li>
    </ul>
    <h2>Ecosystem</h2>
    <ul>
      <li>
        <a
          href="http://router.vuejs.org/"
          target="_blank"
        >
          vue-router
        </a>
      </li>
      <li>
        <a
          href="http://vuex.vuejs.org/"
          target="_blank"
        >
          vuex
        </a>
      </li>
      <li>
        <a
          href="http://vue-loader.vuejs.org/"
          target="_blank"
        >
          vue-loader
        </a>
      </li>
      <li>
        <a
          href="https://github.com/vuejs/awesome-vue"
          target="_blank"
        >
          awesome-vue
        </a>
      </li>
    </ul>
    <CjExcelCell value="1" :editable="true"/>
     <CjExcelCell value="2"/>
  </div>
</template>

<script>
import WipChart from '../components/WipChart'
import CjExcelCell from '../components/CjExcelTable/CjExcelCell'
export default {
  name: 'HelloWorld',
  components:{
    WipChart,
    CjExcelCell
  },
  data () {
    return {
      msg: 'Welcome to Your Vue.js App',
      wipChartData:{
        data:[{
          stage:'Stage1',
          ct:21,
          stageMove:0,
          holdLot:1,
          superHot:0,
          hot:1,
          other:[5],
          accWip:72,
        },{ 
          stage:'Stage2',
          ct:18,
          stageMove:1,
          holdLot:0,
          superHot:1,
          hot:0,
          other:[8],
          accWip:45
          },
          { 
          stage:'Stage3',
          ct:15,
          stageMove:3,
          holdLot:1,
          superHot:1,
          hot:0,
          other:[15],
          accWip:32
          }],
        options:{
          height:600,
          prod:['caojinText'],
          key:'Stage3(15)',
          month:'Stage2(18)'
        }
      }
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style>
h1, h2 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}

.cj-excel-cell {
  width: 40px;
  height: 25px;
  overflow: hidden;
}
.cj-excel-cell .cj-input{
  width: 100%;
  height:100%;
}

</style>
