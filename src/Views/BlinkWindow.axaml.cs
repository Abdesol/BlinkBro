using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Threading;

namespace BlinkBro.Views;

public partial class BlinkWindow : Window
{
    public BlinkWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
    
    public BlinkWindow(Screen screen)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
        Height = screen.Bounds.Height;
        Width = screen.Bounds.Width;
        Position = screen.Bounds.Position;
        
        new Thread(() =>
        {
            Thread.Sleep(500);
            Dispatcher.UIThread.InvokeAsync(() => Close());
        }).Start();
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}