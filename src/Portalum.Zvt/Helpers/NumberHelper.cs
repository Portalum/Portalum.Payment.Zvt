﻿using System;
using System.Linq;

namespace Portalum.Zvt.Helpers
{
    /// <summary>
    /// NumberHelper
    /// </summary>
    public static class NumberHelper
    {
        /// <summary>
        /// Get number of digits
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int GetNumberOfDigits(decimal value)
        {
            var abs = Math.Abs(value);

            return abs < 1 ? 0 : (int)(Math.Log10(decimal.ToDouble(abs)) + 1);
        }

        /// <summary>
        /// Convert decimal to Binary Coded Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] DecimalToBcd(decimal value, int length = 6)
        {
            var x = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
            var x1 = (long)(x * 100);

            if (GetNumberOfDigits(value) > 10)
            {
                return new byte[0];
            }

            var data = new byte[length];

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(x1 % 10);
                x1 /= 10;

                data[i] |= (byte)((x1 % 10) << 4);
                x1 /= 10;
            }

            return data.Reverse().ToArray();
        }

        /// <summary>
        /// Convert Binary Coded Decimal to decimal
        /// </summary>
        /// <param name="bcdBytes"></param>
        /// <returns></returns>
        public static decimal BcdToDecimal(byte[] bcdBytes)
        {
            long result = 0;

            foreach (var bcdByte in bcdBytes)
            {
                var highNibble = (bcdByte >> 4) & 0xF;
                var lowNibble = bcdByte & 0xF;

                if (highNibble > 9 || lowNibble > 9)
                {
                    throw new ArgumentException("Invalid BCD-Format");
                }

                result = result * 10 + highNibble;
                result = result * 10 + lowNibble;
            }

            return result / 100m;
        }

        /// <summary>
        /// Convert int to Binary Coded Decimal
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] IntToBcd(int value, int length = 3)
        {
            var data = new byte[length];

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(value % 10);
                value /= 10;

                data[i] |= (byte)((value % 10) << 4);
                value /= 10;
            }

            return data.Reverse().ToArray();
        }

        /// <summary>
        /// Convert Binary Coded Decimal to int
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int BcdToInt(Span<byte> data)
        {
            var tempNumber = ByteHelper.ByteArrayToHex(data);
            if (int.TryParse(tempNumber, out var number))
            {
                return number;
            }

            return 0;
        }

        /// <summary>
        /// Get Int16 from bytes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static short ToInt16LittleEndian(Span<byte> data)
        {
            var tempData = data.ToArray();
            Array.Reverse(tempData);
            return BitConverter.ToInt16(tempData, 0);
        }

        /// <summary>
        /// Convert Bit array to Int
        /// </summary>
        /// <param name="boolArray"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int BoolArrayToInt(params bool[] boolArray)
        {
            if (boolArray.Length > 31)
            {
                throw new ArgumentException("Too many elements to be converted to a single int", nameof(boolArray));
            }

            var val = 0;
            for (var i = 0; i < boolArray.Length; ++i)
            {
                if (boolArray[i]) val |= 1 << i;
            }

            return val;
        }
    }
}
