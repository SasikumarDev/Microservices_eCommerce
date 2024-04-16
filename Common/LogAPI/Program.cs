using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(new ConnectionSettings(new Uri(builder.Configuration["elasticURI"] ?? "")));
builder.Services.AddCors(confgi =>
{
    confgi.AddPolicy("logmonitor", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseCors("logmonitor");
app.Run();
