var MailDetailsController = function ($scope, $http, $routeParams, $sce) {
	var id = $routeParams.mailId;
	$http.get("/api/mail/" + id).then(function (response) {
		$scope.mail = response.data;
	});

	$scope.to_trusted = function(htmlCode) {
		return $sce.trustAsHtml(htmlCode);
	}
};

MailDetailsController.inject = ['$scope', '$http', '$routeParams', '$sce'];