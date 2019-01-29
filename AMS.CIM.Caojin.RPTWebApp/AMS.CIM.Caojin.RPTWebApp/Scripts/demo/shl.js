$(document).ready(function () {
    $('.dual_select').bootstrapDualListbox({
        selectorMinimalHeight: 160,
        showFilterInputs: false,
        infoText: false,
        nonSelectedListLabel: 'Available Priority:',
        selectedListLabel: 'Selected Priority:',
    });
});


var tableView0 = new Vue({
    el: '#table-view-0',
    data: {
        Conditions: '',
        Rows: []
    },
    computed: {
        display: function () {
            return this.Conditions ? 'block' : 'none';
        },
    }
});