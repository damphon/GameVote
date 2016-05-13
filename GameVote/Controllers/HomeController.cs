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

        public ActionResult David()
        {
            return View();
        }

        public ActionResult Connie()
        {
            return View();
        }

        public ActionResult Gary()
        {
            return View();
        }

        public ActionResult Jose()
        {
            return View();
        }

        public ActionResult Jeremy()
        {
            return View();
        }

        public ActionResult Sam()
        {
            return View();
        }
    }
}