using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class SendDataService : BaseService
    {
        public SendDataService(DeviceInformation serviceInfo) : base(BluetoothRobotConstants.SEND_DATA_SERVICE_UUID, serviceInfo)
        {
        }

        public async Task<bool> SendData(byte[] data)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.SEND_DATA_CHARACTERISTIC_UUID, data);
        }
    }
}
