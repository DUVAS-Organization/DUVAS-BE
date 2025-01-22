namespace API.Service;

public class TokenDictionaryService
{
    private readonly IDictionary<string, string> _codeStore;
    private readonly TimeSpan _codeExpirationTime;

    public TokenDictionaryService()
    {
        _codeStore = new Dictionary<string, string>();
        _codeExpirationTime = TimeSpan.FromMinutes(5);
    }

    public string GenerateCode(string email)
    {
        var code = Guid.NewGuid().ToString();
        _codeStore[code] = email;
        Console.WriteLine($"Code generated: {code} for {email}");

        // Schedule removal of the code after expiration time
        Task.Delay(_codeExpirationTime).ContinueWith(_ => RemoveCode(code));
        return code;
    }

    public string? GetEmail(string code)
    {
        _codeStore.TryGetValue(code, out var email);
        return email;
    }

    public void RemoveCode(string code)
    {
        _codeStore.Remove(code);
        Console.WriteLine($"Code {code} removed from store.");
    }

    public void PrintAllCodes()
    {
        foreach (var kvp in _codeStore)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}