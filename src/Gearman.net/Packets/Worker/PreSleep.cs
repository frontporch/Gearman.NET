namespace Gearman.Packets.Worker
{
    public class PreSleep : Packet
    {
        public PreSleep()
        {
            this._Type = PacketType.PRE_SLEEP;
        }
    }
}