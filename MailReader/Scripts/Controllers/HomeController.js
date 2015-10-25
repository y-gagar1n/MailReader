var HomeController = function ($scope, $http, $location) {
	$http.get("/api/mailboxes").then(function(response) {
		$scope.mailboxes = response.data;
	});

	$scope.openMailbox = function(mailbox) {
		$location.path("/").search({mailbox: mailbox});
	}
};

HomeController.inject = ['$scope', '$http', '$location'];