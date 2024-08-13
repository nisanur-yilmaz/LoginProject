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

        foreach (var row in result)
        {
            if (Name == row[0] && password == row[2])
            {
                TempData["Stop"] = "";
                var userId = row[1];
                var token = Helpers.CreateToken();
                appDbContext.RunSqlCommand($"INSERT INTO session(userId,token) VALUES({userId},'{token}')");
                Response.Cookies.Append("LoginProject2AppSessionToken", token);
                return RedirectToPage("Welcome");
            }
            else
            {
                TempData["Stop"] = "username and password are incorrect";
            }
        }


        return RedirectToPage("Index");
    }
}