$(function () {
    var indexViewModel = GetIndexViewModel();
    var x = new Vue({
        el: '#grid-todolist-vue',
        data: {
            toDoLists: indexViewModel.toDoLists,
            gridColumns: indexViewModel.gridColumns,
            sortingKey: '',
            sortingAscending: true
        },
        methods: {
            sortBy: function (column) {
                this.sortingKey = column.key;
                this.sortingAscending = !this.sortingAscending;

            }
        }
    });
});

function GetIndexViewModel() {
    var indexViewModel;

    $.ajax({
        async: false,
        url: window.location.origin + '/api/ListsApi',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            indexViewModel = data;
        },
        error: function (request, error) {
            indexViewModel = [];
        }
    });

    return indexViewModel;
}