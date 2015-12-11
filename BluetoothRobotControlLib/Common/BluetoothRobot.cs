using BluetoothRobotControlLib.Common.Sevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace BluetoothRobotControlLib.Common
{
    abstract public class BluetoothRobot
    {
        public DeviceInformation SendDataServiceInfo { get; set; }

        protected Dictionary<BaseService.TYPE, BaseService> Services { get; private set; }

        public Dictionary<byte, byte> CustomBoardcastData { get; protected set; }

        //public event handlers

        public event EventHandler<BluetoothRobot> DidConnectedEvent;

        public event EventHandler<BluetoothRobot> DidDisconnectedEvent;

        public BluetoothRobot(DeviceInformation sendDataServiceInfo = null)
        {
            SendDataServiceInfo = sendDataServiceInfo;
        }

        public virtual async Task<bool> Connect()
        {
            Services = PrepareServices();

            bool ok = true;

            if (Services.ContainsKey(BaseService.TYPE.RECEIVE_DATA))
            {
                ReceiveDataService receiveDataService = (ReceiveDataService)Services[BaseService.TYPE.RECEIVE_DATA];
                ok = await receiveDataService.NeedReceiveData(true, DidCharacteristicNotify);
            }

            if (ok)
            {
                DidConnectedEvent?.Invoke(this, this);
            }

            return ok;
        }

        protected Task<bool> SendCommand(byte type, params byte[] data)
        {
            List<byte> rawData = new List<byte>();
            //type
            rawData.Add(type);
            //data
            foreach (byte d in data)
            {
                rawData.Add(d);
            }

            return GetService<SendDataService>(BaseService.TYPE.SEND_DATA).SendData(rawData.ToArray());
        }

        //Define Abstract Functions

        protected virtual Dictionary<BaseService.TYPE, BaseService> PrepareServices()
        {
            Dictionary<BaseService.TYPE, BaseService> services = new Dictionary<BaseService.TYPE, BaseService>();
            services.Add(BaseService.TYPE.SEND_DATA, new SendDataService(SendDataServiceInfo));
            services.Add(BaseService.TYPE.RECEIVE_DATA, new ReceiveDataService());
            services.Add(BaseService.TYPE.BATTERY_LEVEL, new BatteryLevelService());
            services.Add(BaseService.TYPE.RSSI_REPORT, new RSSIReportService());
            services.Add(BaseService.TYPE.DEVICE_INFO, new DeviceInfoService());
            services.Add(BaseService.TYPE.DEVICE_SETTING, new DeviceSettingService());
            services.Add(BaseService.TYPE.DFU, new DFUService());
            services.Add(BaseService.TYPE.MODULE_PARAMETER, new ModuleParameterService());

            return services;
        }

        protected abstract void DidNotifyByReceiveDataCharacteristic(byte[] data);

        protected virtual void DidReceiveFirmwareData(byte[] data) { }

        protected virtual void DidCharacteristicNotify(GattCharacteristic characteristic, GattValueChangedEventArgs args)
        {
            byte[] data = args.CharacteristicValue.ToArray();

            if (characteristic.Uuid == BluetoothRobotConstants.RECEIVE_DATA_CHARACTERISTIC_UUID)
            {
                if (IsBTFirmwareUpdateMode())
                {
                    DidReceiveFirmwareData(data);
                }
                else
                {
                    DidNotifyByReceiveDataCharacteristic(BaseService.ConvertBytesFromHexBytes(data));
                }
            }
            else if (characteristic.Uuid == BluetoothRobotConstants.RSSI_REPORT_SERVICE_UUID)
            {
                Debug.WriteLine("RSSI: " + BaseService.ConvertIntFromBytes(data));
            }
            else if (characteristic.Uuid == BluetoothRobotConstants.MODULE_PARAMETER_CUSTOM_BOARDCAST_DATA_CHARACTERISTIC_UUID)
            {
                CustomBoardcastData = BaseService.ConvertDictionaryFromBytes(data);
            }
        }

        //Public Service Functions

        protected T GetService<T>(BaseService.TYPE serviceType) where T : BaseService
        {
            if (Services.ContainsKey(serviceType))
            {
                return (T)Services[serviceType];
            }

            throw new Exception(serviceType.ToString() + " is missed.");
        }

        public Task<bool> EnableBTReceiveDataNotification(bool enable)
        {
            return GetService<ReceiveDataService>(BaseService.TYPE.RECEIVE_DATA).NeedReceiveData(enable, DidCharacteristicNotify);
        }

        public void SetBTFirmwareUpdateMode(bool enable)
        {
            GetService<ReceiveDataService>(BaseService.TYPE.RECEIVE_DATA).IsFirmwareUpdateMode = enable;
        }

        public bool IsBTFirmwareUpdateMode()
        {
            return GetService<ReceiveDataService>(BaseService.TYPE.RECEIVE_DATA).IsFirmwareUpdateMode;
        }

        public Task<int> GetBTBatteryLevel()
        {
            return GetService<BatteryLevelService>(BaseService.TYPE.BATTERY_LEVEL).ReadBatteryLevel();
        }

        public Task<bool> EnableBTRSSIReport(bool enable, int reportInterval)
        {
            return GetService<RSSIReportService>(BaseService.TYPE.RSSI_REPORT).NeedRSSIReport(enable, reportInterval, DidCharacteristicNotify);
        }

        public Task<string> GetBTSystemId()
        {
            return GetService<DeviceInfoService>(BaseService.TYPE.DEVICE_INFO).ReadSystemId();
        }

        public Task<string> GetBTModuleSoftwareVersion()
        {
            return GetService<DeviceInfoService>(BaseService.TYPE.DEVICE_INFO).ReadModuleSoftwareVersion();
        }

        public Task<BluetoothRobotConstants.ACTIVIATION_STATUS> GetBTProductActiviationStatus()
        {
            return GetService<DeviceSettingService>(BaseService.TYPE.DEVICE_SETTING).ReadProductActivationStatus(); ;
        }

        public Task<bool> SetBTProductActiviationStatus(BluetoothRobotConstants.ACTIVIATION_STATUS status)
        {
            return GetService<DeviceSettingService>(BaseService.TYPE.DEVICE_SETTING).WriteProductActivationStatus(status);
        }

        public Task<bool> BTNordicReoobtToMode(byte mode)
        {
            return GetService<DFUService>(BaseService.TYPE.DFU).RebootToMode(mode);
        }

        public Task<string> GetBTDeviceName()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadBTDeviceName();
        }

        public Task<bool> SetBTDeviceName(string name)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteBTDeviceName(name);
        }

        public Task<int> GetBTCommunicationInterval()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadBTCommunicationInterval();
        }

        public Task<BluetoothRobotConstants.UART_BAUD_RATE> GetBTUARTBuardRate()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadUartBuadRate();
        }

        public Task<bool> SetBTUARTBuardRate(BluetoothRobotConstants.UART_BAUD_RATE rate)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteUartBuadRate(rate);
        }

        public Task<bool> RestartBThDevice()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).RestartModule();
        }

        public Task<bool> FactoryResetBTDevice(bool userDataOnly)
        {
            if (userDataOnly)
            {
                return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).RestoreUserDataToFactorySetting();
            }
            else
            {
                return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ResetModuleToFactorySetting();
            }
        }

        public Task<bool> SetBTBoardcastPeriod(BluetoothRobotConstants.BOARDCAST_PERIOD period)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteBoardcastPeriod(period);
        }

        public Task<BluetoothRobotConstants.BOARDCAST_PERIOD> GetBTBoadrcastPeriod()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadBoardcastPeriod();
        }

        public Task<bool> SetBTProductId(short id)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteProductId(id);
        }

        public Task<short> GetBTProductId()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadProductId();
        }

        public Task<bool> SetBTTransmitPower(BluetoothRobotConstants.TRANSMIT_POWER power)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteTransmitPower(power);
        }

        public Task<BluetoothRobotConstants.TRANSMIT_POWER> GetBTTransmitPower()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadTransmitPower();
        }

        public Task<bool> SetBTBoardcstaData(Dictionary<byte, byte> data)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteBoardcastData(data);
        }

        public Task<bool> SetBTBoardcastDataToDefault()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteBoardcastDataToDefault();
        }

        public Task<bool> EnableToGetBTBoardcastData(bool enable)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).EnableToReadBoardcastData(enable, DidCharacteristicNotify);
        }

        public Task<bool> ForceBTModuleSleep()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ForceModuleSleep();
        }

        public Task<bool> SaveBTCurrentIOState()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).SaveCurrentIOState();
        }

        public Task<bool> SetBTStanBydPulseSleepMode(bool pulseSleepMode)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteStandByPulseSleepMode(pulseSleepMode);
        }

        public Task<bool> GetBTStandByPulseSleepMode()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).ReadStandByPulseSleepMode();
        }

        public Task<bool> SaveBTBoardcastDataToFlash()
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteCurrentCustomBoardcastDataToFlash();
        }

        public Task<bool> SetBTConnectedBoardcastData(byte[] data)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).WriteConnectedBoadcastData(data);
        }

        public Task<bool> EnableBTConnectedBoardcast(bool enable)
        {
            return GetService<ModuleParameterService>(BaseService.TYPE.MODULE_PARAMETER).EnableConnectedBoardcast(enable);
        }
    }
}
