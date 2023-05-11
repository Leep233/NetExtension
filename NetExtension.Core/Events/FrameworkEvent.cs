using System;
using System.Collections.Generic;
using System.Text;

namespace NetExtension.Core.Events
{
    public class FrameworkEvent:EventBase
    {

    }

    public class FrameworkEvent<T> : EventBase<T>
    {

    }

    public class FrameworkEvent<T0,T1> : EventBase<T0, T1>
    {

    }

    public class FrameworkEvent<T0, T1, T2> : EventBase<T0, T1,T2>
    {

    }

    public class FrameworkEvent<T0, T1, T2,T3> : EventBase<T0, T1, T2, T3>
    {

    }
}
