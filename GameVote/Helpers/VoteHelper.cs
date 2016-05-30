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
            } while (CheckForWin(i-1) != true);
            //After finding that something won, get the name of the game that won and return it.
            int MostVotes = tally.Max(m => m.votes);
            foreach(var game in tally)
            {
                if (game.votes >= MostVotes)
                    win = game.game;
            }
            return win;
        }

        public bool CheckForWin(int i)
        {
            int NeededVotes = (voters * i) / 2;
            foreach(var vote in tally)
            {
                if(vote.votes >= NeededVotes)
                {
                    return true;
                }
            }
            return false;
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

        public void GetGames()
        {
            List<string> Games = new List<string>();
            foreach(var vote in votes)
            {
                if(!(Games.Any(s => s.Contains(vote.game))))
                {
                    Games.Add(vote.game);
                    tally.Add(new GameModels.TallyModel() { game = vote.game, votes = 0 });
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