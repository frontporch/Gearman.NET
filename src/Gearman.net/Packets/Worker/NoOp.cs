namespace Gearman.Packets.Worker
{
    public class NoOp : Packet
    {
        public NoOp()
        {
            this._Type = PacketType.NOOP;
        }
    }
}