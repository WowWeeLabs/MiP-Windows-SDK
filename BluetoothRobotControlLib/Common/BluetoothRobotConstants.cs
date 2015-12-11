using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothRobotControlLib.Common
{
    public class BluetoothRobotConstants
    {
        public static readonly Guid SEND_DATA_SERVICE_UUID = new Guid("0000ffe5-0000-1000-8000-00805f9b34fb");
        public static readonly Guid SEND_DATA_CHARACTERISTIC_UUID = new Guid("0000ffe9-0000-1000-8000-00805f9b34fb");

        public static readonly Guid RECEIVE_DATA_SERVICE_UUID = new Guid("0000ffe0-0000-1000-8000-00805f9b34fb");
        public static readonly Guid RECEIVE_DATA_CHARACTERISTIC_UUID = new Guid("0000ffe4-0000-1000-8000-00805f9b34fb");

        public static readonly Guid BATTERY_LEVEL_SERVICE_UUID = new Guid("0000180f-0000-1000-8000-00805f9b34fb");
        public static readonly Guid BATTERY_LEVEL_REPORT_CHARACTERISTIC_UUID = new Guid("00002a19-0000-1000-8000-00805f9b34fb");

        public static readonly Guid RSSI_REPORT_SERVICE_UUID = new Guid("0000ffa0-0000-1000-8000-00805f9b34fb");
        public static readonly Guid RSSI_REPORT_READ_CHARACTERISTIC_UUID = new Guid("0000ffa1-0000-1000-8000-00805f9b34fb");
        public static readonly Guid RSSI_REPORT_SET_INTERVAL_CHARACTERISTIC_UUID = new Guid("0000ffa2-0000-1000-8000-00805f9b34fb");

        public static readonly Guid DEVICE_INFO_SERVICE_UUID = new Guid("0000180a-0000-1000-8000-00805f9b34fb");
        public static readonly Guid DEVICE_INFO_SYSTEM_ID_CHARACTERISTIC_UUID = new Guid("00002a23-0000-1000-8000-00805f9b34fb");
        public static readonly Guid DEVICE_INFO_MODULE_SOFTWARE_CHARACTERISTIC_UUID = new Guid("00002a26-0000-1000-8000-00805f9b34fb");

        public static readonly Guid DEVICE_SETTING_SERVICE_UUID = new Guid("0000ff10-0000-1000-8000-00805f9b34fb");
        public static readonly Guid DEVICE_SETTING_PRODUCT_ACTIVIATION_CHARACTERISTIC_UUID = new Guid("0000ff1b-0000-1000-8000-00805f9b34fb");

        /*
		Activation Status
		*/
        [Flags]
        public enum ACTIVIATION_STATUS : byte
        {
            NOT_READ = 0x80,
            FACTORY_DEFAULT = 0x00,
            ACTIVATE = 0x01,
            ACTIVATE_SENT_TO_FLURRY = 0x02,
            HACKER_UART_USED = 0x04,
            HACKER_UART_USED_SENT_TO_FLURRY = 0x08
        }

        public static readonly Guid DFU_SERVICE_UUID = new Guid("0000ff30-0000-1000-8000-00805f9b34fb");
        public static readonly Guid DFU_CHARACTERISTIC_UUID = new Guid("0000ff31-0000-1000-8000-00805f9b34fb");

        public static readonly Guid MODULE_PARAMETER_SERVICE_UUID = new Guid("0000ff90-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_DEVICE_NAME_CHARACTERISTIC_UUID = new Guid("0000ff91-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_BT_COMMUNICATION_INTERVAL_CHARACTERISTIC_UUID = new Guid("0000ff92-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_UART_BAUD_RATE_CHARACTERISTIC_UUID = new Guid("0000ff93-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_RESET_MODULE_CHARACTERISTIC_UUID = new Guid("0000ff94-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_BOARDCAST_PERIOD_CHARACTERISTIC_UUID = new Guid("0000ff95-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_PRODUCT_ID_CHARACTERISTIC_UUID = new Guid("0000ff96-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_TRANSMIT_POWER_CHARACTERISTIC_UUID = new Guid("0000ff97-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_CUSTOM_BOARDCAST_DATA_CHARACTERISTIC_UUID = new Guid("0000ff98-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_REMOTE_CONTROL_EXTENSION_CHARACTERISTIC_UUID = new Guid("0000ff99-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_STANDBY_MODE_CHARACTERISTIC_UUID = new Guid("0000ff9a-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_SET_BT_COMMUNICATION_DATA_CHARACTERISTIC_UUID = new Guid("0000ff9b-0000-1000-8000-00805f9b34fb");
        public static readonly Guid MODULE_PARAMETER_SET_BT_COMMUNICATION_ONOFF_CHARACTERISTIC_UUID = new Guid("0000ff9c-0000-1000-8000-00805f9b34fb");

        /*
        Uart Baud Rate
        */
        public enum UART_BAUD_RATE : byte
        {
            RATE_4800 = 0x00,
            RATE_9600 = 0x01,
            RATE_19200 = 0x02,
            RATE_38400 = 0x03,
            RATE_57600 = 0x04,
            RATE_115200 = 0x05
        }

        /*
        Module Parameters
        */
        public enum MODULE_PARAMETER : byte
        {
            RESET_MODULE_TO_FACTORY_SETTING = 0x36,
            RESET_MODULE_TO_REST_USER_DATA = 0x35,
            RESTART_MODULE = 0x55
        }

        /*
        Boardcast Period
        */
        public enum BOARDCAST_PERIOD : byte
        {
            PERIOD_200MS = 0x00,
            PERIOD_500MS = 0x01,
            PERIOD_1000MS = 0x02,
            PERIOD_1500MS = 0x03,
            PERIOD_2000MS = 0x04,
            PERIOD_2500MS = 0x05,
            PERIOD_3000MS = 0x06,
            PERIOD_4000MS = 0x07,
            PERIOD_5000MS = 0x08
        }

        /*
        Transmit Power
        */
        public enum TRANSMIT_POWER : byte
        {
            POWER_PLUS_4DBM = 0x00,
            POWER_ODBM = 0x01,
            POWER_MINUS_6DBM = 0x02,
            POWER_MINUS_23DBM = 0x03
        }

        /*
        Remote Control Extension
        */
        public enum REMOTE_CONTROL_EXTENSION : byte
        {
            SAVE_IO_STATE = 0x00,
            FORCE_SLEEP_MODE = 0x01,
            DISCONNECT_BT_CLIENT = 0x02,
            WRITE_CUSTOM_BOARDCAST_DATA_TO_FLASH = 0x03
        }
    }
}
