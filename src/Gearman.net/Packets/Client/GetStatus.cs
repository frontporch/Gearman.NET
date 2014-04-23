using System;
using System.Text;

namespace Gearman.Packets.Client
{
    public class GetStatus : Packet
    {
        public string JobHandle;
  		
        public GetStatus()
        {
        }
  		
        public GetStatus(string jobHandle)
        {
            this.JobHandle = jobHandle;
            this._Type = PacketType.GET_STATUS;
            this._Size = jobHandle.Length; 
        }
  		
        override public byte[] ToByteArray()
        {
            byte[] result = new byte[this._Size + 12]; 
            byte[] jhbytes = new ASCIIEncoding().GetBytes(this.JobHandle);
            Array.Copy(this.Header, result, this.Header.Length);
            Array.Copy(jhbytes, 0, result, this.Header.Length, jhbytes.Length);
            return result;
        }
    }
}