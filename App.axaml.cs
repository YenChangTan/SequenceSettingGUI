using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SequenceSettingGUI.ViewModels;
using SequenceSettingGUI.Views;
using Splat;
using ReactiveUI;
using SequenceSettingGUI.Converters;
using SequenceSettingGUI.Services;
using SequenceSettingGUI.Interfaces;
using SequenceSettingGUI.Models;

namespace SequenceSettingGUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Locator.CurrentMutable.RegisterLazySingleton<ITCPClientService>(() => new TCPClientService());
        Locator.CurrentMutable.RegisterLazySingleton(() =>
            new MCU(Locator.Current.GetService<ITCPClientService>()), typeof(MCU)
        );
        Locator.CurrentMutable.Register(() =>
            new MCUSettingUserControlViewModel(Locator.Current.GetService<MCU>()), typeof(MCUSettingUserControlViewModel)
        );
        Locator.CurrentMutable.Register(() =>
            new RobotSettingUserControlViewModel(Locator.Current.GetService<MCU>()), typeof(RobotSettingUserControlViewModel)
        );
        Locator.CurrentMutable.Register(() => new MCUSettingUserControlView(), typeof(IViewFor<MCUSettingUserControlViewModel>));
        Locator.CurrentMutable.Register(() => new RobotSettingUserControlView(), typeof(IViewFor<RobotSettingUserControlViewModel>));
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            Resources["BooleanToStatusFillConverter"] = new BooleanToStatusFillConverter();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}