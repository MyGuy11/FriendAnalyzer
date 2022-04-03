// Authors = MyGuy, Jasuv

using System;
using System.Collections.Generic;

namespace Analyzer
{
    [Serializable] // I don't know if this is necessary for JSON serialization
    public class Person
    {
        public string Name { get; private set; }
        public List<string> Tags { get; private set; }

        public Person(string inName, List<string> inTags)
        {
            Name = inName;
            Tags = inTags;
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