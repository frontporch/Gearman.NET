namespace Gearman.Packets.Worker
{
    public class NoJob : Packet 
    {
        public NoJob()
        {
            this._Type = PacketType.NO_JOB;
        }
    }
}