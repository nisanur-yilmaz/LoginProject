using System.Data;
using System.Diagnostics.SymbolStore;
using LoginProject2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LoginProject2.Pages;

public class SignUp : PageModel
{
    public void OnGet()
    {
    }

    public string? Name => (string?)TempData[nameof(Name)];
    public string? Password => (string?)TempData[nameof(Password)];
    public string? PasswordAgain => (string?)TempData[nameof(PasswordAgain)];
    public string? Gender => (string?)TempData[nameof(Gender)];

    private readonly AppDbContext appDbContext;

    public SignUp()
    {
        appDbContext = new AppDbContext();
    }

    public RedirectToPageResult OnPost([FromForm] string name, [FromForm] string password,
        [FromForm] string passwordAgain, [FromForm] string gender)
    {
        TempData["Name"] = name;
        TempData["Password"] = password;
        TempData["PasswordAgain"] = passwordAgain;
        TempData["Gender"] = gender;
        var problemYok = true;
        if (string.IsNullOrEmpty(name))
        {
            TempData["Name.Error"] = "Name cannot be empty";
            problemYok = false;
        }

        if (string.IsNullOrEmpty(password))
        {
            TempData["Password.Error"] = "Password cannot be empty";
            problemYok = false;
        }
        else
        {
            var problemsiz = Helpers.IsPasswordLengthValid(password);
            if(!problemsiz)
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
                        "The password must contain at least 1 number, 1 lowercase letter, 1 uppercase letter and 1 symbol.";
                    problemYok = false;
                }
            }
        }

        if (string.IsNullOrEmpty(passwordAgain))
        {
            TempData["PasswordAgain.Error"] = "Password Again cannot be empty";
            problemYok = false;
        }

        if (string.IsNullOrEmpty(gender))
        {
            TempData["Gender.Error"] = "Gender cannot be empty";
            problemYok = false;
        }

        if (password != passwordAgain)
        {
            TempData["Stop"] = "password does not match";
            problemYok = false;
        }

        if (problemYok)
        {
            var kullanıcıvar = false;
            var result = appDbContext.RunSqlCommand("SELECT * FROM user");
            for (int i = 0; i <= result.Count - 1; i++)
            {
                if (name == result[i][0])
                {
                    kullanıcıvar = true;
                    break;
                }
            }

            if (kullanıcıvar)
            {
                TempData["UserName.Error"] = $"there is already a user {name}";
            }
            else
            {
                TempData["Welcome"] = $"Welcome {name}!";
                TempData["Stop"] = "";
                appDbContext.RunSqlCommand(
                    $"INSERT INTO user(userName,password,gender_id)values(\"{name}\",\"{password}\",{gender})");
            }
        }

        return RedirectToPage("sign up");
    }
}