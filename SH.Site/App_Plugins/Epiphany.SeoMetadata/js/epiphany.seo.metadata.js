angular.module('umbraco').filter('ellipsisLimit', function () {
    return function (value, max) {
        if (!value) {
        	return '';
        }

        max = parseInt(max, 10);
        if (!max) {
        	return value;
        }

        if (value.length <= max) {
        	return value;
        }

        value = value.substr(0, max);
        return value + ' …';
    };
});
angular.module('umbraco').directive('maxlen', function () {
    return {
        require: 'ngModel',
        link: function (scope, el, attrs, ctrl) {

            var validate = false;
            var length = 999999;

            if (attrs.name === 'title') {
                validate = scope.model.config.allowLongTitles !== '1';
                length = scope.serpTitleLength;
            } else if (attrs.name === 'description') {
                validate = scope.model.config.allowLongDescriptions !== '1';
                length = scope.serpDescriptionLength;
            }

            ctrl.$parsers.unshift(function (viewValue) {
                if (validate && viewValue.length > length) {
                    ctrl.$setValidity('maxlen', false);
                } else {
                    ctrl.$setValidity('maxlen', true);
                }

                return viewValue;
            });
        }
    };
});

angular.module("umbraco").controller("EpiphanySeoMetadataController", [
  '$scope', function($scope) {

    $scope.invalidate = true;
    $scope.model.hideLabel = true;
    $scope.serpTitleLength =  !!$scope.model.config.serpTitleLength ? $scope.model.config.serpTitleLength : 65;
    $scope.serpDescriptionLength = !!$scope.model.config.serpDescriptionLength ? $scope.model.config.serpDescriptionLength : 150;
    $scope.developerName = $scope.model.config.developerName || 'your agency';
    $scope.doNotIndexExplanation = $scope.model.config.doNotIndexExplanation || '';

    // default model.value
    if (!$scope.model.value) {
      $scope.model.value = { title: '', description: '', urlName: '', noIndex: false };
    }

    $scope.GetUrl = function() {

      var urlName = $scope.model.value.urlName && $scope.model.value.urlName.length ? '/' + $scope.model.value.urlName + '/' : $scope.GetParentContent().urls[0];

      if (urlName === '' || urlName === 'This item is not published') {
        urlName = '/unpublished-page/';
      }

      return $scope.ProtocolAndHost() + urlName;

    };

    $scope.ProtocolAndHost = function() {

      var http = location.protocol;
      var slashes = http.concat("//");
      return slashes.concat(window.location.hostname);

    };

    $scope.GetParentContent = function() {
      var currentScope = $scope.$parent;

      for (var i = 0; i < 150; i++) {
        if (currentScope.content) {
          return currentScope.content;
        }

        currentScope = currentScope.$parent;
      }

      return null;
    };


    $(window).resize(function() {
      $scope.$apply(function() {
        if (window.innerWidth <= 1500 && !$scope.hideSerp) {
        	$scope.hideSerp = true;
        }
        if (window.innerWidth > 1500) {
        	$scope.hideSerp = false;
        }
      });
    });
  }
]);
