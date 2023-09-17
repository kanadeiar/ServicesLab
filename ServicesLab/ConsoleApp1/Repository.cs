using System.Threading.Channels;

namespace ConsoleApp1;

[AttributeUsage(AttributeTargets.Method)]
public class LogAttribute : Attribute
{
    public readonly string Text;

    public LogAttribute(string text)
    {
        Text = text;
    }
}

public class Repository<T> : IRepository<T>
{
    [Log("Before")]
    public void Add(T entity)
    {
        Console.WriteLine("Adding {0}", entity);
    }
    public void Delete(T entity)
    {
        Console.WriteLine("Deleting {0}", entity);
    }
    public void Update(T entity)
    {
        Console.WriteLine("Updating {0}", entity);
    }
    [Log("Before")]
    public IEnumerable<T> GetAll()
    {
        Console.WriteLine("Getting entities");
        return Array.Empty<T>();
    }
    public T? GetById(int id)
    {
        Console.WriteLine("Getting entity {0}", id);
        return default;
    }
}