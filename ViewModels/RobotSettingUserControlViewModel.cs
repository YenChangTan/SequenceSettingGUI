using ReactiveUI;
using Avalonia.ReactiveUI;
using SequenceSettingGUI.Views;
using System.Reactive;
using Splat.ApplicationPerformanceMonitoring;
using SequenceSettingGUI.Models;

namespace SequenceSettingGUI.ViewModels;

public partial class RobotSettingUserControlViewModel : ReactiveObject
{
    public MCU _mcu { get; }

    public RobotSettingUserControlViewModel(MCU mcu)
    {
        _mcu = mcu;
    }



}
