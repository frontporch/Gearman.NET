using System;
using System.Text;

namespace Gearman.Packets.Worker
{
    public class WorkStatus : Packet 
    {
        public string jobhandle; 
        public int completenumerator; 
        public int completedenominator;
  		
        public WorkStatus()
        {
        }
  		
        public WorkStatus(string jobhandle, int numerator, int denominator)
        {
            this.jobhandle = jobhandle;
            this.completenumerator = numerator;
            this.completedenominator = denominator;
            this._Type = PacketType.WORK_STATUS;
            this._Size = jobhandle.Length + 1 + completenumerator.ToString().Length + 1 + completedenominator.ToString().Length;
        }
  		
        public WorkStatus(byte[] pktdata) : base(pktdata)
        {
            int pOff = 0;
            string numerator = "", denominator = "";
            pOff = ParseString(pOff, ref jobhandle);
            pOff = ParseString(pOff, ref numerator);
            pOff = ParseString(pOff, ref denominator);
  			
            completedenominator = Int32.Parse(denominator);
            completenumerator = Int32.Parse(numerator);
        }
  		
        override public byte[] ToByteArray()
        {
            byte[] result = new byte[this._Size + 12]; 
            byte[] header = base.ToByteArray();
            byte[] msgdata = new ASCIIEncoding().GetBytes(jobhandle + '\0' + completenumerator + '\0' + completedenominator);
            Array.Copy(header, result, header.Length);
            Array.Copy(msgdata, 0, result, header.Length, msgdata.Length);
            return result;
        }
    }
}