namespace Gearman.Packets.Worker
{
    public class JobAssign : Packet 
    {
        public string jobhandle;
        public string taskname; 
        public byte[] data; 
  		
        public JobAssign()
        {
            this._Type = PacketType.JOB_ASSIGN;
        }
  		
        public JobAssign(string jobhandle, string taskname, byte[] data)
        {
            this.jobhandle = jobhandle; 
            this.taskname = taskname;
            this.data = data; 
        }
  		
        public JobAssign(byte[] pktdata) : base(pktdata)
        {
            int pOff = 0;
            pOff = ParseString(pOff, ref jobhandle);
            pOff = ParseString(pOff, ref taskname);
            data = _RawData.Slice(pOff, _RawData.Length);
        }
    }
}