﻿using System.Threading;
using System;
using System.Drawing;

namespace Analyzer
{
    class FriendAnalyzer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing FriendAnalyzer");
            Thread.Sleep(2000);
            Console.Write("Analyzing FriendList");
            for (int i = 0; i < 4; i++) // Replace with a while loop that waits for analysis task (Here's where async comes into play, Jasuv)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            Console.WriteLine("Done");
            Thread.Sleep(500);
            Console.WriteLine("Candidates found!"); 
            Console.WriteLine("User: Pega");
            Console.WriteLine("Reason: Mutual Server");
            Console.WriteLine("Reason: Mutual Friends");
            Console.WriteLine("Reason: Compatible");
            Thread.Sleep(500);
            Console.WriteLine("Send friend request(s)? (y/n)");
            string answer = Console.In.ReadLine().ToLower();
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
