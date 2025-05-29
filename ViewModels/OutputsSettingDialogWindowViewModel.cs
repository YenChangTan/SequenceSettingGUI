using ReactiveUI;
using System.Collections.ObjectModel;
using Avalonia.ReactiveUI;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;
using Avalonia.Controls.Platform;
using System.Diagnostics;
using System;
using SequenceSettingGUI.Models;
using System.Collections.Generic;

namespace SequenceSettingGUI.ViewModels;

public partial class OutputsSettingDialogWindowViewModel:ReactiveObject
{
    public ObservableCollection<CheckedState> OutputStates { get; set; } = new ObservableCollection<CheckedState> { };

    public ReactiveCommand<Unit, bool[]> OkCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public Interaction<bool[], Unit> CloseInteraction { get; } =  new Interaction<bool[], Unit>();

    public OutputsSettingDialogWindowViewModel(bool[] initialStates)
    {
        foreach (var initialState in initialStates)
        {
            OutputStates.Add(new CheckedState()
            {
                IsChecked = initialState
            });            
        }
        

        // OkCommand = ReactiveCommand.Create(() => OutputStates.ToArray());
        // CancelCommand = ReactiveCommand.Create(() => { });

        OkCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            ObservableCollection<bool> outputStateResult = new ObservableCollection<bool> { };
            foreach (var outputState in OutputStates)
            {
                outputStateResult.Add(outputState.IsChecked);
            }
            await CloseInteraction.Handle(outputStateResult.ToArray());
            
            
            return outputStateResult.ToArray();
        });

        CancelCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CloseInteraction.Handle(null!);
        });
    }
}
