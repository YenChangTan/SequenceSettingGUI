using System.IO.Pipelines;
using System.Reactive;
using Avalonia.Markup.Xaml.Converters;
using ReactiveUI;
using SequenceSettingGUI.Interfaces;

namespace SequenceSettingGUI.Models;

public class Output : ReactiveObject
{
    private readonly int _index;

    public int Index => _index;

    public ITCPClientService _itcpClientService;
    public Output(ITCPClientService itcpClientService, int index)
    {
        _itcpClientService = itcpClientService;
        _index = index;
        WriteOutputOnCommand = ReactiveCommand.Create(WriteOutputOn);
        WriteOutputOffCommand = ReactiveCommand.Create(WriteOutputOff);
    }


    private bool _isOn = false;
    public bool IsOn
    {
        get => _isOn;
        set => this.RaiseAndSetIfChanged(ref _isOn, value);
    }

    public bool IsNotOn
    {
        get => !_isOn;
    }

    public ReactiveCommand<Unit, Unit> WriteOutputOnCommand { get; }

    public void WriteOutputOn()
    {
        bool result = _itcpClientService.WriteOutput(true, _index);
        IsOn = true;
    }

    public ReactiveCommand<Unit, Unit> WriteOutputOffCommand { get; }

    public void WriteOutputOff()
    {
        bool result = _itcpClientService.WriteOutput(false, _index);
        IsOn = false;
    }
}
