using System;
using System.Collections.Generic;
using System.Text;

namespace NetExtension.Core.Framework
{
    public class ModuleResult
    {

        public string Error { get; set; }

        public object Result { get; set; }
    }

    public class ModuleResult<T>
    {

        public string Error { get; set; }

        public T Result { get; set; }
    }
}
