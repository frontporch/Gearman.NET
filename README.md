# Gearman.net

### What is Gearman
Gearman provides a generic application framework to farm out work to other machines or processes that are better suited to do the work. It allows you to do work in parallel, to load balance processing, and to call functions between languages. It can be used in a variety of applications, from high-availability web sites to the transport of database replication events. In other words, it is the nervous system for how distributed processes communicate. For more see [Gearman.org](http://gearman.org)

### Gearman.net in action
A Gearman powered application consists of three parts: a client, a worker, and a job server. The client is responsible for creating a job to be run and sending it to a job server. The job server will find a suitable worker that can run the job and forwards the job on. The worker performs the work requested by the client and sends a response to the client through the job server. The Job Server (gearmand) can be obtained from [Gearman.org](http://gearman.org) along with installation and configuration instructions. Gearman.net is a .Net assembly that provides the Client and Worker API's.

```csharp
  //Create a Worker and Register a Task with gearmand
  var gWorker = new Gearman.Worker("10.1.0.60", 4710);
  gWorker.RegisterFunction("Reverse", ReverseString);
  gWorker.WorkLoop();

  //Create a Client and Submit a Job To Server
  var jobBytes = System.Text.ASCIIEncoding.ASCII.GetBytes("String To Reverse");
  var gClient = new Gearman.Client("10.1.0.60", 4710);
  var handle = 
    gClient.SubmitJobInBackground("Reverse", jobBytes, Gearman.Client.JobPriority.HIGH);
  while(!gClient.CheckIsDone(handle))
  {
      System.Threading.Thread.Sleep(1000);
  }
```
The client and worker APIs (along with the job server) deal with the job management and network communication so you can focus on the application parts. There a few different ways you can run jobs in Gearman, including background for asynchronous processing and prioritized jobs.

### License

The MIT License (MIT)

Copyright (c) 2014 Front Porch

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
