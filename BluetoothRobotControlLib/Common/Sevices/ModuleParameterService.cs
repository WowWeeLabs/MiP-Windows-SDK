using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class ModuleParameterService : BaseService
    {
        private const int MAX_BT_DEVICE_NAME_LENGTH = 15;

        public ModuleParameterService() : base(BluetoothRobotConstants.MODULE_PARAMETER_SERVICE_UUID)
        {
        }

        public async Task<string> ReadBTDeviceName()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_DEVICE_NAME_CHARACTERISTIC_UUID);

            return BaseService.ConvertStringFromByes(data);
        }

        public async Task<bool> WriteBTDeviceName(string name)
        {
            string deviceName = name;

            //trim the device name
            if (name.Length > MAX_BT_DEVICE_NAME_LENGTH)
            {
                //TODO show log msg

                deviceName = name.Substring(0, MAX_BT_DEVICE_NAME_LENGTH);
            }

            List<byte> data = new List<byte>();
            data.Add(0x00);
            data.AddRange(BaseService.ConvertBytesFromString(deviceName));

            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_DEVICE_NAME_CHARACTERISTIC_UUID, data.ToArray());
        }

        public async Task<int> ReadBTCommunicationInterval()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_BT_COMMUNICATION_INTERVAL_CHARACTERISTIC_UUID);

            return BaseService.ConvertIntFromBytes(data);
        }

        public async Task<bool> WriteBTCommunicationInterval(int interval)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_BT_COMMUNICATION_INTERVAL_CHARACTERISTIC_UUID, new byte[] { (byte)interval });
        }

        public async Task<BluetoothRobotConstants.UART_BAUD_RATE> ReadUartBuadRate()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_UART_BAUD_RATE_CHARACTERISTIC_UUID);

            return BaseService.ConvertEnumFromBytes<BluetoothRobotConstants.UART_BAUD_RATE>(data);
        }

        public async Task<bool> WriteUartBuadRate(BluetoothRobotConstants.UART_BAUD_RATE rate)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_UART_BAUD_RATE_CHARACTERISTIC_UUID, new byte[] { (byte)rate });
        }

        private async Task<bool> ResetModule(BluetoothRobotConstants.MODULE_PARAMETER type)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_RESET_MODULE_CHARACTERISTIC_UUID, new byte[] { (byte)type });
        }

        public async Task<bool> RestartModule()
        {
            return await ResetModule(BluetoothRobotConstants.MODULE_PARAMETER.RESTART_MODULE);
        }

        public async Task<bool> RestoreUserDataToFactorySetting()
        {
            return await ResetModule(BluetoothRobotConstants.MODULE_PARAMETER.RESET_MODULE_TO_REST_USER_DATA);
        }

        public async Task<bool> ResetModuleToFactorySetting()
        {
            return await ResetModule(BluetoothRobotConstants.MODULE_PARAMETER.RESET_MODULE_TO_FACTORY_SETTING);
        }

        public async Task<BluetoothRobotConstants.BOARDCAST_PERIOD> ReadBoardcastPeriod()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_BOARDCAST_PERIOD_CHARACTERISTIC_UUID);
            return BaseService.ConvertEnumFromBytes<BluetoothRobotConstants.BOARDCAST_PERIOD>(data);
        }

        public async Task<bool> WriteBoardcastPeriod(BluetoothRobotConstants.BOARDCAST_PERIOD period)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_BOARDCAST_PERIOD_CHARACTERISTIC_UUID, new byte[] { (byte)period });
        }

        public async Task<short> ReadProductId()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_PRODUCT_ID_CHARACTERISTIC_UUID);
            return BaseService.ConvertShortFromBytes(data);
        }

        public async Task<bool> WriteProductId(short id)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_PRODUCT_ID_CHARACTERISTIC_UUID, BaseService.ConvertBytesFromShort(id));
        }

        public async Task<BluetoothRobotConstants.TRANSMIT_POWER> ReadTransmitPower()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_TRANSMIT_POWER_CHARACTERISTIC_UUID);
            return BaseService.ConvertEnumFromBytes<BluetoothRobotConstants.TRANSMIT_POWER>(data);
        }

        public async Task<bool> WriteTransmitPower(BluetoothRobotConstants.TRANSMIT_POWER power)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_TRANSMIT_POWER_CHARACTERISTIC_UUID, new byte[] { (byte)power });
        }

        public async Task<bool> EnableToReadBoardcastData(bool enable, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> callback)
        {
            return await base.SetupCharacteristicNotifyAsync(BluetoothRobotConstants.MODULE_PARAMETER_CUSTOM_BOARDCAST_DATA_CHARACTERISTIC_UUID, enable, callback);
        }

        public async Task<bool> WriteBoardcastData(Dictionary<byte, byte> data)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_CUSTOM_BOARDCAST_DATA_CHARACTERISTIC_UUID, BaseService.ConvertBytesFromDictionary(data));
        }

        public async Task<bool> WriteBoardcastDataToDefault()
        {
            byte[] data = new byte[16];
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_CUSTOM_BOARDCAST_DATA_CHARACTERISTIC_UUID, data);
        }

        public async Task<bool> ForceModuleSleep()
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_REMOTE_CONTROL_EXTENSION_CHARACTERISTIC_UUID, new byte[] { (byte)BluetoothRobotConstants.REMOTE_CONTROL_EXTENSION.FORCE_SLEEP_MODE });
        }

        public async Task<bool> SaveCurrentIOState()
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_REMOTE_CONTROL_EXTENSION_CHARACTERISTIC_UUID, new byte[] { (byte)BluetoothRobotConstants.REMOTE_CONTROL_EXTENSION.SAVE_IO_STATE });
        }

        public async Task<bool> WriteCurrentCustomBoardcastDataToFlash()
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_REMOTE_CONTROL_EXTENSION_CHARACTERISTIC_UUID, new byte[] { (byte)BluetoothRobotConstants.REMOTE_CONTROL_EXTENSION.WRITE_CUSTOM_BOARDCAST_DATA_TO_FLASH });
        }

        public async Task<bool> ReadStandByPulseSleepMode()
        {
            byte[] data = await base.ReadChacateristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_STANDBY_MODE_CHARACTERISTIC_UUID);
            return BaseService.ConvertBooleanFromBytes(data);
        }

        public async Task<bool> WriteStandByPulseSleepMode(bool isPulseSleepMode)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_STANDBY_MODE_CHARACTERISTIC_UUID, new byte[] { (byte)(isPulseSleepMode ? 1 : 0) });
        }

        public async Task<bool> DisconnectCurrentBTClient()
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_STANDBY_MODE_CHARACTERISTIC_UUID, new byte[] { (byte)BluetoothRobotConstants.REMOTE_CONTROL_EXTENSION.DISCONNECT_BT_CLIENT });
        }

        public async Task<bool> WriteConnectedBoadcastData(byte[] data)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_SET_BT_COMMUNICATION_DATA_CHARACTERISTIC_UUID, data);
        }

        public async Task<bool> EnableConnectedBoardcast(bool enable)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.MODULE_PARAMETER_SET_BT_COMMUNICATION_ONOFF_CHARACTERISTIC_UUID, new byte[] { (byte)(enable ? 1 : 0) });
        }
    }
}
