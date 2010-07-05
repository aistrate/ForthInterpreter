using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret
{
    public class Memory
    {
        private List<byte> byteList;

        public Memory(int capacity)
        {
            byteList = new List<byte>(capacity);
            for (int i = 0; i < capacity; i++)
                byteList.Add(0);

            FirstFreeAddress = 0;
        }

        public int FirstFreeAddress { get; private set; }

        public int FreeMemory { get { return byteList.Count - FirstFreeAddress; } }

        public void PrintDump(int fromAddress, int count)
        {
            byte[] bytes = FetchBytes(fromAddress, count);
            int countCeiling = ((count - 1) / 16 + 1) * 16;
            
            Console.WriteLine();
            if (count > 0)
                for (int i = 0; i < countCeiling; i++)
                {
                    if (i % 16 == 0)
                        Console.Write("{0,8:X2}: ", fromAddress + i);

                    if (i % 16 == 4 || i % 16 == 12)
                        Console.Write(" ");
                    if (i % 16 == 8)
                        Console.Write("- ");
                    if (i < count)
                        Console.Write("{0:X2} ", bytes[i]);
                    else
                        Console.Write("   ");

                    if (i % 16 == 15)
                    {
                        Console.Write(" ");
                        for (int j = i - 15; j <= i && j < count; j++)
                            Console.Write(Regex.Replace(((char)bytes[j]).ToString(), @"[^ \w\p{P}]", "."));
                        Console.WriteLine();
                    }
                }
        }


        // Byte ranges
        public int AllocateBytes(int count)
        {
            int allocatedAddress = FirstFreeAddress;
            FirstFreeAddress += count;

            if (FirstFreeAddress <= byteList.Capacity)
                return allocatedAddress;
            else
            {
                FirstFreeAddress = allocatedAddress;
                throw new OutOfMemoryException(
                    string.Format("Could not allocate {0} bytes of memory (interpreter out of memory).", count));
            }
        }

        public void StoreBytes(byte[] bytes, int toAddress)
        {
            try
            {
                foreach (byte b in bytes)
                    byteList[toAddress++] = b;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ApplicationException("Invalid memory address.");
            }
        }

        public byte[] FetchBytes(int fromAddress, int count)
        {
            try
            {
                byte[] bytes = new byte[count];
                byteList.CopyTo(fromAddress, bytes, 0, count);
                return bytes;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ApplicationException("Invalid memory address.");
            }
        }

        public void FillBytes(int toAddress, int count, byte value)
        {
            try
            {
                if (count > 0)
                {
                    int endAddress = toAddress + count;
                    while (toAddress < endAddress)
                    {
                        byteList[toAddress++] = value;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ApplicationException("Invalid memory address.");
            }
        }

        
        // Bytes
        public void StoreByte(byte value, int toAddress)
        {
            byte[] bytes = new byte[] { value };
            StoreBytes(bytes, toAddress);
        }

        public byte FetchByte(int fromAddress)
        {
            byte[] bytes = FetchBytes(fromAddress, 1);
            return bytes[0];
        }
        
        
        // Cells (integers)
        public int AllocateCell()
        {
            return AllocateBytes(sizeof(int));
        }

        public void StoreCell(int value, int toAddress)
        {
            StoreBytes(BitConverter.GetBytes(value), toAddress);
        }

        public int AllocateAndStoreCell(int value)
        {
            int toAddress = AllocateCell();
            StoreCell(value, toAddress);
            return toAddress;
        }

        public int FetchCell(int fromAddress)
        {
            return BitConverter.ToInt32(FetchBytes(fromAddress, sizeof(int)), 0);
        }


        // Chars
        public void StoreChar(char value, int toAddress)
        {
            StoreByte(CharType.ToByte(value), toAddress);
        }

        public char FetchChar(int fromAddress)
        {
            return CharType.ToChar(FetchByte(fromAddress));
        }


        // Strings
        public void StoreCharString(string text, int toAddress)
        {
            byte[] bytes = (from ch in text.ToCharArray() select (byte)ch).ToArray();
            StoreBytes(bytes, toAddress);
        }

        public void StoreCountedString(string text, int toAddress)
        {
            byte length = (byte)text.Length;
            if (length < text.Length)
                text = text.Substring(0, length);
            
            StoreByte(length, toAddress);
            StoreCharString(text, 1 + toAddress);
        }

        public CharString AllocateAndStoreCharString(string text)
        {
            int toAddress = AllocateBytes(text.Length);
            StoreCharString(text, toAddress);
            return new CharString(toAddress, text.Length);
        }

        public CountedString AllocateAndStoreCountedString(string text)
        {
            int toAddress = AllocateBytes(1 + (byte)text.Length);
            StoreCountedString(text, toAddress);
            return new CountedString(toAddress);
        }

        public string FetchCharString(CharString charString)
        {
            char[] chars = (from b in FetchBytes(charString.CharAddress, charString.Length) select (char)b).ToArray();
            return new string(chars);
        }

        public string FetchCountedString(CountedString countedString)
        {
            return FetchCharString(ToCharString(countedString));
        }

        public CharString ToCharString(CountedString countedString)
        {
            return new CharString(1 + countedString.CounterAddress, FetchByte(countedString.CounterAddress));
        }
    }
}
