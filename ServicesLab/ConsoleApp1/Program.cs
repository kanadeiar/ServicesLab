namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Опытное приложение");

        //var customerRepository = new Repository<Customer>();
        var customerRepository = DynamicProxy<IRepository<Customer>>.Decorate(new Repository<Customer>());
        
        var customer = new Customer
        {
            Id = 1,
            Name = "Customer 1",
            Address = "Address 1"
        };
        customerRepository.Add(customer);
        customerRepository.Update(customer);
        customerRepository.Delete(customer);
        var elems = customerRepository.GetAll();
        var one = customerRepository.GetById(1);


        Console.WriteLine("Нажать любую кнопку для завершения работы ...");
        Console.ReadKey();
    }
}