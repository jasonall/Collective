Collective
==========

Collective is a small set of helpful extensions for .NET collections, lists, and enumerators. These extensions falls into three categories:

1. Collection converters: These allow you to efficiently convert a collection of one type to a collection of a different type, without having to copy the collection or allocate additional memory.

2. ListDictionary: A new collection type with the properties of both a List and a Dictionary.

3. ListExtensions: Helpful extensions to the IList type, including 'Exists', 'Find', 'FindFirst', 'FindLast', etc.

Collection Converters:

Let's say you've got a simple type called Customer:

class Customer
{
    public string Name;
    public Address Address;
    public string Title;
}

Your project contains a list of Customers: 

List<Customer> customers:

Christmas is coming up, and you'd like to send holiday greetings to all your customers. To do this, you need to call SendGreetings(IList<string> names). But the SendGreetings method requires a list of strings, and you've got a list of Customers. What to do?

You could create a copy of the list, like this:

IList<string> names = customers.ConvertAll(c => c.Name);

But this uses extra memory. If all you need is a read-only list containing the new type, you can do it with very little memory overhead by using a ListConverter instead:

IList<string> names = customers.ToListConverter(c => c.Name);

The difference between ConvertAll and ToListConverter is that ConvertAll allocates a new list, while ToListConverter does not actually allocate anything. Rather, it performs the conversion on demand whenever you iterate through the list, or access an element in the list. For this reason, the converter routine itself should be memory efficient.
