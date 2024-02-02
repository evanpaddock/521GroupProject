using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using IronParadise.Controllers;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using IronParadise.Models;
using Microsoft.Extensions.Configuration;


namespace IronParadise
{
    public class GPT
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<List<WorkoutPlan>> GetDataAsync(WorkoutPlanRequest request)
        {

            // var builder = new ConfigurationBuilder()
            // .AddUserSecrets<Program>();
            // var configuration = builder.Build();

            // string apiKey = configuration["GPTapiKey"];
            string apiKey = "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";

            // Construct the prompt using the request data
            string prompt = $"Create a custom workout plan for a {request.gender} who is {request.height} inches tall, weighs {request.weight} pounds, and is {request.age} years old. " +
                $"The individual is a {request.fitnessLevel} in fitness and is looking to work out for {request.days} of each week only, these workouts cannot be more than {request.timePerSession} minutes each. The goal is to {request.goal}. " +
                "The plan should follow a push, pull, and legs split as much as possible. " +
                "Format the plan for the specified days with each day's workout detailed. For workout days, specify exercises with sets and reps. " +
                "Provide the workout plan in C# code format, with one list for each day, following this format: " +
                "\nList<Tuple<string, int, int>> DayName = new List<Tuple<string, int, int>>() {" +
                "\n    Tuple.Create(\"Exercise Name\", Sets, Reps)," +
                "\n    // Add more exercises as needed" +
                "\n};";

            var requestData = new
            {
                prompt = prompt,
                max_tokens = 3000
            };

            string contentString = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(contentString, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/engines/text-davinci-003/completions", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return ParseResponse(responseBody);
            }
            catch (HttpRequestException e)
            {
                // Handle exception
                Console.WriteLine($"Error: {e.Message}");
                return null;
            }
        }

        private List<WorkoutPlan> ParseResponse(string responseBody)
        {
            var plans = new List<WorkoutPlan>();

            // Deserialize the JSON response
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
            string workoutData = responseObject.choices[0].text;

            var workoutPlanRegex = new Regex(@"List<Tuple<string, int, int>> (\w+) = new List<Tuple<string, int, int>>\(\) \{([\s\S]*?)\};");
            var exerciseRegex = new Regex(@"Tuple.Create\(""(.*?)"", (\d+), (\d+(?:-\d+)?)\)");

            var workoutPlansMatches = workoutPlanRegex.Matches(workoutData);

            foreach (Match workoutPlan in workoutPlansMatches)
            {
                string dayName = workoutPlan.Groups[1].Value;
                string exercises = workoutPlan.Groups[2].Value;

                var exerciseList = new List<Tuple<string, int, int>>();
                foreach (Match exercise in exerciseRegex.Matches(exercises))
                {
                    string exerciseName = exercise.Groups[1].Value;
                    int sets = int.Parse(exercise.Groups[2].Value);
                    string repMatch = exercise.Groups[3].Value;
                    int reps = repMatch.Contains("-") ? int.Parse(repMatch.Split('-')[0]) : int.Parse(repMatch); // Handle range if present

                    exerciseList.Add(Tuple.Create(exerciseName, sets, reps));
                }

                plans.Add(new WorkoutPlan { DayName = dayName, Exercises = exerciseList });
            }

            return plans;
        }
    }
}
