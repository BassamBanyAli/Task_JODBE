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
                $window.location.href = '/Home/Index'; // This will work if you're not using Angular routing.
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



app.controller('AddUserController', ['$scope', '$http', function ($scope, $http) {
    // Initialize the user object to bind form fields
    $scope.user = {
        name: '',
        email: '',
        mobileNumber: '',
        password: ''
    };

    // Function to handle form submission
    $scope.submitForm = function () {
        if ($scope.user.name && $scope.user.email && $scope.user.mobileNumber && $scope.user.password) {
            var formData = new FormData();

            // Append form fields to FormData
            formData.append("name", $scope.user.name);
            formData.append("email", $scope.user.email);
            formData.append("mobileNumber", $scope.user.mobileNumber);
            formData.append("password", $scope.user.password);

            // Append the file to FormData if selected
            var fileInput = document.getElementById("userImage");
            if (fileInput.files.length > 0) {
                formData.append("userImage", fileInput.files[0]);
            }

            // Send the form data (including image) to the server
            $http.post('/UsersList/AddUser', formData, {
                headers: { 'Content-Type': undefined }  // Important to specify 'undefined' so that the browser sets the correct content-type
            })
                .then(function (response) {
                    alert('User added successfully!');
                }, function (error) {
                    alert('Error adding user!');
                });
        } else {
            alert('All fields are required!');
        }
    };
}]);








app.controller('UserImportController', ['$scope', function ($scope) {
    $scope.uploadExcel = function () {
        if (!$scope.file) {
            alert("Please select a file to upload.");
            return;
        }

        // File size validation (for example, 100 MB limit)
        var maxFileSize = 100 * 1024 * 1024; // 100 MB
        if ($scope.file.size > maxFileSize) {
            alert("File is too large. Maximum allowed size is 100 MB.");
            return;
        }

        var formData = new FormData();
        formData.append('file', $scope.file);
        console.time('File Upload Execution Time'); // Start timing

        $.ajax({
            url: '/UsersList/ImportExcel',
            type: 'POST',
            data: formData,
            contentType: false, // Let the browser set the content-type
            processData: false, // Don't process the data into a query string
            beforeSend: function () {
                console.log('AJAX request started...');
            },
            success: function (response) {
                console.timeEnd('File Upload Execution Time');
                console.log('Response received:', response);
                $scope.$apply(function () {
                    $scope.message = response.message;
                });
            },
            error: function (error) {
                console.timeEnd('File Upload Execution Time');
                console.error('Error during upload:', error.responseText || error);
                $scope.$apply(function () {
                    $scope.message = "Error uploading file: " + (error.responseText || 'Unknown error');
                });
            }
        });
    };
}]);
;

