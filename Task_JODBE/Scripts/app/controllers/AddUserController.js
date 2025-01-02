



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

