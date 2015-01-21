using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web.Security;
using DMTools.Models;
using DMTools.Services;

namespace DMTools.Hubs
{
    public class TableHub : Hub
    {
        private Table GetTable(int table)
        {
            Table t;
            if (!Table.AllTables.TryGetValue(table, out t))
            {
                Clients.Caller.showMessage("Invalid table ID");
                return null;
            }

            return t;
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            foreach (var table in Table.AllTables.Values)
            {
                string name = table.RemoveUser(Context.ConnectionId);
                if (name != null)
                    Clients.Group(table.ID.ToString()).showMessage(name + " left the table. " + (stopCalled ? "Stop called." : "Stop not called."));
            }

            return base.OnDisconnected(stopCalled);
        }

        public async void Join(int table, string name)
        {
            Table t = GetTable(table);
            if (t == null)
                return;

            bool isDM;
            if (t.GetUserName(Context.ConnectionId) == null)
            {
                if (!t.IsNameAvailable(name))
                {
                    Clients.Caller.forceRename();
                    return;
                }

                isDM = t.AddUser(Context.ConnectionId, name);
            }
            else
                isDM = name == t.DM;

            await Groups.Add(Context.ConnectionId, table.ToString());
            Clients.Caller.setupUi(isDM);
            Clients.Group(table.ToString()).showMessage(name + " joined the table.");
        }

        public void Say(int table, string message)
        {
            Table t = GetTable(table);
            if (t == null)
                return;

            string userName = t.GetUserName(Context.ConnectionId);
            if (userName == null)
                return;

            if (message.StartsWith("/"))
            {
                var space = message.IndexOf(' ');
                string command, args;
                if (space == -1)
                {
                    command = message.Substring(1);
                    args = string.Empty;
                }
                else
                {
                    command = message.Substring(1, space - 1);
                    args = message.Substring(space + 1);
                }
                try
                {
                    HandleCommand(t, userName, command.ToLower(), args);
                }
                catch
                {
                    Clients.Caller.showMessage("Unable to process command: " + command);
                }
            }
            else
                Clients.Group(table.ToString()).showChatMessage(userName, message);
        }

        private void HandleCommand(Table table, string userName, string command, string args)
        {
            switch(command)
            {
                case "roll":
                    Clients.Caller.showDiceRoll(userName, DiceService.Roll(args));
                    break;
                default:
                    Clients.Caller.showMessage("Invalid command: " + command);
                    break;
            }
        }
    }
}