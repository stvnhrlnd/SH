(function () {
    'use strict';

    angular
        .module('umbraco')
        .controller('JSONCacheController', JSONCacheController);

    JSONCacheController.$inject = ['$http', 'notificationsService'];
    function JSONCacheController($http, notificationsService) {
        var vm = this;
        vm.refresh = refresh;

        //////////

        function refresh() {
            vm.refreshing = true;

            $http.get('backoffice/api/JsonCache/Refresh').then(
                function (response) {
                    vm.refreshing = false;
                    notificationsService.success('Success', 'JSON cache updated.');
                },
                function (response) {
                    vm.refreshing = false;
                    notificationsService.error('Error', 'JSON cache failed to update.');
                });
        }
    }

})();