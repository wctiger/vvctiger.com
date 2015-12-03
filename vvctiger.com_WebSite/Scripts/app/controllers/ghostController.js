siteCtrls.controller('ghostController', ['$scope', '$timeout',
    function ($scope, $timeout) {

        $scope.step = 0;
        $scope.gameSetting = {};
        var gameDataStore = {};

        var ghostHub = $.connection.ghostHub;

        /*Real time client functions*/
        ghostHub.client.broadcastStatusMessage = function (name, statusMsg) {
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(statusMsg).html();
            $('#ulStatusBoard').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };

        ghostHub.client.waitForGameStart = function (creator) {            
            $('#txtUserName').hide();
            $('#btnCreateGame').prop("disabled", true);
            $('#btnCreateGame').val('Waiting for ' + creator + "'s game to start...");            
        };

        ghostHub.client.configNewGame = function () {
            $timeout(function () {
                $scope.step = 1;
            }, 500);
        };
        
        ghostHub.client.enableHall = function (gameData) {
            console.log(gameData);
            gameDataStore = gameData;
            $timeout(function () {
                $scope.step = 2;
            }, 500);
        };
        /*End of real time client functions*/

        $.connection.hub.start().done(function () {

            $scope.createNewGame = function (userName) {                
                ghostHub.server.createNewGame(userName);
            }

            $scope.startGame = function (gameSetting) {
                console.log(gameSetting);
                ghostHub.server.startGame(gameSetting);
            }

            $('#btnSend').click(function () {
                ghostHub.server.sendStatusMessage($('#txtName').val(), $('#txtMessage').val());
            });
        });

        
    }]);