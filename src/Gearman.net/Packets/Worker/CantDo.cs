using System;
using System.Text;

namespace Gearman.Packets.Worker
{
    public class CantDo : Packet
    {
        public string functionName;
  		
        public CantDo(string function)
        {
            this.functionName = function;
            this._Size = function.Length;
            this._Type = PacketType.CANT_DO;
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