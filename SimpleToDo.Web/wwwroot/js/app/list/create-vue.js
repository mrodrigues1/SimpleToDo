$(function () {
    var createList = new Vue({
        el: '#create-list',
        data: {
            list: {
                name: ''
            }
        },
        methods: {
            onSubmit: function () {
                debugger;
                var list = this.list;

                $.ajax({
                    url: window.location.origin + '/api/ListsApi',
                    data: { list: list },
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    success: function (response) {
                        debugger;
                    },
                    error: function (request, error) {

                    }
                });
            }
        }
    });
});