using Microsoft.Data.Sqlite;

public class UserRepository
{
    public void GetUser(string username)
    {
        using var conn = new SqliteConnection("Data Source=app.db");
        conn.Open();
        var query = "SELECT * FROM users WHERE username = '" + username + "'";
        using var cmd = new SqliteCommand(query, conn);
        var reader = cmd.ExecuteReader();
    }
}
