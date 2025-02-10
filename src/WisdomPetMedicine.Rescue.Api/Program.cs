using Microsoft.AspNetCore.Builder;
using WisdomPetMedicine.Rescue.Api.ApplicationServices;
using WisdomPetMedicine.Rescue.Api.Extensions;
using WisdomPetMedicine.Rescue.Api.Infrastructure;
using WisdomPetMedicine.Rescue.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRescueDb(builder.Configuration);
builder.Services.AddScoped<AdopterApplicationService>();
builder.Services.AddScoped<IRescueRepository, RescueRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.EnsureRescueDbIsCreated();
app.UseHttpsRedirection();
app.UseAuthorization();

// Opsætter at vi vil bruge CloudEvents (Pubsub benytter dette)
app.UseCloudEvents();
// Handler for at kunne subscribe på CloudEvents
app.MapSubscribeHandler();

app.MapControllers();

app.Run();