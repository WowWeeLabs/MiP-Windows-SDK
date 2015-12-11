using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class DeviceSettingService : BaseService
    {
        private BluetoothRobotConstants.ACTIVIATION_STATUS activationStatus = BluetoothRobotConstants.ACTIVIATION_STATUS.NOT_READ;

        public DeviceSettingService() : base(BluetoothRobotConstants.DEVICE_SETTING_SERVICE_UUID)
        {
        }

        public async Task<BluetoothRobotConstants.ACTIVIATION_STATUS> ReadProductActivationStatus()
        {
            if (activationStatus == BluetoothRobotConstants.ACTIVIATION_STATUS.NOT_READ)
            {
                byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.DEVICE_SETTING_PRODUCT_ACTIVIATION_CHARACTERISTIC_UUID);
                activationStatus = BaseService.ConvertEnumFromBytes<BluetoothRobotConstants.ACTIVIATION_STATUS>(data);
            }

            return activationStatus;
        }

        public async Task<bool> WriteProductActivationStatus(BluetoothRobotConstants.ACTIVIATION_STATUS status)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.DEVICE_SETTING_PRODUCT_ACTIVIATION_CHARACTERISTIC_UUID, new byte[] { (byte)status });
        }
    }
}
