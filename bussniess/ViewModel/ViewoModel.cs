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
using System.Text.RegularExpressions;

namespace ReadTextByVoice
{
   public  class ViewoModel:BindableBase
    {
        public ViewoModel()
        {
            reader = new SpeechSynthesizer();
            reader.SpeakCompleted += Reader_SpeakCompleted;
            reader.SpeakProgress += Reader_SpeakProgress;
            reader.Rate = 1;
            LoadLocalBookinfos();
        }

        private void Reader_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            double p =Math.Round(((e.CharacterPosition*1.0d) / (SelectedBook.CurrentReadedChapter.ChapterConent.Length*1.0d))*100,1);
            Progress = (int)p;
            if (p>=99)
            {
                Console.WriteLine(e.CharacterPosition + "," + SelectedBook.CurrentReadedChapter.ChapterConent.Length);
                Console.WriteLine("自动开始下一章"+ SelectedBook.CurrentReadedChapter.NextChapter?.Name);
                NextCmd.Execute(null);
                progress = 0;
            }
        }

        private void Reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            //NextCmd.Execute(null);
        }

        SpeechSynthesizer reader;

        string targetText;
        string filePath;
        int bookmarker;
        Novel selectedBook;
        int progress;
        ObservableCollection<Novel> allBooks = new ObservableCollection<Novel>();
        Prompt currentPlayprompt = null;
        /// <summary>
        /// 开始播放选中的txt
        /// </summary>
        public ICommand ReadTextCmd
        {
            get
            {
                return new DelegateCommand(Play);
            }
        }

        bool ispause = false;
        /// <summary>
        /// 开始播放选中的txt
        /// </summary>
        public ICommand PauseCmd
        {
            get
            {
                return new DelegateCommand(()=> 
                {
                    if (!ispause)
                    {
                        reader.Pause();
                        ispause = true;
                    }
                    else
                    {
                        ispause = false;
                        reader.Resume();
                    }
                });
            }
        }

        private void Play()
        {

            reader.SpeakAsyncCancelAll();
            
            //加载选中的章节，加载并进行阅读
            if (!string.IsNullOrEmpty(SelectedBook.CurrentReadedChapter.ChapterConent))
            {
                reader.SpeakAsync(SelectedBook.CurrentReadedChapter.ChapterConent);
            }
        }

        /// <summary>
        /// 上一章
        /// </summary>
        public ICommand PreviouCmd
        {
            get
            {
                return new DelegateCommand(()=> 
                {
                    if (SelectedBook != null)
                    {
                        SelectedBook.MovePreviou();
                        Play();
                    }
                });
            }
        }

        /// <summary>
        /// 上一章
        /// </summary>
        public ICommand NextCmd
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedBook != null)
                    {
                        SelectedBook.MoveNext();
                        Play();
                    }
                });
            }
        }

        /// <summary>
        /// reset the novel
        /// </summary>
        public ICommand RestCmd
        {
            get
            {
                return new DelegateCommand(()=> 
                {
                    if (SelectedBook!=null)
                    {
                        SelectedBook.CurrentReadedChapter = null;
                    }
                });
            }
        }

        string chapterNameText;
        /// <summary>
        /// Serch and load target chapter in the selected novel 
        /// </summary>
        public ICommand SerchCmd
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (SelectedBook != null)
                    {
                        if (string.IsNullOrEmpty(chapterNameText))
                        {
                            return;
                        }
                       Chapter echapter=SelectedBook.Catalogs.FirstOrDefault(o => o.Name.Contains(chapterNameText));
                        if (echapter != null)
                        {
                            SelectedBook.CurrentReadedChapter = echapter;
                            MessageBox.Show($"成功找到{echapter.Name}");
                        }
                        else
                        {
                            MessageBox.Show($"没有找到");
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public ICommand SaveCmd
        {
            get
            {
                return new DelegateCommand(SaveAllBookInfos);
            }
        }

        /// <summary>
        ///select novel
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
                    Novel ebook = new Novel(filePath);
                    if (allBooks.Where(o => o.BookName.Equals(ebook.BookName)).Count() == 0)
                    {
                        AllBooks.Add(ebook);
                        SelectedBook = ebook;
                        SaveAllBookInfos();
                    }
                }
            });
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

        public ObservableCollection<Novel> AllBooks { get => allBooks; set => allBooks = value; }
        public Novel SelectedBook { get => selectedBook; set =>SetProperty<Novel>(ref selectedBook,value); }
        public string ChapterNameText { get => chapterNameText; set => chapterNameText = value; }
        public int Progress { get => progress; set =>SetProperty<int>(ref progress , value); }


        /// <summary>
        /// 保存加载的图书信息到本地
        /// </summary>
        void SaveAllBookInfos()
        {
            if (allBooks.Count > 0)
            {
                //FileStream fs = new FileStream("\\bookinfo.xml", FileMode.OpenOrCreate, FileAccess.Write);
                //XmlSerializer serial = new XmlSerializer(typeof(ObservableCollection<Novel>));
                //serial.Serialize(fs, allBooks);
                //fs.Close();
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
            XmlSerializer serial = new XmlSerializer(typeof(ObservableCollection<Novel>));
            allBooks= serial.Deserialize(fs) as ObservableCollection<Novel>;
            fs.Close();
        }

        /// <summary>
        /// 加载选中图书，显示目录信息
        /// </summary>
        /// <param name="ebook"></param>
        void ShowBookCatalog(Novel ebook)
        {

        }


        /// <summary>
        /// 以文本的方式显示图书
        /// </summary>
        /// <param name="ebook"></param>
        void ShowBookByText(Novel ebook)
        {

        }

        /// <summary>
        /// 重头开始阅读
        /// </summary>
        /// <param name="ebook"></param>
        void ReReader(Novel ebook)
        {

        }


      
    }
}
