using System;
using System.Linq;
using System.Security.Cryptography;

namespace CTR
{
    public class AesCtr
    {
        private readonly AesManaged Aes = new AesManaged();
        private readonly ICryptoTransform Encryptor;
        private readonly AesCounter Counter;

        public AesCtr(byte[] key, byte[] iv)
        {
            Aes.Key = key;
            Aes.Mode = CipherMode.ECB;
            Aes.Padding = PaddingMode.None;
            Counter = new AesCounter(iv);
            Encryptor = Aes.CreateEncryptor();
        }

        public AesCtr(byte[] key, ulong PartitionID, ulong InitialCount)
        {
            Aes.Key = key;
            Aes.Mode = CipherMode.ECB;
            Aes.Padding = PaddingMode.None;
            Counter = new AesCounter(PartitionID, InitialCount);
            Encryptor = Aes.CreateEncryptor();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            int BlockLength;
            for (int i = 0; i < inputCount; i += BlockLength)
            {
                BlockLength = inputCount - i > AesCounter.BufferSize ? AesCounter.BufferSize : inputCount - i;
                Encryptor.TransformBlock(Counter.ManageBufferCounters(BlockLength), 0, BlockLength, outputBuffer, outputOffset + i);
                for (int BlockWalker = i; BlockWalker < i + BlockLength; BlockWalker += 8)
                {
                    Array.Copy(BitConverter.GetBytes(BitConverter.ToInt64(outputBuffer, outputOffset + BlockWalker) ^ BitConverter.ToInt64(inputBuffer, inputOffset + BlockWalker)), 0, outputBuffer, outputOffset + BlockWalker, 8);
                }
            }
            return inputCount;
        }
    }
    public class AesCounter
    {
        public const int BufferSize = 0x400000; //4 MB Buffer
        private readonly byte[] Counter = new byte[0x10];
        private readonly byte[] Buffer = new byte[BufferSize];

        public AesCounter(ulong high, ulong low)
        {
            Array.Copy(BitConverter.GetBytes(high).Reverse().ToArray(), Counter, 0x8);
            Array.Copy(BitConverter.GetBytes(low).Reverse().ToArray(), 0, Counter, 0x8, 0x8);
        }

        public AesCounter(byte[] iv)
        {
            Array.Copy(BitConverter.GetBytes(BitConverter.ToUInt64(iv, 0)).Reverse().ToArray(), Counter, 0x10);
        }

        public void Increment()
        {
            for (int i = Counter.Length - 1; i >= 0; i--)
            {
                if (++Counter[i] != 0)
                    return;
            }
        }

        public byte[] ManageBufferCounters(int size)
        {
            for (int i = 0; i < size; i += 0x10)
            {
                Array.Copy(Counter, 0, Buffer, i, 0x10);
                Increment();
            }
            return Buffer;
        }

        public ulong SwapBytes(ulong value)
        {
            ulong uvalue = value;
            ulong swapped =
                    0x00000000000000FF & (uvalue >> 56)
                    | 0x000000000000FF00 & (uvalue >> 40)
                    | 0x0000000000FF0000 & (uvalue >> 24)
                    | 0x00000000FF000000 & (uvalue >> 8)
                    | 0x000000FF00000000 & (uvalue << 8)
                    | 0x0000FF0000000000 & (uvalue << 24)
                    | 0x00FF000000000000 & (uvalue << 40)
                    | 0xFF00000000000000 & (uvalue << 56);
            return swapped;
        }
    }
}
