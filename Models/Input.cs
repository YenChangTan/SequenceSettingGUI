using ReactiveUI;

namespace SequenceSettingGUI.Models;

public class Input : ReactiveObject
{
    private readonly int _index;

    public int Index => _index;
    private bool _isOn = false;
    public bool IsOn
    {
        get => _isOn;
        set => this.RaiseAndSetIfChanged(ref _isOn, value);
    }

    public Input(int index)
    {
        _index = index;
    }


}
