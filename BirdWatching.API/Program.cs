using BirdWatching.API.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureAppServices(builder.Configuration);

var app = builder.Build();
app.ConfigureApp(app.Environment);
app.Run();
