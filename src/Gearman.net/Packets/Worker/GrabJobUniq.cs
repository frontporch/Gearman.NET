namespace Gearman.Packets.Worker
{
    public class GrabJobUniq : Packet
    {
        public GrabJobUniq()
        {
            this._Type = PacketType.GRAB_JOB_UNIQ;
        }
    }
}