using System;
using System.IO;
using BlinkBro.Models;
using BlinkBro.Services;
using Newtonsoft.Json;
using ReactiveUI;

namespace BlinkBro.ViewModels;


public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        OpenDb();
        BlinkService.Current.BlinkCount = BlinkCount;
        BlinkService.Current.Start();
        
        this.WhenAnyValue(x => x.BlinkCount)
            .Subscribe((i => UpdateDb()));
    }

    private int _blinkCount;

    public int BlinkCount
    {
        get => _blinkCount;
        set => this.RaiseAndSetIfChanged(ref _blinkCount, value);
    }

    private void OpenDb()
    {
        if (File.Exists("data.json"))
        {
            try
            {
                var json = File.ReadAllText("data.json");
                var data = JsonConvert.DeserializeObject<DbModel>(json);
                BlinkCount = data.BlinkCount <= 30 ? data.BlinkCount : 30;
                return;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        var model = new DbModel() { BlinkCount = 15 };
        var jsonModel = JsonConvert.SerializeObject(model);
        File.WriteAllText("data.json", jsonModel);
    }

    private void UpdateDb()
    {
        BlinkService.Current.BlinkCount = BlinkCount;
        File.WriteAllText("data.json", JsonConvert.SerializeObject(new DbModel() { BlinkCount = BlinkCount }));
    }
}