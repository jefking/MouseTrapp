using System;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Azure.Devices.Client;
using System.Text;
using System.Threading.Tasks;

// CANITPRO.NET FTW

namespace MouseTrapp.IOT
{

    public sealed partial class MainPage : Page
    {
        private const int LED_PIN = 6;
        private const int BUTTON_PIN = 5;

        private DeviceClient deviceClient;

        private GpioPin ledPin;
        private GpioPin buttonPin;
        private GpioPinValue ledPinValue = GpioPinValue.High;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        // Use the device specific connection string here
        private const string IOT_HUB_CONN_STRING = "HostName=mousetrapp.azure-devices.net;DeviceId=MouseTrappIOT;SharedAccessKey=W3ayO1IDKvJ0VW56OCTRih9FnNfINpUwtlfKDRbVqlM=;GatewayHostName=ssl://MouseTrappIOT:8883";
        // Use the name of your Azure IoT device here - this should be the same as the name in the connections string
        private const string IOT_HUB_DEVICE = "MT01";
        // Provide a short description of the location of the device, such as 'Home Office' or 'Garage'
        private const string IOT_HUB_DEVICE_LOCATION = "MSFT - Building 33";

        public MainPage()
        {
            InitializeComponent();
            InitGPIO();
        }

        private void InitGPIO()
        {
            // Instantiate the Azure device client
            deviceClient = DeviceClient.CreateFromConnectionString(IOT_HUB_CONN_STRING);

            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            buttonPin = gpio.OpenPin(BUTTON_PIN);
            ledPin = gpio.OpenPin(LED_PIN);

            // Initialize LED to the OFF state by first writing a HIGH value
            // We write HIGH because the LED is wired in a active LOW configuration
            ledPin.Write(GpioPinValue.High);
            ledPin.SetDriveMode(GpioPinDriveMode.Output);

            // Check if input pull-up resistors are supported
            if (buttonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                buttonPin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                buttonPin.SetDriveMode(GpioPinDriveMode.Input);

            // Set a debounce timeout to filter out switch bounce noise from a button press
            buttonPin.DebounceTimeout = TimeSpan.FromMilliseconds(5);

            // Register for the ValueChanged event so our buttonPin_ValueChanged 
            // function is called when the button is pressed
            buttonPin.ValueChanged += buttonPin_ValueChanged;

            GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private void buttonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            //// toggle the state of the LED every time the button is pressed
            //if (e.Edge == GpioPinEdge.FallingEdge)
            //{
            //    ledPinValue = (ledPinValue == GpioPinValue.Low) ?
            //        GpioPinValue.High : GpioPinValue.Low;
            //    ledPin.Write(ledPinValue);
            //} 

            // need to invoke UI updates on the UI thread because this event
            // handler gets invoked on a separate thread.
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (buttonPin.Read() == GpioPinValue.High)
                {
                    ledPinValue = GpioPinValue.High;
                    ledEllipse.Fill = grayBrush;
                    ledPin.Write(ledPinValue);
                    GpioStatus.Text = "No Mouse...";
                    SendMessageToIoTHubAsync(1);
                }
                if (buttonPin.Read() == GpioPinValue.Low)
                {
                    ledPinValue = GpioPinValue.Low;
                    ledEllipse.Fill = redBrush;
                    ledPin.Write(ledPinValue);
                    GpioStatus.Text = "Dead Mouse...";
                    SendMessageToIoTHubAsync(2);
                }
            });
        }

        private async Task SendMessageToIoTHubAsync(int status)
        {
            try
            {
                var payload = "{\"TrapId\": \"" +
                    "DC60269A-81FF-4445-9822-FE68C28FC7D1" +
                    "\", \"Building\": \"" +
                    IOT_HUB_DEVICE_LOCATION +
                    "\", \"Location\": \"" +
                    IOT_HUB_DEVICE_LOCATION +
                    "\", \"Type\": " +
                    status +
                    ", \"Time\": " +
                    DateTime.Now.ToLocalTime().ToString() +
                    "\"}";

                // UI updates must be invoked on the UI thread
                var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MessageLog.Text = "Sending message: " + payload + "\n" + MessageLog.Text;
                });

                var msg = new Message(Encoding.UTF8.GetBytes(payload));

                await deviceClient.SendEventAsync(msg);
            }
            catch (Exception ex)
            {
                // UI updates must be invoked on the UI thread
                var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MessageLog.Text = "Sending message: " + ex.Message + "\n" + MessageLog.Text;
                });
            }

        }
    }
}
