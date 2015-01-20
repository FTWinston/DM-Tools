using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMTools.Models
{
    public class Table
    {
        public static SortedList<int, Table> AllTables = new SortedList<int, Table>();
        private static int nextID = 1;

        public Table()
        {
            ID = nextID ++;
            AllTables.Add(ID, this);

            UserNames = new SortedSet<string>();
            UserNamesByID = new SortedList<string, string>();
        }

        public int ID { get; private set; }
        public string DM { get; private set; }
        private SortedSet<string> UserNames { get; set; }
        private SortedList<string, string> UserNamesByID { get; set; }

        public bool IsNameAvailable(string name)
        {
            return UserNamesByID.Count == 0 || !UserNames.Contains(name);
        }

        public string GetUserName(string connectionID)
        {
            string name;
            if (UserNamesByID.TryGetValue(connectionID, out name))
                return name;
            return null;
        }

        public bool AddUser(string connectionID, string name)
        {
            bool isDM = UserNamesByID.Count == 0;
            if (isDM)
                DM = name;

            UserNamesByID.Add(connectionID, name);
            UserNames.Add(name);

            return isDM;
        }

        public string RemoveUser(string connectionID)
        {
            string name;
            if (UserNamesByID.TryGetValue(connectionID, out name))
            {
                UserNames.Remove(name);
                UserNamesByID.Remove(connectionID);
                return name;
            }
            return null;
        }
    }
}