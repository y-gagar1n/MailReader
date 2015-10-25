var MailListController = function ($scope, $http, $location, $routeParams) {

	function load(take, skip, mailbox) {
		$http.get("/api/mail" + "?mailbox=" + mailbox + "&take=" + take + "&skip=" + skip).then(function (response) {
			$scope.mails = response.data.Messages;
			$scope.take = response.data.Take;
			$scope.skip = response.data.Skip;
		});
	}

	var mailbox = $routeParams.mailbox || "INBOX";
	load(20, 0, mailbox);

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

MailListController.inject = ['$scope', '$http', '$location', '$routeParams'];