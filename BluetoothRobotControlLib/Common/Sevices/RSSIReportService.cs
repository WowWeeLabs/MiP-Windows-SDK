using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class RSSIReportService : BaseService
    {
        public RSSIReportService() : base(BluetoothRobotConstants.RSSI_REPORT_SERVICE_UUID)
        {
        }

        public Task<bool> SetInterval(int value)
        {
            //TODO: exception that can't write anything through this characteristic
            //return base.WriteCharacteristicValueAsync(BluetoothRobotConstants.RSSI_REPORT_SET_INTERVAL_CHARACTERISTIC_UUID, new byte[] { (byte)value });

            return Task.FromResult(true);
        }

        public async Task<int> GetInterval()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.RSSI_REPORT_SET_INTERVAL_CHARACTERISTIC_UUID);
            return BaseService.ConvertIntFromBytes(data);
        }

        public async Task<bool> NeedRSSIReport(bool needRSSIReport, int interval, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> callback)
        {
            if (!needRSSIReport)
            {
                interval = 0;
            }

            bool result = true;
            result &= await SetInterval(interval);
            result &= await SetupCharacteristicNotifyAsync(BluetoothRobotConstants.RSSI_REPORT_SERVICE_UUID, needRSSIReport, callback);

            return result;
        }
    }
}
