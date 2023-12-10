using System;
using System.Collections.Generic;
using System.Text;

namespace NetExtension.Core.Text
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringConverter
    {

        private const string HEX_PRIFEX = "0x";   

        public static byte[] ToBytes(string hexString)
        {
            string content = hexString.Replace("\n", "").
               Replace(" ", "").
               Replace(",", "").ToLower().
               Replace(HEX_PRIFEX, "");
  
            if ((content.Length % 2) != 0)
                content = content.Insert(0,"0");

            int length = content.Length/2;

            Console.WriteLine(content);

            byte[] result = new byte[length];

            for (int i = 0; i < result.Length; i++)
            {
                string str = content.Substring(i * 2, 2);
              
                result[i] = Convert.ToByte(str, 16);
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tobase">2,8,16</param>
        /// <returns></returns>
        public static string ToString(byte[] content, int tobase = 16)
        {
            if(content is null || content.Length<=0) return string.Empty;

            string [] strs = new string [content.Length];

            for (int i = 0; i < content.Length; i++)
            {
                strs[i] = Convert.ToString(content[i], tobase);
            }

            return string.Join(" ", strs);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Int转成BCD
        /// </summary>
        /// <param name="value">int 值</param>
        /// <returns></returns>
        public static byte IntToBCD(byte value)
        {
            byte b1 = (byte)(value / 10);
            byte b2 = (byte)(value % 10);
            return (byte)((b1 << 4) | b2);
        }

        /// <summary>
        /// BCD转成int
        /// </summary>
        /// <param name="value">返回值</param>
        /// <returns></returns>
        public static byte BCDToInt(byte value)
        {
            byte b1 = (byte)((value >> 4) & 0xF);
            byte b2 = (byte)(value & 0xF);
            return (byte)(b1 * 10 + b2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        public static int GetBitValue(byte number, int bitIndex)
        {
            if (bitIndex >= 8 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            return ((number & (1 << bitIndex)) != 0) ? 1 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="bitIndex"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static byte SetBitValue(byte number, int bitIndex, bool flag)
        {


            if (bitIndex >= 8 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            bitIndex++;

            int v = bitIndex < 2 ? bitIndex : (2 << (bitIndex - 2));
            return (byte)(flag ? number | v : number & ~v);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        public static int GetBitValue(ushort number, int bitIndex)
        {
            if (bitIndex >= 16 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            return ((number & (1 << bitIndex)) != 0) ? 1 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="bitIndex"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static ushort SetBitValue(ushort number, int bitIndex, bool flag)
        {

            if (bitIndex >= 16 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            bitIndex++;

            int v = bitIndex < 2 ? bitIndex : (2 << (bitIndex - 2));
            return (ushort)(flag ? number | v : number & ~v);
        }

        /// <summary>
        /// 获取某一位的值 0 或 1
        /// </summary>
        /// <param name="number">输出目标值</param>
        /// <param name="bitIndex">需要获取第几位</param>
        /// <returns></returns>
        public static int GetBitValue(uint number, int bitIndex)
        {
            if (bitIndex >= 32 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            return ((number & (1 << bitIndex)) != 0) ? 1 : 0;
        }
        /// <summary>
        /// 设置某一位 
        /// </summary>
        /// <param name="number">目标数</param>
        /// <param name="bitIndex">第几位</param>
        /// <param name="flag">true = 1 flase = 0</param>
        /// <returns></returns>
        public static uint SetBitValue(uint number, int bitIndex, bool flag)
        {


            if (bitIndex >= 32 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            bitIndex++;

            uint v = (uint)(bitIndex < 2 ? bitIndex : (2 << (bitIndex - 2)));
            return (uint)(flag ? (number | v) : (number & ~v));
        }

        /// <summary>
        /// 获取某一位的值 0 或 1
        /// </summary>
        /// <param name="number">输出目标值</param>
        /// <param name="bitIndex">需要获取第几位</param>
        /// <returns></returns>

        public static int GetBitValue(ulong number, int bitIndex)
        {
            if (bitIndex >= 64 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            return ((number & (ulong)(1 << bitIndex)) != 0) ? 1 : 0;
        }
        /// <summary>
        /// 设置某一位 
        /// </summary>
        /// <param name="number">目标数</param>
        /// <param name="bitIndex">第几位</param>
        /// <param name="flag">true = 1 flase = 0</param>
        /// <returns></returns>
        public static ulong SetBitValue(ulong number, int bitIndex, bool flag)
        {


            if (bitIndex >= 64 || bitIndex < 0)
                throw new ArgumentOutOfRangeException();

            bitIndex++;

            ulong v = (ulong)(bitIndex < 2 ? bitIndex : (2 << (bitIndex - 2)));

            return (ulong)(flag ? (number | v) : (number & ~v));
        }
    }
}
