using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExaltIt.katas.SameHashCode
{
    class Program
    {
        static List<StringHolder> LsMatchHolder = new List<StringHolder>();
        static Dictionary<int, string> LsStringMap = new Dictionary<int, string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Generation of the 3 strings with the same hashcode");
            Console.WriteLine($"{DateTime.Now.ToString("T")} \t Init Tasks");
            Task<List<string>>[] lsTasks = new Task<List<string>>[5];

            for (int i = 0; i < 5; i++)
            {
                int taskIndex = i + 1;

                Console.WriteLine($"{DateTime.Now.ToString("T")} \t Load Task : {taskIndex}");

                lsTasks[i] = Task.Run(() => { return GenerateSameHashCodeString(new Random(), taskIndex); });
            }

            while (true)
            {
                Task.WaitAny(lsTasks);
                Task<List<string>> completedTask = lsTasks.Where(x => x.IsCompleted && x.Result != null).FirstOrDefault();

                String stringA = completedTask.Result[0];
                String stringB = completedTask.Result[1];
                String stringC = completedTask.Result[2];


                if (stringA.GetHashCode() == stringB.GetHashCode() && !String.Equals(stringA, stringB) &&
                    stringB.GetHashCode() == stringC.GetHashCode() && !String.Equals(stringB, stringC) &&
                    !String.Equals(stringA, stringC)
    )
                {
                    Console.WriteLine("Result : ");
                    Console.WriteLine($"{stringA} with {stringA.GetHashCode()}");
                    Console.WriteLine($"{stringB} with {stringB.GetHashCode()}");
                    Console.WriteLine($"{stringC} with {stringC.GetHashCode()}");
                    break;
                }

            }

            Console.ReadLine();
        }

        static List<string> GenerateSameHashCodeString(Random random, int index)
        {
            Console.WriteLine($"{DateTime.Now.ToString("T")} \t start Task : {index}");
            while (true)
            {
                string tempString = string.Empty;

                string generatedString = GenerateRandomString(random);
                int hashCode = generatedString.GetHashCode();

                lock (LsStringMap)
                {
                    if (LsStringMap == null)
                    {
                        Console.WriteLine($"{DateTime.Now.ToString("T")} \t Strop Task : {index}");
                        return null;
                    }

                    if (LsStringMap.TryGetValue(hashCode, out tempString) && tempString != generatedString)
                    {
                        LsMatchHolder.Add(new StringHolder() { HashCode = hashCode, StringValue = tempString });
                        LsMatchHolder.Add(new StringHolder() { HashCode = hashCode, StringValue = generatedString });

                        var queryCheckMatch = LsMatchHolder.Where(x => x.HashCode == hashCode).Select(x => x.StringValue).Distinct();

                        if (queryCheckMatch.Count() >= 3)
                        {
                            var stringRst = queryCheckMatch.Take(3).ToList();

                            Console.WriteLine($"{DateTime.Now.ToString("T")} \t Task : {index} \t Match Found : {string.Join(" - ", stringRst)} with HashCode : {hashCode}");

                            LsStringMap = null;

                            Console.WriteLine($"{DateTime.Now.ToString("T")} \t Strop Task : {index}");

                            return stringRst;
                        }
                    }

                    LsStringMap[hashCode] = generatedString;
                }
            }
        }

        static string GenerateRandomString(Random random)
        {
            string generatedString = string.Empty;

            for (int i = 0; i < 10; i++)
            {
                generatedString += (char)random.Next(char.MaxValue);
            }
            return generatedString;
        }
    }

    class StringHolder
    {
        public string StringValue { get; set; }
        public int HashCode { get; set; }
    }
}
