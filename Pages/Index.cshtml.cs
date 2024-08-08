using System.Data;
using LoginProject2.Services;
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
                return RedirectToPage("Welcome", new { name = $"{Name}", gender = $"{row[3]}" });
            }
            else
            {
                TempData["Stop"] = "username and password are incorrect";
            }
        }


        return RedirectToPage("Index");
    }
}

