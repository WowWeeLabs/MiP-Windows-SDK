using BluetoothRobotControlLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace BluetoothRobotControlLib.MipRobot
{
    public class BluetoothRobotFinder<T> where T : BluetoothRobot, new()
    {
        //public 
        private List<T> foundRobotList = new List<T>();
        public List<T> FoundRobotList { get { return foundRobotList; } }

        private List<T> connectedRobotList = new List<T>();
        public List<T> ConnectedRobotList { get { return connectedRobotList; } }

        //public functions
        public IAsyncAction ScanForRobots(int scanDuration = 0)
        {
            if (scanDuration != 0)
            {
                //unavailable for WinRT
            }

            return UpdateFoundMipRobotList().AsAsyncAction();
        }

        public void StopScanForRobots()
        {
            //unavailable for WinRT
        }

        public T FirstConnectedRobot()
        {
            return ConnectedRobotList.FirstOrDefault();
        }

        public void ClearFoundRobotList()
        {
            ConnectedRobotList.Clear();

            FoundRobotList.Clear();
        }

        //private functions
        private async Task UpdateFoundMipRobotList()
        {
            string deviceSelector = GattDeviceService.GetDeviceSelectorFromUuid(BluetoothRobotConstants.SEND_DATA_SERVICE_UUID);
            DeviceInformationCollection collection = await DeviceInformation.FindAllAsync(deviceSelector);
            foreach (DeviceInformation info in collection)
            {
                //exclude any found mip
                if (FoundRobotList.FirstOrDefault(m => m.SendDataServiceInfo == info) == null)
                {
                    T robot = new T();
                    robot.SendDataServiceInfo = info;

                    //callback when mip was connected
                    robot.DidConnectedEvent += (sender, connectedMipRobot) =>
                    {
                        //prevent any duplicated mip
                        if (ConnectedRobotList.FirstOrDefault(m => m == connectedMipRobot) == null)
                        {
                            ConnectedRobotList.Add((T)connectedMipRobot);
                        }
                    };

                    //callback when mip was disconnected
                    robot.DidDisconnectedEvent += (sender, disconnectedMipRobot) =>
                    {
                        ConnectedRobotList.Remove((T)disconnectedMipRobot);

                        FoundRobotList.Remove((T)disconnectedMipRobot);
                    };

                    //TODO: show log for found new mip

                    FoundRobotList.Add(robot);
                }
            }
        }
    }
}
