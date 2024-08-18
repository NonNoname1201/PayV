namespace PayV;

public struct PaymentData(
    string paymentStatus,
    string paymentMethod,
    string paymentCurrency,
    float paymentAmount,
    string from,
    string to,
    DateTime expirationTime)
{
    private string PaymentStatus { get; set; } = paymentStatus;
    private string PaymentMethod { get; set; } = paymentMethod;
    private string PaymentCurrency { get; set; } = paymentCurrency;
    private float PaymentAmount { get; set; } = paymentAmount;
    private string From { get; set; } = from;
    private string To { get; set; } = to;
    private DateTime ExpirationTime { get; set; } = expirationTime;
    
    public DateTime GetExpirationTime() => ExpirationTime;
}