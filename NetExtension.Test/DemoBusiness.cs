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
        private string tag = "";
  
        public override void Create(object arg)
        {
            tag = GetType()?.FullName??"";

            Console.WriteLine(tag + " created");
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

            Console.WriteLine($"{tag} Protected Method");
        }

        private void PrivateMethod()
        {

            Console.WriteLine($"{tag} Private Method");
        }

        public void PublicMethod()
        {
            Console.WriteLine($"{tag} Public Method");
        }


        public void PublicMethod(int num)
        {
            Console.WriteLine($"{tag} Public Method ( arg = {num})");
        }

        public string PublicMethod(string message)
        {
            return message;
        }

    }
}
