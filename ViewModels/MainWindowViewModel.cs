using ReactiveUI;
using SequenceSettingGUI.Views;
using Splat;
using System.Reactive;


namespace SequenceSettingGUI.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private ReactiveObject _currentView;
    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);

    }

    public ReactiveCommand<Unit, Unit> ShowMCUSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowRobotSettingCommand { get; }

    public MainWindowViewModel()
    {
        CurrentView = Locator.Current.GetService<MCUSettingUserControlViewModel>();
        ShowMCUSettingCommand = ReactiveCommand.Create(() => { CurrentView = Locator.Current.GetService<MCUSettingUserControlViewModel>(); });
        ShowRobotSettingCommand = ReactiveCommand.Create(() => { CurrentView = Locator.Current.GetService<RobotSettingUserControlViewModel>(); });
    }


}
