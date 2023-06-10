using NetExtension.Core.Events;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;

namespace NetExtension.Core.Framework
{

    public class BusinessModule : Module
    {
        private ModuleEventTable<object> _moduleEventTable;

        private SynchronizationContext _context;


        public BusinessModule()
        {
            _context = SynchronizationContext.Current;

        }

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="arg"></param>
        public virtual void Create(object arg = null)
        {
            Invoke("OnCreated", arg);
        }

        /// <summary>
        /// 外部使用，
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IListener<Action<object>> Event(string @event)
        {
            return EventTable.Event(@event);
        }

        /// <summary>
        /// 如果需要显示UI，
        /// </summary>
        /// <param name="arg"></param>
        public void ShowUI(params object[] args)
        {
            if (_context != null)
            {
                _context.Post(o => { OnShowUI(o); }, args);
            }
        }

        /// <summary>
        /// 重写必须调用基类
        /// </summary>
        /// <param name="arg"></param>
        public override void Release(params object[] args)
        {
            EventTable.Clear();
        }


        protected virtual void OnShowUI(params object[] args)
        {

        }

        protected void Invoke(string @event, object arg)
        {
            EventTable.Event(@event).Invoke(arg);
        }

        /// <summary>
        /// 清空某个事件监听
        /// </summary>
        /// <param name="event"></param>
        protected void Clear(string @event)
        {

            EventTable.Event(@event).Clear();
        }



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
        /// 外部设置当前业务模块事件表
        /// </summary>
        /// <param name="table"></param>
        internal void SetEventTable(ModuleEventTable<object> table)
        {
            this._moduleEventTable = table;
        }

        /// <summary>
        /// 业务模块与业务模块之间的消息处理
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        internal object HandleMessage(string methodName, params object[] args)
        {
            Type[] types = args.Select(a => a.GetType()).ToArray();

            MethodInfo methodInfo = this.GetType().GetRuntimeMethod(methodName, types);

            if (methodInfo is null)
            {
                throw new NotImplementedException($"not found method({methodName}) by [{this.GetType().Name}](maybe  method is private/protected)");
            }

            return methodInfo.Invoke(this, args);
        }

    }
}
