app.controller('LoginController', function ($scope, $http, $window) {
    $scope.adminUser = {
        Email: '',
        password: ''
    };
    $scope.isLoggedIn = false;
    $scope.errorMessage = '';

    // Function to handle the form submission
    $scope.login = function () {
        const data = {
            Email: $scope.adminUser.Email,
            password: $scope.adminUser.password
        };

        const csrfToken = document.querySelector('meta[name="csrf-token"]').getAttribute('content');

        $http.post('/Login/Index', data, {
            headers: {
                'RequestVerificationToken': csrfToken
            }
        }).then(function (response) {
            if (response.data.isLoggedIn) {
                $scope.isLoggedIn = true;
                $window.location.href = '/UsersList/Index'; // This will work if you're not using Angular routing.
                // If you're using AngularJS routing:
                // $location.path('/home'); // Example if you're using ngRoute or ui-router
            } else {
                $scope.errorMessage = response.data.message || 'Invalid email or password.';
            }
        }, function (error) {
            console.error("Error:", error);
            $scope.errorMessage = 'An error occurred while logging in.';
        });
    };
    ;

});
