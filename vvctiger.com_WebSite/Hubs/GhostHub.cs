using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using vvctiger.com_WebSite.ViewModels;
using Newtonsoft.Json;

namespace vvctiger.com_WebSite.Hubs
{
    public static class GhostGameApp
    {
        public static List<string> UserConnectionIds = new List<string>();
        public static List<string> UserNames = new List<string>();
        public static GhostGameStatus GameStatus = GhostGameStatus.NotStarted;       
    }

    public enum GhostGameStatus
    {
        NotStarted = 1,
        Preparing = 2,
        Ready = 3,
        Started = 4
    }

    public class GhostHub : Hub
    {

        public void SendStatusMessage(string name, string statusMsg)
        {
            Clients.All.broadcastStatusMessage(name, statusMsg);
        }

        public void CreateNewGame(string creator)
        {
            Clients.Caller.configNewGame();            
            GhostGameApp.GameStatus = GhostGameStatus.Preparing;
            Clients.All.setGameStatus(GhostGameApp.GameStatus);
        }

        public void JoinGame(string userName, bool leaderFlag)
        {
            GhostGameApp.UserNames.Add(userName);
            Clients.Caller.joinGameHall();
            if(leaderFlag)
                Clients.Others.enableJoin();
            GhostGameApp.GameStatus = GhostGameStatus.Ready;
            Clients.All.setGameStatus(GhostGameApp.GameStatus);
            if (leaderFlag)
            {
                Clients.All.broadcastStatusMessage(userName + "(Leader)", "Joined the game");
            }
            else
            {
                Clients.All.broadcastStatusMessage(userName, "Joined the game");
            }
        }

        public void StartGame(GhostGameSettingVM gameSetting, string leaderName)
        {
            if (gameSetting.GameMode.Equals("MANUAL"))
            {
                int totalCount = gameSetting.HumanCount + gameSetting.FoolCount + gameSetting.GhostCount;
                List<string> userIds = GhostGameApp.UserConnectionIds;
                if (totalCount != userIds.Count - 1)
                {
                    Clients.Caller.raiseError(string.Format("Game player count doesn't match user counts. {0} players(except leader) in the hall while {1} game counts issued", userIds.Count - 1, totalCount));
                    return;
                }

                List<string> userNames = GhostGameApp.UserNames.ToList();
                userNames.Remove(leaderName);
                var gameData = GenerateGameData(gameSetting.HumanWord, gameSetting.FoolWord, gameSetting.GhostWord, gameSetting.HumanCount, gameSetting.FoolCount, gameSetting.GhostCount, userNames);
                SendStatusMessage("System", "Game started!!");
                Clients.All.start(gameData);
                GhostGameApp.GameStatus = GhostGameStatus.Started;
                Clients.All.setGameStatus(GhostGameApp.GameStatus);
                
            }
            else if(gameSetting.GameMode.Equals("AUTOMATIC"))
            {
                //TODO
            }

        }

        public void ResetGame()
        {
            SendStatusMessage("System", "Leader has reset the game. Game closing..");
            System.Threading.Thread.Sleep(1500);
            GhostGameApp.UserNames = new List<string>();
            GhostGameApp.GameStatus = GhostGameStatus.NotStarted;
            
            Clients.All.resetGame();
        }

        public override Task OnConnected()
        {
            GhostGameApp.UserConnectionIds.Add(Context.ConnectionId);
            Clients.Caller.setGameStatus(GhostGameApp.GameStatus);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            GhostGameApp.UserConnectionIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        private IDictionary<string, string> GenerateGameData(string humanWord, string foolWord, string ghostWord, int humanCount, int foolCount, int ghostCount, IList<string> userNames)
        {
            Dictionary<string, string> gameMap = new Dictionary<string, string>();
            Random rnd = new Random();
            for(int i = 0; i < humanCount; i++)
            {
                int r = rnd.Next(userNames.Count);
                gameMap.Add(userNames[r], humanWord);
                userNames.RemoveAt(r);
            }
            for(int i = 0; i < foolCount; i++)
            {
                int r = rnd.Next(userNames.Count);
                gameMap.Add(userNames[r], foolWord);
                userNames.RemoveAt(r);
            }
            for(int i = 0; i < ghostCount; i++)
            {
                int r = rnd.Next(userNames.Count);
                gameMap.Add(userNames[r], ghostWord);
                userNames.RemoveAt(r);
            }
            return gameMap;
        }
    }    
}