(function (app) {
    app.controller('provinceCtrl', function provinceCtrl($scope, DTOptionsBuilder, apiService, DTColumnDefBuilder, DTColumnBuilder, notificationService) {
        //Get List Province
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
            DTColumnDefBuilder.newColumnDef(3),
            DTColumnDefBuilder.newColumnDef(4),
        ];
        $scope.provinces = [];
        $scope.initData = function () {
            $scope.getAll();
        }
        $scope.getAll = function () {
            apiService.get('/api/province/getall', null, function (result) {
                $scope.provinces = result.data;
               
            }, function () {
                console.log('Load provinces failed.');
            });
        }
        //Add/Update Province
        $scope.isEditProvince = true;
        $scope.province = {
        };
        $scope.enableAddProvince = function () {
            $scope.isEditProvince = false;
            $('#form-province').modal('show');
            $scope.province = {
                isActive: true
            };
        }
        $scope.enableEditProvince = function (province) {
            $scope.isEditProvince = true;
            $('#form-province').modal('show');
            $scope.province = angular.copy(province);
        }
        $scope.updateProvince = function () {
            apiService.post('/api/province/update', $scope.province,
                function (result) {
                    province = result.data;
                    
                    if ($scope.isEditProvince) {
                        for (var i = 0; i < $scope.provinces.length; i++) {
                            if ($scope.provinces[i].Id == province.Id) {
                                $scope.provinces[i] = province;
                                notificationService.displaySuccess('Đã cập nhật tỉnh thành');
                                break;
                            }
                        }
                    } else {
                        $scope.provinces.push(province);
                    }
                    $scope.closeEditForm();
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                    $scope.closeEditForm();
                });
        }
        $scope.closeEditForm = function () {
            $('#form-province').modal('hide');
        }
        
    });
})(angular.module('app'));