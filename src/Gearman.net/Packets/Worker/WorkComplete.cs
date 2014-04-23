using System;

namespace Gearman.Packets.Worker
{
    public class WorkComplete : WorkData 
    {
        public WorkComplete(String jobhandle, byte[] data) : base(jobhandle, data)
        {
            this._Type = PacketType.WORK_COMPLETE;
        }
  		
        public WorkComplete(byte[] pktdata) : base(pktdata)
        {
            this._Type = PacketType.WORK_COMPLETE;
        }
    }
}