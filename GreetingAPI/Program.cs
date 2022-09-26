using GreetingAPI;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<GreetingDb>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();
   
app.MapGet("/todoitems", async (GreetingDb db) =>
    await db.Greetings.Select(x => new GreetingItemDTO(x)).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, GreetingDb db) =>
    await db.Greetings.FindAsync(id)
        is Greeting todo
            ? Results.Ok(new GreetingItemDTO(todo))
            : Results.NotFound());

app.MapPost("/todoitems", async (GreetingItemDTO todoItemDTO, GreetingDb db) =>
{
    var todoItem = new Greeting
    {
        IsComplete = todoItemDTO.IsComplete,
        Content = todoItemDTO.Content
    };

    db.Greetings.Add(todoItem);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todoItem.Id}", new GreetingItemDTO(todoItem));
});

app.MapPut("/todoitems/{id}", async (int id, GreetingItemDTO todoItemDTO, GreetingDb db) =>
{
    var todo = await db.Greetings.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Content = todoItemDTO.Content;
    todo.IsComplete = todoItemDTO.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, GreetingDb db) =>
{
    if (await db.Greetings.FindAsync(id) is Greeting todo)
    {
        db.Greetings.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(new GreetingItemDTO(todo));
    }

    return Results.NotFound();
});

app.Run();
