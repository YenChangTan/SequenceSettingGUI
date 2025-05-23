using ReactiveUI;
using Avalonia.ReactiveUI;
using SequenceSettingGUI.Views;
using System.Reactive;
using Splat.ApplicationPerformanceMonitoring;

namespace SequenceSettingGUI.ViewModels;

public partial class RobotSettingUserControlViewModel : ReactiveObject
{
    private string _ipAddress = "";
    public string IPAddress
    {
        get => _ipAddress;
        set => this.RaiseAndSetIfChanged(ref _ipAddress, value);
    }

    private bool _isConnected = false;
    public bool IsConnected
    {
        get => _isConnected;
        set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }

    public ReactiveCommand<Unit, Unit> ConnectCommand { get; }

    public RobotSettingUserControlViewModel()
    {
        ConnectCommand = ReactiveCommand.Create(ConnectToTCP);
    }



    public void ConnectToTCP()
    {
        IsConnected = !IsConnected;
    }



}
