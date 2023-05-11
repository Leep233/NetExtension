using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetExtension.Core.Events
{

    public interface IListener<T> {

        void AddListener(T arg);
        void RemoveListener(T arg);
        void Clear();

    }

    public abstract class EventBase: IListener<Action>
    {
        protected  Action listeners;
        public virtual void AddListener(Action action)
        {

            if (listeners is null)
                listeners = new Action(action);
            else
                listeners += action;
        }
        public virtual void RemoveListener(Action action) {
            if (listeners is null) return;
            listeners -= action;
        }
        public virtual void Clear() { listeners = null; }
        public void Invoke() 
        {
            listeners?.Invoke();
        }       
    }

    public abstract class EventBase<T>: IListener<Action<T>>
    {
        protected Action<T> listeners;
        public virtual void AddListener(Action<T> action)
        {
            if (listeners is null)
                listeners = new Action<T>(action);
            else
                listeners += action;
        }
        public virtual void RemoveListener(Action<T> action)
        {
            if (listeners is null) return;
            listeners -= action;
        }
        public virtual void Clear() { listeners = null; }

        public void Invoke(T arg)
        {
            listeners?.Invoke(arg);
        }
    }

    public abstract class EventBase<T0,T1> : IListener<Action<T0, T1>>
    {
        protected Action<T0, T1> listeners;
        public virtual void AddListener(Action<T0, T1> action)
        {
            if (listeners is null) listeners = new Action<T0, T1>(action);
            else listeners += action;
        }
        public virtual void RemoveListener(Action<T0, T1> action)
        {
            if (listeners is null) return;
            listeners -= action;
        }
        public virtual void Clear() { listeners = null;  }
        public void Invoke(T0 arg0, T1 arg1)
        {
            listeners?.Invoke(arg0, arg1);
        }
    }

    public abstract class EventBase<T0, T1, T2> : IListener<Action<T0, T1, T2>>
    {
        protected Action<T0, T1, T2> listeners;

        public virtual void AddListener(Action<T0, T1, T2> action)
        {
            if (listeners is null) listeners = new Action<T0, T1, T2>(action);
            else listeners += action;
        }
        public virtual void RemoveListener(Action<T0, T1, T2> action)
        {
            listeners -= action;
        }
        public virtual void Clear() { listeners = null; }
        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            listeners.Invoke(arg0, arg1, arg2);
        }
    }

    public abstract class EventBase<T0, T1, T2, T3>
    {
        protected Action<T0, T1, T2, T3> listeners;
        public virtual void AddListener(Action<T0, T1, T2, T3> action)
        {
            if (listeners is null) listeners = new Action<T0, T1, T2, T3>(action);
            else listeners += action;
        }

        public virtual void RemoveListener(Action<T0, T1, T2, T3> action)
        {
            listeners -= action;
        }
        public virtual void Clear() { listeners = null; }
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            listeners.Invoke(arg0, arg1, arg2, arg3);
        }
    }
}
