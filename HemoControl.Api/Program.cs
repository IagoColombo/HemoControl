using HemoControl.Core.Data;
using HemoControl.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Environment.SetEnvironmentVariable("ConnectionStrings__Default", "Server=localhost;Port=3306;Database=hemocontrol;User=root;Password=;");

builder.Services.AddDbContext<HemoControlDbContext>();
builder.Services.AddTransient<PacienteService>();
builder.Services.AddTransient<RetiradaService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
