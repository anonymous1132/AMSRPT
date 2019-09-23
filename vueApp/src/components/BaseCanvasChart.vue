<template>
  <div>
    <canvas :height="height" :width="width" ref="canvas"></canvas>
    <slot name="renders" :canvas="$refs.canvas"></slot>
  </div>
</template>

<script>
export default {
  name: "baseCanvasChart",
  props: {
    width: {
      type: Number,
      default: () => 650
    },
    height: {
      type: Number,
      default: () => 500
    },
    marge: {
      type: Object,
      default: () => {
        return {
          top: 50,
          left: 50,
          bottom: 50,
          right: 50
        };
      }
    },
    y: {
      type: Array,
      default: () => {
        return [
          {
            location: "left",
            category: "continuum",
            min: 0,
            max: 10,
            ticks: 10
          }
        ];
      }
    },
    x: {
      type: Array,
      default: () => {
        return [
          {
            location: "bottom",
            category: "quantum",
            items: ["1", "2", "3", "4", "5"]
          }
        ];
      }
    }
  },
  data() {
    return {
      color: "gray",
      lineWidth: "0.5"
    };
  },
  computed: {
    xWidth: function() {
      return this.width - this.marge.left - this.marge.right;
    },
    yHeight: function() {
      return this.height - this.marge.bottom - this.marge.top;
    }
  },
  methods: {
    scaleBand(domain, range) {
      if (!Array.isArray(domain)) throw new TypeError("domain must be array");
      if (domain.length < 1)
        throw new RangeError("domain must have one item at least");
      if (!Array.isArray(range)) throw new TypeError("range must be array");
      if (range.length !== 2) throw new RangeError("range must have 2 items");
      let len = domain.length;
      let unit = (range[1] - range[0]) / (len + 1);
      let res = [];
      for (let i = 0; i < len; i++) {
        res.push({ item: domain[i], value: range[0] + i * unit });
      }
      return res;
    },
    scaleLiner(domain, range, value) {
      if (!Array.isArray(domain)) throw new TypeError("domain must be array");
      if (domain.length !== 2) throw new RangeError("domain must have 2 items");
      if (!Array.isArray(range)) throw new TypeError("range must be array");
      if (range.length !== 2) throw new RangeError("range must have 2 items");
      if (
        (value > domain[0] && value > domain[1]) ||
        (value < domain[0] && value < domain[1])
      )
        throw new RangeError("value must in domain");
      let len1 = domain[1] - domain[0];
      let len2 = range[1] - range[0];
      let k = len2 / len1;
      return (value - domain[0]) * k + range[0];
    },
    xScale(xValue, index) {
      let xAxis;
      if (index === 1) {
        xAxis = this.x[1];
      } else {
        xAxis = this.x[0];
      }
      if (xAxis.category == "continuum") {
        return this.scaleLiner(
          [xAxis.min, xAxis.max],
          [0, this.xWidth],
          xValue
        );
      } else {
        return this.scaleBand(xAxis.items, [0, this.yHeight]).find(
          f => f.item === xValue
        ).value;
      }
    },
    yScale(yValue, index) {
      let yAxis;
      if (index === 1) {
        yAxis = this.y[1];
      } else {
        yAxis = this.y[0];
      }
      if (yAxis.category == "continuum") {
        return this.scaleLiner(
          [yAxis.min, yAxis.max],
          [0, this.yHeight],
          yValue
        );
      } else {
        return this.scaleBand(yAxis.items, [0, this.yHeight]).find(
          f => f.item === xValue
        ).value;
      }
    },
    renderXAsix() {
      let canvas = this.$refs.canvas;
      let context = canvas.getContext("2d");
      this.x.forEach(element => {
        let x1Site = this.marge.left;
        let x2Site = this.width - this.marge.right;
        let ySite =
          element.location === "bottom"
            ? this.height - this.marge.bottom
            : this.marge.top;
        context.beginPath();
        context.moveTo(x1Site, ySite);
        context.lineTo(x2Site, ySite);
        context.strokeStype = this.color;
        context.lineWidth = this.lineWidth;
        if (element.category == "continuum") {
            //连续数字的情况
            
        } else {
          //不连续的情况
        }
            context.stroke();
      });
    }
  }
};
</script>

<style>
</style>
