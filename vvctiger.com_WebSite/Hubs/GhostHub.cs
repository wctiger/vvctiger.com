using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace vvctiger.com_WebSite.Hubs
{
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
    }
}