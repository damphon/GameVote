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
        public ActionResult Manage(string formName, int formMinPlayers, int formMaxPlayers, int formTime, int formPlayed, string formDescription, string formImage) 
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


        [HttpPost]
        public ActionResult CreateList(int formPlayers, int formPlayTime)
        {
            GameModels List = new GameModels();
            string query = List.GameListQuery(formPlayers, formPlayTime);
            var path = Server.MapPath(@"~/Content/QueryResults.txt");
            System.IO.File.WriteAllText(path, query);
            return Redirect("~/Home/");
        }

        [HttpGet]
        public ActionResult Player()
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
        public ActionResult Edit(string formName, int formMinPlayers, int formMaxPlayers, int formTime, int formPlayed, string formDescription, string formImage, String uid)
        {
            bool hasPlayed = false;
            string Description = formDescription;
            string Image = formImage;

            GameModels.Delete(uid); //Deletes existing entry before making new one for the game.

            DatabaseHelper dbhelp = new DatabaseHelper();

            //If it is 1 then hasPlayed remains false, no need for an else.
            if (formPlayed == 0) hasPlayed = true;
            if (Description == "") Description = "No Information Provided";
            if (Image == "") Image = "http://gn.jeremytoler.net/Content/Images/SW.jpg";

            dbhelp.NewGame(formName, Description, Image, formMinPlayers, formMaxPlayers, formTime, hasPlayed);
            return Redirect("~/Home/Manage");
        }

        [HttpPost]
        public ActionResult Delete(string uid)
        {
            GameModels.Delete(uid);
            return Redirect("~/Home/Manage");
        }

        [HttpPost]
        public ActionResult SetPlayed(string uid)
        {
            GameModels.GamePlayed(uid);
            return Redirect("~/Home/Manage");
        }

        [HttpGet]
        public ActionResult VoteResults()
        {
            VoteHelper voteHelper = new VoteHelper();
            ViewBag.result = voteHelper.PrintVotes();
            ViewBag.voters = voteHelper.GetVoters();
            return View();
        }

        [HttpPost]
        public ActionResult VoteResults(IEnumerable<GameModels.VoteModel> jsonData)
        {
            GameModels.VoteModel.Vote(jsonData);
            return Json(new { success = true });
        }
    }
}