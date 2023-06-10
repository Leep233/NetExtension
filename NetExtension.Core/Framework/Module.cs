using System;
using System.Collections.Generic;
using System.Text;

namespace NetExtension.Core.Framework
{
    public abstract class Module 
    {
        public string Name { get; set; }

        public Module()
        {
            Name = this.GetType().Name;
        }

        public virtual void Release(params object[] args)
        {
           
        }
    }
}
