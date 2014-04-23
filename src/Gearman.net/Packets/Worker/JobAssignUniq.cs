namespace Gearman.Packets.Worker
{
    public class JobAssignUniq : Packet 
    {
        public string jobhandle, taskname, unique_id; 
        public byte[] data;
  		
        public JobAssignUniq()
        {
            this._Type = PacketType.JOB_ASSIGN_UNIQ;
        }
  		
        public JobAssignUniq(byte[] pktdata) : base(pktdata)
        {
            int pOff = 0;

            pOff = ParseString(pOff, ref jobhandle);
            pOff = ParseString(pOff, ref taskname);
            pOff = ParseString(pOff, ref unique_id);
            data = pktdata.Slice(pOff, pktdata.Length);
        }
    }
}