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
        int voters = 0;

        public string PrintVotes()
        {
            DatabaseHelper dbhelp = new DatabaseHelper();
            List<GameModels.VoteModel> votes = dbhelp.GetVotes();
            GetGames(votes);
            NumberOfVoters(votes);
            return Tally(votes); //This returns a UUID
        }

        public string Tally(List<GameModels.VoteModel> votes)
        {
            string win = "Error: Could not determine a winner!";
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
            } while (CheckForWin(i) != true);
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
                if(vote.votes <= NeededVotes)
                {
                    return true;
                }
            }
            return false;
        }

        public int NumberOfVoters(List<GameModels.VoteModel> votes)
        {
            List<string> person = new List<string>();
            foreach (var vote in votes)
            {
                if (person.Any(s => s.Contains(vote.name)))
                {
                    person.Add(vote.name);
                    voters++;
                }
            }
            return voters;
        }

        public void GetGames(List<GameModels.VoteModel> votes)
        {
            List<string> Games = new List<string>();
            foreach(var vote in votes)
            {
                if(Games.Any(s => s.Contains(vote.game)))
                {
                    Games.Add(vote.game);
                    tally.Add(new GameModels.TallyModel() { game = vote.game, votes = 0 });
                }
            }
        }
    }
}