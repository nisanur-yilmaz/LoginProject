namespace LoginProject2.Services;

class Helpers
{
    public static bool IsPasswordValid(string password)
    {
        var digitFound = false;
        var lowerCaseFound = false;
        var upperCaseFound = false;
        var symbolFound = false;
        foreach (var letter in password)
        {
            switch (letter)
            {
                case >= '0' and <= '9':
                    digitFound = true;
                    break;
                case >= 'a' and <= 'z':
                    lowerCaseFound = true;
                    break;
                case >= 'A' and <= 'Z':
                    upperCaseFound = true;
                    break;
                default:
                    symbolFound = true;
                    break;
            }
        }

        return digitFound && lowerCaseFound && upperCaseFound && symbolFound;
    }

    public static bool IsPasswordLengthValid(string password)
    {
        var problemsiz = true;
        if (password.Length < 6)
        {
            problemsiz = false;
        }

        return problemsiz;
    }
}