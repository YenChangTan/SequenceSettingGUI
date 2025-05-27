using ReactiveUI;

namespace SequenceSettingGUI.Models;

public class OutputState : ReactiveObject
{
    private bool _isChecked = false;
    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }
}
