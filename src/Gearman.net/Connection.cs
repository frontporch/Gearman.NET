using System;
using System.Net;
using System.Net.Sockets;
using Gearman.Packets.Client; 
using Gearman.Packets.Worker; 

namespace Gearman
{	
	public class Connection
	{
        private readonly Socket _Conn; 
        private string _HostName;
        private readonly int port;
        private IPAddress _Address;
        private readonly IPEndPoint _EndPoint;
		
		public Connection()
		{	}
		
		public Connection(string hostName, int port) 
		{	
			this._HostName = hostName; 
			this.port = port; 
				
			try 
            {
				_Address = IPAddress.Parse(hostName);
			} 
            catch (Exception) 
            {
				// Connect to the first DNS entry
				_Address = Dns.GetHostAddresses(hostName)[0];	
			}
			
			
			_EndPoint = new IPEndPoint(_Address, port);
		
			if(_Address.AddressFamily.Equals(AddressFamily.InterNetworkV6))
			{
				//IPv6 hostname
				_Conn = new Socket(
						AddressFamily.InterNetworkV6,
						SocketType.Stream,
						ProtocolType.Tcp);
			} 
            else 
            {
				// IPv4 IP or hostname
				_Conn = new Socket(
					AddressFamily.InterNetwork, 
					SocketType.Stream,
					ProtocolType.Tcp);
			}		
			
			try 
            { 
				_Conn.Connect(_EndPoint);
			} 
            catch (Exception e) 
            { 
				Console.WriteLine("Error initializing connection to job server: " + e.ToString());
			}

		}
	
		public void SendPacket(Packet packet)
		{
			try 
            { 
				_Conn.Send(packet.ToByteArray());
			} 
            catch (Exception) 
            {
			}
		}
		
		public override String ToString()
		{
			return String.Format("{0}:{1}", this._HostName, this.port);
		}
		
		public Packet GetNextPacket()
		{
			int messagesize = -1; 
			int messagetype = -1; 
			
			// Initialize to 12 bytes (header only), and resize later as needed
			byte[] header = new byte[12];
			byte[] packet; 
			
			messagesize = -1; 
			
			try 
            {
				// Read the first 12 bytes (header)
				_Conn.Receive(header, 12, 0);
						
				// Check byte count
				byte[] sizebytes = header.Slice(8,12); 
				byte[] typebytes = header.Slice(4,8);
				
				if(BitConverter.IsLittleEndian)
				{
					Array.Reverse(sizebytes);
					Array.Reverse(typebytes);
				}		
				
				messagesize = BitConverter.ToInt32(sizebytes, 0);
				messagetype = BitConverter.ToInt32(typebytes, 0);
				
				if (messagesize > 0) 
				{					
					// Grow packet buffer to fit Data
					packet = new byte[12 + messagesize];
					Array.Copy(header, packet, header.Length);
					
					// Receive the remainder of the message 
					_Conn.Receive(packet, 12, messagesize, 0); 
				} 
                else 
                {
					packet = header; 
				}
										
				switch((PacketType)messagetype)
				{
					case PacketType.JOB_CREATED:
						return new JobCreated(packet);
						
					case PacketType.WORK_DATA:
						return new WorkData(packet);
						
					case PacketType.WORK_WARNING:
						return new WorkWarning(packet);
						
					case PacketType.WORK_STATUS:
						return new WorkStatus(packet);
						
					case PacketType.WORK_COMPLETE:
						return new WorkComplete(packet);
						
					case PacketType.WORK_FAIL:
						return new WorkFail(packet);
						
					case PacketType.WORK_EXCEPTION:
						return new WorkException(packet);
						
					case PacketType.STATUS_RES:
						return new StatusResponse(packet);
						
					case PacketType.OPTION_RES:
						// TODO Implement option response
						break;
					
					/* Client and JobWorker response packets */
					case PacketType.ECHO_RES:
						// TODO Implement the echo response
						break;
					case PacketType.ERROR:
						// TODO Implement the error packet
						break;
			
					/* Worker response packets */
					case PacketType.NOOP:
						return new NoOp();
						
					case PacketType.NO_JOB:
						return new NoJob();
					
					case PacketType.JOB_ASSIGN:
						return new JobAssign(packet);
						
					case PacketType.JOB_ASSIGN_UNIQ:
						return new JobAssignUniq(packet);
						
					default: 
						return null;
				}
				
			} 
            catch (Exception e) 
            { 
				Console.WriteLine("Exception reading data: {0}", e.ToString());
				return null;
			} 
			
			return null;
		}
	}
}
