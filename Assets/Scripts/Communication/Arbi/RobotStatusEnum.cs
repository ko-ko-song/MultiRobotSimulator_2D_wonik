using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotStatusEnum 
{
    public enum RobotStatus
    {
        Login = 0, Ready = 1, Move = 2, Paused = 3, Loading = 4, Unloading = 5,
        ChargeIn = 6, ChargeOut = 7, Charging = 8, ChargeStopping = 9, Error = 10, EmergencyStop = 11,
        Packing = 12, IDLE = 13
    }
}
