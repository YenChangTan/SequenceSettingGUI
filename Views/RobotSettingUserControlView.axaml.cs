using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using System;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SequenceSettingGUI.ViewModels;

namespace SequenceSettingGUI.Views;

public partial class RobotSettingUserControlView : ReactiveUserControl<RobotSettingUserControlViewModel>
{
    public RobotSettingUserControlView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
