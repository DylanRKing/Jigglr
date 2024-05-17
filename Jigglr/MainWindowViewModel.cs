using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Jigglr;
public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _timerAction = "Start";

    private bool userStarted = false;

    [ObservableProperty]
    private Idler idler;

    public MainWindowViewModel()
    {
        idler = new Idler();
    }

    [RelayCommand]
    private async Task StartStop()
    {
        if (userStarted)
        {
            await Idler.StopAsync();
            userStarted = false;
            TimerAction = "Start";
        }
        else
        {
            await Idler.StartAsync();
            userStarted = true;
            TimerAction = "Stop";
        }
    }
}
