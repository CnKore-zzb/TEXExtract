using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace TEXView
{
    public class ParserBase
    {
        public bool Read<T>(byte[] buffer, int index, ref T retval)
        {
            if (index == buffer.Length) return false;
            int size = Marshal.SizeOf(typeof(T));
            if (index + size > buffer.Length) throw new IndexOutOfRangeException();
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                IntPtr addr = (IntPtr)((long)handle.AddrOfPinnedObject() + index);
                retval = (T)Marshal.PtrToStructure(addr, typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return true;
        }

        public bool Read<T>(Stream stream, ref T retval)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = null;
            if (buffer == null || size > buffer.Length) buffer = new byte[size];
            int len = stream.Read(buffer, 0, size);
            if (len == 0) return false;
            if (len != size) throw new EndOfStreamException();
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                retval = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return true;
        }

        public bool ReadBytes(Stream stream, UInt32 readSize, ref Byte[] bArray)
        {
            int size = (int)readSize;
            int len = stream.Read(bArray, 0, size);
            if (len == 0) return false;
            if (len != size) throw new EndOfStreamException();
            return true;
        }

        public byte ReadByte(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            return br.ReadByte();
        }

        public Int16 ReadInt16(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            return br.ReadInt16();
        }

        public Int32 ReadInt32(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            return br.ReadInt32();
        }

        public float ReadFloat(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            return br.ReadSingle();
        }

        public bool ReadUInt32s(Stream stream, UInt32 readSize, ref UInt32[] bArray)
        {
            int size = (int)readSize * sizeof(UInt32);
            byte[] buffer = null;
            if (buffer == null || size > buffer.Length) buffer = new byte[size];
            int len = stream.Read(buffer, 0, size);
            if (len == 0) return false;
            if (len != size) throw new EndOfStreamException();
            Buffer.BlockCopy(buffer, 0, bArray, 0, size);
            return true;
        }

        public void ReadAndSetByType(FieldInfo field, TypedReference tr, BinaryReader br, Stream _stream)
        {
            if (!field.FieldType.IsArray)
            {
                if (field.FieldType == typeof(Byte))
                {
                    field.SetValueDirect(tr, br.ReadByte());
                }
                else if (field.FieldType == typeof(Int16))
                {
                    field.SetValueDirect(tr, br.ReadInt16());
                }
                else if (field.FieldType == typeof(Int32))
                {
                    field.SetValueDirect(tr, br.ReadInt32());
                }
                else if (field.FieldType == typeof(Int64))
                {
                    field.SetValueDirect(tr, br.ReadInt64());
                }
                else if (field.FieldType == typeof(float))
                {
                    field.SetValueDirect(tr, br.ReadSingle());
                }
                else if (field.FieldType == typeof(Guid))
                {
                    Guid gs = new Guid();
                    Read<Guid>(_stream, ref gs);
                    field.SetValueDirect(tr, gs);
                }
                else if (field.FieldType == typeof(string))
                {
                    Int32 _count = br.ReadInt32();
                    Byte[] _data = br.ReadBytes(_count);
                    string _asciistring = UnicodeEncoding.ASCII.GetString(_data);
                    field.SetValueDirect(tr, _asciistring);
                }
                else if (field.FieldType == typeof(ArraySegment<Byte>))
                {
                    Int32 _count = br.ReadInt32();
                    Byte[] _data = br.ReadBytes(_count);
                    ArraySegment<Byte> _bytes = new ArraySegment<byte>(_data);
                    field.SetValueDirect(tr, _bytes);
                }
                else if (field.FieldType == typeof(List<Int32>))
                {
                    Int32 _count = br.ReadInt32();
                    List<Int32> _l = new List<int>();
                    if (_count > 0)
                    {
                        for (int k = 0; k < _count; ++k)
                        {
                            _l.Add(br.ReadInt32());
                        }
                    }
                    field.SetValueDirect(tr, _l);
                }
                else if (field.FieldType == typeof(List<string>))
                {
                    Int32 _count = br.ReadInt32();
                    List<string> _l = new List<string>();
                    if (_count > 0)
                    {
                        for (int k = 0; k < _count; ++k)
                        {
                            Int32 _count2 = br.ReadInt32();
                            Byte[] _data = br.ReadBytes(_count2);
                            string _asciistring = UnicodeEncoding.ASCII.GetString(_data);
                            _l.Add(_asciistring);
                        }
                    }
                    field.SetValueDirect(tr, _l);
                }
            }
            else
            {
                object[] attributes = field.GetCustomAttributes(typeof(MarshalAsAttribute), false);
                MarshalAsAttribute marshal = (MarshalAsAttribute)attributes[0];
                int sizeConst = marshal.SizeConst;
                if (field.FieldType == typeof(Int32[]))
                {
                    Int32[] _Int = new Int32[sizeConst];
                    for (int j = 0; j < sizeConst; ++j)
                    {
                        _Int[j] = br.ReadInt32();
                    }
                    field.SetValueDirect(tr, _Int);
                }
                else if (field.FieldType == typeof(Int16[]))
                {
                    Int16[] _Int = new Int16[sizeConst];
                    for (int j = 0; j < sizeConst; ++j)
                    {
                        _Int[j] = br.ReadInt16();
                    }
                    field.SetValueDirect(tr, _Int);
                }
                else if (field.FieldType == typeof(Int64[]))
                {
                    Int64[] _Int = new Int64[sizeConst];
                    for (int j = 0; j < sizeConst; ++j)
                    {
                        _Int[j] = br.ReadInt64();
                    }
                    field.SetValueDirect(tr, _Int);
                }
            }
        }

        public T ReadStreamStruct<T>(Stream _stream)
        {
            BinaryReader br = new BinaryReader(_stream);
            T _ObjStruct = (T)Activator.CreateInstance(typeof(T));
            TypedReference tr = __makeref(_ObjStruct);
            foreach (var field in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                ReadAndSetByType(field, tr, br, _stream);
            }
            return _ObjStruct;
        }

        public void DecompressData(Stream pStream, out MemoryStream Outz)
        {
            MemoryStream msInner = new MemoryStream();
            using (System.IO.Compression.DeflateStream z = new System.IO.Compression.DeflateStream(pStream, System.IO.Compression.CompressionMode.Decompress))
            {
                MemoryStream _o = new MemoryStream();
                z.CopyTo(_o);
                _o.Seek(0, SeekOrigin.Begin);
                Outz = _o;
            }
        }

        public static bool Write<T>(ref byte[] buffer, T data, ref int writesize)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr marshal_buff = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(data, marshal_buff, true);
                Marshal.Copy(marshal_buff, buffer, 0, size);
                writesize = size;
            }
            finally
            {
                Marshal.FreeHGlobal(marshal_buff);
            }
            return true;
        }
    }
}
