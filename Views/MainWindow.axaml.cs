using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using System;
using System.Threading.Tasks;
using System.Threading;
using SequenceSettingGUI.ViewModels;
using ReactiveUI;

namespace SequenceSettingGUI.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    private Point _startPoint;
    private bool _isDragging = false;
    private bool _sliderIsOut => transform.X > -100;
    private TranslateTransform transform;


    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(disposables => { });
        transform = (TranslateTransform)SlideOutMenu.RenderTransform!;
        
    }

    private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        _startPoint = e.GetPosition(this);
        _isDragging = true;
    }

    private void Window_PointerMoved(object sender, PointerEventArgs e)
    {
        if (_isDragging)
        {
            var current = e.GetPosition(this);
            var deltaX = current.X - _startPoint.X;
            if (!_sliderIsOut)
            {
                if (deltaX > 0 && deltaX <= 200)
                {
                    // Slide menu in with finger drag
                    transform!.X = -200 + deltaX;
                }
            }
            else
            {
                if (deltaX < 0 && deltaX >= -200)
                {
                    transform!.X = 0 + deltaX;
                }
            }
        }
    }

    private void Window_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
        var finalX = (transform as TranslateTransform)!.X;

        // Snap open if dragged more than halfway
        if (!_sliderIsOut)
        {
            if (finalX > -100)
            {
                (transform as TranslateTransform)!.X = 0;

            }
            else
            {
                (transform as TranslateTransform)!.X = -200;
            }
        }
        else
        {
            if (finalX < -100)
            {
                (transform as TranslateTransform)!.X = -200;

            }
            else
            {
                (transform as TranslateTransform)!.X = 0;
            }
        }
        
    }
}