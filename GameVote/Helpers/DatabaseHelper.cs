using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using GameVote.Models;

namespace GameVote.Helpers
{
    public class DatabaseHelper
    {
        public string NewGame(string name, string description, string image, int min, int max, int time, bool played)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO GameList (Name, Description, ImageURL, MinPlayers, MaxPlayers, HasPlayed, Duration) VALUES (@Name, @Description, @ImageURL, @MinPlayers, @MaxPlayers, @HasPlayed, @Duration)";
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@ImageURL", image);
                    command.Parameters.AddWithValue("@MinPlayers", min);
                    command.Parameters.AddWithValue("@MaxPlayers", max);
                    command.Parameters.AddWithValue("@HasPlayed", played);
                    command.Parameters.AddWithValue("@Duration", time);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        return "Error: " + e.Message;
                    }
                }
            }
            return "Game has been added";
        }

        public List<GameModels> GetGames()
        {
            var ListOfGames = new List<GameModels>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                connection.Open();
                string query = "SELECT * FROM GameList";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var GameList = new GameModels();
                            GameList.uid = reader.GetGuid(reader.GetOrdinal("Key"));
                            GameList.name = reader.GetString(reader.GetOrdinal("Name"));
                            GameList.description = reader.GetString(reader.GetOrdinal("Description"));
                            GameList.image = reader.GetString(reader.GetOrdinal("ImageURL"));
                            GameList.min = reader.GetByte(reader.GetOrdinal("MinPlayers"));
                            GameList.max = reader.GetByte(reader.GetOrdinal("MaxPlayers"));
                            GameList.time = reader.GetInt16(reader.GetOrdinal("Duration"));
                            GameList.played = (bool)reader["HasPlayed"];

                            ListOfGames.Add(GameList);
                        }
                    }
                }
            }
            return ListOfGames;
        }

        public List<GameModels> GetGame(string uid)
        {
            var GameEntry = new List<GameModels>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                connection.Open();
                string query = String.Format("SELECT * FROM GameList WHERE [Key] = '{0}'", uid);
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var GameList = new GameModels();
                            GameList.uid = reader.GetGuid(reader.GetOrdinal("Key"));
                            GameList.name = reader.GetString(reader.GetOrdinal("Name"));
                            GameList.description = reader.GetString(reader.GetOrdinal("Description"));
                            GameList.image = reader.GetString(reader.GetOrdinal("ImageURL"));
                            GameList.min = reader.GetByte(reader.GetOrdinal("MinPlayers"));
                            GameList.max = reader.GetByte(reader.GetOrdinal("MaxPlayers"));
                            GameList.time = reader.GetInt16(reader.GetOrdinal("Duration"));
                            GameList.played = (bool)reader["HasPlayed"];

                            GameEntry.Add(GameList);
                        }
                    }
                }
            }
            return GameEntry;
        }

        public string EditGamePlayed(string name)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE GameList SET HasPlayed=1 WHERE [Key]=@Name;";
                    command.Parameters.AddWithValue("@Name", name);
                    
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        return "Error: " + e.Message;
                    }
                }
            }
            return "Game has been added";
        }

        public string RemoveGame(string uid)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM GameList WHERE [Key] = @Key";
                    command.Parameters.AddWithValue("@Key", uid);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        return "Error: " + e.Message;
                    }
                }
            }
            return "Game has been destroyed";
        }
        /**********************************Player Vote Database Items below this line********************************************/
        public string PlayerVote(string username, string gamekey, int vote)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO TempGameList (UserName, GameKey, Vote) VALUES (@Name, @Game, @Rank)";
                    command.Parameters.AddWithValue("@Name", username);
                    command.Parameters.AddWithValue("@Game", gamekey);
                    command.Parameters.AddWithValue("@Rank", vote);
                    
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        return "Error: " + e.Message;
                    }
                }
            }
            return "Votes Submitted";
        }

        public string DeleteVoteList()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM TempGameList";

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        return "Error: " + e.Message;
                    }
                }
            }
            return "Game has been destroyed";
        }

        public List<GameModels.VoteModel> GetVotes()
        {
            var ListOfVotes = new List<GameModels.VoteModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                connection.Open();
                string query = "SELECT * FROM TempGameList";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var VoteList = new GameModels.VoteModel();
                            VoteList.name = reader.GetString(reader.GetOrdinal("UserName"));
                            VoteList.game = reader.GetString(reader.GetOrdinal("GameKey"));
                            VoteList.pos = reader.GetByte(reader.GetOrdinal("Vote"));

                            ListOfVotes.Add(VoteList);
                        }
                    }
                }
            }
            return ListOfVotes;
        }
    }
}