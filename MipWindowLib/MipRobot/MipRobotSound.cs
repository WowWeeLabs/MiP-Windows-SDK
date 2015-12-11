using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipWindowLib.MipRobot
{
    public class MipRobotSound
    {
        public MipRobotConstants.SOUND_FILE File { get; set; }
        public byte Delay { get; set; }
        public int Volume { get; set; }

        public MipRobotSound(MipRobotConstants.SOUND_FILE file, byte delay=0, int volume=-1)
        {
            File = file;
            Delay = delay;
            Volume = volume;
        }
    }
}
