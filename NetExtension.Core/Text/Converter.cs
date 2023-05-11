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
        /// <summary>
        /// Hex 转 byte数组
        /// </summary>
        /// <param name="hexString">需要转换的字符串</param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace(",", "").Replace("0x", "").Replace("0X", "").Replace("\n", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// byte数组转Hex
        /// </summary>
        /// <param name="bytes">需要转成的二进制数组</param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += string.Format(" {0}", bytes[i].ToString("X2"));
                }
            }
            return returnStr;
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
