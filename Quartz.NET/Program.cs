using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl;

namespace Quartz.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerHelper.Info("服务启动:"+DateTime.Now.ToString());

            RunProgram().GetAwaiter().GetResult();

            Console.WriteLine("输入任意字符关闭。。。");
            Console.ReadKey();
        }

        private static async Task RunProgram()
        {
            try
            {
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };

                // 创建作业调度器
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                IScheduler scheduler = await factory.GetScheduler();

                // 启动调度器
                await scheduler.Start();

                // 创建作业
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()//马上执行
                    //.StartAt(DateTime.Parse("04:00"))//计划某一时间执行
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(5)//执行周期
                        .RepeatForever()//重复执行无限制次数
                        //.WithRepeatCount(100)//重复执行100次
                        )
                    .Build();

                // 加入到作业调度器中
                await scheduler.ScheduleJob(job, trigger);
            }
            catch (SchedulerException se)
            {
                LoggerHelper.Error("任务启动异常", se);
                await Console.Error.WriteLineAsync(se.ToString());
            }
        }
    }
}
