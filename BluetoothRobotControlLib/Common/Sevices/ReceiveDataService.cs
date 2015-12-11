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
    public class ReceiveDataService : BaseService
    {
        public bool IsFirmwareUpdateMode { get; set; }

        public ReceiveDataService() : base(BluetoothRobotConstants.RECEIVE_DATA_SERVICE_UUID)
        {
        }

        public Task<bool> NeedReceiveData(bool needReceiveData, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> callback)
        {
            return base.SetupCharacteristicNotifyAsync(BluetoothRobotConstants.RECEIVE_DATA_CHARACTERISTIC_UUID, needReceiveData, callback);
        }
    }
}
