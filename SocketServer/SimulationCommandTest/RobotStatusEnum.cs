using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace socketClient
{
    class RobotStatusEnum
    {
        public enum RobotStatus
        {
            Login = 0, Ready = 1, Move = 2, Paused = 3, Loading = 4, Unloading = 5,
            ChargeIn = 6, ChargeOut = 7, Charging = 8, ChargeStopping = 9, Error = 10, EmergencyStop = 11
        }
    }
}
