using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameVote.Models;
using GameVote.Helpers;

namespace GameVote.Controllers
{
    public class HomeController : Controller
    {
        string GameQuery = "Query has not been run yet!";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Manage()
        {
            GameModels List = new GameModels();
            ViewBag.GameList = List.FullGameListString();
            return View();
        }

        [HttpPost]
        //Description and Image have default values in case they are not filled in.
        public ActionResult Manage(String formName, int formMinPlayers, int formMaxPlayers, int formTime, int formPlayed, string formDescription = "No Information Provided", string formImage = "~/Content/Images/SW.jpg") 
        {
            bool hasPlayed = false;
            DatabaseHelper dbhelp = new DatabaseHelper();

            //If it is 1 then hasPlayed remains false, no need for an else.
            if (formPlayed == 0) hasPlayed = true;
            
            dbhelp.NewGame(formName, formDescription, formImage, formMinPlayers, formMaxPlayers, formTime, hasPlayed);
            return View();
        }

        [HttpGet]
       public ActionResult Player()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Player(int formPlayers, int formPlayTime, string formPlayerName)
        {
            GameModels List = new GameModels();
            GameQuery = List.GameListQuery(formPlayers, formPlayTime); //THIS IS NOT PERSISTANT
            return Redirect("~/Home/PlayerChoice/" + formPlayerName);
        }

        [HttpGet]
        public ActionResult PlayerChoice()
        {
            //Query DB for items matching the needed playtime/players 
            ViewBag.Query = GameQuery;
            return View();
        }

        [HttpPost]
        public ActionResult PlayerChoice(string formPlayerName, string uid1, string uid2, string uid3)
        {
            //Make a temp database and redirect to the voteing page for the player
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string uid)
        {
            GameModels.Delete(uid);
            return Redirect("~/Home/Manage");
        }
    }
}