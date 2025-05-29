using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

using SequenceSettingGUI.Models;
using SequenceSettingGUI.Views;

namespace SequenceSettingGUI.ViewModels;

public class SequenceBlockSettingDialogWindowViewModel : ReactiveObject
{
    public SequenceBlock sequenceBlock { get; set; } = new SequenceBlock();

    public SequenceBlockSettingDialogWindowViewModel(SequenceBlock _sequenceBlock)
    {
        for (int i = 0; i < 16; i++)
        {
            
            sequenceBlock.Outputs[i].IsChecked = _sequenceBlock.Outputs[i].IsChecked;
            sequenceBlock.Inputs[i].IsChecked = _sequenceBlock.Inputs[i].IsChecked;
        }
        OkCommand = ReactiveCommand.CreateFromTask(async () => {

            await CloseInteraction.Handle(sequenceBlock);


            return sequenceBlock;
        });

        CancelCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CloseInteraction.Handle(null!);
        });
    }

    public Interaction<SequenceBlock, Unit> CloseInteraction { get; } = new Interaction<SequenceBlock, Unit>();
    public ReactiveCommand<Unit, SequenceBlock> OkCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

}


