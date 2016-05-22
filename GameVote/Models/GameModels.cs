using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameVote.Helpers;
using System.Text;

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

        //Gat list of all games
        public static List<GameModels> GameList()
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            List<GameModels> FullList = dbhelp.GetGames();
            return FullList;
        }

        //Get a single Game 
        public static List<GameModels> SingleGame(string uid)
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            List<GameModels> Game = dbhelp.GetGame(uid);
            return Game;
        }

        public string GameListQuery(int Players, int PlayTime)
        {
            List<GameModels> FullList = GameList();
            string GameQuery = "";
            foreach (var game in FullList)
            {
                if((game.min <= Players) && (game.max >= Players))
                    {
                        int Time = game.time;
                        if (game.played == false)
                            Time += 60;
                        if(Time <= PlayTime)
                        {
                            StringBuilder sb = new StringBuilder(GameQuery);
                            sb.Append("<li id='");
                            sb.Append(game.uid);
                            sb.Append("' class='well NotSelected'><h2 class='GameName'>");
                            sb.Append(game.name);
                            sb.Append("</h2><img src='");
                            sb.Append(game.image);
                            sb.Append("' Height = '150' class = 'GameImage'/><p class='GameDescription'>");
                            sb.Append(game.description);
                            sb.Append("</p><p class = 'GameStats'>Entertains ");
                            sb.Append(game.min);
                            sb.Append(" - ");
                            sb.Append(game.max);
                            sb.Append(" players for ");
                            sb.Append(game.time);
                            sb.Append("Min.</p><button onClick=\"gameSelect('" + game.uid + "')\">Select Game</button>");
                            sb.Append("   Primary: <input type='radio' name='1' value='1'>  Secondary: <input type='radio' name='2' value='2'>  Tertiary: <input type='radio' name='3' value='3'></li>");
                        GameQuery = sb.ToString();
                        }
                    }
            }
            return GameQuery;
        }

        //Delete Game 
        public static string Delete(string uid)
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            return dbhelp.RemoveGame(uid);
        }

        //Get Full list of games as String
        public string FullGameListString()
        {
            var List = GameList();
            string GameString = "";
            foreach (var game in List)
            {
                StringBuilder sb = new StringBuilder(GameString);
                sb.Append("<li class='well'><h2 class='GameName'>");
                sb.Append(game.name);
                sb.Append("</h2><img src='");
                sb.Append(game.image);
                sb.Append("' Height = '150' class = 'GameImage'/><p class='GameDescription'>");
                sb.Append(game.description);
                sb.Append("</p><p class = 'GameStats'>Entertains ");
                sb.Append(game.min);
                sb.Append(" - ");
                sb.Append(game.max);
                sb.Append(" players for ");
                sb.Append(game.time);
                sb.Append("Min.</p><a href='/Home/Edit/");
                sb.Append(game.uid);
                sb.Append("' class='GameEdit'>Edit</a></li>");
                GameString = sb.ToString();
            }
            return GameString;
        }

        //Get Single Game as String
        public string SingleGameString(string uid)
        {
            var List = SingleGame(uid);
            string GameString = "";
            foreach (var game in List)
            {
                StringBuilder sb = new StringBuilder(GameString);
                sb.Append("<li class='well'><h2 class='GameName'>");
                sb.Append(game.name);
                sb.Append("</h2><img src='");
                sb.Append(game.image);
                sb.Append("' Height = '150' class = 'GameImage'/><p class='GameDescription'>");
                sb.Append(game.description);
                sb.Append("</p><p class = 'GameStats'>Entertains ");
                sb.Append(game.min);
                sb.Append(" - ");
                sb.Append(game.max);
                sb.Append(" players for ");
                sb.Append(game.time);
                sb.Append("Min.</p></li>");
                GameString = sb.ToString();
            }
            return GameString;
        } 
        
        public void Vote(string username, string gamekey, int vote)
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            dbhelp.DeleteVoteList();
            dbhelp.PlayerVote(username, gamekey, vote);
        }
    }
}