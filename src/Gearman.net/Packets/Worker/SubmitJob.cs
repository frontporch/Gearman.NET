using System;
using System.Text;

namespace Gearman.Packets.Worker
{
    public class SubmitJob : Packet
    {
        public string functionName, uniqueId; 
        public byte[] data; 
  		
        public SubmitJob()
        {
        }
  		
        public SubmitJob(string function, string uniqueId, byte[] data, bool background)
        {
            this.functionName = function; 
            this.uniqueId = uniqueId; 
            this.data = data;
            this._Size = function.Length + 1 + uniqueId.Length + 1 + data.Length;
  			
            if (background)
            {
                this._Type = PacketType.SUBMIT_JOB_BG;
            }
            else
            {
                this._Type = PacketType.SUBMIT_JOB;
            }
        }
  		
        public SubmitJob(string function, string uniqueId, byte[] data, bool background, Gearman.Client.JobPriority priority) : this(function, uniqueId, data, background)
        {
            switch (priority)
            {
                case Gearman.Client.JobPriority.HIGH:
                    this._Type = background ? PacketType.SUBMIT_JOB_HIGH_BG : PacketType.SUBMIT_JOB_HIGH;
                    break;
                case Gearman.Client.JobPriority.NORMAL:
                    this._Type = background ? PacketType.SUBMIT_JOB_BG : PacketType.SUBMIT_JOB;
                    break;
                case Gearman.Client.JobPriority.LOW:
                    this._Type = background ? PacketType.SUBMIT_JOB_LOW_BG : PacketType.SUBMIT_JOB_LOW;
                    break;
                default:
                    break;
            }
        }
  		
        override public byte[] ToByteArray()
        {
            byte[] result = new byte[this._Size + 12]; 
            byte[] metadata = new ASCIIEncoding().GetBytes(functionName + '\0' + uniqueId + '\0');
            Array.Copy(this.Header, result, this.Header.Length);
            Array.Copy(metadata, 0, result, this.Header.Length, metadata.Length);
            Array.Copy(data, 0, result, Header.Length + metadata.Length, data.Length);
            return result;
        }
    }
}