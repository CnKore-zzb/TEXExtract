using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

namespace TEXView
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DatFileInfo
    {
        public string Filename;
        public Int32 PackID;
        public Int32 Pos;
        public Int32 UnCompressSize;
        public Int32 CompressSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DTXHeader
    {
        public Byte Version;
        public Byte ImgType;
        public Int32 ImgCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ChunkInfo
    {
        public Int32 Pos;
        public Int32 Row;
        public Int32 ChunkSize;
    }

    //For Type 3
    public struct ChunkInfoT3
    {
        public Int32 Pos;
        public Int32 Row;
        public Int32 Pixel;
        public ArraySegment<Byte> Raw;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImgHeader
    {
        //--Format
        public Int16 _Short1;
        public Int16 _Short2; //0
        public Int16 _Short3; //0
        public Int16 _Short4; //0
        public Int16 _Short5; //0
        public Int16 _Short6; //0
        public Int16 _Short7; //0

        public Int16 _Short8; //0
        public Int16 _Short9; //0
        public Int16 _Short10; //0

        public Int16 _Short11; //0
        public Int16 _Short12; //0

        public Int32 _SubStrSize;
        public Int16 _Short13; //0
        public Int16 _Short14; //0
        public Int16 _Short15; //0
        public Int16 _Short16; //0
        public Int16 _Short17; //0
        public Int16 _Short18; //0
        public Int16 _Short19; //0

        public float Float1;
        public float Float2;
        public float Float3;

        public Int32 _2D; //0
        public Int16 _After2D; //0

        public Int32 Width;
        public Int32 Height;

        public Int32 ChunkCount;
        public Int32 X2Size;

        //Borrow
        public Int32 GlobalX;
        public Int32 GlobalY;
    }

    public struct ImgInfo
    {
        public ImgHeader Info;
        public List<Object> ChunkList;
        public Bitmap Img;
        public string Name;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageDefineT50
    {
        public Int32 Idx;
        public Int32 GlobalX;
        public Int32 GlobalY;
        public Int32 Width;
        public Int32 Height;
        public float Float1;
        public float Float2;
        public Int16 RetX;
        public Int16 RetY;
        public Int16 X2;
        public Int16 Y2;
        public Int32 Int2D;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageDefineT70
    {
        public Int32 Idx;
        public Int32 GlobalX;
        public Int32 GlobalY;
        public Int32 Width;
        public Int32 Height;
        public float Float1;
        public float Float2;
        public string Name;
        public Int16 RetX;
        public Int16 RetY;
        public Int16 X2;
        public Int16 Y2;
        public Int32 UnknowInt1;
        public Int16 X3;
        public Int16 Y4;
        public Int32 UnknowInt2;
        public Int32 Int2D;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageDefineT73
    {
        public Int32 Idx;
        public Int32 GlobalX;
        public Int32 GlobalY;
        public Int32 Width;
        public Int32 Height;
        public float Float1;
        public float Float2;
        public string Name;
        public Int32 RetX;
        public Int32 RetY;
        public Int32 UnknowInt1;
        public Int16 X2;
        public Int16 Y2;
        public Int32 UnknowInt2;
        public Int32 Int2D;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImageDefineT83
    {
        public Int32 Idx;
        public Int32 GlobalX;
        public Int32 GlobalY;
        public Int32 Width;
        public Int32 Height;
        public Int32 UnknowInt1;
        public float Float0;
        public string Name;
        public Int32 UnknowInt2;
        public Int32 UnknowInt3;
        public Int32 UnknowInt4;
        public float Float1;
        public float Float2;
        public Int32 UnknowInt5;
        public Int16 RetX;
        public Int16 RetY;
        public Int32 UnknowInt6;
        public Int32 Int2D;
    }
}
