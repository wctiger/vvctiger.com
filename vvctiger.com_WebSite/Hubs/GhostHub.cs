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
    }

    public class GhostHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendStatusMessage(string name, string statusMsg)
        {
            Clients.All.broadcastStatusMessage(name, statusMsg);
        }

        public void CreateNewGame(string creator)
        {
            Clients.Caller.configNewGame();
            Clients.Others.waitForGameStart(creator);
        }

        public void JoinGame(string userName, bool leaderFlag)
        {
            GhostGameApp.UserNames.Add(userName);
            Clients.Caller.joinGameHall();
            if(leaderFlag)
                Clients.Others.enableJoin();
            Clients.All.broadcastStatusMessage(userName, "Joined the game");
        }

        public void StartGame(GhostGameSettingVM gameSetting)
        {
            if (gameSetting.GameMode.Equals("MANUAL"))
            {
                int totalCount = gameSetting.HumanCount + gameSetting.FoolCount + gameSetting.GhostCount;
                List<string> userIds = GhostGameApp.UserConnectionIds;
                if (totalCount != userIds.Count)
                {
                    Clients.All.raiseError("Game player count doesn't match user counts. C'mon and get everybody involved!");
                    return;
                }

                List<string> userNames = GhostGameApp.UserNames.ToList();
                var gameData = GenerateGameData(gameSetting.HumanWord, gameSetting.FoolWord, gameSetting.GhostWord, gameSetting.HumanCount, gameSetting.FoolCount, gameSetting.GhostCount, userNames);

                Clients.All.start(gameData);
                
                
            }
            else if(gameSetting.GameMode.Equals("AUTOMATIC"))
            {
                //TODO
            }

        }

        public override Task OnConnected()
        {
            GhostGameApp.UserConnectionIds.Add(Context.ConnectionId);
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