using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetExtension.Core.IO
{
    /// <summary>
    /// 指针
    /// </summary>
    public class MemoryBlock
    {
        /// <summary>
        /// 对象转指针 注意 这个是非托管内容 注意释放
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static IntPtr ToIntPtr(object structure)
        {
            int size = Marshal.SizeOf(structure);

            return Marshal.AllocHGlobal(size);
        }

        /// <summary>
        /// 对象转指针 注意 这个是非托管内容 注意释放
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IntPtr ToIntPtr(Type type)
        {
            int size = Marshal.SizeOf(type);

            return Marshal.AllocHGlobal(size);
        }

        /// <summary>
        /// 指针转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr"></param>
        /// <param name="isFree">是否释放传入指针</param>
        /// <returns></returns>

        public static T ToStructure<T>(IntPtr ptr, bool isFree = true)
        {
            T t = (T)Marshal.PtrToStructure(ptr, typeof(T));

            if (isFree)
                Marshal.FreeHGlobal(ptr);

            return t;
        }


        /// <summary>
        /// 二进制数组转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            T target = default(T);
            using (Stream sm = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                target = (T)bf.Deserialize(sm);
            }
            return target;
        }

        /// <summary>
        /// 对象转2进制数组
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static byte[] Serialize(object target)
        {
            byte[] data;
            using (Stream sm = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(sm, target);
                sm.Position = 0;
                data = new byte[sm.Length];
                sm.Read(data, 0, data.Length);
            }
            return data;
        }

    }
}
