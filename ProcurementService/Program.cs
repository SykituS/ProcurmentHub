using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using ProcurementService.Data;
using ProcurementService.Data.Query;
using ProcurementService.Functions;
using ProcurementService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddDbContext<ProcurementContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("ProcurementContext")));
builder.Services.AddTransient<VerifyLogin>();
builder.Services.AddTransient<CodeGenerator>();
builder.Services.AddTransient<GetUsersQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapGrpcService<ProcurementService.Services.ProcurementService>();

app.Run();
