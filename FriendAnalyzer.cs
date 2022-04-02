// Authors = MyGuy, Jasuv

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json; // We're converting the storage medium to JSON
using System.Threading;

// This is useful if you wanna find info about c# stuff
// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/

namespace Analyzer
{
    public static class FriendAnalyzer
    {
        private static string FriendAnalyzerFolderPath; // {LocalAppData}/FriendAnalyzer/
        private static DirectoryInfo FriendAnalyzerFolderDirectory; // DirectoryInfo of the above directory, might be useful later
        public static List<Person> PeopleList { get; internal set; } // Stores the People object of all people registered
        private static List<string> PeopleNames = new(); // Stores the names of the people in peopleList 
        private static List<string> PeopleTags = new(); // Stores all the Tags that People in your list possess
        private static List<List<string>> Manual = new(); // Stores the descriptions of all the functions for Help()
        private static ThreadLocal<int> Sebek = new(() => 0); // Keeps track of recurses; Thx to Jeroen van Langen on Stack Overflow for this
        private static bool _break; // Used for Break()

        static FriendAnalyzer()
        {
            PeopleList = new();
            SetPath();
        }

        public static void Main()
        {

            Console.WriteLine("Initializing FriendAnalyzer");
            Console.WriteLine("Analyzing FriendList");
            Console.WriteLine("\0");
            ReadPeople();

            if (PeopleList.Count == 0) { Console.WriteLine("You have no friends!"); }
            Console.WriteLine("Errorcode: " + Menu(new(), new()));
        }


        // This is how you tell the IDE what info to display when u interact with the Method
        /// <summary>
        /// The menu system for the program.<br />
        /// The number of <paramref name="options"/> and <paramref name="methods"/> must be equal.
        /// </summary>
        /// <returns> Returns an int error code.<br />
        /// (0 = ok)<br />
        /// (-1 = inequal amount of <paramref name="options"/> and <paramref name="methods"/>) 
        /// </returns>
        internal static int Menu(List<string> options, List<Action> methods)
        {
            Sebek.Value++;
            string input = "NULL";
            bool exit = false;

            if (options.Contains("list") == false)
            {
                options.Add("list");
                methods.Add(() => List(new() { options, methods })); // So, this is a delegate
                // Think of it as a reference to a method with a specific parameter
            }

            if (options.Contains("path") == false)
            {
                options.Add("path");
                methods.Add(() => Console.WriteLine(FriendAnalyzerFolderPath));
            }

            if (options.Contains("help") == false)
            {
                options.Add("help");
                methods.Add(() => Help(options));
            }

            if (Sebek.Value > 2 && options.Contains("back") == false)
            {
                options.Add("back");
                methods.Add(() => Break());
            }

            if (options.Contains("exit") == false)
            {
                options.Add("exit");
                methods.Add(() => exit = true);
            }

            if (options.Count != methods.Count) { return -1; }

            while (!exit)
            {
                Console.WriteLine("What would you like to do? (Type \"help\" for options)");
                input = Console.ReadLine().ToLower(CultureInfo.CurrentCulture);
                
                for (int i = 0; i < options.Count; i++)
                {
                    if (input.Length == 0) { break; }

                    if (input == options[i])
                    {
                        methods[i]();
                    }
                }

                if (_break)
                {
                    _break = false;
                    break;
                }
            }

            return 0;
        }

        // Used for the "back" function, breaks out of the current recursion's loop
        private static void Break()
        {
            _break = true;
        }

        private static void List(List<IList> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                IList list = lists[i];
                for (int j = 0; j < list.Count; j++)
                {
                    Console.WriteLine(list.ToString() + '[' + j + "]: " + list[j]);
                }
            }
        }

        private static void Help(List<string> list)
        {
            Console.Write('\n');
            Console.WriteLine("Available options:");
            foreach (string item in list)
            {
                Console.WriteLine(item);
            }
            Console.Write('\n');
        }

        internal static void People()
        {
            
        }

        // Adds a person to the registered list
        internal static void AddPerson()
        {
            string input = "\0";
            Console.WriteLine("Name?");
            input = Console.ReadLine().ToLower(CultureInfo.CurrentCulture);
        }


        // Sets the path to the folder containing data files
        private static void SetPath()
        {
            FriendAnalyzerFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FriendAnalyzer");

            // This exists because Microsoft doesn't know that their own operating system isn't supposed to return '/' in it's path strings
            // Thanks, Microsoft
            if (Environment.OSVersion.VersionString.Contains("Windows"))
            {
                int index;
                string temp;
                string temp2;
                while (FriendAnalyzerFolderPath.Contains('/'))
                {
                    index = FriendAnalyzerFolderPath.IndexOf('/');
                    temp = FriendAnalyzerFolderPath.Substring(0, index);

                    // Apparently this means Substring (c# 8+)
                    // I think it's because it grabs all chars from the index onwards (hence the ..)
                    // This replaces FriendAnalyzerFolderPath.Substring(index + 1);
                    temp2 = FriendAnalyzerFolderPath[(index + 1)..];
                    FriendAnalyzerFolderPath = temp + '\\' + temp2;
                }
            }
        }

        // Serializes all registered people to their own .json file
        // This is done so that people can easily import and export people, without having to edit
        // the .json file outside of the program
        internal static async void SerializePeople()
        {
            string path;
            foreach (Person person in PeopleList)
            {
                path = Path.Combine(FriendAnalyzerFolderPath, person.Name + ".json");
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
            // I know i could do !File.Exists(path), but it's explicitly definied for clarity
            if (Directory.Exists(FriendAnalyzerFolderPath) == false)
            {
                Console.WriteLine("Creating Friend Analyzer data directory at " + FriendAnalyzerFolderPath);
                FriendAnalyzerFolderDirectory = Directory.CreateDirectory(FriendAnalyzerFolderPath);
            }

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