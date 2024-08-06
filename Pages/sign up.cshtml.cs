using System.Data;
using System.Diagnostics.SymbolStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoginProject2.Pages;

public class sign_up : PageModel
{
    public void OnGet()
    {
    }

    public string? Name => (string?)TempData[nameof(Name)];
    public string? Password => (string?)TempData[nameof(Password)];
    public string? PasswordAgain => (string?)TempData[nameof(PasswordAgain)];
    public string? Gender => (string?)TempData[nameof(Gender)];

    private readonly AppDbContext appDbContext;

    public sign_up()
    {
        appDbContext = new AppDbContext();
    }

    public RedirectToPageResult OnPost([FromForm] string Name, [FromForm] string Password,
        [FromForm] string PasswordAgain, [FromForm] string Gender)
    {
        TempData["Name"] = Name;
        TempData["Password"] = Password;
        TempData["PasswordAgain"] = PasswordAgain;
        TempData["Gender"] = Gender;
        var promblemYok = true;
        if (string.IsNullOrEmpty(Name))
        {
            TempData["Name.Error"] = "Name cannot be empty";
            promblemYok = false;
        }

        if (string.IsNullOrEmpty(Password))
        {
            TempData["Password.Error"] = "Password cannot be empty";
            promblemYok = false;
        }
        else
        {
            if (Password.Length < 6)
            {
                TempData["Password.Error"] = "Password length should not be less than 6 digits";
                promblemYok = false;
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
                        "The password must contain at least 1 number, 1 lowercase letter, 1 uppercase letter and 1 symbol.";
                    promblemYok = false;
                }
            }
        }

        if (string.IsNullOrEmpty(PasswordAgain))
        {
            TempData["PasswordAgain.Error"] = "Password Aganin cannot be empty";
            promblemYok = false;
        }

        if (string.IsNullOrEmpty(Gender))
        {
            TempData["Gender.Error"] = "Gender cannot be empty";
            promblemYok = false;
        }

        if (Password != PasswordAgain)
        {
            TempData["stop"] = "password does not match";
            promblemYok = false;
        }

        if (promblemYok)
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
                TempData["UserName.Error"] = $"there is already a user {Name}";
            }
            else
            {
                TempData["Welcome"] = $"Welcome {Name}!";
                TempData["Stop"] = "";
                appDbContext.RunSqlCommand($"INSERT INTO user(userName,password,gender_id)values(\"{Name}\",\"{Password}\",{Gender})");
            }

        }

        return RedirectToPage("sign up");
     }

  }