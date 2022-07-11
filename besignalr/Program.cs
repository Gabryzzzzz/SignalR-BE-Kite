using BE.SignalR.Hub;
using BE.SignalR.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "cors",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader()
                                 .AllowCredentials()
                                 .WithOrigins("http://localhost:4200", "https://signalr-fe.azurewebsites.net/");
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISignalRChatService, SignalRChatService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();

app.UseCors("cors");

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers();
    endpoints.MapHub<SignalRChatHub>("/chathub"); //Associo l'entrata all'hub all'endpoint
});

app.Run();
