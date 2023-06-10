using NetExtension.Core.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using static NetExtension.Core.Framework.BusinessModule;

namespace NetExtension.Core.Framework
{
    /// <summary>
    /// 业务模块管理
    /// </summary>
    public sealed class ModuleManager : ServiceModule<ModuleManager>
    {

        public event EventHandler<string> ModuleCreationFailured;

        /// <summary>
        /// 
        /// </summary>
        private  Dictionary<string, BusinessModule> loadedModuls;

        /// <summary>
        /// 模块还未创建时 需要等待监听的事件
        /// </summary>
        private Dictionary<string, ModuleEventTable<object>> perlistenerEvents;

        /// <summary>
        /// 模块还未创建时 需要等待处理的事件
        /// </summary>
        private Dictionary<string, List<ModuleMessage>> perprocessingMsg;

        /// <summary>
        /// 业务模块管理初始化
        /// </summary>
        /// <param name="arg">业务模块实例所在的命名空间</param>
        public override void Initialize(object arg = null)
        {
            loadedModuls = new Dictionary<string, BusinessModule>();

            perlistenerEvents = new Dictionary<string, ModuleEventTable<object>>();

            perprocessingMsg = new Dictionary<string, List<ModuleMessage>>();

            base.Initialize(arg);
        }

        /// <summary>
        /// 创建业务模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        public T Register<T>(object arg = null) where T : BusinessModule
        {
            Type type = typeof(T);

            string moduleName = type.Name;
   
            return CreateInstance(moduleName, type, arg) as T;   
        }

        /// <summary>
        /// 创建业务模块
        /// </summary>
        /// <param name="moduleName">模块类名</param>
        /// <param name="assemblyName">所在程序集名称</param>
        /// <param name="arg">创建参数</param>
        /// <returns></returns>
        public BusinessModule Register(string domain,string moduleName, object arg = null)
        {
            string typePath = string.Format("{0}.{1}", domain, moduleName);

            Assembly assembly = Assembly.GetEntryAssembly();

            Type type = assembly.GetType(typePath); //Type.GetType(typePath, false);// 

            BusinessModule module = CreateInstance(moduleName, type, arg);

            if (module != null)
                loadedModuls.Add(moduleName, module);

            return module;
        }

        /// <summary>
        /// 创建业务模块
        /// </summary>
        /// <param name="domain">模块所在程序域（命名空间）名称</param>
        /// <param name="moduleName">模块类名</param>
        /// <param name="assemblyName">所在程序集名称</param>
        /// <param name="arg">创建参数</param>
        /// <returns></returns>
        public BusinessModule RegisterByAssembly(string assemblyFile, string domain, string moduleName, object arg)
        {
            string typePath = $"{domain}.{moduleName}";

            Assembly assembly = Assembly.LoadFrom(assemblyFile);

            Type type = assembly.GetType(typePath);

            BusinessModule module = CreateInstance(moduleName, type, arg);

            if (module != null)
                loadedModuls.Add(moduleName, module);

            return module;
        }

        private BusinessModule CreateInstance(string moduleName,Type type,object arg) 
        {
            BusinessModule module = null;

            if (type is null)
            {
                ModuleCreationFailured?.Invoke(this, $"module's type is null!!! check path [{moduleName}]");
            }
            else
            {
                module = Activator.CreateInstance(type) as BusinessModule;

                if (module is null)
                {
                    ModuleCreationFailured?.Invoke(this, $"create module instance failed!!! check path [{moduleName}]");
                }
                else
                {
                    module.Name = moduleName;

                    if (perlistenerEvents.ContainsKey(moduleName))
                    {
                        module.SetEventTable(perlistenerEvents[moduleName]);
                        perlistenerEvents.Remove(moduleName);
                    }
                    
                    module.Create(arg);

                    if (perprocessingMsg.ContainsKey(moduleName))
                    {
                        List<ModuleMessage> msgList = perprocessingMsg[moduleName];

                        for (int i = 0; i < msgList.Count; i++)
                        {
                            module.HandleMessage(msgList[i].Method, msgList[i].Args);
                        }

                        perprocessingMsg[moduleName].Clear();

                        perprocessingMsg.Remove(moduleName);
                    }

                }
            }
            return module;
        }


  

        /// <summary>
        /// 获取已经加载的模块
        /// </summary>
        /// <typeparam name="T">模块类</typeparam>
        /// <returns></returns>
        public T Module<T>() where T : BusinessModule
        {
            return Module(typeof(T).Name) as T;
        }

        /// <summary>
        /// 获取已经加载的模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public BusinessModule Module(string moduleName)
        {
            if (loadedModuls.ContainsKey(moduleName))
                return loadedModuls[moduleName];
            return null;
        }


        /// <summary>
        /// 事件监听 ，如果模块未创建 将会缓存，待模块创建时 设置监听
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="eventName">事件名称</param>
        /// <returns></returns>
        public IListener<Action<object>> Event(string moduleName, string eventName)
        {
            IListener<Action<object>> frameworkEvent = null;

            if (loadedModuls.ContainsKey(moduleName))
            {
                frameworkEvent = loadedModuls[moduleName].Event(eventName);
            }
            else
            {
                frameworkEvent = PerlistenerEvent(moduleName, eventName);
            }
            return frameworkEvent;
        }

        /// <summary>
        /// 指定模块需要处理的函数 ,如果模块未创建 将会缓存，待模块创建时执行
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="args">函数参数</param>
        public ModuleResult SendMessage(string moduleName, string methodName, params object[] args)
        {
            ModuleResult result = new ModuleResult();

            if (loadedModuls.ContainsKey(moduleName))
            {
                result.Result = loadedModuls[moduleName].HandleMessage(methodName, args);
            }
            else
            {
                PerprocessingMessageList(moduleName).Add(new ModuleMessage(moduleName,methodName, args));

                result.Error = string.Format("[{0}] not created !!! we'll cache until {0} to created.",moduleName);
            }

            return result;
        }

        /// <summary>
        /// 指定模块需要处理的函数 ,如果模块未创建 将会缓存，待模块创建时执行
        /// </summary>
        /// <example>
        /// <code>
        /// 
        ///  ModuleResult<int> result =  ModuleManager.Instance.SendMessage<int>("TestBinsinssModule","Sum",1,2);
        ///  
        ///  Debug.WriteLine(result.Result);
        ///  
        /// </code>
        /// </example>
        /// <param name="moduleName">模块名称</param>
        /// <param name="methodName">函数名称</param>
        /// <param name="args">函数参数</param>
        public ModuleResult<T> SendMessage<T>(string moduleName, string methodName, params object[] args)
        {
            ModuleResult<T> result = new ModuleResult<T>();

            if (loadedModuls.ContainsKey(moduleName))
            {
                result.Result = (T)loadedModuls[moduleName].HandleMessage(methodName, args);
            }
            else
            {
                PerprocessingMessageList(moduleName).Add(new ModuleMessage(moduleName, methodName, args));

                result.Error = string.Format("[{0}] not created !!! we'll cache until {0} to created.", moduleName);
            }

            return result;
        }

        /// <summary>
        /// 指定业务模块UI界面展示
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="arg"></param>
        public void ShowUI(string moduleName, object arg)
        {
            SendMessage(moduleName, "ShowUI", arg);
        }

        /// <summary>
        /// 释放所有注册过的模块
        /// </summary>
        /// <param name="arg"></param>
        public override void Release(params object [] arg)
        {
            if (perlistenerEvents != null)
            {
                foreach (var item in perlistenerEvents)
                {
                    item.Value.Clear();
                }
                perlistenerEvents.Clear();
            }

            if (perprocessingMsg != null)
            {
                foreach (var item in perprocessingMsg)
                {
                    item.Value.Clear();
                }
                perprocessingMsg.Clear();
            }

            if (loadedModuls != null)
            {
                foreach (var item in loadedModuls)
                {
                    item.Value.Release();
                }
                loadedModuls.Clear();
            }
        }

        private FrameworkEvent<object> PerlistenerEvent(string moduleName, string eventName)
        {
            if (!perlistenerEvents.ContainsKey(moduleName))
                perlistenerEvents.Add(moduleName, new ModuleEventTable<object>());

            return perlistenerEvents[moduleName].Event(eventName);
        }

        private List<ModuleMessage> PerprocessingMessageList(string moduleName)
        {
            if (!perprocessingMsg.ContainsKey(moduleName))
                perprocessingMsg.Add(moduleName, new List<ModuleMessage>());

            return perprocessingMsg[moduleName];

        }
      
        /// <summary>
        /// 
        /// </summary>
         class ModuleMessage
        {

            public string  Module { get; set; }
            /// <summary>
            /// 函数名称
            /// </summary>
            public string Method { get; set; }
            /// <summary>
            /// 函数需要传入的参数
            /// </summary>
            public object[] Args { get; set; }

            public ModuleMessage(string module,string method,object [] args)
            {
                Module = module;
                Method = method;
                Args = args;
            }
        }
    }
}
