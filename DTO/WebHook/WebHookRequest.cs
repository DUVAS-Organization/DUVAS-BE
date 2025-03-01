namespace DTO.WebHook;

public class WebHookRequest
{
    public string? error { get; set; }  // Can be string or number, you can change it to int? if it's always a number
    public List<Data>? data { get; set; }
    public override string ToString()
    {
        var dataStrings = data != null ? string.Join(", ", data.ConvertAll(d => d.ToString())) : string.Empty;
        return $"error: {error}, data: [{dataStrings}]";
    }
}

public class Data
{
    public object? Id { get; set; }  // Use object to handle both string and numeric values
    public string? Tid { get; set; }
    public string? Description { get; set; }
    public object? Amount { get; set; }  // Use object to handle numeric values like 599000 and strings
    public object? Cusum_balance { get; set; }  // Same as Amount
    public string? When { get; set; }
    public string? Bank_sub_acc_id { get; set; }
    public string? SubAccId { get; set; }
    public string? BankName { get; set; }
    public string? BankAbbreviation { get; set; }
    public string? VirtualAccount { get; set; }
    public string? VirtualAccountName { get; set; }
    public string? CorresponsiveName { get; set; }
    public string? CorresponsiveAccount { get; set; }
    public string? CorresponsiveBankId { get; set; }
    public string? CorresponsiveBankName { get; set; }
    public override string ToString()
    {
        return $"Id: {Id}, Tid: {Tid}, Description: {Description}, Amount: {Amount}, Cusum_balance: {Cusum_balance}, When: {When}, " +
               $"Bank_sub_acc_id: {Bank_sub_acc_id}, SubAccId: {SubAccId}, BankName: {BankName}, BankAbbreviation: {BankAbbreviation}, " +
               $"VirtualAccount: {VirtualAccount}, VirtualAccountName: {VirtualAccountName}, CorresponsiveName: {CorresponsiveName}, " +
               $"CorresponsiveAccount: {CorresponsiveAccount}, CorresponsiveBankId: {CorresponsiveBankId}, CorresponsiveBankName: {CorresponsiveBankName}";
    }
}