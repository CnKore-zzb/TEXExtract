using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections;

namespace TEXView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Hashtable _hpacks = new Hashtable();
            OpenFileDialog _openDlg = new OpenFileDialog();
            _openDlg.Filter = "TEX.DAT|TEX.DAT|All Files (*.*)|*.*";
            _openDlg.FilterIndex = 1;
            DialogResult result = _openDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                string _directory = Path.GetDirectoryName(_openDlg.FileName);
                Stream _datsource = new FileStream(_openDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(_datsource);
                ParserBase pb = new ParserBase();
                Byte[] _header = br.ReadBytes(24); //!--don't care
                Int32 _filenum = 0;
                do
                {
                    DatFileInfo d = pb.ReadStreamStruct<DatFileInfo>(_datsource);
                    if( !_hpacks.ContainsKey( d.PackID ) )
                    {
                        Stream _sdatsource = new FileStream(_directory  + string.Format("\\TEX{0}.DAT", d.PackID), FileMode.Open, FileAccess.Read, FileShare.Read);
                        _hpacks[d.PackID] = _sdatsource;
                    }

                    Stream _sf = (Stream)_hpacks[d.PackID];
                    _sf.Seek(d.Pos, SeekOrigin.Begin);
                    Byte[] _filedata = new Byte[d.CompressSize];
                    _sf.Read(_filedata, 0, d.CompressSize);
                    DTXFile _f = new DTXFile();

                    string _dirname = string.Format("\\DTX_{0}", _filenum++);
                    string ImgDir = _directory  + _dirname;
                    MemoryStream _mf = new MemoryStream(_filedata);
                    if (_f.ReadFromStream(_mf))
                    {
                        if (!Directory.Exists(ImgDir))
                        {
                            Directory.CreateDirectory(ImgDir);
                        }
                        for (int imgidx = 0; imgidx < _f.ImgLists.Count; ++imgidx)
                        {
                            ImgInfo _i = _f.ImgLists[imgidx];
                            _i.Img.Save(ImgDir + string.Format("\\{0}.bmp", imgidx), ImageFormat.Bmp);
                        }
                        Console.WriteLine(string.Format("Filenum {0} Ver:{1} Type:{2}", _filenum,_f.Header.Version, _f.Header.ImgType));
                    }
                }
                while (_datsource.Position < _datsource.Length);

                br.Close();
                _datsource.Close();
                foreach ( Stream spack in _hpacks.Values)
                {
                    spack.Close();
                }
            }
            _hpacks.Clear();
            _openDlg.Dispose();

            //!--Test
            //var files = Directory.GetFiles("C:\\TEX", "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".zip"));
            //foreach (string f in files)
            //{
            //    string filename = Path.GetFileNameWithoutExtension(f);
            //    string directory = Path.GetDirectoryName(f);
            //    string ImgDir = "C:\\TEX3\\" + filename;
            //    DTXFile _f = new DTXFile();
            //    if (_f.ReadFromFile(f))
            //    {
            //        if (!Directory.Exists(ImgDir))
            //        {
            //            Directory.CreateDirectory(ImgDir);
            //        }
            //        for (int imgidx = 0; imgidx < _f.ImgLists.Count; ++imgidx)
            //        {
            //            ImgInfo _i = _f.ImgLists[imgidx];
            //            _i.Img.Save(ImgDir + string.Format("\\{0}.bmp", imgidx), ImageFormat.Bmp);
            //        }
            //    }
            //}
        }
    }
}
