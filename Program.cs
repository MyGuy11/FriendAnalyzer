using System;

namespace FriendAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing FriendAnalyzer");
            Console.WriteLine("Analyzing FriendList....");
            Console.WriteLine("Candidates found!");
            Console.WriteLine("User: Pega");
            Console.WriteLine("Reason: Mutual Server");
            Console.WriteLine("Reason: Mutual Friends");
            Console.WriteLine("Reason: Compatible");
            Console.WriteLine("Send friend request(s)? (y/n)");
            string answer = Console.In.ReadLine();
            while (!(answer.Equals("y") || answer.Equals("Y") || answer.Equals("n") || answer.Equals("N")))
            {
                Console.WriteLine("I said (y/n), loner");
                answer = Console.In.ReadLine();
            }
            if (answer.Equals("y") || answer.Equals("Y")) { Console.WriteLine("Friend request(s) sent"); }
            if (answer.Equals("n") || answer.Equals("N")) { Console.WriteLine("Friend request(s) not sent"); }
        }
    }
}
