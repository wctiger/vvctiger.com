siteCtrls.controller('mainController', ['$scope', '$rootScope',
    function ($scope, $rootScope) {

        $scope.startLoad = function (message, domSelector) {
            if(domSelector != null && domSelector.length > 0)
            {
                $(domSelector).block({ message: "<label><img src='/Content/Images/ajax-loader.gif' /> " + message + "</label>" });
            }
            else
            {
                $.blockUI({ message: "<label><img src='/Content/Images/ajax-loader.gif' /> " + message + "</label>" });
            }
        };

        $scope.endLoad = function (domSelector) {
            if (domSelector != null && domSelector.length > 0) {
                $(domSelector).unblock();
            }
            else {
                $.unblockUI();
            }
        };
    }]);