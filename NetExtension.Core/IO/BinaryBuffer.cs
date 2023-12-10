using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetExtension.Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryBuffer
    {
       private readonly object BUFFER_LOCKER = new object();
       
       private int writedPosition;
       
       private int readedPosition;
       
       public int ReadableCount => writedPosition - readedPosition;
       
       public int WritableCount => Capacity - writedPosition;
       
       private byte[] buffer;
       
       public byte this[int index]=>buffer[index];

       public bool IsLittleEndian { get; set; }
        
       public int Capacity{get;protected set;}

       private BinaryBuffer(byte[]buffer,bool isLittleEndian = true){
            this.buffer = buffer;
            this.writedPosition = this.buffer.Length;
            this.Capacity =  this.buffer.Length;
           this.IsLittleEndian = isLittleEndian;
            this.readedPosition = 0;
        }
        
        private BinaryBuffer(int capacity,bool isLittleEndian = true):this(new byte[capacity],isLittleEndian){
            this.writedPosition =0;
            this.Capacity = capacity;
        }
 
        public static BinaryBuffer Allocate(int bufferSize, bool isLittleEndian = true) => new BinaryBuffer(bufferSize, isLittleEndian);

        public static BinaryBuffer Allocate(byte[] buffer, bool isLittleEndian = true) => new BinaryBuffer(buffer, isLittleEndian);
       

       public void Write(byte[]data,int offset,int count)
       {
            lock(BUFFER_LOCKER){
                CapacityExpansion(count);
                Buffer.BlockCopy(data,offset,this.buffer,this.writedPosition,count);
                writedPosition += count;
            }
       }
       public void Write(byte[]data)=>Write(data,0,data.Length);
        public void Write(float value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(char value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(byte value)
        {
            byte[] data = new byte[] { value };
            Write(data, 0, 1);
        }
        public void Write(short value) 
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(ushort value) 
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(int value)
         {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(uint value)
         {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(long value)
         {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }
        public void Write(ulong value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Flip(data);
            Write(data, 0, data.Length);
        }

        public byte[] Peek(int count)
        {
            if (count > ReadableCount) throw new ArgumentOutOfRangeException();

            byte[] data = new byte[count];

            lock (BUFFER_LOCKER)
            {
                Buffer.BlockCopy(this.buffer, this.readedPosition, data, 0, count);
            }

            return data;
        }
        public byte PeekByte() => Peek(1)[0];
        public ushort PeekU16() => BitConverter.ToUInt16(Flip(Peek(sizeof(ushort))), 0);
        public short PeekI16() => BitConverter.ToInt16(Flip(Peek(sizeof(short))), 0);
        public uint PeekU32() => BitConverter.ToUInt32(Flip(Peek(sizeof(uint))), 0);
        public int PeekI32() => BitConverter.ToInt32(Flip(Peek(sizeof(int))), 0);
        public long PeekInt64() => BitConverter.ToInt64(Flip(Peek(sizeof(long))), 0);
        public ulong PeekUInt64() => BitConverter.ToUInt64(Flip(Peek(sizeof(ulong))), 0);
        public float PeekSingle() => BitConverter.ToInt32(Flip(Peek(sizeof(float))), 0);
        public double PeekDouble() => BitConverter.ToInt32(Flip(Peek(sizeof(double))), 0);     

        public byte[] Read(int count){
            
            if(count>ReadableCount) throw new ArgumentOutOfRangeException();
            
            byte[] data = new byte[count];
            
            lock(BUFFER_LOCKER)
            {
                Buffer.BlockCopy(this.buffer,this.readedPosition,data,0,count);

                this.readedPosition += count;

                if (readedPosition == writedPosition)
                {
                    //已读的下标 = 已写下标 表示读取完了
                    //重置下标，避免无限扩充缓存
                    this.writedPosition = this.readedPosition = 0;
                }           
                        
            } 
            
            return data;
        }
        public byte ReadByte() => Read(1)[0];
        public short ReadInt16() => BitConverter.ToInt16(Flip(Read(sizeof(short))), 0);      
        public ushort ReadUInt16() => BitConverter.ToUInt16(Flip(Read(sizeof(ushort))), 0);
        public int ReadInt32() => BitConverter.ToInt32(Flip(Read(sizeof(int))), 0);      
        public uint ReadUInt32()=> BitConverter.ToUInt32(Flip(Read(sizeof(uint))), 0);     
        public long ReadInt64()=> BitConverter.ToInt64(Flip(Read(sizeof(long))), 0);
        public ulong ReadUInt64()=> BitConverter.ToUInt64(Flip(Read(sizeof(ulong))), 0);  
        public float ReadSingle() => BitConverter.ToSingle(Flip(Read(sizeof(float))), 0);  
        public double ReadDouble()=> BitConverter.ToDouble(Flip(Read(sizeof(double))), 0);     
        public byte[] ReadToEnd()=> Read(ReadableCount);

        /// <summary>
        /// 字节反转
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private byte[] Flip(byte[] content)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(content);
            return content;
        }
        /// <summary>
        /// 容量扩充
        /// </summary>
        /// <param name="needCapacity"></param>
        private void CapacityExpansion(int needCapacity)
        {

            if (needCapacity <= WritableCount) return;

            int futureCapacity = (Capacity + needCapacity) << 1;

            byte[] futureBuffer = new byte[futureCapacity];

            //将原先buffer内的数据拷贝到新的buffer内
            Buffer.BlockCopy(buffer, this.readedPosition, futureBuffer, 0, ReadableCount);

            this.buffer = futureBuffer;

            Capacity = futureCapacity;
        }


    }
}
