using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleTables;

namespace API_Client
{
    class Joke
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("setup")]
        public string Setup { get; set; }

        [JsonPropertyName("punchline")]
        public string Punchline { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            var keepTheJokesComing = true;
            while (keepTheJokesComing)
            {
                Console.WriteLine($"\nChoose: (R)andom Joke, (T)en Jokes, (P)ick the category of your random joke, (Q)uit");
                var choice = Console.ReadLine().ToUpper();

                // pick the joke category
                if (choice == "P")
                {
                    Console.WriteLine($"Category: ");
                    var jokeCategory = Console.ReadLine();

                    var client = new HttpClient();
                    var responseAsStream = await client.GetStreamAsync($"https://official-joke-api.appspot.com/jokes/{jokeCategory}/random");

                    var jokes = await JsonSerializer.DeserializeAsync<List<Joke>>(responseAsStream);
                    var table = new ConsoleTable("Type", "Setup", "Punchline");

                    foreach (var joke in jokes)
                    {
                        table.AddRow(joke.Type, joke.Setup, joke.Punchline);
                    }
                    table.Write();
                }

                // to Quit
                if (choice == "Q")
                {
                    keepTheJokesComing = false;
                    Console.WriteLine($"Have a funny day!");
                }

                // Get 10 jokes
                if (choice == "T")
                {
                    var client = new HttpClient();
                    var responseAsStream = await client.GetStreamAsync("https://official-joke-api.appspot.com/jokes/ten");

                    var jokes = await JsonSerializer.DeserializeAsync<List<Joke>>(responseAsStream);
                    var table = new ConsoleTable("Type", "Setup", "Punchline");

                    foreach (var joke in jokes)
                    {
                        table.AddRow(joke.Type, joke.Setup, joke.Punchline);
                    }

                    table.Write();
                }

                // Get Random Joke
                if (choice == "R")
                {
                    var client = new HttpClient();
                    var responseAsStream = await client.GetStreamAsync("https://official-joke-api.appspot.com/jokes/random");

                    var jokes = await JsonSerializer.DeserializeAsync<Joke>(responseAsStream);
                    var table = new ConsoleTable("Type", "Setup", "Punchline");
                    table.AddRow(jokes.Type, jokes.Setup, jokes.Punchline);

                    table.Write();
                }
            }
        }
    }
}