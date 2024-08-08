using System.Data;
using Microsoft.EntityFrameworkCore;

namespace LoginProject2.Services;

public class AppDbContext : DbContext
{
    public string DbPath { get; }
    public object Users { get; set; }

    public AppDbContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        // DbPath = System.IO.Path.Join(path, "lesson1.db");
        DbPath = "/Users/nisanur/Desktop/c sharp/LoginProject2/LoginProject2/lesson1.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite($"Data Source={DbPath}");

    public List<List<string>> RunSqlCommand(string sqlCommand)
    {
        List<List<string>> resultArray = [];

        var command = Database.GetDbConnection().CreateCommand();
        command.CommandText = sqlCommand;
        command.CommandType = CommandType.Text;
        Database.OpenConnection();
        var result = command.ExecuteReader();
        while (result.Read())
        {
            List<string> rowResult = new List<string>();
            for (var i = 0; i < result.FieldCount; i++)
            {
                rowResult.Add(Convert.ToString(result[i]));
            }

            resultArray.Add(rowResult);
        }

        return resultArray;
    }
}