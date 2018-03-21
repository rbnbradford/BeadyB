using Gherkin.Ast;
using Xunit.Abstractions;

namespace BeadyB.Loggers
{
    public class ConsoleTestLogger
    {
        private readonly ITestOutputHelper _output;

        public ConsoleTestLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public void LogBeginFeature(Feature feature)
        {
            _output.WriteLine($"\nFeature: {feature.Name}");
        }

        public void LogBeginScenario(ScenarioDefinition scenario)
        {
            _output.WriteLine($"\n  Scenario : {scenario.Name}");
        }

        public void LogCompleteStep(Step step)
        {
            _output.WriteLine($"    {step.Keyword}{step.Text} ✔");
        }
    }
}