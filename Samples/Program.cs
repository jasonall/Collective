using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Collective;

namespace Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ListConverterSample();
        }

        public static void ListConverterSample()
        {
            // Create a list of customers.
            List<Customer> customers = new List<Customer>();
            customers.Add(new Customer { Name = "George", Title = "Principal", Active = true });
            customers.Add(new Customer { Name = "Jane", Title = "Baker", Active = true });
            customers.Add(new Customer { Name = "Yuko", Title = "Firefighter", Active = false });

            // The inefficient way - copy the list:
            List<string> inefficientNames = customers.ConvertAll(c => c.Name);
            SendGreetings(inefficientNames);

            // The efficient way - avoid copying the list and convert items on demand:
            IList<string> efficientNames = customers.ToListConverter(c => c.Name);
            SendGreetings(efficientNames);

            // Or, if you don't want to use the extension method:
            IList<string> efficientNames2 = new ListConverter<Customer, string>(customers, c => c.Name);
            SendGreetings(efficientNames2);

            // The same can be done for Collections using CollectionConverter, and for Enumerables using EnumerableConverter.
        }

        public static void SendGreetings(IList<string> names)
        {
            foreach (string name in names)
            {
                Console.WriteLine("Hello " + name);
            }
        }
    }
}
