(function (app) {
    app.controller('districtCtrl', function districtCtrl($scope, DTOptionsBuilder, apiService, DTColumnDefBuilder, DTColumnBuilder, notificationService) {
        //Get List District
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
            DTColumnDefBuilder.newColumnDef(5),
        ];
        $('select').chosen({ width: '100%' });
        $scope.districts = [];
        $scope.initData = function () {
            $scope.getProvinces();
            $scope.getAll();
        }
        $scope.getProvinces = () => {
            apiService.get('/api/province/getall', null, function (result) {
                $scope.provinces = result.data;

            }, function () {
                console.log('Load provinces failed.');
            });
        }
        $scope.getAll = function () {
            apiService.get('/api/district/getall', null, function (result) {
                $scope.districts = result.data;

            }, function () {
                console.log('Load districts failed.');
            });
        }
        //Add/Update District
        $scope.isEditDistrict = true;
        $scope.district = {
        };
        $scope.enableAddDistrict = function () {
            $scope.isEditDistrict = false;
            $('#form-district').modal('show');
            $scope.district = {
                isActive: true
            };
        }
        $scope.enableEditDistrict = function (district) {
            $scope.isEditDistrict = true;
            $('#form-district').modal('show');
            $scope.district = angular.copy(district);
        }
        $scope.updateDistrict = function () {
            apiService.post('/api/district/update', $scope.district,
                function (result) {
                    district = result.data;
                    if ($scope.isEditDistrict) {
                        for (var i = 0; i < $scope.districts.length; i++) {
                            if ($scope.districts[i].Id == district.Id) {
                                $scope.districts[i] = district;
                                notificationService.displaySuccess('Đã cập nhật quận huyện');
                                break;
                            }
                        }
                    } else {
                        $scope.districts.push(district);
                    }
                    $scope.closeEditForm();
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                    $scope.closeEditForm();
                });
        }
        $scope.closeEditForm = function () {
            $('#form-district').modal('hide');
        }

    });
})(angular.module('app'));