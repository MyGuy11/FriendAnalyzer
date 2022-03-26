using System.Diagnostics.CodeAnalysis;
// Authors = MyGuy, Jasuv

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;
using System.Text.Json; // We're converting the storage medium to JSON

namespace Analyzer
{
    public static class FriendAnalyzer
    {
        private static string friendAnalyzerFolderPath; // LocalAppData/FriendAnalyzer/
        public static List<Person> peopleList { get; internal set; } // Stores the People object of all people registered
        private static List<string> peopleNames = new(); // Stores the names of the people in peopleList 
        private static List<string> peopleTags = new(); // Stores all the Tags that People in your list possess
        private static bool exit;

        static FriendAnalyzer()
        {
            peopleList = new();
        }

        public static void Main()
        {
            Console.WriteLine("Initializing FriendAnalyzer");
            SetPath();
            Console.Write("Analyzing FriendList");
            ReadPeople();
            if (peopleList.Count == 0)
            {
                Console.WriteLine("You have no friends!");
            }
            string input = "NULL";
            while (!exit) // while exit == false
            {
                Console.WriteLine("What would you like to do?");
                input = Console.In.ReadLine();
                
                if (string.Compare("AddPerson", input) >= 0)
                {
                    AddPerson();
                }

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Do you want to exit FriendAnalyzer? (y/n)");
                    
                    while (input != "y" || input != "n")
                    {
                        input = Console.In.ReadLine();
                        if (input.Equals("y", StringComparison.OrdinalIgnoreCase)) { exit = true; break; }
                        else if (input.Equals("n", StringComparison.OrdinalIgnoreCase)) { break; }
                        else { Console.WriteLine("(y/n)"); }
                    }
                }
            }
        }

        // Adds a person to the registered list
        private static void AddPerson()
        {
            // Ask for name and tags
        }


        // Sets the path to the folder containing data files
        private static void SetPath()
        {
            friendAnalyzerFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FriendAnalyzer");

            // This exists because Microsoft doesn't know that their own operating system isn't supposed to return '/' in it's path strings
            // Thanks, Microsoft
            if (Environment.OSVersion.VersionString.Contains("Windows"))
            {
                int index;
                string temp;
                string temp2;
                while (friendAnalyzerFolderPath.Contains('/'))
                {
                    index = friendAnalyzerFolderPath.IndexOf('/');
                    temp = friendAnalyzerFolderPath.Substring(0, index);
                    temp2 = friendAnalyzerFolderPath.Substring(index + 1);
                    friendAnalyzerFolderPath = temp + '\\' + temp2;
                }
            }
        }

        // Serializes all registered people to their own .json file
        // This is done so that people can easily import and export people, without having to edit
        // the .json file outside of the program
        internal static async void SerializePeople()
        {
            string path;
            foreach (Person person in peopleList)
            {
                path = Path.Combine(friendAnalyzerFolderPath, person.name + ".json");
                using (FileStream stream = File.Create(path))
                {
                    await JsonSerializer.SerializeAsync<Person>(stream, person);
                    await stream.DisposeAsync();
                }
            }
        }

        // Reads all the .json files in the data folder and puts the Person in peopleList
        internal static async void ReadPeople()
        {
            // Look at every Person.json file and read it into peopleList
        }

        // OLD ANALYZER
        // Sorry for scrapping your work, Jasuv

        /* 
        private void TheAnalyzer()
        {
            if (!Directory.Exists(friendAnalyzerFolderPath))  
            {  
                Directory.CreateDirectory(friendAnalyzerFolderPath);
            } 
            
            Thread.Sleep(2000);
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
                if (answer.Equals("n")) { Console.WriteLine("Friend request(s) not sent"); }
            }
            else
            {
                Console.WriteLine("\nIt appears your people list is empty...");
                Console.WriteLine("Please input at least 3 people into the list\n");
                Thread.Sleep(500);
                for (int i = 0; i < 3; i++)
                {
                    AddPeople();
                }
                using (StreamWriter personList = File.CreateText(peopleListTextPath))
                {
                    foreach (Person person in peopleList)
                    {
                        personList.WriteLine(person.Name);
                        foreach (string tag in person.Tags)
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

        private void AddPeople()
        {
            Console.WriteLine("Name? ");
            string inName = Console.In.ReadLine().ToLower();
            Console.WriteLine("Input at least 1 tag for " + inName + ". Type stop to finish...");
            List<string> inTags = new();
            bool exit = false;
            while (!exit)
            {
                string inTag = Console.In.ReadLine().ToLower();
                if (inTag != "stop") { inTags.Add(inTag); }
                else { exit = true; }
            }
            Console.WriteLine("Adding " + inName + " to people list...\n");
            Person newPerson = new(inName, inTags);
            peopleList.Add(newPerson);
        }
    }
    */
    }
}