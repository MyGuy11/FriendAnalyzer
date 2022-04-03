// Authors = MyGuy, Jasuv

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Analyzer
{
    public class Person
    {
        public string Name { get; private set; }
        public List<string> Tags { get; private set; }
        
        [JsonConstructor]
        public Person(string Name, List<string> Tags)
        {
            this.Name = Name;
            this.Tags = Tags;
        }

        public void AddTag(string inTag)
        {
            Tags.Add(inTag);
        }

        public void AddTag(List<string> inTags)
        {
            foreach (string tag in inTags)
            {
                Tags.Add(tag);
            }
        }
    }
}