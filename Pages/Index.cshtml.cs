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

    public IActionResult OnGet()
    {
       var token = Request.Cookies["LoginProject2AppSessionToken"];
        var result2 =
            appDbContext.RunSqlCommand(
                $"SELECT session.token FROM user INNER JOIN session ON user.id=session.userId WHERE session.token='{token}'");
        if (result2.Count > 0)
        {
            return RedirectToPage("Welcome");
        }

        return null;
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

        var result = appDbContext.User.ToList();
        foreach (var row in result)
        {
            if (Name == row.userName && password == row.password)
            {
                TempData["Stop"] = "";
                var userId = row.id;
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