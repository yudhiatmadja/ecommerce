public class LoginRequest
{
    public string Username {get; set; } = null!;
    public string Password {get; set; } = null!;
}

public class LoginResponse
{
    public string Token {get; set; } = null!;
}


public class RegisterRequest
{
    public string Username {get; set; } = null!;
    public string Password {get; set; } = null!;
}