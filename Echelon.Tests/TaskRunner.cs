//using System;
//using System.Threading;
//using Echelon.TaskRunner;
//using NUnit.Framework;
//using Rhino.ServiceBus;
//using StructureMap;
//using StructureMap.Configuration.DSL;
//
//namespace Echelon.Tests
//{
//    public class TaskRunner
//    {
//        [Test]
//        public void TaskRunner_SendReceive_Message_Tests()
//        {
//            var container = new Container();
//            var client = new TaskRunnerClient<IOnewayBus>(container);
//            client.Bus.Send(new HelloWorldCommand
//            {
//                Text = "Test"
//            });
//
//            TaskRunnerServer.Start<TaskRunnerBootStrapper>(container);
//            Thread.Sleep(TimeSpan.FromSeconds(2));
//        }
//    }
////
////    public sealed class TaskRunnerBootStrapper: StructureMapBootStrapper
////    {
////        public TaskRunnerBootStrapper(IContainer container) : base(container)
////        {
////            ConfigureContainer();
////
////            Container.Configure(cfg =>
////            {
////                cfg.AddRegistry<TestRegistry>();
////            });
////        }
////    }
//
//    public class TestRegistry : Registry
//    {
//        public TestRegistry()
//        {
////            Scan(scan => For<ICustomLogger>().Transient().Use(scope => new Log4NetFileLogger()));
//        }
//    }
//
//    public class HelloWorldCommand
//    {
//        public string Text { get; set; }
//    }
//}