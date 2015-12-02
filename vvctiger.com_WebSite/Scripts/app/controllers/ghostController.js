siteCtrls.controller('ghostController', ['$scope',
    function ($scope) {
        var ghostHub = $.connection.ghostHub;
        /*Real time client functions*/
        ghostHub.client.broadcastStatusMessage = function (name, statusMsg) {
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(statusMsg).html();
            $('#ulStatusBoard').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };

        /*End of real time client functions*/
        $.connection.hub.start().done(function () {
            $('#btnSend').click(function () {
                ghostHub.server.sendStatusMessage($('#txtName').val(), $('#txtMessage').val());
            });
        })
    }]);