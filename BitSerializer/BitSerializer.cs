using System;
using System.Text;
#if UNITY
using UnityEngine;
using Unity.Collections;
#endif

namespace NVentimiglia
{
    /// <summary>
    ///     Datamodel serialization helper
    /// </summary>
    public class BitSerializer : IDisposable
    {
        #region  CONSTANTS

        /// <summary>
        /// size of a buffer array
        /// </summary>
        public static int BUFFER_SIZE = 512;

        /// <summary>
        /// for null objects
        /// </summary>
        public const byte NULL = 0;

        /// <summary>
        /// for null objects
        /// </summary>
        public const byte NOTNULL = 1;

        #endregion

        #region HEAD

        /// <summary>
        /// IS WRITING TO THE STREAM (SERIALIZING, false for DESERIALIZING)
        /// </summary>
        public bool IsWriting;

        /// <summary>
        /// Read Data index.
        /// </summary>
        public int ReadIndex;

        /// <summary>
        /// Write Data index, size
        /// </summary>
        public int WriteIndex;

        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// ctor
        /// </summary>
        public BitSerializer() : this(BUFFER_SIZE)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BitSerializer(int size)
        {
            Data = new byte[size];
        }

        /// <summary>
        /// Reset Indicies
        /// </summary>
        public virtual void Reset()
        {
            ReadIndex = WriteIndex = 0;
        }

        /// <summary>
        /// Reads to Data, includes header
        /// </summary>
        /// <param name="source"></param>
        public void CopyFrom(byte[] source)
        {
            var length = source.Length > Data.Length ? Data.Length : source.Length;
            Buffer.BlockCopy(source, 0, Data, 0, length);
        }

        /// <summary>
        /// Reads to Data, includes header
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(byte[] target)
        {
            var length = target.Length > Data.Length ? Data.Length : target.Length;
            Buffer.BlockCopy(Data, 0, target, 0, length);
        }

        /// <summary>
        /// Reads to Data, includes header
        /// </summary>
        public void CopyTo(byte[] target, int length)
        {
            Buffer.BlockCopy(Data, 0, target, 0, length);
        }

        /// <summary>
        /// Writes copy of paylod
        /// </summary>
        public byte[] Copy()
        {
            var b = new byte[WriteIndex];
            Buffer.BlockCopy(Data, 0, b, 0, WriteIndex);
            return b;
        }

        public void Dispose()
        {
            Data = null;
        }

        void Ensures(bool assertion)
        {
            if (!assertion)
                throw new ArgumentException("BufferSerializer : Method failed sanity.");
        }

        #endregion

        #region Peek              

        public unsafe byte PeekByte()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(byte));

            return Data[ReadIndex];
        }

        public unsafe sbyte PeekSByte()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(sbyte));
            return (sbyte)Data[ReadIndex];
        }

        public unsafe bool PeekBool()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(sbyte));

            bool value;

            fixed (byte* ptr = Data)
            {
                value = *(bool*)(ptr + ReadIndex);
            }
            return value;
        }

        public unsafe short PeekShort()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(sbyte));

            short value;

            fixed (byte* ptr = Data)
            {
                value = *(short*)(ptr + ReadIndex);
            }
            return value;
        }

        public unsafe ushort PeekUShort()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(sbyte));

            ushort value;

            fixed (byte* ptr = Data)
            {
                value = *(ushort*)(ptr + ReadIndex);
            }
            return value;
        }

        public unsafe int PeekInt()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(sbyte));

            int value;

            fixed (byte* ptr = Data)
            {
                value = *(int*)(ptr + ReadIndex);
            }
            return value;
        }

        public unsafe uint PeekUInt()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(sbyte));

            uint value;

            fixed (byte* ptr = Data)
            {
                value = *(uint*)(ptr + ReadIndex);
            }
            return value;
        }

        public unsafe Guid PeekGuid()
        {
            Ensures(Data.Length >= ReadIndex + sizeof(Guid));
            Guid value;

            fixed (byte* ptr = Data)
            {
                value = *(Guid*)(ptr + ReadIndex);
            }
            return value;
        }
        #endregion

        #region PRIMITIVES

        public unsafe void Parse(ref byte value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(byte));

                Data[WriteIndex] = value;
                WriteIndex += sizeof(byte);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(byte));

                value = Data[ReadIndex];
                ReadIndex += sizeof(byte);
            }
        }
        public byte Parse(byte value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref sbyte value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(sbyte));

                Data[WriteIndex] = (byte)value;
                WriteIndex += sizeof(sbyte);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(sbyte));

                value = (sbyte)Data[ReadIndex];
                ReadIndex += sizeof(sbyte);
            }
        }
        public sbyte Parse(sbyte value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref bool value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(bool));

                fixed (byte* ptr = Data)
                {
                    *(bool*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(bool);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(bool));

                fixed (byte* ptr = Data)
                {
                    value = *(bool*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(bool);
            }
        }

        public bool Parse(bool value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref short value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(short));

                fixed (byte* ptr = Data)
                {
                    *(short*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(short);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(short));

                fixed (byte* ptr = Data)
                {
                    value = *(short*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(short);
            }
        }

        public unsafe void Parse(ref ushort value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(ushort));

                fixed (byte* ptr = Data)
                {
                    *(ushort*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(ushort);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(ushort));

                fixed (byte* ptr = Data)
                {
                    value = *(ushort*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(ushort);
            }
        }

        public ushort Parse(ushort value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref int value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(int));

                fixed (byte* ptr = Data)
                {
                    *(int*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(int);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(int));

                fixed (byte* ptr = Data)
                {
                    value = *(int*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(int);
            }
        }

        public int Parse(int value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref uint value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(uint));

                fixed (byte* ptr = Data)
                {
                    *(uint*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(uint);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(uint));

                fixed (byte* ptr = Data)
                {
                    value = *(uint*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(uint);
            }
        }

        public uint Parse(uint value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref long value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(long));

                fixed (byte* ptr = Data)
                {
                    *(long*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(long);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(long));

                fixed (byte* ptr = Data)
                {
                    value = *(long*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(long);
            }
        }
        public long Parse(long value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref ulong value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(ulong));

                fixed (byte* ptr = Data)
                {
                    *(ulong*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(ulong);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(ulong));

                fixed (byte* ptr = Data)
                {
                    value = *(ulong*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(ulong);
            }
        }
        public ulong Parse(ulong value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref double value)
        {

            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(double));

                fixed (byte* ptr = Data)
                {
                    *(double*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(double);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(double));

                fixed (byte* ptr = Data)
                {
                    value = *(double*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(double);
            }
        }

        public double Parse(double value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref float value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(float));

                fixed (byte* ptr = Data)
                {
                    *(float*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(float);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(float));

                fixed (byte* ptr = Data)
                {
                    value = *(float*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(float);
            }
        }

        public float Parse(float value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref Guid value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(Guid));

                fixed (byte* ptr = Data)
                {
                    *(Guid*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(Guid);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(Guid));

                fixed (byte* ptr = Data)
                {
                    value = *(Guid*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(Guid);
            }
        }

        public Guid Parse(Guid value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref DateTime value)
        {
            long ticks = value.ToBinary();
            Parse(ref ticks);
            value = DateTime.FromBinary(ticks);
        }

        public DateTime Parse(DateTime value)
        {
            Parse(ref value);
            return value;
        }

        #endregion

        #region ARRAYS

        public unsafe void Parse(ref byte[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                Buffer.BlockCopy(value, 0, Data, WriteIndex, value.Length);

                WriteIndex += length;
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new byte[length];
                if (length > 0)
                {
                    Buffer.BlockCopy(Data, ReadIndex, value, 0, length);
                }
                ReadIndex += length;
            }
        }

        public unsafe byte[] Parse(byte[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref bool[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new bool[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe bool[] Parse(bool[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref ushort[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new ushort[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe ushort[] Parse(ushort[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref short[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new short[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe short[] Parse(short[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref int[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new int[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe int[] Parse(int[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref uint[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new uint[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe uint[] Parse(uint[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref long[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new long[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe long[] Parse(long[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref ulong[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new ulong[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe ulong[] Parse(ulong[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref double[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new double[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe double[] Parse(double[] value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref float[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new float[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }
        public unsafe float[] Parse(float[] value)
        {
            Parse(ref value);
            return value;
        }

        #endregion

        #region STRINGS

        public unsafe void Parse(ref char value)
        {
            if (IsWriting)
            {
                Ensures(Data.Length >= WriteIndex + sizeof(char));

                fixed (byte* ptr = Data)
                {
                    *(char*)(ptr + WriteIndex) = value;
                }
                WriteIndex += sizeof(char);
            }
            else
            {
                Ensures(Data.Length >= ReadIndex + sizeof(char));

                fixed (byte* ptr = Data)
                {
                    value = *(char*)(ptr + ReadIndex);
                }
                ReadIndex += sizeof(char);
            }
        }

        public unsafe char Parse(char value)
        {
            Parse(ref value);
            return value;
        }

        public unsafe void Parse(ref string value)
        {
            if (IsWriting)
            {
                if (value == null)
                {
                    int empty = 0;
                    Parse(ref empty);
                }
                else
                {
                    byte[] data;
                    data = Encoding.UTF8.GetBytes(value);
                    Parse(ref data);
                }
            }
            else
            {
                byte[] buffer = null;
                Parse(ref buffer);

                value = Encoding.UTF8.GetString(buffer);
            }
        }

        public unsafe string Parse(string value)
        {
            Parse(ref value);
            return value;
        }


        public unsafe void Parse(ref string[] value)
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                value = new string[length];
                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }

        public unsafe string[] Parse(string[] value)
        {
            Parse(ref value);
            return value;
        }

        #endregion

        #region Native
#if UNITY        
        public unsafe void Parse<T>(ref NativeArray<T> value) where T : struct, IBufferModel
        {
            if (IsWriting)
            {
                var length = value.Length;
                Parse(ref length);

                for (int i = 0; i < value.Length; i++)
                {
                    var v = value[i];
                    Parse(ref v);
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                for (int i = 0; i < length; i++)
                {
                    T v = default(T);
                    Parse(ref v);
                    value[i] = v;
                }
            }
        }

        public unsafe void Parse<T>(ref NativeSlice<T> value) where T : struct, IBufferModel
        {
            if (IsWriting)
            {
                var length = value.Length;
                Parse(ref length);

                for (int i = 0; i < value.Length; i++)
                {
                    var v = value[i];
                    Parse(ref v);
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                for (int i = 0; i < length; i++)
                {
                    T v = default(T);
                    Parse(ref v);
                    value[i] = v;
                }
            }
        }
#endif
        #endregion

        #region OBJECTS

        /// <summary>
        /// READ / WRITE BASED ON this.IsWriting FLAG
        /// </summary>
        public unsafe void Parse<T>(ref T value) where T : struct, IBitModel
        {
            value.Parse(this);
        }
        public unsafe T Parse<T>(T value) where T : struct, IBitModel
        {
            Parse(ref value);
            return value;
        }

        /// <summary>
        /// READ / WRITE BASED ON this.IsWriting FLAG
        /// </summary>
        public unsafe void Parse<T>(ref T[] value) where T : struct, IBitModel
        {
            if (IsWriting)
            {
                var length = value == null ? 0 : value.Length;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        Parse(ref value[i]);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                if (value == null)
                    value = new T[length];
                else if (value.Length != length)
                    Array.Resize(ref value, length);

                for (int i = 0; i < length; i++)
                {
                    Parse(ref value[i]);
                }
            }
        }

        public unsafe T[] Parse<T>(T[] value) where T : struct, IBitModel
        {
            Parse(ref value);
            return value;
        }

        /// <summary>
        /// READ / WRITE BASED ON this.IsWriting FLAG
        /// </summary>
        public unsafe void Parse<T>(ref System.Collections.Generic.List<T> value) where T : struct, IBitModel
        {
            if (value == null)
                value = new System.Collections.Generic.List<T>();

            if (IsWriting)
            {
                var length = value.Count;
                Parse(ref length);

                if (value != null)
                {
                    for (int i = 0; i < value.Count; i++)
                    {
                        var val = value[i];
                        Parse(ref val);
                    }
                }
            }
            else
            {
                int length = 0;
                Parse(ref length);

                //ordered
                for (int i = 0; i < length; i++)
                {
                    T _item = default(T);
                    Parse(ref _item);
                    if (i >= value.Count)
                        value.Add(_item);
                    else
                        value[i] = _item;
                }

                //prune end
                if (value.Count > length)
                {
                    value.RemoveRange(length, value.Count - length);
                }
            }
        }

        public unsafe System.Collections.Generic.List<T> Parse<T>(System.Collections.Generic.List<T> value) where T : struct, IBitModel
        {
            Parse(ref value);
            return value;
        }

        #endregion
    }
}
