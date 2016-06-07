using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameVote.Models;

namespace GameVote.Helpers
{
    public class VoteHelper
    {
        List<GameModels.TallyModel> tally = new List<GameModels.TallyModel>();
        List<GameModels.VoteModel> votes = new List<GameModels.VoteModel>();
        int voters = 0;

        public string PrintVotes()
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            votes = dbhelp.GetVotes();
            List<GameModels> games = dbhelp.GetGames();
            string win = "Error: PrintVotes did not update winner";
            GetGames();
            NumberOfVoters();
            win = Tally();
            foreach(var game in games)
            {
                if(game.uid.ToString() == win)
                {
                    win = game.name;
                }
            }

            return win; 
        }

        public string Tally()
        {
            string win = "Error: Tally could not determine winner";
            Random rand = new Random();
            int i = 1;
            do
            {
                //Only loop through the votes that are in the position we are working with.
                foreach (var vote in votes.Where(x => x.pos == i))
                {
                    //Find game position in tally that matches the game the foreach loop is itterating through then add one to the value of tally.votes for that game.
                    tally.Where(y => y.game == vote.game).ToList().ForEach(z => z.votes += 1);
                }
                i++;
                if (i > 3) break; //This will use handle win to get game with most votes.
            } while (CheckForWin() != true);
            //After finding that something won, Remove all games from the list that did not win.
            HandleWin(tally.Max(m => m.votes));
            //Randomly select a winner from games that did win in case of tie.
            win = tally[rand.Next(0, tally.Count)].game;
            return win;
        }

        public void GetGames()
        {
            List<string> Games = new List<string>();
            foreach (var vote in votes)
            {
                if (!(Games.Any(s => s.Contains(vote.game))))
                {
                    Games.Add(vote.game);
                    tally.Add(new GameModels.TallyModel() { game = vote.game, votes = 0 });
                }
            }
        }

        public int NumberOfVoters()
        {
            List<string> person = new List<string>();
            foreach (var vote in votes)
            {
                if (!(person.Any(s => s.Contains(vote.name))))
                {
                    person.Add(vote.name);
                    voters++;
                }
            }
            return voters;
        }

        public bool CheckForWin()
        {
            int NeededVotes = (voters / 2) + 1;
            foreach (var vote in tally)
            {
                if (vote.votes >= NeededVotes)
                {
                    return true;
                }
            }
            return false;
        }

        public void HandleWin(int MostVotes)
        {
            //Need temp list or modifications to tally will break the loop.
            List<GameModels.TallyModel> Temp = tally.ToList();
            foreach (var game in Temp)
            {
                if (game.votes < MostVotes)
                {
                    tally.Remove(game);
                }
            }
        }

        public List<string> GetVoters()
        {
            List<string> Voters = new List<string>();
            foreach (var vote in votes)
            {
                if (!(Voters.Any(s => s.Contains(vote.name))))
                {
                    Voters.Add(vote.name);
                }
            }
            return Voters;
        }
    }
}