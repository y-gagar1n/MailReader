var MailDetailsController = function ($scope, $http, $routeParams, $sce, $location) {
	var id = $routeParams.mailId;
	$http.get("/api/mail/" + id).then(function (response) {
		$scope.mail = response.data;
	});

	$scope.to_trusted = function(htmlCode) {
		return $sce.trustAsHtml(htmlCode);
	}

	$scope.deleteMail = function(id) {
		$http.delete("/api/mail/" + id).then(function(response) {
			$location.path("/");
		});
	}
};

MailDetailsController.inject = ['$scope', '$http', '$routeParams', '$sce', '$location'];