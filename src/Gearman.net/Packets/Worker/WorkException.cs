namespace Gearman.Packets.Worker
{
    public class WorkException : Packet
    {
        public string jobhandle; 
        public byte[] exception; 
  		
        public WorkException(string jobhandle, byte[] exception)
        {
            this.jobhandle = jobhandle;
            this.exception = exception;
            this._Size = jobhandle.Length + 1 + exception.Length;
            this._Type = PacketType.WORK_EXCEPTION;
        }
  		
        public WorkException(byte[] pktdata) : base(pktdata)
        {
            int pOff = 0;
            pOff = ParseString(pOff, ref jobhandle);
            exception = pktdata.Slice(pOff, _RawData.Length);
        }
    }
}