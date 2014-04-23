namespace Gearman.Packets.Worker
{
    public class ResetAbilities : Packet
    {
        public ResetAbilities() 
        {
            this._Type = PacketType.RESET_ABILITIES;
        }
    }
}