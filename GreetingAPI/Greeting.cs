using Microsoft.EntityFrameworkCore;

namespace GreetingAPI;

public class Greeting
{
    public int Id { get; set; }

    public string? Content { get; set; }

    public bool IsComplete { get; set; }

    public string? Secret { get; set; }
}

public class GreetingItemDTO
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public bool IsComplete { get; set; }

    public GreetingItemDTO() { }
    public GreetingItemDTO(Greeting GreetingItem) =>
    (Id, Content) = (GreetingItem.Id, GreetingItem.Content);
}

class GreetingDb : DbContext
{
    public GreetingDb(DbContextOptions<GreetingDb> options)
        : base(options) { }

    public DbSet<Greeting> Greetings => Set<Greeting>();
}
