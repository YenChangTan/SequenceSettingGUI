using ReactiveUI;
using SequenceSettingGUI.Views;
using System.Reactive;


namespace SequenceSettingGUI.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private ReactiveObject _currentView = new MCUSettingUserControlViewModel();
    public ReactiveObject CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);

    }

    public ReactiveCommand<Unit, Unit> ShowMCUSettingCommand { get; }
    public ReactiveCommand<Unit, Unit> ShowRobotSettingCommand { get; }

    public MainWindowViewModel()
    {
        CurrentView = new MCUSettingUserControlViewModel();
        ShowMCUSettingCommand = ReactiveCommand.Create(() => { CurrentView = new MCUSettingUserControlViewModel(); });
        ShowRobotSettingCommand = ReactiveCommand.Create(() => { CurrentView = new RobotSettingUserControlViewModel(); });
    }


}
