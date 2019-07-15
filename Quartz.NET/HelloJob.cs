using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Quartz.NET
{
    public class HelloJob : IJob
    {
        Task IJob.Execute(IJobExecutionContext context)
        {
            return Task.Run(()=> { Console.WriteLine("作业执行!" + DateTime.Now.ToString()); });
        }
    }
}
