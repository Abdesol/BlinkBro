using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform;
using Avalonia.Threading;
using BlinkBro.Views;

namespace BlinkBro.Services;

public class BlinkService
{
    public static BlinkService Current { get; } = new();
    public int BlinkCount { get; set; }

    public void Start()
    {
        void Blink()
        {
            var previousBlinkCount = BlinkCount;
            var waitDuration = 60 / BlinkCount;
            while (true)
            {
                if (previousBlinkCount != BlinkCount!)
                {
                    previousBlinkCount = BlinkCount;
                    waitDuration = 60 / BlinkCount;
                }
                Thread.Sleep(TimeSpan.FromSeconds(waitDuration));
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (Application.Current!.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                        return;
                    var screens = desktop.MainWindow.Screens;
                    var blinkWindows = screens!.All.Select(screen => new BlinkWindow(screen)).ToList();
                    foreach (var blinkWindow in blinkWindows)
                        blinkWindow.ShowDialog(desktop.MainWindow);
                });  
            }   
        }
        new Thread(Blink).Start();
    }
}