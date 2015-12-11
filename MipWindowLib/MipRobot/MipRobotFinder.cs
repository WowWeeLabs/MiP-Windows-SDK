using BluetoothRobotControlLib.Common;
using BluetoothRobotControlLib.MipRobot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipWindowLib.MipRobot
{
    public class MipRobotFinder : BluetoothRobotFinder<MipRobot>
    {
        //static functions
        private static MipRobotFinder instance = null;

        public static MipRobotFinder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MipRobotFinder();
                }

                return instance;
            }
        }
    }
}
