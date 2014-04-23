using System;

namespace Gearman.Packets.Client
{
    public class StatusResponse : Packet 
    {
        public string JobHandle; 
        public bool KnownStatus; 
        public bool Running; 
        public int PercentCompleteNumerator;
        public int PercentCompleteDenominator; 
  			
        public StatusResponse()
        {
        }
  		
        public StatusResponse(byte[] pktData) : base(pktData)
        {
            int pOff = 0; 
            pOff = ParseString(pOff, ref JobHandle);
            KnownStatus = ((int)Char.GetNumericValue((char)_RawData[pOff]) == 1);
  			
            // increment past the null terminator
            pOff += 2;
            Running = ((int)Char.GetNumericValue((char)_RawData[pOff]) == 1);
  			
            pOff += 2;
            string numerator = "";
            pOff = ParseString(pOff, ref numerator);
  			
            string denominator = "";
            pOff = ParseString(pOff, ref denominator);
  			
            PercentCompleteDenominator = int.Parse(denominator);
            PercentCompleteNumerator = int.Parse(numerator);
        }
    }
}