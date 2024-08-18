using PayV;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var temporaryLinks = new Dictionary<string, PaymentData>() { };

app.MapPost("/getlink", (string from, string to, float amount) =>
{
    var link = Guid.NewGuid().ToString() + DateTime.UtcNow.Ticks.ToString();
    temporaryLinks.Add(link, new PaymentData
    (
        "Pending",   // PaymentStatus
        "undefined", // PaymentMethod
        "undefined", // PaymentCurrency
        amount,
        from,
        to
    ));
    return link;
}).WithName("GetLink").WithOpenApi();

app.MapPost("/payment/{id}", (string id, bool approval) => 
    approval ? $"Payment with id {id} was successful" : $"Payment with id {id} was declined")
    .WithName("FinishPayment").WithOpenApi();

app.Run();
