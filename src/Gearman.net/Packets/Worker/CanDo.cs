using System;
using System.Text;

namespace Gearman.Packets.Worker
{
    public class CanDo : Packet
    {
        public string functionName; 
  		
        public CanDo(string function)
        {
            this._Type = PacketType.CAN_DO;
            this.functionName = function; 
            this._Size = functionName.Length;
        }
  		
        override public byte[] ToByteArray()
        {
            byte[] result = new byte[this._Size + 12]; 
            byte[] functionbytes = new ASCIIEncoding().GetBytes(functionName);
            Array.Copy(this.Header, result, this.Header.Length);
            Array.Copy(functionbytes, 0, result, this.Header.Length, functionbytes.Length);
            return result;
        }
    }
}