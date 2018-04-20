(function () {
    'use strict';

    angular
        .module(appName)
        .directive('fileUpload', ['common', function (common) {
            return {
                restrict: 'E',
                templateUrl: common.getBaseUrl() + '/Scripts/Administrator/App/fileUpload/fileUpload.html',
                scope: {
                    nfuDocument: "=",
                    nfuName: "@",
                    //nfuAccepts: "@"
                    nfuAccepts: "image/gif, image/jpeg, application/pdf, application/zip"
                },
                controller: ['$scope', function ($scope) {
                    var vm = this;
                    vm.nfuDisplayImageInDialog = nfuDisplayImageInDialog;

                    var FILE_UNIQUE_URL_TEMPLATE = "{0}?v={1}";

                    function nfuDisplayImageInDialog() {
                        var guid = Guid.newGuid();
                        common.displayImageDialog(FILE_UNIQUE_URL_TEMPLATE.format(vm.nfuDocument.DownloadUrl, guid), vm.nfuName);
                    }
                }],
                controllerAs: 'vm',
                bindToController: true,
                link: function (scope, elem, attr) {
                    elem.find(".browserfile").each(function (index) {
                        var filebtnWidth = $(this).parent().width();
                        filebtnWidth = parseInt(filebtnWidth) + 50 + "px";
                        $(this).css("width", filebtnWidth);
                    });

                    elem.find('.dotInfo').tooltip();
                }
            }
        }]);
})();