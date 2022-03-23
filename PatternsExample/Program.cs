using System;
using System.Threading.Tasks;

namespace PatternsExample
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            UserSerlization userSerlization = new UserSerlization(@"C:\Users\ladon\Desktop\hello.txt", @"C:\Users\ladon\Desktop\users.json", new MyJsonSerialization());

            await userSerlization.Serializable();


            UserSerlization userSerlization1 = new UserSerlization(@"C:\Users\ladon\Desktop\hello.txt", @"C:\Users\ladon\Desktop\users.xml", new MyXmlSerialization ());

            await userSerlization1.Serializable();


            Console.WriteLine(1);
        }
    }
}
