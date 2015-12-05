siteCtrls.controller('ghostController', ['$scope', '$timeout',
    function ($scope, $timeout) {
        $scope.$parent.startLoad("Game Loading...");
        $scope.step = 0;
        $scope.gameSetting = {};
        $scope.userWord = null;
        $scope.leaderFlag = false;        
        $scope.gameStatus = 0;
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
        };

        ghostHub.client.setGameStatus = function (gameStatus) {            
            $scope.gameStatus = gameStatus;
            $scope.$apply();
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
            $('#btnJoinGame').show();
        };

        ghostHub.client.start = function (gameData) {            
            gameDataStore = gameData;
            if (!$scope.leaderFlag) {
                $timeout(function () {
                    $scope.showUserWord = false;
                    $scope.userWord = gameData[$scope.userName];
                }, 500);
            }            
        };

        ghostHub.client.resetGame = function () {
            $scope.step = 0;
            $scope.gameSetting = {};
            $scope.userWord = null;
            $scope.leaderFlag = false;
            $scope.gameStatus = 0;
            $scope.gameData = null;
            $scope.showAllWords = false;
            gameDataStore = {};
            $scope.$apply();
        };
        
        /*End of real time client functions*/

        $.connection.hub.start().done(function () {

            $scope.$apply();

            $scope.$parent.endLoad();

            $scope.createNewGame = function (userName) {                
                ghostHub.server.createNewGame(userName);
            }

            $scope.joinGame = function (userName) {
                ghostHub.server.joinGame(userName, $scope.leaderFlag);
            };

            $scope.startGame = function (gameSetting, leaderName) {                
                ghostHub.server.startGame(gameSetting, leaderName);
            }

            $scope.reset = function () {
                ghostHub.server.resetGame();
            };

            $scope.viewAllWords = function (userName) {
                $scope.gameData = gameDataStore;
                ghostHub.server.sendStatusMessage(userName, "decided to see all words. Make sure this guy is DEAD!");
                $scope.showAllWords = true;
            };
            
        });

        $scope.toggleUserWord = function () {            
            if ($scope.showUserWord != undefined && $scope.showUserWord != null) {
                $scope.showUserWord = !$scope.showUserWord;
            }
        };

        
    }]);