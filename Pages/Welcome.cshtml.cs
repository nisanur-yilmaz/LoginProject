using System.Data;
using LoginProject2.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoginProject2.Pages;

public class Welcome : PageModel
{
    public string? Password => (string?)TempData[nameof(Password)];
    public string? PasswordAgain => (string?)TempData[nameof(PasswordAgain)];
    private readonly AppDbContext appDbContext;

    public Welcome()
    {
        appDbContext = new AppDbContext();
    }

    public string Name { get; set; }
    public string Gender { get; set; }
    public string Id { get; set; }

    public void OnGet()
    {
        /* var bar = Request.Query["Name"];
         var gender = Request.Query["Gender"];
         Name = $"{bar}";
         if (gender == "1")
         {
             Gender = " Bey!";
         }

         if (gender == "2")
         {
             Gender = " Hanım!";
         }*/
        var result = appDbContext.RunSqlCommand(
            "SELECT user.userName,gender.name,session.token FROM user INNER JOIN gender ON user.gender_id=gender.id INNER JOIN session ON user.id=session.userId");
        var token = Request.Cookies["LoginProject2AppSessionToken"];
        Console.WriteLine(token);
        foreach (var row in result)
        {
            if (token == row[2])
            {
                Name = row[0];
                if (row[1] == "female")
                {
                    Gender = " hanım";
                }
                else
                {
                    Gender = " bey";
                }
            }
        }
    }


    public IActionResult OnPost([FromForm] string password, [FromForm] string PasswordAgain)
    {
        TempData["Password"] = password;
        TempData["PasswordAgain"] = PasswordAgain;
        var problemYok = true;
        if (string.IsNullOrEmpty(password))
        {
            TempData["Password.Error"] = "Password cannot be empty";
            problemYok = false;
        }
        else
        {
            var problemsiz = Helpers.IsPasswordLengthValid(password);
            if (!problemsiz)
            {
                TempData["Password.Error"] = "Password length should not be less than 6 digits";
                problemYok = false;
            }
            else
            {
                var sifreProblemsiz = Helpers.IsPasswordValid(password);
                if (!sifreProblemsiz)
                {
                    TempData["Password.Error"] =
                        "The password must contain at least 1 number, 1 lowercase letter, 1 uppercase letter and 1 symbol";
                    problemYok = false;
                }
            }
        }

        if (string.IsNullOrEmpty(PasswordAgain))
        {
            TempData["PasswordAgain.Error"] = "Password Again cannot be empty";
            problemYok = false;
        }

        if (password != PasswordAgain)
        {
            TempData["Stop"] = "password does not match";
            problemYok = false;
        }

        if (problemYok)
        {
            Console.WriteLine("1");
            var result = appDbContext.RunSqlCommand(
                "SELECT session.token,session.userId FROM user INNER JOIN session ON user.id=session.userId");
            
            var token = Request.Cookies["LoginProject2AppSessionToken"];
            Console.WriteLine("2");
            foreach (var row in result)
            {
                Console.WriteLine("3");
                Console.WriteLine(row[0]);
                if (token == row[0])
                {
                    Console.WriteLine("4");
                    var Id = row[1];
                    Console.WriteLine($"Update user SET password=\"{password}\" WHERE id =\"{Id}\"");
                    appDbContext.RunSqlCommand($"Update user SET password=\"{password}\" WHERE id =\"{Id}\"");
                    TempData["PasswordTrue"] = "your password has been changed!";
                    Console.WriteLine("5");
                }

            }
            // todo username e gore degıl, userId ye gore SQL sorgusu atılmalı
            // todo userId cookıe den alınan bılgı ıle elde edılmelı
        }

        return RedirectToPage("Welcome");
    }
}