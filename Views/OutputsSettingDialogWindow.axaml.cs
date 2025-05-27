using Avalonia.ReactiveUI;
using SequenceSettingGUI.ViewModels;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using Avalonia.Interactivity;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Disposables;
using System.Diagnostics;

namespace SequenceSettingGUI.Views;

public partial class OutputsSettingDialogWindow : ReactiveWindow<OutputsSettingDialogWindowViewModel>
{

    public OutputsSettingDialogWindow(OutputsSettingDialogWindowViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
        this.WhenActivated(disposables =>
        {
            if (DataContext is OutputsSettingDialogWindowViewModel vm)
            {
                vm.CloseInteraction.RegisterHandler(interaction =>
                {
                    Close(interaction.Input);  // Pass the bool[] or null to Close()
                    interaction.SetOutput(Unit.Default);
                }).DisposeWith(disposables);
            }
        });
    }

    
}
