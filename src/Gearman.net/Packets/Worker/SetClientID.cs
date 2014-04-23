namespace Gearman.Packets.Worker
{
    public class SetClientID : Packet
    {
        public string instanceid;
  		
        public SetClientID(string instanceid)
        {
            this.instanceid = instanceid;
            this._Type = PacketType.SET_CLIENT_ID;
        }
    }
}