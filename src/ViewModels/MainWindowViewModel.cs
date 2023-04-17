using System;
using System.IO;
using Newtonsoft.Json;
using ReactiveUI;

namespace BlinkBro.ViewModels;

public class Model
{
    [JsonProperty("BlinkCount")] public int BlinkCount { get; set; }
}

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        OpenDb();
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
                var data = JsonConvert.DeserializeObject<Model>(json);
                BlinkCount = data.BlinkCount <= 30 ? data.BlinkCount : 30;
                return;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        var model = new Model() { BlinkCount = 15 };
        var jsonModel = JsonConvert.SerializeObject(model);
        File.WriteAllText("data.json", jsonModel);
    }

    private void UpdateDb()
    {
        File.WriteAllText("data.json", JsonConvert.SerializeObject(new Model() { BlinkCount = BlinkCount }));
    }
}