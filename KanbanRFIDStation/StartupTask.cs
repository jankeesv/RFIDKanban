using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using Windows.Devices.Gpio;
using Windows.Foundation.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http.Headers;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace KanbanRFIDStation
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        private const int PIEZO_PIN = 5;
        private GpioPin piezoPin;
        private GpioPinValue piezoPinValue;
        private Mfrc522 mfrc;
        private string hostName;
        private LoggingChannel loggingChannel;
        private HttpClient client;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            // Initiate a logger to debug
            loggingChannel = new LoggingChannel("defaultlogger", null, new Guid("6D667125-F314-4C54-9834-8A11E01CEAD6"));

            InitPiezo();
            InitRC522Async();
        }

        public async void InitRC522Async()
        {
            mfrc = new Mfrc522();
            hostName = GetHostName();

            client = new HttpClient();
            //TODO: Find a solution to fix the static IP into a dynamic solution. First idea is to set the server IP by a get from the server webapplication.
            client.BaseAddress = new Uri("http://192.168.178.21/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            loggingChannel.LogMessage("RC522 init start");
            await mfrc.InitIO();
            loggingChannel.LogMessage("RC522 init finish");

            while (true)
            {
                if (mfrc.IsTagPresent())
                {
                    string newUid = mfrc.ReadUid().ToString();
                    //Value 00000000 is a wrong read
                    if (!newUid.Equals("00000000"))
                    {
                        loggingChannel.LogMessage("RFID tag present with id: " + newUid + " at host " + hostName);

                        try
                        {
                            await PostRfidRegistration(newUid);
                            await PlayTune();
                            //Wait a second to prevent double scans
                            await Task.Delay(1000);
                        }
                        catch (Exception ex)
                        {
                            loggingChannel.LogMessage("RFID tag not registered because of error: " + ex.Message);
                        }
                    }
                    mfrc.HaltTag();
                }
                else
                {
                    mfrc.HaltTag();
                }
            }
        }

        private async Task PostRfidRegistration(string newUid)
        {
            string json = string.Format(@"{{""rfidUid"":""{0}"",""hostName"":""{1}""}}", newUid, hostName);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("api/values", httpContent);
            response.EnsureSuccessStatusCode();
            loggingChannel.LogMessage("RFID tag registered");
        }

        private string GetHostName()
        {
            foreach (Windows.Networking.HostName name in Windows.Networking.Connectivity.NetworkInformation.GetHostNames())
            {
                if (Windows.Networking.HostNameType.DomainName == name.Type)
                {
                    return name.DisplayName;
                }
            }
            return null;
        }

        private async Task PlayTune()
        {
            piezoPinValue = GpioPinValue.High;
            piezoPin.Write(piezoPinValue);
            await Task.Delay(100);
            piezoPinValue = GpioPinValue.Low;
            piezoPin.Write(piezoPinValue);
        }

        private void InitPiezo()
        {
            GpioController gpio = GpioController.GetDefault();
            piezoPin = gpio.OpenPin(PIEZO_PIN);
            piezoPinValue = GpioPinValue.Low;
            piezoPin.Write(piezoPinValue);
            piezoPin.SetDriveMode(GpioPinDriveMode.Output);
            loggingChannel.LogMessage("GPIO initiated");
        }
    }
}
