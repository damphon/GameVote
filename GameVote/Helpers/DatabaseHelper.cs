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

        //Add a way to modify an existing entry

        public string RemoveGame(string uid)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MCDBConnection"].ToString()))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "DELETE FROM GameList WHERE KEY = @Key";
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
    }
}