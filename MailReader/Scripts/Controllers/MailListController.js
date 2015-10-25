var MailListController = function ($scope, $http, $location) {

	function load(take, skip) {
		$http.get("/api/mail" + "?take=" + take + "&skip=" + skip).then(function (response) {
			$scope.mails = response.data.Messages;
			$scope.take = response.data.Take;
			$scope.skip = response.data.Skip;
		});
	}

	load(20, 0);

	$scope.openMailDetails = function(id) {
		$location.path('/mail/' + id);
	};

	$scope.openPreviousPage = function() {
		load($scope.take, $scope.skip - $scope.take);
	}

	$scope.openNextPage = function () {
		load($scope.take, $scope.skip + $scope.take);
	}
};

MailListController.inject = ['$scope', '$http', '$location'];