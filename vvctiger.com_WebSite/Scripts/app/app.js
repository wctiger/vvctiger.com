var app = angular.module('siteApp', ['ngRoute', 'ngAnimate', 'ui.bootstrap', 'siteControllers']);

var siteCtrls = angular.module('siteControllers', []);

app.config(['$routeProvider',
    function($routeProvider){
        $routeProvider.when('/ghostgame', {
            templateUrl: '/StaticViews/Ghost/Game.html',
            controller: 'ghostController'
        }).
        otherwise({
            redirectTo: 'Index.html'
        });
    }]); 