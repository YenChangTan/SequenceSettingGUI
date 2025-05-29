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



public partial class ConfirmationWindow : ReactiveWindow<ConfirmationWindowViewModel>
{
    public ConfirmationWindow(ConfirmationWindowViewModel viewModel)
    {
        InitializeComponent();
        this.DataContext = viewModel;
        this.WhenActivated(disposables =>
        {
            if (DataContext is ConfirmationWindowViewModel vm)
            {
                vm.CloseInteraction.RegisterHandler(interaction =>
                {
                    bool result = interaction.Input;

                    Close(result); // <-- your own method or override

                    interaction.SetOutput(Unit.Default);
                }).DisposeWith(disposables);
            }
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }


}