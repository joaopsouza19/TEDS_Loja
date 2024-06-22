using Microsoft.EntityFrameworkCore;
using loja.data;
using loja.models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar conexao com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options=>options.UseMySql(connectionString,new MySqlServerVersion(new Version(8, 0, 36))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/createproduto", async (LojaDbContext dbContext, Produto newProduto) =>
{
    dbContext.Produtos.Add(newProduto);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createproduto/{newProduto.Id}", newProduto);
});

app.MapGet("/produtos", async (LojaDbContext dbContext) =>
{
    var produtos = await dbContext.Produtos.ToListAsync();
    return Results.Ok(produtos);
});

app.MapGet("/produtos/{id}", async (int id, LojaDbContext dbContext) =>
{
    var produto = await dbContext.Produtos.FindAsync(id);
    if (produto == null){
        return Results.NotFound($"Produto with ID {id} not found.");
    }
    return Results.Ok(produto);
});

// Endpoint para atualizar um produto existente
app.MapPut("/produtos/{id}", async (int id, LojaDbContext dbContext, Produto updatedProduto) =>
{
    var existingProduto = await dbContext.Produtos.FindAsync(id);
    if (existingProduto == null){
        return Results.NotFound($"Produto with ID {id} not found");
    }

    // Atualiza os dados do existingProduto 
    existingProduto.Nome = updatedProduto.Nome;
    existingProduto.Preco = updatedProduto.Preco;
    existingProduto.Fornecedor = updatedProduto.Fornecedor;

    // Salva no banco de dados
    await dbContext.SaveChangesAsync();

    // Retorna para o cliente que invocou o endpoint
    return Results.Ok(existingProduto);

});

// Parte Cliente ---------------------------------------------------------------------------

// Endpoint para criar um novo cliente
app.MapPost("/createcliente", async (LojaDbContext dbContext, Cliente newCliente) =>
{
    dbContext.Clientes.Add(newCliente);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createcliente/{newCliente.Id}", newCliente);
}
);

// Endpoint para mostrar todos os clientes
app.MapGet("/clientes", async (LojaDbContext dbContext) =>
{
    var clientes = await dbContext.Clientes.ToListAsync();
    return Results.Ok(clientes);
});

// Endpoint para mostrar cliente por ID
app.MapGet("/clientes/{id}", async (int id, LojaDbContext dbContext) =>
{
    var clientes = await dbContext.Clientes.FindAsync(id);
    if (clientes == null){
        return Results.NotFound($"Cliente with ID {id} not found.");
    }
    return Results.Ok(clientes);
});

// Endpoin para atualizar cliente por ID
app.MapPut("/clientes/{id}", async (int id, LojaDbContext dbContext, Cliente updatedFornecedor) =>
{
    var existingCliente = await dbContext.Clientes.FindAsync(id);
    if (existingCliente == null){
        return Results.NotFound($"Fornecedor with ID {id} not found");
    }

    // Atualiza os dados do existingCliente 
    existingCliente.Nome = updatedFornecedor.Nome;
    existingCliente.Cpf = updatedFornecedor.Cpf;
    existingCliente.Email = updatedFornecedor.Email;

    // Salva no banco de dados
    await dbContext.SaveChangesAsync();

    // Retorna para o cliente que invocou o endpoint
    return Results.Ok(existingCliente);

});


// Desafio 1 - Fornecedor --------------------------------------------------------------------
// Endpoint para criar um novo fornecedor
app.MapPost("/createfornecedor", async (LojaDbContext dbContext, Fornecedor newFornecedor) =>
{
    dbContext.Fornecedores.Add(newFornecedor);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/createfornecedor/{newFornecedor.Id}", newFornecedor);
}
);

// Endpoint para mostrar todos os fornecedores
app.MapGet("/fornecedores", async (LojaDbContext dbContext) =>
{
    var fornecedores = await dbContext.Fornecedores.ToListAsync();
    return Results.Ok(fornecedores);
});

// Endpoint para mostrar fornecedor por ID
app.MapGet("/fornecedores/{id}", async (int id, LojaDbContext dbContext) =>
{
    var fornecedores = await dbContext.Fornecedores.FindAsync(id);
    if (fornecedores == null){
        return Results.NotFound($"Fornecedores with ID {id} not found.");
    }
    return Results.Ok(fornecedores);
});

// Endpoint para atualizar fornecedor por ID
app.MapPut("/fornecedores/{id}", async (int id, LojaDbContext dbContext, Fornecedor updatedFornecedor) =>
{
    var existingFornecedor = await dbContext.Fornecedores.FindAsync(id);
    if (existingFornecedor == null){
        return Results.NotFound($"Produto with ID {id} not found");
    }

    // Atualiza os dados do existingFornecedor 
    existingFornecedor.Cnpj = updatedFornecedor.Cnpj;
    existingFornecedor.Nome = updatedFornecedor.Nome;
    existingFornecedor.Endereco = updatedFornecedor.Endereco;
    existingFornecedor.Email = updatedFornecedor.Email;
    existingFornecedor.Telefone = updatedFornecedor.Telefone;

    // Salva no banco de dados
    await dbContext.SaveChangesAsync();

    // Retorna para o cliente que invocou o endpoint
    return Results.Ok(existingFornecedor);

});



var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
