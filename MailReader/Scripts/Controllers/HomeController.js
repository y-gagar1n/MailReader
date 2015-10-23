var HomeController = function ($scope, $http) {
	$http.get("/api/mails").then(function(response) {
		$scope.mails = response.data;
	});
};

HomeController.inject = ['$scope', '$http'];