namespace Gearman.Packets.Worker
{
    public class GrabJob : Packet
    {
        public GrabJob()
        {
            this._Type = PacketType.GRAB_JOB;
        }
    }
}