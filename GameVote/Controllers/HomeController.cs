using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameVote.Models;
using GameVote.Helpers;
using GameVote.Properties;

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
        public ActionResult Manage(String formName, int formMinPlayers, int formMaxPlayers, int formTime, int formPlayed, string formDescription, string formImage) 
        {
            bool hasPlayed = false;
            string Description = formDescription;
            string Image = formImage;

            DatabaseHelper dbhelp = new DatabaseHelper();

            //If it is 1 then hasPlayed remains false, no need for an else.
            if (formPlayed == 0) hasPlayed = true;
            if (Description == "") Description = "No Information Provided";
            if (Image == "") Image = "http://gn.jeremytoler.net/Content/Images/SW.jpg";

            dbhelp.NewGame(formName, Description, Image, formMinPlayers, formMaxPlayers, formTime, hasPlayed);
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
            string query = List.GameListQuery(formPlayers, formPlayTime);
            var path = Server.MapPath(@"~/Content/QueryResults.txt");
            System.IO.File.WriteAllText(path, query);
            return Redirect("~/Home/PlayerChoice/" + formPlayerName);
        }

        [HttpGet]
        public ActionResult PlayerChoice()
        {
            var path = Server.MapPath(@"~/Content/QueryResults.txt");
            ViewBag.Query = System.IO.File.ReadAllText(path);
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