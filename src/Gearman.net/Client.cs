
using System;
using System.Collections.Generic;
using Gearman.Packets.Client;
using Gearman.Packets.Worker;


namespace Gearman
{
    public class Client
    {
        private readonly List<Connection> _Managers; 
        
        private int _ConnectionIndex;
        
        public enum JobPriority 
        { 
            HIGH = 1, 
            NORMAL,
            LOW
        };
        
        private Client()
        {
            _Managers = new List<Connection>();
        }
            
        public Client (string hostName, int port) : this()
        {
            Connection c = new Connection(hostName, port);
            _Managers.Add(c);
        }
        
        public Client (string hostName) : this(hostName, 4730) 
        { 
            
        }
        
        public void AddNewConnection(string hostName, int port) 
        {
            _Managers.Add(new Connection(hostName, port));
        }

        public void AddNewConnection(string hostName)
        {
            this.AddNewConnection(hostName, 4730);
        }
        
        public byte[] SubmitJob(string jobName, byte[] jobData)
        {
            
            try {
                Packet packet = null;
                bool submitted = false;
                Connection connection = null; 
                
                string jobid = System.Guid.NewGuid().ToString();
                
                SubmitJob submitJob = new SubmitJob(jobName, jobid, jobData, false);
                
                while(!submitted) 
                {
                    
                    // Simple round-robin submission for now
                    connection = _Managers[_ConnectionIndex++ % _Managers.Count];
                    
                    connection.SendPacket(submitJob);
                
                    // We need to get back a JOB_CREATED packet
                    packet = connection.GetNextPacket();
                    
                    // If we get back a JOB_CREATED packet, we can continue
                    // otherwise try the next job manager
                    if (packet.Type == PacketType.JOB_CREATED) 
                    {
                        submitted = true; 	
                    }
                }
                
                
                // This method handles synchronous requests, so we wait 
                // until we get a work complete packet
                while(true) { 
                    
                    packet = connection.GetNextPacket(); 
                
                    if(packet.Type == PacketType.WORK_COMPLETE)
                    {
                        WorkComplete wc = (WorkComplete)packet;
                        
                        return wc.data;
                    } 
                }
        
            } 
            catch (Exception) 
            { 
                return null;
            }
        }
                
        public string SubmitJobInBackground(string jobName, byte[] jobData, JobPriority jobPriority)
        {
            try {
                Connection connection = null; 
                
                string jobid = System.Guid.NewGuid().ToString();

                SubmitJob submitJob = new SubmitJob(jobName, jobid, jobData, true, jobPriority);
                    
                Packet packet;
                
                while(true) 
                {
                    
                    // Simple round-robin submission for now
                    connection = _Managers[_ConnectionIndex++ % _Managers.Count];
                    
                    connection.SendPacket(submitJob);
                    
                    // We need to get back a JOB_CREATED packet
                    packet = connection.GetNextPacket();
                    
                    // If we get back a JOB_CREATED packet, we can continue,
                    // otherwise try the next job manager
                    if (packet.Type == PacketType.JOB_CREATED) 
                    {
                        return ((JobCreated)packet).JobHandle;
                    }
                }
                
        
            } 
            catch (Exception) 
            {
                return null;
            }
        }
    
        public bool CheckIsDone(string jobHandle)
        {
            GetStatus statusPkt = new GetStatus(jobHandle);
            
            Packet packet = null; 
            
            foreach (Connection connection in _Managers)
            {
                connection.SendPacket(statusPkt);
                
                packet = connection.GetNextPacket(); 
                
                if(packet.Type == PacketType.STATUS_RES)
                {		
                    StatusResponse statusResult = (StatusResponse)packet; 
                    
                    if(statusResult.JobHandle != jobHandle) 
                    { }
                    else 
                    {
                        
                        float percentdone = 0;
                        
                        if(statusResult.PercentCompleteDenominator != 0)
                        {
                            percentdone = statusResult.PercentCompleteNumerator / statusResult.PercentCompleteDenominator; 						
                        }  
                            
                        
                        // Check to see if this response has a known status 
                        // and if it's Running
                        
                        if(statusResult.KnownStatus && statusResult.Running) 
                        {		
                            
                        } 
                        else 
                        {
                            if (!statusResult.KnownStatus)
                            { }

                            if (!statusResult.Running)
                            { }
                        }	
                        
                        return (percentdone == 1);
                        
                    }
                }		
            }	
            
            return false;
        }
    
        public List<Connection> Managers 
        { 
            get 
            { 
                return this._Managers;	
            }
        }
    }
    
}
