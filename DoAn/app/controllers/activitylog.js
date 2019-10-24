(function (app)
{
    app.controller('acivitylog', acivitylog);
    acivitylog.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter']

    function acivitylog($scope, apiService, notificationService, $ngBootbox, $filter)
    {
        $scope.activity = [];
        $scope.page = 0;
        $scope.pagescount = 0;
        $scope.totalCount = 0;
        $scope.keyword = '';

        $scope.getactivity = getactivity;
        function getactivity(page)
        {

            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 4
                }
            }
            apiService.get('/api/activity/getpaging', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('khong tim thay ban ghi nao ');
                }
                else {
                    notificationService.displaySuccess('tim thay ' + result.data.TotalCount + ' ban ghi');
                }
                console.log(result);
                $scope.activity = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            },
                function () {
                    console.log('load product fail');
                }
            );

        }
        $scope.getactivity();
    }
    
})(angular.module('app'));
