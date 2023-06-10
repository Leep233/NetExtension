using NetExtension.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetExtension.Test
{
    public class DemoBusiness : BusinessModule
    {

        public override void Create(object arg)
        {

            base.Create(arg);
        }

        private static void PrivateStaticMethod()
        {

            Console.WriteLine($"Private Static Method");
         

        }


        protected static void ProtectedStaticMethod()
        {

            Console.WriteLine("Protected Static Method");

      
        }

        public static void PublicStaticMethod()
        {

            Console.WriteLine($"Public Static Method");
        }

        protected void ProtectedMethod()
        {

            Console.WriteLine($"{Name} Protected Method");
        }

        private void PrivateMethod()
        {

            Console.WriteLine($"{Name} Private Method");

           
        }

        public void PublicMethod()
        {
            Console.WriteLine($"{Name} Public Method");

            Invoke("OnMethod","Method is Runing");
        }


        public void PublicMethod(int num)
        {
            Console.WriteLine($"{Name} Public Method ( arg = {num})");
        }

        public string PublicMethod(string message)
        {
            return message;
        }

    }
}
