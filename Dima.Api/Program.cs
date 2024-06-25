using Dima.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cnnStr = builder.Configuration.GetConnectionString(name: "DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(
    x =>
    {
        x.UseSqlServer(cnnStr);
    });

// Swagger -> Cria uma GUI para o dev manipular a API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});
builder.Services.AddTransient<Handler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Endpoints -> URL para acesso

app.MapGet("/", () => "Hello World!");

///Toda vez que vier uma requisição do tipo Post para /v1/transactions, 
///vai ser lido do Request, retornado uma Response e, no final, produzido uma Resposta.
app.MapPost(
    "/v1/transactions",
    async (Request request, Handler handler) => await Task.FromResult(handler.Handle(request)))
    .WithName("Transactions: Create")
    .WithSummary("Cria uma nova transação")
    .Produces<Response>();

app.Run();

// Request - Requisição
public class Request
{
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public long CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

// Response - Resposta
public class Response
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

// Handler - Quem manipula a resposta
public class Handler
{
    // Faz todo o processo de criação
    // Persiste no banco...
    public Response Handle(Request request)
    {
        return new Response
        {
            Id = 4,
            Title = request.Title,
        };
    }
}
