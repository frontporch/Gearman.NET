using System;
using System.Text;

namespace Gearman.Packets.Worker
{
    public class WorkData : Packet
    { 
        public String jobhandle; 
        public byte[] data; 
  		
        public WorkData()
        {
            this._Type = PacketType.WORK_DATA;
        }
  		
        public WorkData(String jobhandle, byte[] data)
        {
            this.jobhandle = jobhandle; 
            this.data = data;
            this._Size = jobhandle.Length + 1 + data.Length; 
            this._Type = PacketType.WORK_DATA;
        }
  		
        public WorkData(byte[] pktdata) : base(pktdata)
        {
            int pOff = 0;
            pOff = ParseString(pOff, ref jobhandle);
            data = _RawData.Slice(pOff, _RawData.Length);
        }
  		
        override public byte[] ToByteArray()
        {
            byte[] result = new byte[this._Size + 12]; 
            byte[] header = base.ToByteArray();
            byte[] jhbytes = new ASCIIEncoding().GetBytes(jobhandle + '\0');
            Array.Copy(header, result, header.Length);
            Array.Copy(jhbytes, 0, result, header.Length, jhbytes.Length);
            Array.Copy(data, 0, result, header.Length + jhbytes.Length, data.Length);
            return result;
        }
    }
}