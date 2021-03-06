﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameVote.Helpers;
using System.Text;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

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
            List<GameModels> TempList = GameList();
            var rnd = new Random();
            var PartList = TempList.ToList();
            DatabaseHelper dbhelp = new DatabaseHelper();
            dbhelp.DeleteVoteList(); //Delete any existing vote results before making new query
            string GameQuery = "";

            foreach (var game in TempList)
            {
                int Time = game.time;
                if (game.played == false)
                    Time += 60;

                if (((game.min <= Players) && (game.max >= Players)) == false)
                    {
                    PartList.Remove(game);
                    }
                else if(Time > PlayTime)
                    {
                    PartList.Remove(game);
                    }
            }

            var ShortList = (PartList.OrderBy(x => rnd.Next()).ToList().Take(12));

            foreach (var game in ShortList)
            {
                StringBuilder sb = new StringBuilder(GameQuery);
                sb.Append("<li class='well ballot'><h2 class='GameName_ballot'>");
                sb.Append(game.name);
                sb.Append("</h2> Vote1 <input class='VotePos' type='radio' name='" + game.uid + "' value='1'> &nbsp;|&nbsp; Vote2 <input class='VotePos' type='radio' name='" + game.uid + "' value='2'> &nbsp;|&nbsp; Vote3 <input class='VotePos' type='radio' name='" + game.uid + "' value='3'> &nbsp;|&nbsp; Unselect <input class='VotePos2' type='radio' name='" + game.uid + "' value='0' checked='checked'><span class='ShowInfo' tabindex='0'>Show Me More</span><div class='HideInfo'><p class = 'GameStats'>Entertains ");
                sb.Append(game.min);
                sb.Append(" - ");
                sb.Append(game.max);
                sb.Append(" players for ");
                sb.Append(game.time);
                sb.Append("Min.</p><img src='");
                sb.Append(game.image);
                sb.Append("' Height = '150' class = 'GameImage'/><p class='GameDescription'>");
                sb.Append(game.description);
                sb.Append("</p></div>");
                GameQuery = sb.ToString();
            }
            return GameQuery;
        }

        //Delete Game 
        public static string Delete(string uid)
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            return dbhelp.RemoveGame(uid);
        }

        //Edit Game Played
        public static string GamePlayed(string uid)
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            return dbhelp.EditGamePlayed(uid);
        }

        //Get Full list of games as String
        public string FullGameListString()
        {
            var Temp = GameList();
            var List = Temp.OrderBy(x => x.name).ToList();
            string GameString = "";
            foreach (var game in List)
            {
                StringBuilder sb = new StringBuilder(GameString);
                sb.Append("<li class='well'><a href='/Home/Edit/");
                sb.Append(game.uid);
                sb.Append("' class='GameEdit'>Edit</a><h2 class='GameName'>");
                sb.Append(game.name);
                sb.Append("</h2><p class = 'GameStats'>Entertains ");
                sb.Append(game.min);
                sb.Append(" - ");
                sb.Append(game.max);
                sb.Append(" players for ");
                sb.Append(game.time);
                sb.Append("Min.</p><img src='");
                sb.Append(game.image);
                sb.Append("' Height = '150' class = 'GameImage'/><p class='GameDescription'>");
                sb.Append(game.description);
                sb.Append("</p>");
                GameString = sb.ToString();
            }
            return GameString;
        }

        //Get Single Game as String for the edit screen
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
                sb.Append("Min.</p>");
                sb.Append("<div id='GameData' name='");//creates a hidden element with all game data so the JS can fill out edit form automaticly.
                sb.Append(game.name);
                sb.Append("' image='");
                sb.Append(game.image);
                sb.Append("' played='");
                sb.Append(game.played);
                sb.Append("' min='");
                sb.Append(game.min);
                sb.Append("' max='");
                sb.Append(game.max);
                sb.Append("' time='");
                sb.Append(game.time);
                sb.Append("' description='");
                sb.Append(game.description);
                sb.Append("'' style='display: none;'/></li>");
                GameString = sb.ToString();
            }
            return GameString;
        } 

        public class VoteListModel
        {
            public List<VoteModel> list { get; set; }
        }

        public class VoteModel
        {
            public string name { get; set; }
            public string game { get; set; }
            public Int32 pos { get; set; }

            public static void Vote(IEnumerable<GameModels.VoteModel> json)
            {
                DatabaseHelper dbhelp = new DatabaseHelper();
                foreach(var vote in json)
                {
                    dbhelp.PlayerVote(vote.name, vote.game, vote.pos);
                }
            }
        }

        public class TallyModel
        {
            public string game { get; set; }
            public int votes { get; set; }
        }
    }
}