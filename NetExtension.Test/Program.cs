// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using NetExtension.Core.Framework;
using NetExtension.Test;

string path = @"D:\Projects\net-extension\netextension\NetExtension.Business\bin\Debug\netstandard2.0\NetExtension.Business.dll";

string moduleName = "DemoBusiness";

ModuleManager.Instance.Initialize();

ModuleManager.Instance.CreateModule("NetExtension.Business", moduleName, path,null);

//ModuleManager.Instance.CreateModule<DemoBusiness>();

//ModuleManager.Instance.CreateModule(moduleName);


try
{
    ModuleManager.Instance.HandleMessage(moduleName, "PrivateStaticMethod");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

try
{

 
    ModuleManager.Instance.HandleMessage(moduleName, "ProtectedStaticMethod");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

ModuleManager.Instance.HandleMessage(moduleName, "PublicStaticMethod");


try
{
ModuleManager.Instance.HandleMessage(moduleName, "PrivateStaticMethod");
}
catch (Exception ex)
{
Console.WriteLine(ex.Message);
}


try
{
    ModuleManager.Instance.HandleMessage(moduleName, "ProtectedMethod");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

ModuleManager.Instance.HandleMessage(moduleName, "PublicMethod");


ModuleManager.Instance.HandleMessage(moduleName, "PublicMethod", 1);


string message = ModuleManager.Instance.HandleMessage(moduleName, "PublicMethod", "Hello world")?.ToString()??"";

Console.WriteLine(message);


Console.Read();