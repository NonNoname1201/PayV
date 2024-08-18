using PayV;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Dictionary<string, PaymentData>>();
builder.Services.AddHostedService<ExpirationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var temporaryLinks = new Dictionary<string, PaymentData>();

app.MapPost("/getlink", (string from, string to, float amount) =>
{
    var link = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString();
    temporaryLinks.Add(link, new PaymentData
    (
        "Pending", // PaymentStatus
        "undefined", // PaymentMethod
        "undefined", // PaymentCurrency
        amount,
        from,
        to,
        DateTime.UtcNow.AddMinutes(5) // ExpirationTime
    ));
    return link;
}).WithName("GetLink").WithOpenApi();

app.MapPost("/payment/{id}", (string id, bool approval) =>
        temporaryLinks.ContainsKey(id)
            ? (approval ? $"Payment with id {id} was successful" : $"Payment with id {id} was declined")
            : "Payment link does not exists or was expired")
    .WithName("FinishPayment").WithOpenApi();

app.Run();