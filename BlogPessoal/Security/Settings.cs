namespace BlogPessoal.Security;

public class Settings
{
    private static string secret = "100e97326fb70c7b67f6c46cec7ce3b93fdf1cbc9ba77e1294f20489c0eeec0f";

    public static string Secret
    {
        get => secret;
        set => secret = value;
    }
}