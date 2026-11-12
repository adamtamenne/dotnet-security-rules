using Microsoft.Data.Sqlite;

public class UserRepository
{
    public void GetUser(string username)
    {
        using var conn = new SqliteConnection("Data Source=app.db");
        conn.Open();
        using var cmd = new SqliteCommand("SELECT * FROM users WHERE username = @user", conn);
        cmd.Parameters.AddWithValue("@user", username);
        var reader = cmd.ExecuteReader();
    }
}
