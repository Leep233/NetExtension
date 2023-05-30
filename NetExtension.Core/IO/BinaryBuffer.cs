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
       
       public int ReadableCount =>writedPosition-readedPosition;
       
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
       
        private  void CapacityExpansion(int needCapacity){
            
            if(needCapacity <= WritableCount ) return;  
            
            int futureCapacity = (Capacity + needCapacity)<<1;
            
            byte[]futureBuffer = new futureBuffer;
            
            //将原先buffer内的数据拷贝到新的buffer内
            Buffer.BlockCopy(buffer,this.readedPosition,futureBuffer,0,ReadableCount);
            
            this.buffer = futureBuffer;
            
            Capacity = futureCapacity;
        }
      
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
        
        public byte[] Read(int count){
            
            if(count>ReadableCount) throw new ArgumentOutOfRangeException();
            
            byte[] data = new byte[count];
            
            lock(BUFFER_LOCKER){
                Buffer.BlockCopy(this.buffer,this.readedPosition,data,0,count);
                this.readedPosition+=count;
                Buffer.BlockCopy(this.buffer,this.readedPosition,this.buffer,0,ReadableCount);
                this.writedPosition = ReadableCount;
                this.readedPosition = 0;
            } 
            
            return data;
        }
        
        public byte ReadByte(){
            return Read(1)[0];
        }
        
        public short ReadInt16()
           {
            int count = sizeof(short);
            return BitConverter.ToInt16( Flip(Read(count)),0);
        }
        public ushort ReadUInt16()
           {
            int count = sizeof(ushort);
            return BitConverter.ToUInt16( Flip(Read(count)),0);
        }
        public int ReadInt32()
           {
            int count = sizeof(int);
            return BitConverter.ToInt32( Flip(Read(count)),0);
        }
        public uint ReadUInt32()
          {
            int count = sizeof(uint);
            return BitConverter.ToUInt32( Flip(Read(count)),0);
        }
        public long ReadInt64()
         {
            int count = sizeof(long);
            return BitConverter.ToInt64( Flip(Read(count)),0);
        }
        public ulong ReadUInt64()
        {
            int count = sizeof(ulong);
            return BitConverter.ToUInt64( Flip(Read(count)),0);
        }
        public float ReadSingle()
        {
            int count = sizeof(float);
            return BitConverter.ToSingle( Flip(Read(count)),0);
        }
        
        public double ReadDouble(){
            int count = sizeof(double);
            return BitConverter.ToDouble( Flip(Read(count)),0);
        }
        
        private byte[] Flip(byte[] content)
        {
            if (IsLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(content);
            return content;
        }
    }
}
