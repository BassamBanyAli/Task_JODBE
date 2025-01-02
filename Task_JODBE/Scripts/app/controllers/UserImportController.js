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