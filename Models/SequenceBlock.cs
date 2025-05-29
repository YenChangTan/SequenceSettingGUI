using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;

namespace SequenceSettingGUI.Models;

public class SequenceBlock: ReactiveObject
{
    private int _delayTimeBeforeOutput = 0;
    public int DelayTimeBeforeOutput
    {
        get => _delayTimeBeforeOutput;
        set => this.RaiseAndSetIfChanged(ref _delayTimeBeforeOutput, value);
    }

    public ObservableCollection<CheckedState> Outputs { get; set; } = new ObservableCollection<CheckedState>(Enumerable.Range(0, 16)
                    .Select(_ => new CheckedState())
                    .ToList());

    private int _delayTimeAfterOutput = 0;
    public int DelayTimeAfterOutput
    {
        get => _delayTimeAfterOutput;
        set => this.RaiseAndSetIfChanged(ref _delayTimeAfterOutput, value);
    }

    public ObservableCollection<CheckedState> Inputs { get; set; } = new ObservableCollection<CheckedState>(Enumerable.Range(0, 16)
                    .Select(_ => new CheckedState())
                    .ToList());
    private int _checkingInputRegularTime = 0;
    public int CheckingInputRegularTime
    {
        get => _checkingInputRegularTime;
        set => this.RaiseAndSetIfChanged(ref _checkingInputRegularTime, value);
    }

    private int _checkingInputTimeOut = 0;
    public int CheckingInputTimeout
    {
        get => _checkingInputTimeOut;
        set => this.RaiseAndSetIfChanged(ref _checkingInputTimeOut, value);
    }
}
