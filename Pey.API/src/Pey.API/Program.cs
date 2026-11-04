using Pey.Modules.Auctions;
using Pey.Modules.Bidding;
using Pey.Modules.Identity;
using Pey.Modules.Notifications;
using Pey.Modules.Payments;
using Pey.Shared;
using Pey.Shared.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();
builder.Services.AddShared();

builder.Services
    .AddIdentityModule()
    .AddAuctionsModule()
    .AddBiddingModule()
    .AddPaymentsModule()
    .AddNotificationsModule();

var app = builder.Build();

// Pipeline'ın en başında olmalı ki sonraki her adımdaki exception'ları yakalasın.
app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
