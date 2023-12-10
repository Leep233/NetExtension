using System;
using System.Collections.Generic;
using System.Text;

namespace NetExtension.Core.Framework
{
    public interface IModuleResult
    {
        string Message { get; }

        Exception Exception { get; }

        object Content { get; }

    }


    public interface IModuleResult<T>
    {
        string Message { get; }

        Exception Exception { get; }

        T Content { get; }

    }
}
