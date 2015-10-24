var MailController = function ($scope, $http) {
	$http.get("/api/mail").then(function (response) {
		$scope.mails = response.data;
	});

	$scope.openMail = function(id) {
		alert(id);
	};
};

MailController.inject = ['$scope', '$http'];