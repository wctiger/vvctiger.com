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

        public void StartGame(GhostGameSettingVM gameSetting)
        {
            if (gameSetting.GameMode.Equals("MANUAL"))
            {
                int totalCount = gameSetting.HumanCount + gameSetting.FoolCount + gameSetting.GhostCount;
                List<string> users = GhostGameApp.UserConnectionIds;
                if (totalCount != users.Count)
                    Clients.All.raiseError("Game player count doesn't match user counts. C'mon and get everybody involved!");
                var gameData = GenerateGameData(gameSetting.HumanWord, gameSetting.FoolWord, gameSetting.GhostWord, gameSetting.HumanCount, gameSetting.FoolCount, gameSetting.GhostCount, users);
                Clients.All.enableHall(gameData);
                foreach(var userConnectionId in users)
                {
                    Clients.User(userConnectionId).getUserWord(userConnectionId);
                }
                
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

        private IDictionary<string, string> GenerateGameData(string humanWord, string foolWord, string ghostWord, int humanCount, int foolCount, int ghostCount, IList<string> userIds)
        {
            Dictionary<string, string> gameMap = new Dictionary<string, string>();
            Random rnd = new Random();
            for(int i = 0; i < humanCount; i++)
            {
                int r = rnd.Next(userIds.Count);
                gameMap.Add(userIds[r], humanWord);
                userIds.RemoveAt(r);
            }
            for(int i = 0; i < foolCount; i++)
            {
                int r = rnd.Next(userIds.Count);
                gameMap.Add(userIds[r], foolWord);
                userIds.RemoveAt(r);
            }
            for(int i = 0; i < ghostCount; i++)
            {
                int r = rnd.Next(userIds.Count);
                gameMap.Add(userIds[r], ghostWord);
                userIds.RemoveAt(r);
            }
            return gameMap;
        }
    }    
}