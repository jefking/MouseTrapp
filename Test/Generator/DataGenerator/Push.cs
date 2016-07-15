using King.Service;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Push : InitializeTask
    {

        private static async Task<string> GetOrAddDeviceAsync()
        {

            var registryManager = RegistryManager.CreateFromConnectionString(Properties.Settings.Default.IotHubConnectionString);

            Device device;
                        try
            {
                device = await registryManager.AddDeviceAsync(new Device(Properties.Settings.Default.TestDeviceName));
                Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(Properties.Settings.Default.TestDeviceName);
                Console.WriteLine("Retrieved device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
            }

            return device.Authentication.SymmetricKey.PrimaryKey;
        }

        public override void Run()
        {
            Trace.Write("starting");

            var deviceKey = GetOrAddDeviceAsync().Result;
            string deviceConnectionString = String.Format("{0};DeviceId={1};DeviceKey={2}", Properties.Settings.Default.IotHubConnectionString, Properties.Settings.Default.TestDeviceName, deviceKey);
            var client = DeviceClient.CreateFromConnectionString(deviceConnectionString);
            var TwoMonthsAgo = DateTime.Now.AddMonths(-2);

            var r = new Random();

            foreach (var td in this.GetTestTraps())
            {
                var m = new EventModel
                {
                    TrapId = Guid.Parse(td.TrapId),
                    Location = td.Location,
                    Building = td.Building
                };

                var current = new DateTime(TwoMonthsAgo.Year, TwoMonthsAgo.Month, 1);
                var switched = false;

                while (current < DateTime.UtcNow)
                {
                    m.Time = current;
                    m.Type = (byte)(switched ? 2 : 1);

                    var msg = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(m)));

                    client.SendEventAsync(msg).Wait();

                    switched = !switched;

                    var samplePeriod = r.Next(45, 2880);
                    current = current.AddMinutes(samplePeriod);
                }
            }

            Trace.Write("Completed");

        }

        private class TestData
        {
            public string TrapId;
            public string Location;
            public string Building;
        }
        private IEnumerable<TestData> GetTestTraps()
        {
            yield return new TestData {TrapId = "4807BE67-4BC2-482D-8B08-DDF49F488417", Location = "1", Building = "33" };
            yield return new TestData {TrapId = "B369FF26-F400-4E24-B132-AC8FCD1D0006", Location = "2", Building = "33" };
            yield return new TestData {TrapId = "{1B44AA62-4D65-444E-9487-EA91F66143BE}", Location = "3", Building = "33" };
            yield return new TestData {TrapId = "{05C159BD-359F-44B7-984C-97B87CA2DD3E}", Location = "4", Building = "33" };
            yield return new TestData {TrapId = "{710160FB-5700-4299-8B84-69025EF570C9}", Location = "5", Building = "33" };
            yield return new TestData {TrapId = "{2BC21EFF-8565-48BB-AB62-92D12331D4F5}", Location = "6", Building = "33" };
            yield return new TestData {TrapId = "{50C19048-2076-4B85-97AA-3F4A5C29DC22}", Location = "7", Building = "33" };
            yield return new TestData {TrapId = "{964FFD3D-AD12-47A8-B116-062F753DA0F4}", Location = "8", Building = "33" };
            yield return new TestData {TrapId = "{C33F229C-F763-4D89-9C80-5C35D6D0DAE2}", Location = "9", Building = "33" };
            yield return new TestData {TrapId = "{C764ECA9-8E09-4D33-876F-7C01C424E5E4}", Location = "10", Building = "33" };
            yield return new TestData {TrapId = "{14260A17-0F21-47C1-9A84-9FC8B4C82493}", Location = "11", Building = "33" };
            yield return new TestData {TrapId = "{E0B3144D-9B1A-480A-A605-11CEE430F68B}", Location = "12", Building = "33" };
            yield return new TestData {TrapId = "{3E47FE83-759D-4D27-A114-9DDC0BE1B440}", Location = "13", Building = "33" };
            yield return new TestData {TrapId = "{694201B7-1B06-493B-B60D-3B42ABAD52DF}", Location = "14", Building = "33" };
            yield return new TestData {TrapId = "{FF29F513-635D-416E-AC44-B779B3B1D084}", Location = "15", Building = "33" };
            yield return new TestData {TrapId = "{26DA2F70-10E1-4917-8656-42A86F84C243}", Location = "16", Building = "33" };
            yield return new TestData {TrapId = "{A747FA12-E9F9-43BD-8A0E-02D18B25389E}", Location = "17", Building = "33" };
            yield return new TestData {TrapId = "{30C73618-AFF6-4D8F-AC39-638668547C71}", Location = "18", Building = "33" };
            yield return new TestData {TrapId = "{DF2CB1EB-EB75-40B2-B3AA-295DE9D36D99}", Location = "19", Building = "33" };
            yield return new TestData {TrapId = "{8408CD64-3BE1-426B-BA49-FB255B48FBE5}", Location = "20", Building = "33" };
            yield return new TestData {TrapId = "{DB761AC4-8FE6-4875-B38B-2C1CFBD23DB9}", Location = "1", Building = "26" };
            yield return new TestData {TrapId = "{9E0CBF00-2BED-42F0-8CAF-65674C6F1C06}", Location = "2", Building = "26" };
            yield return new TestData {TrapId = "{D4EE71D9-3C29-4631-B94B-B6FA535129C5}", Location = "3", Building = "26" };
            yield return new TestData {TrapId = "{959BAD7C-C7AE-456B-8903-0033C78E2E0E}", Location = "1", Building = "88" };
            yield return new TestData {TrapId = "{0967DC22-4985-4694-AFB6-E73C35132E65}", Location = "2", Building = "88" };
            yield return new TestData {TrapId = "{BE15F518-A5F6-4A28-A054-DBC89B40CFC0}", Location = "3", Building = "88" };
        }
    }
}
