angular.module('main')
    .controller('InviteController', ['$scope', 'WebApiServce', 'FileUploader', function ($scope, WebApiServce, FileUploader) {

        $scope.ImageData = "";
        $scope.ImageType = "";
        $scope.Email = "";
        var GetInfo = function () {
            WebApiServce.GetWeddingSetting()
                .success(function (data) {
                    $scope.ImageData = data.InviteConentBody;
                    $scope.ImageType = data.InviteContentType;
                    $scope.Email = data.MailAddress;
                })
                .error(function () { });
        }

        $scope.SaveChanges = function ()
        {
            WebApiServce.SaveWeddingSetting($scope.Email)
                         .success(function (data) {
                             $scope.ImageData = data.InviteConentBody;
                             $scope.ImageType = data.InviteContentType;
                             $scope.Email = data.MailAddress;
                         })
                         .error(function () { });
        }
       
        var uploader = $scope.uploader = new FileUploader({
            url: '/api/FileUpload/Upload',
            headers: { "Accept": "application/json" },
            queueLimit: 1,
            removeAfterUpload: true
        });

        uploader.filters.push({
            name: 'imageFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            }
        });


        uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
          //  console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader.onAfterAddingFile = function (fileItem) {
           // console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
          //  console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
         //   console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
          //  console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
          //  console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (fileItem, response, status, headers) {
            $scope.ImageData = response.InviteConentBody;
            $scope.ImageType = response.InviteContentType;
            console.log(response);
            //console.info('onSuccessItem', fileItem, response, status, headers);
        };
        uploader.onErrorItem = function (fileItem, response, status, headers) {
           // console.info('onErrorItem', fileItem, response, status, headers);
        };
        uploader.onCancelItem = function (fileItem, response, status, headers) {
           // console.info('onCancelItem', fileItem, response, status, headers);
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
          //  console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader.onCompleteAll = function () {
           // console.info('onCompleteAll');
        };

        console.info('uploader', uploader);
        GetInfo();
    }]);