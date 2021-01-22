using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using System.Linq;

namespace Lab09
{
    delegate bool Evaluate(int number);
    class Program
    {
        static void Main(string[] args)
        {
            AnalyzeJsonNeighborhood();
        }
        static void AnalyzeJsonNeighborhood()
        {
            //find the path, read it, turn it into strings
            string path = @"../../../../data.json";
            string jsonString = File.ReadAllText(path);
            //deserialize the string we got from json
            Root root = JsonConvert.DeserializeObject<Root>(jsonString);

            outputAll(root);
            filterNonNamed(root);
            removeDuplicates(root);
            masterQuery(root);
            QueryMethods(root);
        }

        public static void outputAll(Root root)
        {
            int count = 1;
            foreach (Feature feature in root.features)
            {
                Console.WriteLine($"{count}, {feature.properties.neighborhood}");
                count++;
            }
        }
        public static void filterNonNamed(Root root)
        {
            int counter = 1;
            var query = from feature in root.features
                        where feature.properties.neighborhood != ""
                        select feature.properties.neighborhood;

            foreach (var feature in query)
            {
                Console.WriteLine($"{counter}, {feature}");
                counter++;
            }
        }
        public static void removeDuplicates(Root root)
        {
            int counter = 1;
            var query = from feature in root.features
                        where feature.properties.neighborhood != ""
                        select feature.properties.neighborhood;
            var noDuplicates = query.Distinct();
            
            foreach (var feature in noDuplicates)
            {
                Console.WriteLine($"{counter}, {feature}");
                counter++;
            }
        }
        public static void masterQuery(Root root)
        {
            int counter = 1;
            var query = (from feature in root.features
                        where (feature.properties.neighborhood != "")
                        select (feature.properties.neighborhood)).Distinct();

            foreach (var feature in query)
            {
                Console.WriteLine($"master {counter}, {feature}");
                counter++;
            }
        }
        public static void QueryMethods(Root root)
        {
            int counter = 1;
            var query = root.features
                         .Select(feature => new {feature.properties.neighborhood});

            foreach (var feature in query)
            {
                Console.WriteLine($"using method {counter}, {feature}");
                counter++;
            }
        }
    }


    //https://jsonutils.com/
    //website converts JSON files into classes! Neat way to get this done fast.

    public class Geometry
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class Properties
    {
        public string zip { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string address { get; set; }
        public string borough { get; set; }
        public string neighborhood { get; set; }
        public string county { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public Properties properties { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }
}
