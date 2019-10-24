(function (app) {
    app.controller('deviceCtrl', function deviceCtrl($scope, DTOptionsBuilder, apiService, $ngBootbox, DTColumnDefBuilder, DTColumnBuilder, notificationService) {
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
            DTColumnDefBuilder.newColumnDef(6),
            DTColumnDefBuilder.newColumnDef(7),

        ];
        $('select').chosen({ width: '100%' });
        $scope.devices = [];
        $scope.deleteDevice = deleteDevice;
        $scope.initData = () =>
        {
            $scope.getProvinces();
            $scope.getAll();
        }
        $scope.getProvinces = () =>
        {
            apiService.get('/api/province/getall', null, function (result) {
                $scope.provinces = result.data;

            }, function () {
                console.log('Load provinces failed.');
            });
        }
        function deleteDevice(Id) {
            $ngBootbox.confirm('Bạn có chắc muốn xóa?')
                .then(function () {
                    var config = {
                        params: {
                            id: Id
                        }
                    }
                    apiService.del('/api/device/delete', config, function () {
                        notificationService.displaySuccess('Đã xóa thành công.');

                    },
                        function () {
                            notificationService.displayError('Xóa không thành công.');
                        });
                });
        }
        $scope.getDistrictByProvineId = (id) =>
        {
            if (id != null)
            {
                id = parseInt(id);
                apiService.get('/api/district/getbyprovinceid?id=' + id, null, function (result) {
                    $scope.districts = result.data;
                  
                   
                }, function () {
                    console.log('Load districts failed.');
                });
            }
        }
       
        $scope.getAll = () => {
            apiService.get('/api/device/getall', null, function (result) {
                $scope.devices = result.data;

            }, function () {
                console.log('Load devices failed.');
            });
        }
        //Add/Update Device
        $scope.isEditDevice = true;
        $scope.device = {
        };
        $scope.enableAddDevice = function () {
            $scope.isEditDevice = false;
            $('#form-device').modal('show');
            $scope.device = {
                isActive: true,
            };
        }
        $scope.enableEditDevice = function (device) {
            $scope.isEditDevice = true;
            $('#form-device').modal('show');
            $scope.device = angular.copy(device);
            if ($scope.device.DistrictId != null) {
                apiService.get('api/province/getprovincebydistrictid/' + $scope.device.DistrictId, null, function (result)
                {
                    $scope.device.DistrictId = result.data.Id;
                //    $scope.device.PhuongXaId = result.data.PhuongXaId;
                //    $scope.getLakeByHoDanId($scope.device.HoDanId);
                 //   $scope.getHodanByCommuneId($scope.device.PhuongXaId);
                    apiService.get('api/district/getdistrictbydeviceid/' + $scope.device.DeviceId, null, function (result)
                    {
                        $scope.device.DistrictId = result.data.Id;
                     
                        $scope.device.ProvinceId = result.data.ProvinceId;

                    }, function ()
                        {
                        console.log("error")
                    })
                }, function ()
                    {
                    console.log("error")
                })

            }

        }
        $scope.updateDevice = () => {
            apiService.post('/api/device/update', $scope.device,
                function (result) {
                    device = result.data;
                    if ($scope.isEditDevice) {
                        for (var i = 0; i < $scope.devices.length; i++) {
                            if ($scope.devices[i].Id == device.Id) {
                                $scope.devices[i] = device;
                                notificationService.displaySuccess('Đã cập nhật thiết bị');
                                break;
                            }
                        }
                    } else {
                        $scope.devices.push(device);
                    }
                    $scope.closeEditForm();
                }, function (error) {
                    console.log(error)
                    notificationService.displayError(error.data.Message);
                    $scope.closeEditForm();
                });
        }
        $scope.closeEditForm = () => {
            $('#form-device').modal('hide');
        }

    });
})(angular.module('app'));