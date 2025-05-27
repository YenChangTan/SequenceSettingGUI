using ReactiveUI;

namespace SequenceSettingGUI.Models;

public class OutputInput : ReactiveObject
{
    private Output _output;
    public Output output
    {
        get => _output;
        set => this.RaiseAndSetIfChanged(ref _output, value);
    }
    private Input _input;
    public Input input
    {
        get => _input;
        set => this.RaiseAndSetIfChanged(ref _input, value);
    }

    public OutputInput(Output outputarg, Input inputarg)
    {
        _output = outputarg;
        _input = inputarg;
    }

}
