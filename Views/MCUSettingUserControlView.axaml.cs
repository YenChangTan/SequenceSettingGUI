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
using Avalonia.VisualTree;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Diagnostics;
using Avalonia.Controls.ApplicationLifetimes;
using System.Reactive.Linq;


namespace SequenceSettingGUI.Views;

public partial class MCUSettingUserControlView : ReactiveUserControl<MCUSettingUserControlViewModel>
{
    public MCUSettingUserControlView()
    {
        InitializeComponent();
        
    
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}
