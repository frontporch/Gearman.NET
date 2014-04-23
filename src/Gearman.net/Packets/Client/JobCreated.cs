using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gearman.Packets.Client
{
    public class JobCreated : Packet
    {
        public string JobHandle;

        public JobCreated()
        {
        }

        public JobCreated(byte[] pktData)
            : base(pktData)
        {
            int pOff = 0;
            pOff = ParseString(pOff, ref JobHandle);
        }

        public JobCreated(string jobHandle)
        {
            this.JobHandle = jobHandle;
            this._Size = jobHandle.Length;
        }
    }
}
