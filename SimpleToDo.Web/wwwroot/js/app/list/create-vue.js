(function (Vue, VeeValidate) {
    if (document.querySelector('#create-todo')) {
        Vue.use(VeeValidate);

        var createList = new Vue({
            el: '#create-todo',
            data: function () {
                return {
                    list: {
                        name: ''
                    },
                    errorMessage: ''
                }
            },
            methods: {
                onSubmit: function () {
                    this.$validator.validateAll().then(result => {
                        if (result) {
                            var list = this.list;

                            $.ajax({
                                url: window.location.origin + '/api/ListsApi',
                                data: JSON.stringify(list),
                                type: 'POST',
                                dataType: 'json',
                                contentType: 'application/json',
                                success: function (response) {
                                    window.location.href = response.redirect;
                                },
                                error: function (request, error) {

                                }
                            });
                        } else {
                            this.errorMessage = 'Please fix all errors';
                        }
                    });
                }
            }
        });
    }
})(Vue, VeeValidate);