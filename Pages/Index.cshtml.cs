using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoginProject2.Pages;

public class IndexModel : PageModel
{
     public string? Name => (string?)TempData[nameof(Name)];
    public string? Password => (string?)TempData[nameof(Password)];
    private readonly AppDbContext appDbContext;

    public IndexModel()
    {
        appDbContext = new AppDbContext();
    }
    public void OnGet()
    {
        SayHello("necip");
    }
    
    // tanımla
    static void SayHello(string SayHello)
    {
        Console.WriteLine("Welcome " + SayHello+"!");
    }

    
    
    

    public IActionResult OnPost([FromForm] string Name, [FromForm] string password)
    {
        TempData["Name"] = Name;
        TempData["Password"] = password;
        if (string.IsNullOrEmpty(Name))
        {
            TempData["Name.Error"] = " Name cannot be empty";
        }

        if (string.IsNullOrEmpty(password))
        {
            TempData["Password.Error"] = "Password cannot be empty";
        }
        var result = appDbContext.RunSqlCommand("SELECT * FROM user");
        /*for (int i = 0; i <=result.Count-1; i++)
        {
            if (Name == result[i][0] && password == result[i][2])
            {
                TempData["Stop"] = "";
                return RedirectToPage("Welcome" , new{name=$"{Name}", gender=$"{result[i][3]}"} );

            }
            else
            {
                TempData["Stop"] = "username and password are incorrect";
            }
        }*/

        foreach (var row in result)
        {
            if (Name == row[0] && password == row[2])
            {
                TempData["Stop"] = "";
                return RedirectToPage("Welcome" , new{name=$"{Name}", gender=$"{row[3]}"} );
 
            }
            else
            {
                TempData["Stop"] = "username and password are incorrect";
            }
        }


        return RedirectToPage("Index");
    }
    
}
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
