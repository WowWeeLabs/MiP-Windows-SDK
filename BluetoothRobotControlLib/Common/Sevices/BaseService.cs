using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class BaseService
    {
        public enum TYPE
        {
            SEND_DATA,
            RECEIVE_DATA,
            BATTERY_LEVEL,
            RSSI_REPORT,
            DEVICE_INFO,
            DEVICE_SETTING,
            DFU,
            MODULE_PARAMETER
        };

        public Guid ServiceUUID { get; private set; }
        public DeviceInformation ServiceInfo { get; private set; }
        public GattDeviceService Service { get; private set; }

        protected BaseService(Guid serviceUUID, DeviceInformation serviceInfo = null)
        {
            ServiceUUID = serviceUUID;
            ServiceInfo = serviceInfo;
        }

        public async Task<bool> ConnectServiceAsync()
        {
            if (ServiceUUID != null)
            {
                if (ServiceInfo == null)
                {
                    string deviceSelector = GattDeviceService.GetDeviceSelectorFromUuid(ServiceUUID);
                    DeviceInformationCollection collection = await DeviceInformation.FindAllAsync(deviceSelector);
                    ServiceInfo = collection.FirstOrDefault();
                }

                if (ServiceInfo != null)
                {
                    Service = await GattDeviceService.FromIdAsync(ServiceInfo.Id);

                    return true;
                }
            }

            return false;
        }

        protected async Task<bool> WriteCharacteristicValueAsync(Guid chacteristicUUID, byte[] data)
        {
            if (Service == null)
            {
                await ConnectServiceAsync();
            }

            if (Service != null)
            {
                GattCharacteristic characteristic = Service.GetCharacteristics(chacteristicUUID).FirstOrDefault();
                if (characteristic != null)
                {
                    GattCommunicationStatus status;
                    if (characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.WriteWithoutResponse))
                    {
                        status = await characteristic.WriteValueAsync(data.AsBuffer(), GattWriteOption.WriteWithoutResponse);
                    }
                    else
                    {
                        status = await characteristic.WriteValueAsync(data.AsBuffer(), GattWriteOption.WriteWithoutResponse);
                    }

                    return status == GattCommunicationStatus.Success;
                }
            }

            return false;
        }

        protected async Task<byte[]> ReadChacateristicValueAsync(Guid characteristicUUID)
        {
            if (Service == null)
            {
                await ConnectServiceAsync();
            }

            if (Service != null)
            {
                GattCharacteristic characteristic = Service.GetCharacteristics(characteristicUUID).FirstOrDefault();
                if (characteristic != null)
                {
                    GattReadResult result = await characteristic.ReadValueAsync(Windows.Devices.Bluetooth.BluetoothCacheMode.Uncached);
                    if (result.Status == GattCommunicationStatus.Success)
                    {
                        return result.Value.ToArray();
                    }
                }
            }

            return null;
        }

        protected async Task<bool> SetupCharacteristicNotifyAsync(Guid characteristicUUID, bool enable, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> callback)
        {
            if (Service == null)
            {
                await ConnectServiceAsync();
            }

            if (Service != null)
            {
                GattCharacteristic characteristic = Service.GetCharacteristics(characteristicUUID).FirstOrDefault();
                if (characteristic != null)
                {
                    GattClientCharacteristicConfigurationDescriptorValue value = enable ? GattClientCharacteristicConfigurationDescriptorValue.Notify : GattClientCharacteristicConfigurationDescriptorValue.None;

                    GattCommunicationStatus status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(value);
                    if (status == GattCommunicationStatus.Success)
                    {
                        if (enable)
                        {
                            characteristic.ValueChanged += callback;
                        }
                        else
                        {
                            characteristic.ValueChanged -= callback;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        public static byte[] ConvertBytesFromHexBytes(byte[] hexBytes)
        {
            string hex = UTF8Encoding.UTF8.GetString(hexBytes, 0, hexBytes.Count());

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static int ConvertIntFromBytes(byte[] bytes)
        {
            if (bytes != null)
            {
                if (bytes.Length == 1)
                {
                    return (int)bytes[0];
                }
            }

            return -1;
        }

        public static string ConvertStringFromByes(byte[] bytes)
        {
            return UTF8Encoding.UTF8.GetString(bytes);
        }

        public static string ConvertHexStringFromByes(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", String.Empty);
        }

        public static byte[] ConvertBytesFromString(string str)
        {
            return UTF8Encoding.UTF8.GetBytes(str);
        }

        public static T ConvertEnumFromInt<T>(int intVal)
        {
            return (T)Enum.Parse(typeof(T), intVal.ToString());
        }

        public static T ConvertEnumFromBytes<T>(byte[] bytes)
        {
            return ConvertEnumFromInt<T>(ConvertIntFromBytes(bytes));
        }

        public static short ConvertShortFromBytes(byte[] bytes)
        {
            if (bytes.Length == 2)
            {
                return (short)((bytes[1] << 8) + bytes[0]);
            }

            return -1;
        }

        public static byte[] ConvertBytesFromShort(short shortVal)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(shortVal & 0xff);
            bytes[1] = (byte)((shortVal >> 8) & 0xff);

            return bytes;
        }

        public static Dictionary<byte, byte> ConvertDictionaryFromBytes(byte[] bytes)
        {
            Dictionary<byte, byte> dict = new Dictionary<byte, byte>();

            for (int i = 0; i < bytes.Length; i += 2)
            {
                dict[bytes[i + 0]] = bytes[i + 1];
            }

            return dict;
        }

        public static byte[] ConvertBytesFromDictionary(Dictionary<byte, byte> dict)
        {
            List<byte> data = new List<byte>();

            foreach (KeyValuePair<byte, byte> item in dict)
            {
                data.Add(item.Key);
                data.Add(item.Value);
            }

            return data.ToArray();
        }

        public static bool ConvertBooleanFromBytes(byte[] bytes)
        {
            return bytes[0] != 0;
        }
    }
}
