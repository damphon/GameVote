using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameVote.Helpers;

namespace GameVote.Models
{
    public class GameModels
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public Int32 min { get; set; }
        public Int32 max { get; set; }
        public Int32 time { get; set; }
        public bool? played { get; set; }
        public Guid uid { get; set; }

        public static List<GameModels> GameList()
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            List<GameModels> FullList = dbhelp.GetGames();
            return FullList;
        }
    }
}