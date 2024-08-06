using System.Data;
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
    public string Gender{ get; set; }
    
    public void OnGet()
    {
        var bar = Request.Query["Name"];
        var gender = Request.Query["Gender"];
        Name = $"{bar}";
        if (gender == "1")
        {
            Gender = " Bey!";
        }

        if (gender == "2")
        {
            Gender = " Hanım!";
        }
    }

    public IActionResult OnPost([FromForm] string Name, [FromForm] string Password, [FromForm] string PasswordAgain)
    {
        TempData["Password"] = Password;
        TempData["PasswordAgain"] = PasswordAgain;
        var problemYok = true;
        if (string.IsNullOrEmpty(Password))
        {
            TempData["Password.Error"] = "Password cannot be empty";
            problemYok = false;
        }
        else
        {
            if (Password.Length < 6)
            {
                TempData["Password.Error"] = "Password length should not be less than 6 digits";
                problemYok = false;
            }
            else
            {
                var digitFound = false;
                var lowerCaseFound = false;
                var upperCaseFound = false;
                var symbolFound = false;
                foreach (var letter in Password)
                {
                    int letterCode = (int)letter;
                    if (letter >= '0' && letter <= '9')
                    {
                        digitFound = true;
                    }
                    if (letter >= 'a' && letter <= 'z')
                    {
                        lowerCaseFound = true;
                    }
                    if (letter >= 'A' && letter <= 'Z')
                    {
                        upperCaseFound = true;
                    }
                    else
                    {
                        symbolFound = true;
                    }

                }

                if (!digitFound || !lowerCaseFound || !upperCaseFound || !symbolFound)
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

        if (Password != PasswordAgain)
        {
            TempData["stop"] = "password does not match";
            problemYok = false;
        }

        if (problemYok)
        {
            var kullanıcıvar = false;
            var result = appDbContext.RunSqlCommand("SELECT * FROM user");
            for (int i = 0; i <= result.Count-1; i++)
            {
                if (Name == result[i][0])
                {
                    kullanıcıvar = true;
                    break;
                }
            }
            if (kullanıcıvar)
            {
                TempData["Stop"] = "";
                Console.WriteLine("Update user SET password=\"{Password}\" WHERE userName =\"{Name}\"");
                appDbContext.RunSqlCommand($"Update user SET password=\"{Password}\" WHERE userName =\"{Name}\"");
                TempData["PasswordTrue"] = "your password has been change!";
            }
            else
            {
                TempData["stop"] = "user not found";
            }
            
        }
        
        return RedirectToPage("Welcome");
    }
    
}
