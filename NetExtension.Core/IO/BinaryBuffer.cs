using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetExtension.Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryBuffer : IDisposable
    {

        /// <summary>
        /// 缓存数组
        /// </summary>
        public byte[] Cache { get; private set; }

        /// <summary>
        /// 只读缓存
        /// </summary>
        public byte[] ReadOnlyCache
        {
            get
            {
                if (Cache == null || Cache.Length <= 0)
                    return null;

                byte[] data = new byte[Cache.Length];

                Buffer.BlockCopy(Cache, 0, data, 0, data.Length);

                return data;
            }
        }

        /// <summary>
        /// 大小端 ，默认值 是当前机器的,设置这个值 如果大小端和当前机器不一致，那么写入时将会反转
        /// </summary>
        public bool IsLittleEndian { get; set; }

        /// <summary>
        /// 读到的下标位置
        /// </summary>
        public long ReadedPosition { get; set; }

        /// <summary>
        /// 写的下标位置
        /// </summary>
        public long WritedPosition { get; set; }



        /// <summary>
        ///当前可以读取的长度
        /// </summary>
        public long CanReadSize => (WritedPosition - ReadedPosition);

        private BinaryBuffer(int bufferSize, bool isLittleEndian)
        {
            Cache = new byte[bufferSize];
            IsLittleEndian = isLittleEndian;
            ReadedPosition = WritedPosition = 0;
        }

        private BinaryBuffer(byte[] buffer, bool isLittleEndian)
        {
            Cache = buffer;
            IsLittleEndian = isLittleEndian;
            ReadedPosition = 0;
            WritedPosition = Cache.Length;
        }

        /// <summary>
        /// 分配一段内存
        /// </summary>
        /// <param name="bufferSize">内存长度</param>
        /// <param name="isLittleEndian"></param>
        /// <returns>缓存对象</returns>
        public static BinaryBuffer Allocate(int bufferSize, bool isLittleEndian = true) { return new BinaryBuffer(bufferSize, isLittleEndian); }

        /// <summary>
        /// 分配一段内存 注:这里是浅拷贝
        /// </summary>
        /// <param name="buffer">内存</param>
        /// <param name="isLittleEndian"></param>
        /// <returns>缓存对象</returns>
        public static BinaryBuffer Allocate(byte[] buffer, bool isLittleEndian = true)
        {
            return new BinaryBuffer(buffer, isLittleEndian);
        }

        /// <summary>
        /// 析构 当虚拟机自己判断脚本再无引用时  是否所有内存
        /// </summary>
        ~BinaryBuffer()
        {
            Dispose();
        }

        #region [ 读取 ]

        /// <summary>
        /// 读取指定数量字节
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <param name="count">需要读取的数量</param>
        /// 
        /// <returns>读取的字节数组</returns>
        public byte[] Read(int count)
        {
            byte[] data = new byte[count];
            lock (this)
            {

                if (CanReadSize < count)
                    throw new ArgumentOutOfRangeException("可读取长度不足");

                using (BinaryReader reader = new BinaryReader(new MemoryStream(this.Cache) { Position = ReadedPosition }))
                {
                    reader.Read(data, 0, count);
                    ReadedPosition = reader.BaseStream.Position;
                }

            }

            return data;
        }

        /// <summary>
        /// bool值读取
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public bool ReadBoolean()
        {
            byte[] data = Read(sizeof(bool));
            Flip(data);
            return BitConverter.ToBoolean(data, 0);
        }

        /// <summary>
        /// 读取一个字节
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public byte ReadByte()
        {
            return Read(sizeof(byte))[0];
        }

        /// <summary>
        /// 读取单个字符
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public char ReadChar()
        {
            byte[] data = Read(sizeof(char));
            Flip(data);
            return BitConverter.ToChar(data, 0);
        }

        /// <summary>
        /// 读取字符串
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <param name="count">字符串长度</param>
        /// <returns>读取值</returns>
        public string ReadChars(int count, Encoding encoding)
        {
            return encoding.GetString(Read(count));
        }



        /// <summary>
        /// 读取decimal
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public decimal ReadDecimal()
        {
            decimal value;

            Decimal.TryParse(ReadChars(sizeof(decimal), Encoding.ASCII), out value);

            return value;
        }

        /// <summary>
        /// 读取 double
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public double ReadDouble()
        {
            byte[] data = Read(sizeof(double));
            Flip(data);
            return BitConverter.ToDouble(data, 0);
        }

        /// <summary>
        /// 读取 short
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public short ReadInt16()
        {
            byte[] data = Read(sizeof(short));
            Flip(data);
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// 读取 int
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public int ReadInt32()
        {
            byte[] data = Read(sizeof(int));
            Flip(data);
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// 读取 long
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        /// <returns>读取值</returns>
        public long ReadInt64()
        {
            byte[] data = Read(sizeof(long));
            Flip(data);
            return BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        /// 读取 float
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        public float ReadSingle()
        {
            byte[] data = Read(sizeof(float));
            Flip(data);
            return BitConverter.ToSingle(data, 0);
        }

        /// <summary>
        /// 读取 ushort
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        public ushort ReadUInt16()
        {
            byte[] data = Read(sizeof(ushort));
            Flip(data);

            return BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// 读取 uint
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        public uint ReadUInt32()
        {
            byte[] data = Read(sizeof(uint));
            Flip(data);

            return BitConverter.ToUInt32(data, 0);
        }

        /// <summary>
        /// 读取 ulong
        /// <para>
        /// 抛出异常 ： LengthException，ArgumentExcePtion;   
        /// </para>
        /// </summary>
        public ulong ReadUInt64()
        {
            byte[] data = Read(sizeof(ulong));
            Flip(data);
            return BitConverter.ToUInt64(data, 0);
        }

        #endregion

        #region [ 写入 ]

        /// <summary>
        ///  /// <summary>
        /// 往缓存区写入一个数组
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="isFlip">是否需要反转</param>
        public void Write(byte[] data, int index, int count)
        {
            lock (this)
            {
                //如果需要扩充缓存区进行扩充
                TruncateCache(count);

                using (BinaryWriter writer = new BinaryWriter(new MemoryStream(Cache) { Position = WritedPosition }))
                {
                    writer.Write(data, index, count);
                    writer.Flush();

                    WritedPosition = writer.BaseStream.Position;

                }
            }
        }

        /// <summary>
        /// 往缓存区写入字符串
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(string value, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(value);
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// 往缓存区写入一个 float
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(float value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// 往缓存区写入一个 double
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
    
        /// <summary>
        /// 往缓存区写入一个 char
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(char value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// 往缓存区写入一个 byte
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(byte value)
        {
            byte[] data = new byte[] { value };
            Write(data, 0, 1);
        }

        /// <summary>
        /// 往缓存区写入一个 bool
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(bool value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// 往缓存区写入一个 Int16
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(Int16 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }


        /// <summary>
        /// 往缓存区写入一个 UInt16
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(UInt16 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }


        /// <summary>
        /// 往缓存区写入一个 Int32
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(Int32 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }


        /// <summary>
        /// 往缓存区写入一个 UInt32
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(UInt32 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }


        /// <summary>
        /// 往缓存区写入一个 Int64
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(Int64 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }

        /// <summary>
        /// 往缓存区写入一个 UInt64
        /// <para>
        /// 抛出异常 ： ArgumentException，ArgumentNullException，ArgumentOutOfRangeException，IOException，ObjectDisposedException;   
        /// </para>
        /// </summary>
        public void Write(UInt64 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }

        #endregion

        /// <summary>
        /// 释放组件所有资源
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// 扩充缓存区 
        /// </summary>
        /// <param name="count">需要写入缓存的大小</param>
        /// <returns></returns>
        private bool TruncateCache(int count)
        {
            bool needTruncate = false;

            int remainingSize = (int)(Cache.Length - WritedPosition);

            if (remainingSize < count)
            {

                int expansionSize = (Cache.Length + count) << 1; // 扩充大小  size * 2

                byte[] funtrueBuffer = new byte[expansionSize];

                System.Buffer.BlockCopy(Cache, 0, funtrueBuffer, 0, Cache.Length);

                this.Cache = funtrueBuffer;

                needTruncate = true;
            }

            return needTruncate;
        }

        /// <summary>
        /// 如果输入大小端与当前系统大小端不一致 需要反转
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private byte[] Flip(byte[] content)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(content);
            return content;
        }
    }
}
