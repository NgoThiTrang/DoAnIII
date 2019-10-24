(function (app) {
    app.controller('roleCtrl', function roleCtrl($scope, DTOptionsBuilder, $ngBootbox, apiService, DTColumnDefBuilder, DTColumnBuilder, notificationService) {
        //Get List Role
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
        $scope.roles = [];
      
        $scope.initData = function () {
            $scope.getAll();
        }
        $scope.getAll = () => {
            apiService.get('/api/applicationRole/getall', null, function (result) {
                $scope.roles = result.data;
            }, function () {
                console.log('Load roles failed.');
            });
        }
        //Add Role
        $scope.role = {};
        $scope.enableAddRole = function ()
        {
            $('#form-role').modal('show');
            $scope.role = {};
        }
        $scope.deleteRole=   function (Id)
        {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            id:Id
                        }
                    }
                    apiService.del('/api/applicationRole/delete', config, function () {
                        notificationService.displaySuccess('Đã xóa thành công.');
                        
                    },
                        function () {
                            notificationService.displayError('Xóa không thành công.');
                        });
                });
        }
        $scope.addRole = () => {
            apiService.post('/api/applicationRole/update', $scope.role,
                function (result) {
                    role = result.data;
                    $scope.roles.push(role);
                    $scope.closeAddForm();
                    notificationService.displaySuccess("Đã cập nhật quyền")
                }, function (error) {
                    notificationService.displayError(error.data.Message);
                    $scope.closeAddForm();
                });
        }      
        $scope.closeAddForm = function () {
            $('#form-role').modal('hide');
        }
    });
})(angular.module('app'));