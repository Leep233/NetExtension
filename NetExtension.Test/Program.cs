// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using NetExtension.Core.Framework;
using NetExtension.Core.Text;
using NetExtension.Test;

ModuleTest();

void ModuleTest() {
    Tester.Register();

    Tester.ExecuteModuleFunction();

}