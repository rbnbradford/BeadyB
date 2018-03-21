using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BeadyB.Loggers;
using Gherkin;
using Gherkin.Ast;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace BeadyB
{
    public abstract class Context<T> where T : Context<T>
    {
        private readonly Dictionary<string, Action<IEnumerable<string>>> _stepDefinitions =
            new Dictionary<string, Action<IEnumerable<string>>>();

        private readonly ConsoleTestLogger _testLogger;

        protected Context(ITestOutputHelper output)
        {
            _testLogger = new ConsoleTestLogger(output);
            DefineStepImplementations();
        }

        protected void Define(string stepTemplate, Action<IEnumerable<string>> action)
        {
            _stepDefinitions[stepTemplate] = action;
        }

        [Theory]
        [MemberData(nameof(RunFeatures))]
        public void RunScenario(
            string featureName,
            string scenarioName,
            Feature feature,
            ScenarioDefinition scenarioDefinition)
        {
            _testLogger.LogBeginFeature(feature);
            _testLogger.LogBeginScenario(scenarioDefinition);

            foreach (var step in scenarioDefinition.Steps)
            {
                var extractions = ExtractTemplateAndArguments(step.Text);
                RunStep(step, extractions.StepTemplate, extractions.Arguments);
            }
        }

        public static IEnumerable<object[]> RunFeatures()
        {
            foreach (var feature in GetFeaturesToRun().Select(ParseFeature))
            foreach (var scenario in feature.Children)
                yield return new object[] {feature.Name, scenario.Name, feature, scenario};
        }

        private static IEnumerable<string> GetFeaturesToRun()
        {
            var conf = GetConfig();
            var context = typeof(T).Name;

            foreach (var feature in conf)
                if (feature.Value.Contains(context))
                    yield return feature.Key;
        }

        private void RunStep(Step step, string stepTemplate, IEnumerable<string> arguments)
        {
            if (!_stepDefinitions.ContainsKey(stepTemplate)) throw new StepNotDefinedException(stepTemplate);
            _stepDefinitions[stepTemplate](arguments);
            _testLogger.LogCompleteStep(step);
        }

        private static (string StepTemplate, List<string> Arguments) ExtractTemplateAndArguments(string stepText)
        {
            var arguments = new List<string>();
            var stepTemplate = stepText;
            while (stepTemplate.Contains("\'"))
            {
                var first = stepTemplate.IndexOf("\'", StringComparison.Ordinal);
                var second = stepTemplate.IndexOf("\'", first + 1, StringComparison.Ordinal);
                var substring = stepTemplate.Substring(first, second - first + 1);
                arguments.Add(substring.Substring(1, substring.Length - 2));
                stepTemplate = stepTemplate.Replace(substring, "*");
                Console.WriteLine(substring);
                Console.WriteLine(stepTemplate);
            }

            return (StepTemplate: stepTemplate, Arguments: arguments);
        }

        private static Dictionary<string, string[]> GetConfig()
        {
            using (var stream = File.OpenRead(Path.Combine(ProjectDirectory, "BDD", "Conf.json")))
            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
                return JsonSerializer.Deserialize<Dictionary<string, string[]>>(jsonTextReader);
        }

        private static Feature ParseFeature(string featureFilePath)
        {
            return GherkinParser
                .Parse(Path.Combine(ProjectDirectory, "BDD", "Features", $"{featureFilePath}.feature"))
                .Feature;
        }

        private static readonly JsonSerializer JsonSerializer = new JsonSerializer();

        private static readonly Parser GherkinParser = new Parser();

        private static readonly string ProjectDirectory =
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        private void DefineStepImplementations()
        {
            foreach (var methodInfo in GetType().GetMethods())
            {
                var attribute = methodInfo.GetCustomAttribute<StepAttribute>(true);
                if (attribute != null)
                {
                    Define(
                        attribute.StepTemplate,
                        args => methodInfo.Invoke(this, new object[] {args})
                    );
                }
            }
        }
    }
}
