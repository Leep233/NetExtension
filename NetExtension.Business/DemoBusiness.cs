using NetExtension.Core.Framework;
using System;

namespace NetExtension.Business
{
    public class DemoBusiness:BusinessModule
    {
        private string Tag;



        public override void Create(object arg = null)
        {
            Tag = this.GetType().FullName;

            Console.WriteLine(Tag + " created");
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

            Console.WriteLine($"{Tag} Protected Method");
        }

        private void PrivateMethod()
        {

            Console.WriteLine($"{Tag} Private Method");
        }

        public void PublicMethod()
        {
            Console.WriteLine($"{Tag} Public Method");
        }


        public void PublicMethod(int num)
        {

            Console.WriteLine($"{Tag} Public Method ( arg = {num})");

        }

        public string PublicMethod(string message)
        {
            return message;
        }

    }
}
