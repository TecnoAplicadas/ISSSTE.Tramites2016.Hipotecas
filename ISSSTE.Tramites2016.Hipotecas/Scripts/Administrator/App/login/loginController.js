(function () {
    "use strict";

    var controllerId = "loginController";
    angular
        .module(appName)
        .controller(controllerId, ["$window", "common", "authenticationService", loginController]);


    function loginController($window, common, authenticationService) {
        
        console.log(new Error().stack);
        //#region Controller Members

        var vm = this;

        vm.continueToLogin = continueToLogin;
        vm.login = login;
        vm.logout = logout;

        //#endregion

        //#region Functions


        function continueToLogin(returnUrl) {
            $window.location.replace("externallogin?returnUrl=" + encodeURIComponent(returnUrl));
        }

        function login(userName, returnUrl, clientId) {
            debugger 
            authenticationService.login(clientId, userName)
                .then(function(response) {
                    $window.location.replace(returnUrl);
                }).catch(function(reason) {
                    $window.location.replace("loginerror");
                });
        }

        function logout(logoutUrl, returnUrl, soft) {
            authenticationService.clearToken();

            if (soft) {
                $window.setTimeout(function() {
                    $window.location.replace(returnUrl);
                }, 2000);
            } else {
                $window.setTimeout(function() {
                    $window.location.replace(logoutUrl + "?returnUrl=" + encodeURIComponent(returnUrl));
                }, 2000);
            }
        }

//#endregion
    };
})();