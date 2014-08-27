angular.element(document).ready(function () {
    showData('allDataDiv');
});

function showData(visibleElem) {
    count = 0;
    ['allDataDiv', 'bigCoverDiv', 'bigAdDiv', 'totalPageCoverageDiv'].forEach(function (elem) {
        e = angular.element(document.querySelector('#' + elem));
        if (elem == visibleElem) {
            e.addClass('visible');
            e.removeClass('invisible');
        } else {
            e.addClass('invisible');
            e.removeClass('visible');
        };
    });
}

// Angular module and controllers.
var adApp = angular.module('adApp', []);

adApp.controller('AllDataController', function ($scope, $http) {
    var _scope = $scope;
    var _http = $http;
    var _sortBy = 'brandname';
    var _skip = 0;
    var _count = 0;

    _http.get('/adData/201101-201104').success(
        function (data, status, headers, config) {
            _count = data.count;
            _scope.data = data;
            _scope.navInfo = "Records " + (_skip + 1) + " through " + (_skip + 20) + " of " + _count + ".";
        });

    $scope.sortBy = function (field) {
        _sortBy = field;
        _skip = 0; // reset skip count on resort.
        _http.get('/adData/201101-201104/?sortBy=' + _sortBy + '&skipCount=' + _skip).success(
            function (data, status, headers, config) {
                _scope.data = data;
            });
        _scope.navInfo = "Records " + (_skip + 1) + " through " + (_skip + 20) + " of " + _count + ".";
        return false;
    };

    $scope.skip = function (count) {
        _skip = count; // do not reset sort field.
        _http.get('/adData/201101-201104/?sortBy=' + _sortBy + '&skipCount=' + _skip).success(
            function (data, status, headers, config) {
                _scope.data = data;
            });
        _scope.navInfo = "Records " + (_skip + 1) + " through " + (_skip + 20) + " of " + _count + ".";
        return false;
    };

});

adApp.controller('BigCoverController', function ($scope, $http) {
    var _scope = $scope;
    var _http = $http;
    var _sortBy = 'brandname';
    var _skip = 0;
    var _count = 0;

    _http.get('/adData/BigCovers/201101-201104').success(
        function (data, status, headers, config) {
            _count = data.count;
            _scope.data = data;
            _scope.navInfo = "Records " + (_skip + 1) + " through " + (_skip + 20) + " of " + _count + ".";
        });

    $scope.sortBy = function (field) {
        _sortBy = field;
        _skip = 0; // reset skip count on resort.
        _http.get('/adData/BigCovers/201101-201104/?sortBy=' + _sortBy + '&skipCount=' + _skip).success(
            function (data, status, headers, config) {
                _scope.data = data;
            });
        _scope.navInfo = "Records " + (_skip + 1) + " through " + (_skip + 20) + " of " + _count + ".";
        return false;
    };

    $scope.skip = function (count) {
        _skip = count; // do not reset sort field.
        _http.get('/adData/BigCovers/201101-201104/?sortBy=' + _sortBy + '&skipCount=' + _skip).success(
            function (data, status, headers, config) {
                _scope.data = data;
            });
        _scope.navInfo = "Records " + (_skip + 1) + " through " + (_skip + 20) + " of " + _count + ".";
        return false;
    };

});

adApp.controller('BigAdController', function ($scope, $http) {
    $http.get('/adData/BigAds/201101-201104').success(
        function (data, status, headers, config) {
            $scope.data = data;
        });
});

adApp.controller('TotalPageCoverageController', function ($scope, $http) {
    $http.get('/adData/TotalPageCoverage/201101-201104').success(
        function (data, status, headers, config) {
            $scope.data = data;
        });
});

adApp.controller('FiltersController', function ($scope) {
    $scope.filters = [
        { text: 'All', element: 'allDataTable' },
        { text: 'Big Covers', element: 'bigCoverTable' },
        { text: 'Largest Ads by Brand', element: 'bigAdTable' },
        { text: 'Brands by Total Pages', element: 'totalPageCoverageTable' }
    ];
});

