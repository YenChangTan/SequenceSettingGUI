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

public partial class ConfirmationWindowViewModel:ReactiveObject
{
    public string Message { get; set; }

    public ReactiveCommand<Unit, bool> OkCommand { get; }
    public ReactiveCommand<Unit, bool> CancelCommand { get; }

    public Interaction<bool, Unit> CloseInteraction { get; } =  new Interaction<bool, Unit>();

    public ConfirmationWindowViewModel(string popOutMessage)
    {

        Message = popOutMessage;
        OkCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            
            await CloseInteraction.Handle(true);
            
            
            return true;
        });

        CancelCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CloseInteraction.Handle(false);
            return false;
        });
    }
}
