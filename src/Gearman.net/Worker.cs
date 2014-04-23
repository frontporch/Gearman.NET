using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gearman.Packets.Worker;

namespace Gearman
{
	public delegate void Taskdelegate(Job j); 
		
	public class Worker
	{
		private Connection _Conn;
		
		private Dictionary<string, Taskdelegate> _MethodMapping;
		
		private CancellationTokenSource _CancelTokenSource;

		private Task _RunningTask;

		public Worker()
		{
			_MethodMapping = new Dictionary<string, Taskdelegate>();
		}
		
		public Worker (string hostName, int port)
		{
			this._Conn = new Connection(hostName, port);
			_MethodMapping = new Dictionary<string, Taskdelegate>();
		}
		
		public Worker (string hostName) : this(hostName, 4730)
		{ }
		
		public void RegisterFunction(string taskName, Taskdelegate function)
		{	
			if(!_MethodMapping.ContainsKey(taskName))
			{
				_Conn.SendPacket(new CanDo(taskName));
			}
			
			_MethodMapping[taskName] = function;
		}
		
		public void WorkLoop()
		{
			_CancelTokenSource = new CancellationTokenSource();
			_RunningTask = new Task(() => this.Run(), this._CancelTokenSource.Token);
			_RunningTask.Start();
		}
		
		public void StopWorkLoop()
		{
			this._CancelTokenSource.Cancel();
			this._RunningTask.Wait();
		}
		
		public void Run()
		{
			while(! this._RunningTask.IsCanceled)
			{
				try
				{
					CheckForJob();
				}
				catch { }
				
				Thread.Sleep(2000);
			}		
		}

		private void CheckForJob()
		{
			ASCIIEncoding encoder = new ASCIIEncoding(); 
			
			_Conn.SendPacket(new GrabJob());
			
			Packet response = _Conn.GetNextPacket();
			
			if (response.Type == PacketType.JOB_ASSIGN)
			{
				Job job = new Job((JobAssign)response, this);
				
				// Fire event for listeners
				_MethodMapping[job.TaskName](job);
						
			}
		}
		
		public Connection Conn 
		{
			get 
			{
				return _Conn; 
			}
			
			set { }
		}
	}
}
