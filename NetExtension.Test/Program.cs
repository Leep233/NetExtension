// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using NetExtension.Core.Framework;
using NetExtension.Core.Text;
using NetExtension.Test;

using System.Text.RegularExpressions;

string str = "     0XFF,0XE0,0XAA,   0XEAFF,    0X1234,   0xffff       ";


byte[] bytes = StringConverter.ToBytes(str);


Console.WriteLine(StringConverter.ToString(bytes));

