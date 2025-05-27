using ReactiveUI;
using Avalonia.ReactiveUI;
using SequenceSettingGUI.Views;
using System.Reactive;
using SequenceSettingGUI.Models;
using SequenceSettingGUI.Interfaces;

namespace SequenceSettingGUI.ViewModels;

public partial class MCUSettingUserControlViewModel : ReactiveObject
{
    public MCU _mcu { get; }

    public MCUSettingUserControlViewModel(MCU mcu)
    {
        _mcu = mcu;
    }

    
}
