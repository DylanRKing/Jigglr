using CommunityToolkit.Mvvm.ComponentModel;

namespace Jigglr;
public partial class Idler : ObservableObject
{
    [ObservableProperty]
    private double _interval = 30;

    private readonly System.Timers.Timer activityTimer;
    private readonly System.Timers.Timer jiggleTimer;
    private readonly MouseHandler mouseHandler;

    public Idler()
    {
        mouseHandler = new MouseHandler();
        activityTimer = new System.Timers.Timer();
        jiggleTimer = new System.Timers.Timer(interval: 5000);
        activityTimer.Elapsed += async (s, e) => await ActivityTimer_Elapsed();
        jiggleTimer.Elapsed += async (s, e) => await JiggleTimer_Elapsed();
    }

    #region OnChange

    partial void OnIntervalChanged(double value)
    {
        activityTimer.Interval = Interval * 1000;
    }

    #endregion

    #region Start/Stop

    public async Task StartAsync()
    {
        await Task.Run(async () =>
        {
            await mouseHandler.GetCursorPositionAsync();
            activityTimer.Start();
        });
    }

    public async Task StopAsync()
    {
        await Task.Run(async () =>
        {
            await mouseHandler.GetCursorPositionAsync();
            if (activityTimer.Enabled)
            {
                activityTimer.Stop();
            }

            if (jiggleTimer.Enabled)
            {
                jiggleTimer.Stop();
            }
        });
    }

    #endregion

    #region Activity

    private async Task ActivityTimer_Elapsed()
    {
        await Task.Run(async () =>
        {
            await mouseHandler.GetCursorPositionAsync();
            bool res = mouseHandler.Compare();
            if (res)
            {
                activityTimer.Stop();
                mouseHandler.SetIdlePos();
                jiggleTimer.Start();
                return;
            }
        });
    }

    #endregion

    #region Jiggle

    private async Task JiggleTimer_Elapsed()
    {
        await Task.Run(async () =>
        {
            await mouseHandler.GetCursorPositionAsync();
            bool res = mouseHandler.Compare();
            if (!res)
            {
                jiggleTimer.Stop();
                activityTimer.Start();
                return;
            }
            await mouseHandler.JiggleMouseAsync();
        });
    }

    #endregion
}
