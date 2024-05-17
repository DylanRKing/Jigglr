using System.Runtime.InteropServices;

namespace Jigglr;
public class MouseHandler
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    private POINT? startPos;
    private POINT? endPos;
    private POINT? idlePos;

    // Import the GetCursorPos function from user32.dll
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    // Import the SetCursorPos function from user32.dll
    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public async Task GetCursorPositionAsync()
    {
        await Task.Run(() =>
        {
            _ = GetCursorPos(out POINT lpPoint);
            if (startPos is null)
            {
                startPos = lpPoint;
            }
            else if (endPos is null)
            {
                endPos = lpPoint;
            }
            else
            {
                startPos = endPos;
                endPos = lpPoint;
            }
        });
    }

    public void SetIdlePos()
    {
        _ = GetCursorPos(out POINT lpPoint);
        idlePos = lpPoint;
    }

    public bool Compare()
    {
        return endPos.Equals(startPos);
    }

    public static async Task SetCursorPositionAsync(int x, int y)
    {
        await Task.Run(() =>
        {
            _ = SetCursorPos(x, y);
        });
    }

    public async Task JiggleMouseAsync()
    {
        await SetCursorPositionAsync(idlePos.Value.X, idlePos.Value.Y + 20);
        await Task.Delay(20);
        await SetCursorPositionAsync(idlePos.Value.X + 20, idlePos.Value.Y + 20);
        await Task.Delay(20);
        await SetCursorPositionAsync(idlePos.Value.X + 20, idlePos.Value.Y - 20);
        await Task.Delay(20);
        await SetCursorPositionAsync(idlePos.Value.X, idlePos.Value.Y);
        await Task.Delay(20);
    }
}
