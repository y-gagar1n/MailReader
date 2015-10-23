var app = angular.module("MailReaderApp", ["ngRoute"]);
app.controller("MailController", MailController);
app.controller("HomeController", HomeController);

var configFunction = function($routeProvider) {
	$routeProvider.when('/', {
		templateUrl: "/Home/MailList",
		controller: 'MailController'
	});
};
configFunction.injector = ["$routeProvider"];

app.config(configFunction);