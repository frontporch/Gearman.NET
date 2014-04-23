using System;

namespace Gearman.Packets.Worker
{
    public class WorkWarning : WorkData 
    {
        public WorkWarning(String jobhandle, byte[] data) : base(jobhandle, data)
        {
            this._Type = PacketType.WORK_WARNING;
        }
  		
        public WorkWarning(byte[] pktdata) : base(pktdata)
        {
            this._Type = PacketType.WORK_WARNING;
        }
    }
}