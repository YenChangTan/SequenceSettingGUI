using ReactiveUI;
using Avalonia.ReactiveUI;
using SequenceSettingGUI.Views;
using System.Reactive;
using SequenceSettingGUI.Models;
using SequenceSettingGUI.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Diagnostics;

namespace SequenceSettingGUI.ViewModels;

public partial class MCUSettingUserControlViewModel : ReactiveObject
{
    public MCU _mcu { get; }

    // private int _selectedBlock = 0;
    // public int SelectedBlock
    // {
    //     get => _selectedBlock;
    //     set => this.RaiseAndSetIfChanged(ref _selectedBlock, value);
    // }

    private int _selectedSequenceBlockIndex = 0;
    public int SelectedSequenceBlockIndex
    {
        get => _selectedSequenceBlockIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedSequenceBlockIndex, value);
    }
    private SequenceBlock _selectedSequenceBlock;
    public SequenceBlock SelectedSequenceBlock
    {
        get => _selectedSequenceBlock;
        set => this.RaiseAndSetIfChanged(ref _selectedSequenceBlock, value);
    }

    public ObservableCollection<SequenceBlock> sequenceBlocksList { get; set; } = new ObservableCollection<SequenceBlock> { new SequenceBlock() };

    public MCUSettingUserControlViewModel(MCU mcu)
    {
        _mcu = mcu;
        ConfirmDeleteInteraction = new Interaction<string, bool>();
        AddSequenceCommand = ReactiveCommand.CreateFromTask(AddSequence);
        DeleteSequenceCommand = ReactiveCommand.CreateFromTask(DeleteSequence);
        EditSequenceCommand = ReactiveCommand.CreateFromTask(EditSequence);
        UpSequenceCommand = ReactiveCommand.Create(UpSequence);
        DownSequenceCommand = ReactiveCommand.Create(DownSequence);

    }

    public ReactiveCommand<Unit, Unit> AddSequenceCommand { get; }
    private async Task AddSequence()
    {
        var dialog = new SequenceBlockSettingDialogWindow(new SequenceBlockSettingDialogWindowViewModel(sequenceBlocksList[SelectedSequenceBlockIndex]));

        var parent = App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime life
            ? life.MainWindow
            : null;

        if (parent is null) return;
        dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        SequenceBlock result = await dialog.ShowDialog<SequenceBlock>(parent);

        if (result != null)
        {
            sequenceBlocksList.Add(result);
            SelectedSequenceBlockIndex += 1;
        }
        else
        {
            Console.WriteLine("User canceled the dialog.");
        }
    }


    public Interaction<string, bool> ConfirmDeleteInteraction { get; }

    public ReactiveCommand<Unit, Unit> DeleteSequenceCommand { get; }
    private async Task DeleteSequence()
    {
        if (SelectedSequenceBlockIndex == 0)
        {
            return;
        }
        var dialog = new ConfirmationWindow(new ConfirmationWindowViewModel("Are you sure you want to delete selected sequence block?"));

        // Get the top-level window (you can pass it in better with DI or view service)
        var parent = App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime life
            ? life.MainWindow
            : null;

        if (parent is null) return;
        dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        bool result = await dialog.ShowDialog<bool>(parent);


        // Perform delete logic here
        if (result)
        {
            int tempBlockIndex = SelectedSequenceBlockIndex;
            sequenceBlocksList.RemoveAt(SelectedSequenceBlockIndex);
            SelectedSequenceBlockIndex = tempBlockIndex - 1;

        }

    }

    public ReactiveCommand<Unit, Unit> EditSequenceCommand { get; }
    private async Task EditSequence()
    {
        var dialog = new SequenceBlockSettingDialogWindow(new SequenceBlockSettingDialogWindowViewModel(sequenceBlocksList[SelectedSequenceBlockIndex]));

        // Get the top-level window (you can pass it in better with DI or view service)
        var parent = App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime life
            ? life.MainWindow
            : null;

        if (parent is null) return;
        dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

        SequenceBlock result = await dialog.ShowDialog<SequenceBlock>(parent);

        if (result != null)
        {
            sequenceBlocksList[SelectedSequenceBlockIndex] = result;
        }
        else
        {
            Console.WriteLine("User canceled the dialog.");
        }
    }

    public ReactiveCommand<Unit, Unit> UpSequenceCommand { get; }
    public void UpSequence()
    {
        if (SelectedSequenceBlockIndex > 0)
        {
            SelectedSequenceBlockIndex -= 1;
        }
    }
    
    public ReactiveCommand<Unit, Unit> DownSequenceCommand { get; }
    public void DownSequence()
    {
        if (SelectedSequenceBlockIndex < sequenceBlocksList.Count()-1)
        {
            SelectedSequenceBlockIndex += 1;
        }
    }
    
}
