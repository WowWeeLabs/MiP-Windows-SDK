using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothRobotControlLib.Common.Sevices
{
    public class DFUService : BaseService
    {
        public DFUService() : base(BluetoothRobotConstants.DFU_SERVICE_UUID)
        {
        }

        public async Task<bool> RebootToMode(byte mode)
        {
            return await base.WriteCharacteristicValueAsync(BluetoothRobotConstants.DFU_CHARACTERISTIC_UUID, new byte[] { mode });
        }
    }
}
