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

        public Table(string dmUser)
        {
            ID = nextID ++;
            AllTables.Add(ID, this);

            DM = dmUser;
            UserNames = new SortedSet<string>();
            UserNames.Add(dmUser);

            UserNamesByID = new SortedList<string, string>();
        }

        public int ID { get; private set; }
        public string DM { get; private set; }
        public SortedSet<string> UserNames { get; private set; }
        public SortedList<string, string> UserNamesByID { get; private set; }

        public bool IsNameAvailable(string name)
        {
            return UserNamesByID.Count == 0 || !UserNames.Contains(name);
        }
    }
}