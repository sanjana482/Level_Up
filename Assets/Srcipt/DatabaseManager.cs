using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine;




public class DatabaseManager : MonoBehaviour
{
    private string dbPath;

    void Awake()
    {
        dbPath = "URI=file:" + UnityEngine.Application.streamingAssetsPath + "/mydb.db";

        CreateDB();
    }

    void CreateDB()
    {
        if (!File.Exists(Application.streamingAssetsPath + "/mydb.db"))
        {
            SqliteConnection.CreateFile(Application.streamingAssetsPath + "/mydb.db");
        }

        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            playerId TEXT,
                                            email TEXT,
                                            password TEXT);";
                cmd.ExecuteNonQuery();
            }
        }
    }

    public bool RegisterUser(string playerId, string email, string password)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();

            // 1?? CHECK IF USER ALREADY EXISTS
            using (var checkCmd = conn.CreateCommand())
            {
                checkCmd.CommandText = "SELECT COUNT(*) FROM users WHERE playerId = @p";
                checkCmd.Parameters.AddWithValue("@p", playerId);

                long count = (long)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    // User already exists
                    return false;
                }
            }

            // 2?? IF NOT EXISTS ? INSERT USER
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO users(playerId, email, password) VALUES(@p,@e,@pw)";
                cmd.Parameters.AddWithValue("@p", playerId);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@pw", password);
                cmd.ExecuteNonQuery();
            }
        }

        return true; // Registration successful
    }


    public bool LoginUser(string playerId, string password)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM users WHERE playerId=@p AND password=@pw";
                cmd.Parameters.AddWithValue("@p", playerId);
                cmd.Parameters.AddWithValue("@pw", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}
