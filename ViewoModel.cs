using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.Collections.ObjectModel;
using System.Xaml;
using System.Xml.Serialization;

namespace ReadTextByVoice
{
   public  class ViewoModel:BindableBase
    {
        public ViewoModel()
        {
            reader = new SpeechSynthesizer();
            reader.SpeakCompleted += Reader_SpeakCompleted;

            LoadLocalBookinfos();
        }

        private void Reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        SpeechSynthesizer reader;

        string targetText;
        string filePath;
        int bookmarker;
        ObservableCollection<book> allBooks = new ObservableCollection<book>();
        public ICommand ReadTextCmd
        {
            get
            {
                return new DelegateCommand(
                    ()=> 
                    {
                        reader.SpeakAsync(TargetText);
                    }
                    );
            }
        }

        /// <summary>
        /// 选择文本
        /// </summary>
        public ICommand SelectFileCmd
        {
            get
            {
                return new DelegateCommand(()=> 
                {
                    using (OpenFileDialog openFileDialog = new OpenFileDialog())
                    {
                        openFileDialog.InitialDirectory = @"C:\Users\anghua\Documents\Tencent Files\469477947\FileRecv";
                        openFileDialog.Filter = "text files (*.txt)|*.txt";
                        openFileDialog.RestoreDirectory = true;

                        DialogResult result = openFileDialog.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            FilePath = openFileDialog.FileName;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 将新的图书信息添加到图书列表中
        /// </summary>
        public ICommand LoadBookCmd
        {
            get => new DelegateCommand(()=> 
            {
                if (File.Exists(FilePath))
                {
                    FileStream fs = new FileStream(FilePath, FileMode.Open);
                    book ebook = new book();
                    ebook.BookName = GetBookNameFromFilePath(filePath);
                    ebook.Size =Math.Round( fs.Length / 1024.0d / 10224.0d,2);
                    fs.Close();
                    if (allBooks.Where(o => o.BookName.Equals(ebook.BookName)).Count() == 0)
                    {
                        allBooks.Add(ebook);
                        SaveAllBookInfos();
                    }
                }
            });
        }

        string GetBookNameFromFilePath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        /// <summary>
        /// 需要被读的文本
        /// </summary>
        public string TargetText
        {
            get
            {
                return targetText;
            }

            set
            {
                targetText = value;
            }
        }

        /// <summary>
        /// 需要读的文件目录
        /// </summary>
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                SetProperty<string>(ref filePath, value);
            }
        }

        /// <summary>
        /// 书签，标记上次读书的位置
        /// </summary>
        public int Bookmarker
        {
            get
            {
                return bookmarker;
            }

            set
            {
                bookmarker = value;
            }
        }

        public ObservableCollection<book> AllBooks { get => allBooks; set => allBooks = value; }


        /// <summary>
        /// 保存加载的图书信息到本地
        /// </summary>
        void SaveAllBookInfos()
        {
            if (allBooks.Count > 0)
            {
                FileStream fs = new FileStream("\\bookinfo.xml", FileMode.OpenOrCreate, FileAccess.Write);
                XmlSerializer serial = new XmlSerializer(typeof(ObservableCollection<book>));
                serial.Serialize(fs, allBooks);
                fs.Close();
            }
        }

        /// <summary>
        /// 加载本地的图书信息
        /// </summary>
        void LoadLocalBookinfos()
        {
            FileStream fs = new FileStream("\\bookinfo.xml", FileMode.OpenOrCreate, FileAccess.Read);
            if (fs.Length == 0)
            {
                fs.Close();
                return;
            }
            XmlSerializer serial = new XmlSerializer(typeof(ObservableCollection<book>));
            allBooks= serial.Deserialize(fs) as ObservableCollection<book>;
            fs.Close();
        }

        /// <summary>
        /// 加载选中图书，显示目录信息
        /// </summary>
        /// <param name="ebook"></param>
        void ShowBookCatalog(book ebook)
        {

        }


        /// <summary>
        /// 以文本的方式显示图书
        /// </summary>
        /// <param name="ebook"></param>
        void ShowBookByText(book ebook)
        {

        }

        /// <summary>
        /// 重头开始阅读
        /// </summary>
        /// <param name="ebook"></param>
        void ReReader(book ebook)
        {

        }
    }
}
