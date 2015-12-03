siteCtrls.controller('ghostController', ['$scope', '$timeout',
    function ($scope, $timeout) {

        $scope.step = 0;
        $scope.gameSetting = {};
        $scope.userWord = null;
        $scope.leaderFlag = false;
        $scope.gameReady = false;
        $scope.gameStarted = false;
        var gameDataStore = {};

        var ghostHub = $.connection.ghostHub;

        /*Real time client functions*/
        ghostHub.client.broadcastStatusMessage = function (name, statusMsg) {
            var encodedName = $('<div />').text(name).html();
            var encodedMsg = $('<div />').text(statusMsg).html();
            $('#ulStatusBoard').append('<li><strong>' + encodedName + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
        };

        ghostHub.client.raiseError = function (errorMessage) {
            alert(errorMessage);
            $timeout(function () {
                $scope.step = 1;
            }, 500);
            $scope.gameSetting = {};
            $scope.gameDataStore = {};
        };

        ghostHub.client.waitForGameStart = function (creator) {                        
            $('#btnCreateGame').prop("disabled", true);
            $('#btnCreateGame').val('Waiting for ' + creator + "'s game to start...");            
        };

        ghostHub.client.configNewGame = function () {
            $timeout(function () {
                $scope.step = 1;
                $scope.leaderFlag = true;
            }, 500);
        };

        ghostHub.client.joinGameHall = function () {
            $timeout(function () {
                $scope.step = 2;                
            }, 500);
        };

        ghostHub.client.enableJoin = function () {
            $('#btnCreateGame').hide();
            $timeout(function () {
                $scope.gameReady = true;
                $scope.$apply();
            }, 300);
            $('#btnJoinGame').show();
        };

        ghostHub.client.start = function (gameData) {
            console.log(gameData);
            gameDataStore = gameData;
            $timeout(function () {
                $scope.gameStarted = true;
                $scope.showUserWord = false;
                $scope.userWord = gameData[$scope.userName];
            }, 500);            
        };
        
        /*End of real time client functions*/

        $.connection.hub.start().done(function () {

            $scope.createNewGame = function (userName) {                
                ghostHub.server.createNewGame(userName);
            }

            $scope.joinGame = function (userName) {
                ghostHub.server.joinGame(userName, $scope.leaderFlag);
            };

            $scope.startGame = function (gameSetting) {                
                ghostHub.server.startGame(gameSetting);
            }            

            //$('#btnSend').click(function () {
            //    ghostHub.server.sendStatusMessage($('#txtName').val(), $('#txtMessage').val());
            //});
        });

        $scope.toggleUserWord = function () {            
            if ($scope.showUserWord != undefined && $scope.showUserWord != null) {
                $scope.showUserWord = !$scope.showUserWord;
            }
        };

        
    }]);