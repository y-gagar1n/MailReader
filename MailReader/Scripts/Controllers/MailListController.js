var MailListController = function ($scope, $http, $location) {
	$http.get("/api/mail").then(function (response) {
		$scope.mails = response.data;
	});

	$scope.openMailDetails = function(id) {
		$location.path('/mail/' + id);
	};
};

MailListController.inject = ['$scope', '$http', '$location'];