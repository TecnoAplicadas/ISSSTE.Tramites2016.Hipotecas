(function () {
    'use strict';

    angular
        .module(appName)
        .directive('fileDisplay', ['common', function (common) {
            return {
                restrict: 'E',
                templateUrl: common.getBaseUrl() + '/Scripts/Administrator/App/fileDisplay/fileDisplay.html',
                scope: {
                    nfuDocument: "=",
                    nfuIsReadOnly: "=",
                    nfuName: "@"
                },
                controller: ['$scope', function ($scope) {
                    var vm = this;
                    vm.isDocumentValid = false;

                    vm.setDocumentApprovedState = setDocumentApprovedState;
                    vm.nfuDisplayImageInDialog = nfuDisplayImageInDialog;
                    vm.setDocumentNullState = setDocumentNullState;

                    var FILE_UNIQUE_URL_TEMPLATE = "{0}?v={1}";

                    $scope.$watch("vm.nfuDocument", function () {
                        if (vm.nfuDocument) {
                            vm.isDocumentValid = vm.nfuDocument.IsValid != null;
                        }
                    });

                    function setDocumentApprovedState(value) {
                        vm.nfuDocument.IsValid = value;
                    }

                    function setDocumentNullState() {
                        vm.nfuDocument.IsValid = null;
                    }

                    function nfuDisplayImageInDialog() {
                        var guid = Guid.newGuid();
                        common.displayImageDialog(FILE_UNIQUE_URL_TEMPLATE.format(vm.nfuDocument.DownloadUrl, guid), vm.nfuName);
                    }
                }],
                controllerAs: 'vm',
                bindToController: true,
                link: function (scope, elem, attr) {
                    elem.find('.dotInfo').tooltip();
                }
            }
        }]);
})();