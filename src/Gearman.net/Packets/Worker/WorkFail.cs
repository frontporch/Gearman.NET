using System;
using System.Text;

namespace Gearman.Packets.Worker
{
    public class WorkFail : Packet
    {
        public string jobhandle; 
  		
        public WorkFail(String jobhandle)
        {
            this.jobhandle = jobhandle;
            this._Size = jobhandle.Length; 
            this._Type = PacketType.WORK_FAIL;
        }
  		
        public WorkFail(byte[] pktdata) : base(pktdata)
        {
            int pOff = 0;
            pOff = ParseString(pOff, ref jobhandle);
        }
  		
        override public byte[] ToByteArray()
        {
            byte[] result = new byte[this._Size + 12]; 
            byte[] header = base.ToByteArray();
            byte[] jhbytes = new ASCIIEncoding().GetBytes(jobhandle);
            Array.Copy(header, result, header.Length);
            Array.Copy(jhbytes, 0, result, header.Length, jhbytes.Length);
            return result;
        }
    }
}