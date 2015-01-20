using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web.Security;
using DMTools.Models;

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

        public async void Join(int table, string name)
        {
            Table t = GetTable(table);
            if (t == null)
                return;

            if (!t.IsNameAvailable(name))
            {
                Clients.Caller.showMessage("That name is already taken");
                return;
            }

            t.UserNamesByID.Add(Context.ConnectionId, name);
            t.UserNames.Add(name);

            await Groups.Add(Context.ConnectionId, table.ToString());
            Clients.Group(table.ToString()).showMessage(name + " joined the table.");
        }

        public void Say(int table, string message)
        {
            Table t = GetTable(table);
            if (t == null)
                return;

            string userName;
            if (!t.UserNamesByID.TryGetValue(Context.ConnectionId, out userName))
                return;

            Clients.Group(table.ToString()).showChatMessage(userName, message);
        }
    }
}