
using System;
using Gearman.Packets.Worker;


namespace Gearman
{

	public class Job
	{
		private readonly JobAssign _JobPacket; 
		
		public Job(JobAssign jobAssign, Worker worker)
		{
			this._JobPacket = jobAssign; 
			this._JobWorker = worker; 
		}

		private Worker _JobWorker; 
		public Worker JobWorker 
		{
			get 
			{
				return _JobWorker;
			}
			set 
			{
				_JobWorker = value;
			}
		}
		
		public void SendResults(byte[] results)
		{
			JobWorker.Conn.SendPacket(new WorkComplete(this.JobHandle, results));  
		}
		
		public void SendWorkUpdate(WorkStatus wspkt)
		{
			JobWorker.Conn.SendPacket(wspkt);				
		}
		
		public byte[] Data 
		{ 
			get 
			{
				return _JobPacket.data;
			}
			
			set 
			{ 
				_JobPacket.data = value;
			}
		}
		
		public string JobHandle 
		{
			get 
			{ 
				return _JobPacket.jobhandle;
			}
			
			set 
			{ 
				_JobPacket.jobhandle = value;
			}
		}
		
		public string TaskName 
		{ 
			get 
			{ 
				return _JobPacket.taskname;
			}
			
			set 
			{ 
				_JobPacket.taskname = value; 
			}
		}
		
	}

}
