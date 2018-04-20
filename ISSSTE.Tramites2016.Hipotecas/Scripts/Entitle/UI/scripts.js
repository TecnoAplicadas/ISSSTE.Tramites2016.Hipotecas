var ALERT_TEMPLATE = '<div class="alert alert-{0} {1}" role="alert"><button type="button" class="close" aria-label="Close"><span aria-hidden="true">&times;</span></button><span>{2}</span></div>';
var ALERT_TEMPLATE_WITH_TITLE = '<div class="alert alert-{0} {1}" role="alert"><button type="button" class="close" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>{2}</strong><br/><span>{3}</span></div>';
var ALERT_ID_TEMPLATE = 'alert-{0}';

var UI =
{
    getBaseUrl: function () {
        return $("#baseUrl").val()
    },
    initTabs: function () {
        $('.nav-tabs a').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        });
    },
    initToolTips: function () {
        //$('.brWizard-dot').tooltip();
        $('[data-toggle="tooltip"]').tooltip()
    },
    initPrintButtons: function () {
        $("#printButton").on("click", function () {

            var tabsInfo = [];
            var printContent = "";

            $(".nav.nav-tabs li a").map(function () {
                var name = $(this).text();

                var contentId = $(this).attr("href");
                var content = $(contentId).html();

                tabsInfo.push({ name: name, content: content });
            }).get();

            printContent += Constants.aboutPrintView.tabContentTemplate.format($("title").text());

            tabsInfo.forEach(function (actualTab) {
                printContent += Constants.aboutPrintView.tabContentTemplate.format(actualTab.name);
                printContent += actualTab.content;
                printContent += Constants.aboutPrintView.tabSeparator;
            });

            var params = [
                'height=' + screen.height,
                'width=' + screen.width,
                'fullscreen=yes' // only works in IE, but here for completeness
            ].join(',');

            var winPrint = window.open('', '', params);
            winPrint.document.write(Constants.aboutPrintView.pageTemplate.format(printContent));

            window.managedPrint(winPrint);
        })
    },
    initNotifications: function () {

        var element = $('#theFixed');





    },
    createInfoMessage: function (message, title) {
        UI.createMessage('info', message, title, false)
    },
    createWarningMessage: function (message, title) {
        UI.createMessage('warning', message, title, false)
    },
    createSuccessMessage: function (message, title) {
        UI.createMessage('success', message, title, true)
    },
    createErrorMessage: function (message, title) {
        UI.createMessage('danger', message, title, true)
    },
    createMessage: function (alertClass, message, title, fadeOut) {
        var alertsContainer = $(".alerts");

        if (alertsContainer) {

            var alertId = ALERT_ID_TEMPLATE.format(Guid.newGuid());
            var alertMessage = "";

            if (message != null) {
                if (angular.isString(message))
                    alertMessage = message.replaceAll('\n', "<br />");
                else
                    alertMessage = JSON.stringify(message);
            }

            if (title)
                alertsContainer.append(ALERT_TEMPLATE_WITH_TITLE.format(alertClass, alertId, title, alertMessage));
            else
                alertsContainer.append(ALERT_TEMPLATE.format(alertClass, alertId, alertMessage));

            if (fadeOut) {
                window.setTimeout(function () {
                    $(".{0}".format(alertId)).fadeTo(7000, 0, function () {
                        $(this).remove();
                    });
                }, 3000);
            }

            $(".{0}>.close".format(alertId)).on("click", function () {
                $(".{0}".format(alertId)).remove();
            });
        }
    },
    initTextToUpper: function () {
        $('#formKid').find(':text').each(function (i, editor) {
            editor.addEventListener('keyup', function () {
                editor.value = editor.value.toUpperCase();
            }, false);
        });
    },
    selectableTable: function (tableId) {
    var Id = "#" + tableId;
    $(Id).on('click', 'tbody tr', function (event) {
        $(this).addClass('active').siblings().removeClass('active');
    });
    }
};