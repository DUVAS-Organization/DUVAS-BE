using Microsoft.CSharp.RuntimeBinder;
using Repositories;
using Repositories.IRepository;

namespace API.Service;

public class TokenExchangeService
{
    private readonly JwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly TokenDictionaryService _tokenDictionaryService;

    public TokenExchangeService(JwtService jwtService, IUserRepository userRepository, TokenDictionaryService tokenDictionaryService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _tokenDictionaryService = tokenDictionaryService;
    }
    
    public string HandleExchangeRequest(string code)
    {
        Console.WriteLine("Handling code exchange...");

        // Print current code store (for debugging)
        _tokenDictionaryService.PrintAllCodes();

        string? email = _tokenDictionaryService.GetEmail(code);
        if (string.IsNullOrEmpty(email))
        {
            throw new RuntimeBinderException("Invalid code");
        }

        var user = _userRepository.GetUserByGmailOrPhoneAsync(email).Result;
        if (user == null)
        {
            throw new RuntimeBinderException("Invalid code");
        }

        // Remove the code after successful exchange
        _tokenDictionaryService.RemoveCode(code);

        // Generate and return JWT token
        return _jwtService.GenerateToken(user);
    }
}