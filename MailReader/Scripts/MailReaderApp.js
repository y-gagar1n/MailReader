var app = angular.module("MailReaderApp", ["ngRoute"]);
app.controller("MailListController", MailListController);
app.controller("MailDetailsController", MailDetailsController);
app.controller("HomeController", HomeController);

var configFunction = function($routeProvider) {
	$routeProvider.when('/', {
		templateUrl: "/Home/MailList",
		controller: 'MailListController'
	})
	.when('/mail/:mailId', {
		templateUrl: "/Home/MailDetails",
		controller: 'MailDetailsController'
	});;
};
configFunction.injector = ["$routeProvider"];

app.config(configFunction);