using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SequenceSettingGUI.Interfaces;
using System.Reactive.Linq;
using DynamicData;
using System.Collections.ObjectModel;
using SequenceSettingGUI.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;
using SequenceSettingGUI.Views;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using Avalonia.Controls;
using System.Timers;

namespace SequenceSettingGUI.Models;

public class MCU : ReactiveObject
{

    private string _ip;
    public string IP
    {
        get => _ip;
        set => this.RaiseAndSetIfChanged(ref _ip, value);
    }

    private int _port;
    public int Port
    {
        get => _port;
        set => this.RaiseAndSetIfChanged(ref _port, value);
    }

    private System.Timers.Timer CheckInputTimer;

    private ObservableCollection<OutputInput> _outputsInputs = new ObservableCollection<OutputInput> { };
    public ObservableCollection<OutputInput> outputsInputs
    {
        get => _outputsInputs;
        set => this.RaiseAndSetIfChanged(ref _outputsInputs, value);
    }

    private ITCPClientService _itcpClientService;

    public MCU(ITCPClientService itcpClientService)
    {
        WriteOutputCommand = ReactiveCommand.CreateFromTask(WriteOutput);
        ConnectToMCUCommand = ReactiveCommand.Create(ConnectToMCU);
        //WriteOutputCommand = ReactiveCommand.Create(WriteOutput);
        _itcpClientService = itcpClientService;
        for (int i = 0; i < 16; i++)
        {
            outputsInputs.Add(new OutputInput(new Output(_itcpClientService, i), new Input(i)));
        }
        CheckInputTimer = new System.Timers.Timer(1000);
        CheckInputTimer.AutoReset = true;
        CheckInputTimer.Elapsed += UpdateInput;
    }

    public ReactiveCommand<Unit, Unit> ConnectToMCUCommand { get; }

    public void ConnectToMCU()
    {
        _itcpClientService.SetIPAddress(IP, Port);
        _itcpClientService.Connect();
        CheckInputTimer.Start();
    }
    public ReactiveCommand<Unit, Unit> WriteOutputCommand { get; }
    private async Task WriteOutput()
    {
        var dialog = new OutputsSettingDialogWindow(new OutputsSettingDialogWindowViewModel(new bool[16]));

        // Get the top-level window (you can pass it in better with DI or view service)
        var parent = App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime life
            ? life.MainWindow
            : null;

        if (parent is null) return;
        dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        bool[] result = await dialog.ShowDialog<bool[]>(parent);

        if (result != null)
        {
            _itcpClientService.WriteOutput(result);
            UpdateOutput(result);
        }
        else
        {
            Console.WriteLine("User canceled the dialog.");
        }
    }

    public void UpdateOutput(bool[] OutputArray)
    {
        if (OutputArray.Length == 16)
        {
            for (int i = 0; i < 16; i++)
            {
                outputsInputs[i].output.IsOn = OutputArray[i];
            }
        }
    }

    private void UpdateInput(object sender, ElapsedEventArgs e)
    {
        bool[] result = _itcpClientService.ReadInput();
        for (int i = 0; i < 16; i++)
        {
            outputsInputs[i].input.IsOn = result[i];
        }
    }


    

}
