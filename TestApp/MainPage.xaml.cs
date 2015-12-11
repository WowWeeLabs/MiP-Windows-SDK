using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using MipWindowLib.MipRobot;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CanvasBitmap joystickBitmap;

        Size joystickCanvasSize;
        Rect joystickDrawRect;
        Vector2 joystickDirection;

        Timer joystickTimer;

        const int JOYSTICK_HALF_SIZE = 50;
        const float MOVE_RATIO_BY_JOYSTICK = 0.01f;
        const int MOVE_TICK_INTERVAL_IN_MS = 66;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == this.ConnectButton)
            {
                await MipRobotFinder.Instance.ScanForRobots();

                MipRobot mip = MipRobotFinder.Instance.FoundRobotList.FirstOrDefault();
                if (mip != null)
                {
                    if (await mip.Connect())
                    {
                        this.ConnectButton.Content = "Connected: " + mip.DeviceName;

                        this.ConnectButton.IsEnabled = false;
                        this.DriveButton.IsEnabled = true;
                        this.DriveCanvas.IsEnabled = true;
                        this.PlaySoundButton.IsEnabled = true;
                        this.ChangeChestButton.IsEnabled = true;
                        this.FalloverButton.IsEnabled = true;
                    }
                }
                else
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                }
            }
            else if (sender == DriveButton)
            {

            }
            else if (sender == this.PlaySoundButton)
            {
                await MipRobotFinder.Instance.FirstConnectedRobot().SetMipVolumeLevel(7);
                await MipRobotFinder.Instance.FirstConnectedRobot().PlayMipSound(new MipRobotSound(MipRobotConstants.SOUND_FILE.MIP_IN_LOVE));
            }
            else if (sender == this.ChangeChestButton)
            {
                await MipRobotFinder.Instance.FirstConnectedRobot().SetMipChestLedWithColor(0xff, 0xff, 0xff, 1);
            }
            else if (sender == this.FalloverButton)
            {
                await MipRobotFinder.Instance.FirstConnectedRobot().MipFalloverWithSytle(MipRobotConstants.POSITION_VALUE.ON_BACK);
            }
        }

        void DriveCanvas_CreateResourcesEvent(CanvasControl control, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(DriveCanvas_CreateResourcesAsync(control).AsAsyncAction());
        }

        async Task DriveCanvas_CreateResourcesAsync(CanvasControl sender)
        {
            joystickBitmap = await CanvasBitmap.LoadAsync(sender, "Assets/Joystick.png");

            joystickCanvasSize = new Size(sender.ActualWidth, sender.ActualHeight);

            joystickDrawRect = new Rect(0, 0, JOYSTICK_HALF_SIZE*2, JOYSTICK_HALF_SIZE*2);
            joystickDirection = new Vector2();

            UpdateJoystick(sender, joystickCanvasSize.Width / 2, joystickCanvasSize.Height / 2, true);

            joystickTimer = new Timer(MipMove, this, MOVE_TICK_INTERVAL_IN_MS, Timeout.Infinite);
        }

        void DriveCanvas_DrawEvent(object sender, CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(joystickBitmap, joystickDrawRect);
        }

        private void DriveCanvas_PointerPressedEvent(object sender, PointerRoutedEventArgs e)
        {
            CanvasControl control = (CanvasControl)sender;
            Point pos = e.GetCurrentPoint(control).Position;
            UpdateJoystick(control, pos.X, pos.Y);
        }

        private void DriveCanvas_PointerReleasedEvent(object sender, PointerRoutedEventArgs e)
        {
            CanvasControl control = (CanvasControl)sender;
            Point pos = e.GetCurrentPoint(control).Position;
            UpdateJoystick(control, joystickCanvasSize.Width / 2, joystickCanvasSize.Height / 2);
        }

        private void DriveCanvas_PointerMovedEvent(object sender, PointerRoutedEventArgs e)
        {
            CanvasControl control = (CanvasControl)sender;
            Point pos = e.GetCurrentPoint(control).Position;
            UpdateJoystick(control, pos.X, pos.Y);
        }

        private void UpdateJoystick(CanvasControl control, double posX, double posY, bool isFirstTime=false)
        {
            joystickDirection.X = (float)(posX - joystickCanvasSize.Width/2);
            joystickDirection.Y = (float)(posY - joystickCanvasSize.Height/2);

            float overRatio = joystickDirection.Length() / (float)JOYSTICK_HALF_SIZE;
            if (overRatio > 1.0f)
            {
                joystickDirection.X /= overRatio;
                joystickDirection.Y /= overRatio;
            }

            joystickDrawRect.X = joystickCanvasSize.Width / 2 + joystickDirection.X - JOYSTICK_HALF_SIZE;
            joystickDrawRect.Y = joystickCanvasSize.Height / 2 + joystickDirection.Y - JOYSTICK_HALF_SIZE;

            if (!isFirstTime)
            {
                control.Invalidate();
            }
        }

        private void MipMove(object state)
        {
            if (joystickDirection.Length() > 0.0001f)
            {
                Vector2 mov = new Vector2(joystickDirection.X, -joystickDirection.Y);
                mov *= MOVE_RATIO_BY_JOYSTICK;

                MipRobotFinder.Instance.FoundRobotList.FirstOrDefault()?.MipDrive(mov);
            }

            joystickTimer.Change(MOVE_TICK_INTERVAL_IN_MS, Timeout.Infinite);
        }
    }
}
