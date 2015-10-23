var MailController = function ($scope, $http) {
	$http.get("/api/mail").then(function (response) {
		$scope.mails = response.data;
	});
};

MailController.inject = ['$scope', '$http'];