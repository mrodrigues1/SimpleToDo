$(function() {
    var toDoLists = GetToDoLists();
    var x = new Vue({
        el: '#grid-todolist-vue',
        data: {
            toDoLists: toDoLists === undefined ? [] : toDoLists
        }
    });
});

function GetToDoLists() {
    $.ajax({
        async: false,
        url: window.location.origin + '/api/ListsApi',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            return data;
        },
        error: function (request, error) {
            return [];
        }
    });
}