(function (app) {
    app.controller('userCtrl', function userCtrl($scope, DTOptionsBuilder, $location, apiService, $ngBootbox, DTColumnDefBuilder, DTColumnBuilder, notificationService) {
        //Get List User
        $scope.dtOptions = DTOptionsBuilder.newOptions()
            .withDOM('<"html5buttons"B>lTfgitp')
            .withButtons([
                { extend: 'copy' },
                { extend: 'excel' },
                { extend: 'pdf' },
                {
                    extend: 'print',
                    customize: function (win) {
                        $(win.document.body).addClass('white-bg');
                        $(win.document.body).css('font-size', '10px');

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', 'inherit');
                    }
                }
            ])
            ;
        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0),
            DTColumnDefBuilder.newColumnDef(1),
            DTColumnDefBuilder.newColumnDef(2),
            DTColumnDefBuilder.newColumnDef(3),
            DTColumnDefBuilder.newColumnDef(4),
        ];
        $scope.users = [];
        $scope.user = {}


        $scope.updateAccount = updateAccount;

        function updateAccount() {
            apiService.put('/api/user/update', $scope.account, addSuccessed, addFailed);
        }
        function addSuccessed() {
            notificationService.displaySuccess($scope.account.FullName + ' đã được cập nhật thành công.');

            $location.url('/user');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }
        $scope.initData = function () {
            $scope.getAll();
        }
        $scope.getAll = function () {
            apiService.get('/api/user/getall', null, function (result) {
                $scope.users = result.data;
            }, function () {
                console.log('Load users failed.');
            });
        }
        //Add/Update User
        $scope.isEditUser = true;
        $scope.user = {};
        $scope.enableAddUser = function () {
            $scope.isEditUser = false;
            $('#form-user').modal('show');
            $scope.user = {
                Avatar: '~/Content/admin/img/images_none.png',
                LockoutEnabled: false,
                Gender : true
            };
        }
        $scope.enableEditUser = function (user) {
            $scope.isEditUser = true;
            $('#form-user').modal('show');
            $scope.user = angular.copy(user);
        }
        function loadGroups() {
            apiService.get('/api/applicationGroup/getall',
                null,
                function (response) {
                    $scope.groups = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhóm.');
                });

        }
        loadGroups();
        //update avatar
        $scope.uploadFile = function (e) {
            let reader = new FileReader();
            let file = e.target.files[0];
            var size = file.size / 1024 / 1024;
            if (size > 1.5) {
                notificationService.displayError("Kích thước ảnh avatar không thể vượt quá 1.5MB");
                return;
            }
            reader.readAsDataURL(file);
            reader.addEventListener("load", function (e) {
                e.preventDefault();
                $scope.$apply(function () {
                    $scope.user.Avatar = e.target.result;
                });

            });
        }
      $scope.deleteUser=  function (id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            id: id
                        }
                    }
                    apiService.del('/api/user/delete', config, function () {
                        notificationService.displaySuccess('Đã xóa thành công.');
                        search();
                    },
                        function () {
                            notificationService.displayError('Xóa không thành công.');
                        });
                });
        }
        //update user
        $scope.updateUser = () => {
            apiService.post('/api/user/update', $scope.user,
                function (result) {
                    user = result.data;
                    if ($scope.isEditUser) {
                        for (var i = 0; i < $scope.users.length; i++) {
                            if ($scope.users[i].Id == user.Id) {
                                $scope.users[i] = user;
                                notificationService.displaySuccess('Đã cập nhật người dùng');
                                break;
                            }
                        }
                    } else {
                        $scope.users.push(user);
                    }
                    $scope.closeEditForm();
                }, function (error) {
                    notificationService.displayError(error.data.Message);
                    $scope.closeEditForm();
                });
        }
        $scope.closeEditForm = function () {
            $('#form-user').modal('hide');
        }
    });
})(angular.module('app'));