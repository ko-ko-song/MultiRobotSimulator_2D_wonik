using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace socketClient
{
    class MessageTypeEnum
    {
		public enum MessageType
		{
			RTSR = unchecked((int)0x53469DFD),
			AckMove = unchecked((int)0x37911A75), AckEndMove = unchecked((int)0x403A1BC4),
			AckCancelMove = unchecked((int)0x4AB1FE2D), AckEndCancelMove = unchecked((int)0x31F00931),
			AckLoad = unchecked((int)0xC7C3BE2C), AckEndLoad = unchecked((int)0x441C94CE),
			AckUnload = unchecked((int)0x868F59B6), AckEndUnload = unchecked((int)0x85D82ED1),
			AckCharge = unchecked((int)0x9A427515), AckEndCharge = unchecked((int)0x7F34A48F),
			AckChargeStop = unchecked((int)0x3412A63E), AckEndChargeStop = unchecked((int)0x06DE35AC),
			AckPause = unchecked((int)0XD96EDE0B), AckResume = 0X1E8509B2,
			AckEndPause = unchecked((int)0x57295C2B), AckEndResume = unchecked((int)0xBD2D122E),
			AckDoorOpen = unchecked((int)0x71B2005B), AckEndDoorOpen = unchecked((int)0xC9D2476A),
			AckDoorClose = unchecked((int)0x70F29DDD), AckEndDoorClose = unchecked((int)0x98378F4B),
			AckPersonCall = unchecked((int)0xA88DA5FE),
			ReqMove = unchecked((int)0x559657E8), ReqCancelMove = unchecked((int)0x83EDC641),
			ReqLoad = unchecked((int)0xF8957DC8), ReqUnload = unchecked((int)0xF03739D3),
			ReqCharge = unchecked((int)0x16B960EE), ReqChargeStop = unchecked((int)0xCBC6486F),
			ReqPause = unchecked((int)0x28EC89C1), ReqResume = unchecked((int)0xE09AF0FB),
			ReqDoorOpen = unchecked((int)0x078F486F), ReqDoorClose = unchecked((int)0x4DDA0F69)
		}
	}
}
