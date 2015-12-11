using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class DeviceInfoService : BaseService
    {
        public DeviceInfoService() : base(BluetoothRobotConstants.DEVICE_INFO_SERVICE_UUID)
        {
        }

        public async Task<string> ReadSystemId()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.DEVICE_INFO_SYSTEM_ID_CHARACTERISTIC_UUID);

            return BaseService.ConvertHexStringFromByes(data);
        }

        public async Task<string> ReadModuleSoftwareVersion()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.DEVICE_INFO_MODULE_SOFTWARE_CHARACTERISTIC_UUID);

            return BaseService.ConvertStringFromByes(data);
        }
    }
}
