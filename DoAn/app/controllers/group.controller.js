(function (app) {
    app.controller('groupCtrl', function groupCtrl($scope, DTOptionsBuilder, apiService, DTColumnDefBuilder, DTColumnBuilder, $ngBootbox, notificationService) {
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
            ]);
        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(0).notSortable(),
            DTColumnDefBuilder.newColumnDef(1),
            DTColumnDefBuilder.newColumnDef(2),
            

        ];
        $('select').chosen({ width: '100%' });
        $scope.groups = [];
        $scope.deleteGroup = deleteGroup;
        $scope.initData = () => {          
            $scope.getAll();
            $scope.getRoles();
        }
        function deleteGroup(Id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            id: Id
                        }
                    }
                    apiService.del('/api/applicationGroup/delete', config, function () {
                        notificationService.displaySuccess('Đã xóa thành công.');
                        
                    },
                        function () {
                            notificationService.displayError('Xóa không thành công.');
                        });
                });
        }
       
        $scope.getAll = () => {
            apiService.get('/api/applicationGroup/getall', null, function (result) {
                $scope.groups = result.data;
            }, function () {
                console.log('Load groups failed.');
            });
        }
        $scope.getRoles = () => {
            apiService.get('/api/applicationRole/getall', null, function (result) {
                $scope.roles = result.data;
            }, function () {
                console.log('Load roles failed.');
            });
        }
        //Add/Update Group
        $scope.isEditGroup = true;
        $scope.group = {
        };
        $scope.enableAddGroup = function () {
            $scope.isEditGroup = false;
            $('#form-group').modal('show');
            $scope.group = {
               
            };
        }
        $scope.enableEditGroup = function (group) {
            $scope.isEditGroup = true;
            $('#form-group').modal('show');
            $scope.group = angular.copy(group);
        }
        $scope.updateGroup= () => {
            apiService.post('/api/applicationGroup/update', $scope.group,
                function (result) {
                    group = result.data;
                    if ($scope.isEditGroup) {
                        for (var i = 0; i < $scope.groups.length; i++) {
                            if ($scope.groups[i].Id == group.Id) {
                                $scope.groups[i] = group;
                                notificationService.displaySuccess('Đã cập nhật nhóm người dùng');
                                break;
                            }
                        }
                    } else {
                        $scope.groups.push(group);
                    }
                    $scope.closeEditForm();
                }, function (error) {
                    notificationService.displayError(error.data.Message);
                    $scope.closeEditForm();
                });
        }
        $scope.closeEditForm = () => {
            $('#form-group').modal('hide');
        }

    });
})(angular.module('app'));