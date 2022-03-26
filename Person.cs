using System;
using System.Collections.Generic;

namespace Analyzer
{
    [Serializable]
    public class Person
    {
        public string name { get; private set; }
        public List<string> tags { get; private set; }

        public Person(string inName, List<string> inTags)
        {
            name = inName;
            tags = inTags;
        }

        public void AddTag(string inTag){
            tags.Add(inTag);
        }

        public void AddTag(List<string> inTags){
            foreach (string tag in inTags)
            {
                tags.Add(tag);
            }
        }
    }
}