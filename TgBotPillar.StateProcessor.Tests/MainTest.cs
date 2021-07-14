using System;
using System.Threading.Tasks;
using TgBotPillar.Core.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace TgBotPillar.StateProcessor.Tests
{
    public class MainTest
    {
        private readonly StateProcessorService _stateProcessor;
        private readonly ITestOutputHelper _testOutputHelper;

        public MainTest(IStateProcessorService stateProcessor, ITestOutputHelper testOutputHelper)
        {
            _stateProcessor = (StateProcessorService) stateProcessor;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task StateProcessor_InitializeOnExamples()
        {
            await _stateProcessor.Initialization;
            foreach (var (stateKey, stateValue) in _stateProcessor.States)
            {
                _testOutputHelper.WriteLine(stateKey);
                if (!string.IsNullOrEmpty(stateValue.Text))
                {
                    _testOutputHelper.WriteLine($"  Text: {stateValue.Text.Replace('\n', ' ')}");
                }

                foreach (var (buttonKey, buttonValue) in stateValue.Buttons)
                {
                    _testOutputHelper.WriteLine($"  Button: {buttonKey}, {buttonValue.Label}, {buttonValue.NextState}");
                }

                if (!string.IsNullOrEmpty(stateValue.Transition))
                {
                    _testOutputHelper.WriteLine($"  Transition: {stateValue.Transition}");
                }

                if (!string.IsNullOrEmpty(stateValue.Input))
                {
                    _testOutputHelper.WriteLine($"  Input: {stateValue.Input}");
                }

                _testOutputHelper.WriteLine(string.Empty);
            }
        }
    }
}