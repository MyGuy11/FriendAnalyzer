using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;

namespace Analyzer
{
    class FriendAnalyzer
    {
        string friendAnalyzerFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".1FriendAnalyzer");
        string peopleListTextPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ".1FriendAnalyzer\\PeopleList.txt");
        public List<Person> peopleList = new List<Person>();
        static void Main(string[] args)
        {
            FriendAnalyzer newAnalyzer = new FriendAnalyzer();
            newAnalyzer.TheAnalyzer();
        }
        
        void TheAnalyzer()
        {
            if (!Directory.Exists(friendAnalyzerFolderPath))  
            {  
                Directory.CreateDirectory(friendAnalyzerFolderPath);
            } 
            Console.WriteLine("Initializing FriendAnalyzer");
            Thread.Sleep(2000);
            Console.Write("Analyzing FriendList");
            for (int i = 0; i < 4; i++)
            {
                Console.Write(".");
                Thread.Sleep(1000);
            }
            
            if (File.Exists(peopleListTextPath))
            {
                //write proper friend request stuff here

                /*Console.WriteLine("\nSend friend request(s)? (y/n)");
                string answer = Console.In.ReadLine().ToLower();
                while (!(answer.Equals("y") || answer.Equals("n")))
                {
                    Console.WriteLine("I said (y/n), loner");
                    answer = Console.In.ReadLine().ToLower();
                }
                if (answer.Equals("y")) { Console.WriteLine("Friend request(s) sent"); }
                if (answer.Equals("n")) { Console.WriteLine("Friend request(s) not sent"); }*/
            }
            else
            {
                Console.WriteLine("\nIt appears your people list is empty...");
                Console.WriteLine("Please input at least 3 people into the list\n");
                Thread.Sleep(500);
                for(int i = 0; i < 3; i++)
                {
                    AddPeople();
                }
                using (StreamWriter personList = File.CreateText(peopleListTextPath))
                {
                    foreach(Person person in peopleList)
                    {
                        personList.WriteLine(person.name);
                        foreach(string tag in person.tags)
                        {
                            personList.WriteLine(tag);
                        }
                    }
                }
                Console.Write("Creating new people list");
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("\n");
                TheAnalyzer();
            }
        }

        void AddPeople()
        {
            Console.WriteLine("Name? ");
            string inName = Console.In.ReadLine().ToLower();
            Console.WriteLine("Input at least 1 tag for " + inName + ". Type stop to finish...");
            List<string> inTags = new List<string>();
            bool exit = false;
            while(!exit)
            {
                string inTag = Console.In.ReadLine().ToLower();
                if (inTag != "stop") {inTags.Add(inTag);}
                else {exit = true;}
            }
            Console.WriteLine("Adding " + inName + " to people list...\n");
            Person newPerson = new Person(inName, inTags);
            peopleList.Add(newPerson);
        }
    }

    public class Person
    {
        public string name;
        public List<string> tags;

        public Person(string inName, List<string> inTags)
        {
            name = inName;
            tags = inTags;
        }   
    }
}