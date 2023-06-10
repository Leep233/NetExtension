using NetExtension.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetExtension.Test
{
    internal class Tester
    {
       const string moduleName = "DemoBusiness";

        public static void RegisterModuleByAssembly()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetExtension.Business.dll");

            if (File.Exists(path)) { Console.WriteLine("Exists"); }

            string domain = "NetExtension.Business";

            ModuleManager.Instance.Initialize();

            ModuleManager.Instance.RegisterByAssembly(path, domain, moduleName, null);
        }

        public static void Register<T>()
        {
            ModuleManager.Instance.Initialize();

            DemoBusiness business =  ModuleManager.Instance.Register<DemoBusiness>();

           
        }

        public static void Register() {

            string moduleName = "DemoBusiness";

            string domain = "NetExtension.Test";

            ModuleManager.Instance.Initialize();

            ListenerModuleEvent();

            ModuleManager.Instance.Register( domain, moduleName, null);
        }

        public static void ListenerModuleEvent() {

            
            ModuleManager.Instance.Event(moduleName, "OnMethod").AddListener(_ => { Console.WriteLine(_); });

            ModuleManager.Instance.Event(moduleName, "OnCreated").AddListener(_ => { Console.WriteLine("创建啦"); });
        }

        public static void ExecuteModuleFunction() {

            try
            {
                ModuleManager.Instance.SendMessage(moduleName, "PrivateStaticMethod");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {


                ModuleManager.Instance.SendMessage(moduleName, "ProtectedStaticMethod");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ModuleManager.Instance.SendMessage(moduleName, "PublicStaticMethod");


            try
            {
                ModuleManager.Instance.SendMessage(moduleName, "PrivateStaticMethod");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            try
            {
                ModuleManager.Instance.SendMessage(moduleName, "ProtectedMethod");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ModuleManager.Instance.SendMessage(moduleName, "PublicMethod");


            ModuleManager.Instance.SendMessage(moduleName, "PublicMethod", 1);


            ModuleResult<string> result = ModuleManager.Instance.SendMessage<string>(moduleName, "PublicMethod", "Hello world");

            Console.WriteLine(result.Result);

        }
    }
}
