using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SequenceSettingGUI.ViewModels;

namespace SequenceSettingGUI.Views;

public partial class SequenceBlockSettingDialogWindow : ReactiveWindow<SequenceBlockSettingDialogWindowViewModel>
{
    public SequenceBlockSettingDialogWindow(SequenceBlockSettingDialogWindowViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
        this.WhenActivated(disposables =>
        {
            if (DataContext is SequenceBlockSettingDialogWindowViewModel vm)
            {
                vm.CloseInteraction.RegisterHandler(interaction =>
                {
                    Close(interaction.Input);
                    interaction.SetOutput(Unit.Default);
                }).DisposeWith(disposables);
            }
        });
    }
}
