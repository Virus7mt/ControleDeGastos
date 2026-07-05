using GastosResidenciais.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os controllers (endpoints da API) e configura para o enum
// TipoTransacao ser enviado/recebido como texto ("Receita"/"Despesa")
// em vez de número (0/1) — fica muito mais legível no front-end.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configura o Entity Framework Core usando SQLite.
// O arquivo "gastos.db" fica salvo na pasta do projeto e garante que os
// dados persistam mesmo depois de fechar a aplicação (requisito do enunciado).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gastos.db"));

// Libera o CORS para o front-end React (que roda em outra porta) conseguir
// chamar essa API sem ser bloqueado pelo navegador.
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // porta padrão do Vite
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Garante que o banco de dados (e as tabelas) existam antes da aplicação subir.
// Usei EnsureCreated() em vez de Migrations para deixar o projeto simples de
// rodar (não precisa instalar a ferramenta dotnet-ef nem rodar comandos extras).
// Em um projeto maior/real, o ideal seria usar Migrations.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
