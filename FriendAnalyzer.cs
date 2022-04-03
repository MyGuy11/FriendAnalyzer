// Authors = MyGuy, Jasuv

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json; // We're converting the storage medium to JSON
using System.Threading;
using System.Threading.Tasks;

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
        private static Dictionary<string, string> Manual = new(); // Stores the descriptions of all the functions for Help()
        private static ThreadLocal<int> Sebek = new(() => 0); // Keeps track of recurses; Thx to Jeroen van Langen on Stack Overflow for this
        private static bool _break; // Used for Break()
        private static bool exit;

        static FriendAnalyzer()
        {
            PeopleList = new();
            SetPath();
        }

        public static void Main()
        {
            Console.WriteLine("Initializing FriendAnalyzer");
            Console.WriteLine("Analyzing FriendList");
            Task read = Task.Run(() => ReadPeople());
            while (read.IsCompletedSuccessfully == false)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.Write('\n');

            if (PeopleList.Count == 0) { Console.WriteLine("You have no friends!"); }

            Dictionary<string, Action> funcs = new()
            {
                { "people", () => People() }
            };

            int err = Menu(funcs);
            Console.WriteLine("Errorcode: " + err);
            Environment.ExitCode = err;
            Task save = Task.Run(() => SerializePeople());
            Console.WriteLine("Saving People");
            while (save.IsCompleted == false)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.Write('\n');
            Console.WriteLine("Save Complete");

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
        internal static int Menu(Dictionary<string, Action> functions)
        {
            Sebek.Value++;
            Console.WriteLine("Sebek: " + Sebek.Value);
            string input = "NULL"; // Setting a value becuase of PTSD from Unity

            if (Sebek.Value > 1 && functions.ContainsKey("back") == false)
            {
                functions.Add("back", () => Break());
            }

            if (functions.ContainsKey("exit") == false)
            {
                functions.Add("exit", () => exit = true);
            }

            if (functions.ContainsKey("help") == false)
            {
                functions.Add("help", () => Help(functions));
            }

            if (functions.ContainsKey("path") == false)
            {
                functions.Add("path", () => Console.WriteLine(FriendAnalyzerFolderPath));
            }

            if (functions.Keys.Count != functions.Values.Count) { return -1; }

            // Sort the functions before execution
            functions = SortFuncs(functions);

            while (!exit)
            {
                Console.WriteLine("What would you like to do? (Type \"help\" for options)");
                input = Console.ReadLine().ToLower(CultureInfo.CurrentCulture);

                if (functions.ContainsKey(input))
                {
                    functions[input]();
                }
                else
                {
                    Console.WriteLine("That option doesn't exist!");
                }

                if (_break)
                {
                    _break = false;
                    break;
                }
            }

            return 0;
        }

        private static Dictionary<string, Action> SortFuncs(Dictionary<string, Action> funcs)
        {
            Dictionary<string, Action> output = new();
            List<string> tempKeys = funcs.Keys.ToList();
            string tempKey = "";

            for (int j = 0; j < tempKeys.Count; j ++)
            {
                for (int i = 0; i < tempKeys.Count; i++)
                {
                    if (string.Compare(tempKeys[j], tempKeys[i], StringComparison.Ordinal) < 0)
                    {
                        tempKey = tempKeys[i];
                        tempKeys[i] = tempKeys[j];
                        tempKeys[j] = tempKey;
                    }
                }
            }

            foreach (string key in tempKeys)
            {
                output.Add(key, funcs[key]);
            }
            return output;
        }

        // Used for the "back" function, breaks out of the current recursion's loop
        private static void Break()
        {
            _break = true;
        }

        private static void Help(Dictionary<string, Action> list)
        {
            Console.Write('\n');
            Console.WriteLine("Available options:");
            foreach (var kvp in list)
            {
                Console.WriteLine(kvp.Key);
            }
            Console.Write('\n');
        }

        internal static void People()
        {
            Dictionary<string, Action> funcs = new()
            {
                { "addperson", () => AddPerson() },
                { "save", () => SavePeople() },
                { "list", () => ListPeople() },
            };

            Environment.ExitCode = Menu(funcs);
        }

        // Adds a person to the registered list
        internal static void AddPerson()
        {
            Console.WriteLine("Name?");
            string tempName = Console.ReadLine();
            List<string> tempTags = new();
            Console.WriteLine("What tags would you use to describe them?");
            Console.WriteLine("Please write tags one at a time. (\"done\" to stop)");

            string input = "NULL";
            string temp = "NULL";
            while (temp != "done")
            {
                input = Console.ReadLine();
                temp = input.ToLower(CultureInfo.CurrentCulture);
                if (temp != "done") { tempTags.Add(input); }
            }

            PeopleList.Add(new(tempName, tempTags));
        }

        internal static void ListPeople()
        {
            Console.WriteLine("Registered People:");
            foreach (Person person in PeopleList)
            {
                Console.WriteLine(person.Name);
            }
            Console.Write('\n');
        }
        
        internal static void SavePeople()
        {
            Task save = Task.Run(() => SerializePeople());
            while (save.IsCompleted == false)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.Write('\n');
            Console.WriteLine("Save Complete");
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
                using (FileStream stream = File.Open(path, FileMode.OpenOrCreate))
                {
                    await JsonSerializer.SerializeAsync<Person>(stream, person);
                    stream.Close();
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
            else
            {
                FriendAnalyzerFolderDirectory = new DirectoryInfo(FriendAnalyzerFolderPath);
            }

            // Look at every Person.json file and read it into peopleList
            FileInfo[] files = FriendAnalyzerFolderDirectory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name.Contains(".json"))
                {
                    using (FileStream stream = File.Open(file.FullName, FileMode.Open))
                    {
                        PeopleList.Add(await JsonSerializer.DeserializeAsync<Person>(stream));
                        stream.Close();
                    }
                }
            }
        }
    }
}