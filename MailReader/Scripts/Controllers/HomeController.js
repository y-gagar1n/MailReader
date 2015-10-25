var HomeController = function ($scope, $http) {
	$http.get("/api/mailboxes").then(function(response) {
		$scope.mailboxes = response.data;
	});
};

HomeController.inject = ['$scope', '$http'];