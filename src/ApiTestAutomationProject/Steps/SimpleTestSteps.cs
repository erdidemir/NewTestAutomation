using TechTalk.SpecFlow;
using FluentAssertions;

namespace ApiTestAutomationProject.Steps
{
    [Binding]
    public class SimpleTestSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public SimpleTestSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I have a simple test")]
        public void GivenIHaveASimpleTest()
        {
            _scenarioContext["TestData"] = "Simple test data";
        }

        [When(@"I run the test")]
        public void WhenIRunTheTest()
        {
            _scenarioContext["StepCompleted"] = true;
        }

        [Then(@"the test should pass")]
        public void ThenTheTestShouldPass()
        {
            var testData = _scenarioContext.Get<string>("TestData");
            var stepCompleted = _scenarioContext.Get<bool>("StepCompleted");
            
            testData.Should().Be("Simple test data");
            stepCompleted.Should().BeTrue();
        }
    }
} 