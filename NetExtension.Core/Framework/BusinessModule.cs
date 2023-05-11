using NetExtension.Core.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NetExtension.Core.Framework
{

    public class BusinessModule:Module
    {
        private ModuleEventTable<object> _moduleEventTable;

        /// <summary>
        /// 获取当前业务模块事件表
        /// </summary>
        internal ModuleEventTable<object> EventTable
        {
            get
            {
                if (_moduleEventTable is null)
                    _moduleEventTable = new ModuleEventTable<object>();
                return _moduleEventTable;
            }
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="arg"></param>
        public virtual void Create(object arg = null)
        {
            
        }
       
        /// <summary>
        /// 业务模块监听事件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FrameworkEvent<object> Event(string name)
        {
            return EventTable.Event(name);
        }

        /// <summary>
        /// 外部设置当前业务模块事件表
        /// </summary>
        /// <param name="eventTable"></param>
        internal void SetEventTable(ModuleEventTable<object> eventTable)
        {
            this._moduleEventTable = eventTable;
        }

        /// <summary>
        /// 业务模块与业务模块之间的消息处理
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        internal object HandleMessage(string methodName, params object[] args)
        {
            Type[] types = new Type[args.Length]; 

            for (int i = 0; i < args.Length; i++)
            {
                types[i] = args[i].GetType();   
            }

            MethodInfo methodInfo = this.GetType().GetRuntimeMethod(methodName, types);

            if (methodInfo is null)
            {      
               throw new NotImplementedException($"not found method({methodName}) by [{this.GetType().Name}](maybe your method is private or protected)");
            }

            return methodInfo.Invoke(this, args);
        }

        /// <summary>
        /// 如果需要显示UI，需要重写
        /// </summary>
        /// <param name="arg"></param>
        public virtual void ShowUI(params object[] args) { }

        /// <summary>
        /// 重写必须调用基类
        /// </summary>
        /// <param name="arg"></param>
        public override void Release(params object [] args)
        {
            EventTable.Clear();
        }
    }
}
