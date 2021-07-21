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

                foreach (var button in stateValue.Buttons)
                {
                    _testOutputHelper.WriteLine($"  Button: {button.Label}, {button.Transition}");
                }

                if (stateValue.Input != null)
                {
                    _testOutputHelper.WriteLine("  Input:");
                    _testOutputHelper.WriteLine($"    Handler:");
                    if (stateValue.Input.Handler != null)
                    {
                        _testOutputHelper.WriteLine($"      Name: {stateValue.Input.Handler.Name}");

                        // foreach (var (key, value) in stateValue.Input.Handler.Parameters)
                        // {
                        //     _testOutputHelper.WriteLine($"      Parameter: {key} -> {value}");
                        // }
                        foreach (var (key, value) in stateValue.Input.Handler.Switch)
                        {
                            _testOutputHelper.WriteLine($"      Switch: {key} -> {value}");
                        }
                    }
                    
                    _testOutputHelper.WriteLine("    Options:");
                    foreach (var option in stateValue.Input.Options)
                    {
                        _testOutputHelper.WriteLine($"      {option.Text}, {option.Transition}");
                    }
                    
                    _testOutputHelper.WriteLine($"    DefaultTransition: {stateValue.Input.DefaultTransition}");
                }

                _testOutputHelper.WriteLine(string.Empty);
            }
        }
    }
}