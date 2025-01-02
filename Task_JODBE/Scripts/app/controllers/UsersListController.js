
app.controller('UsersListController', ['$scope', '$http', function ($scope, $http) {
    $scope.users = [];

    // Fetch users from server
    $http.get('/UsersList/GetUsers').then(function (response) {
        // Apply fallback for Photo directly in the map
        $scope.users = response.data.map(function (user) {
            // Apply fallback to user.Photo if it's null or empty
            user.Photo = user.Photo || '/Content/images/default.jpg';
            return user;
        });
    }, function (error) {
        console.error('Error fetching users:', error);
    });

    $scope.editUser = function (user) {
        alert('Edit user: ' + user.Name);
    };
}]);
