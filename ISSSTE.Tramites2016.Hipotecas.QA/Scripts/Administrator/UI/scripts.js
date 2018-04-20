var ALERT_TEMPLATE = "<div class=\"alert alert-{0} {1}\" role=\"alert\"><button type=\"button\" class=\"close\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><span>{2}</span></div>";
var ALERT_TEMPLATE_WITH_TITLE = "<div class=\"alert alert-{0} {1}\" role=\"alert\"><button type=\"button\" class=\"close\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><strong>{2}</strong><br/><span>{3}</span></div>";
var ALERT_ID_TEMPLATE = "alert-{0}";

var UI =
{
    getBaseUrl: function() {
        return $("#baseUrl").val();
    },
    initTabs: function() {
        $(".nav-tabs a").click(function(e) {
            e.preventDefault();
            $(this).tab("show");
        });
    },
    initStatusDropdown: function() {
        $("#status-dropdow li a").click(function() {

            $(this).parent().parent().parent().find(".btn:first-child").html("Mostrar " + $(this).text() + " <span class=\"caret\"></span>");
            $(this).parent().parent().parent().find(".btn:first-child").val($(this).val());

        });
    },
    initDropdown: function() {
        $("#status-dropdow li a").click(function() {

            $(this).parent().parent().parent().find(".btn:first-child").html($(this).text() + " <span class=\"caret\"></span>");
            $(this).parent().parent().parent().find(".btn:first-child").val($(this).val());

        });
    },
    createInfoMessage: function(message, title) {
        UI.createMessage("info", message, title, false);
    },
    createWarningMessage: function(message, title) {
        UI.createMessage("warning", message, title, false);
    },
    createSuccessMessage: function(message, title) {
        UI.createMessage("success", message, title, true);
    },
    createErrorMessage: function(message, title) {
        UI.createMessage("danger", message, title, false);
    },
    createMessage: function(alertClass, message, title, fadeOut) {
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
                window.setTimeout(function() {
                    $(".{0}".format(alertId)).fadeTo(500, 0, function() {
                        $(this).remove();
                    });
                }, 3000);
            }

            $(".{0}>.close".format(alertId)).on("click", function() {
                $(".{0}".format(alertId)).remove();
            });
        }
    },
    selectableTable: function (tableId) {
        var Id = "#" + tableId;
        $(Id).on('click', 'tbody tr', function (event) {
            $(this).addClass('active').siblings().removeClass('active');
        });
    }
};