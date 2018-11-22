using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ReadTextByVoice
{
    public class Novel:BindableBase
    {

        public Novel()
        {

        }
        public Novel(string path)
        {
            this.path = path;
            LoadChapter();
        }

        IList<string> filter = new List<string>() { "www.pbtxt.cOm" };
        string bookName;
        double progress;
        string path;
        double size;
        Chapter lastReadedChapter;
        IList<Chapter> catalogs = new List<Chapter>();
        string display="未加载";

        public string BookName
        {
            get
            {
                return bookName;
            }

            set
            {
                SetProperty<string>(ref bookName, value);
            }
        }                                               
        public double Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value;
            }
        }

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }

        public double Size { get => size; set =>SetProperty<double>(ref size,value); }
        public IList<Chapter> Catalogs { get => catalogs; set => catalogs = value; }

        public Chapter CurrentReadedChapter {
            get
            {
                if (lastReadedChapter == null)
                {
                    lastReadedChapter = Catalogs.FirstOrDefault();
                }
                return lastReadedChapter;
            }
            set =>SetProperty<Chapter>(ref lastReadedChapter,value); }

        public string Display { get => display; set => SetProperty<string>(ref display,value); }

        public string GetCapterFromString(string capterstring)
        {
            Match result = Regex.Match(capterstring, "[第]([0-9]*|[一-十,四,零]?千?[一-十,四,零]?百?[一-十,四,零]?十?[一-十,四,零]?)[章].*");
            if (result.Success)
            {
                return result.Value;
            }
            else
            {
                return string.Empty;
            }
        }
        string GetBookNameFromFilePath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        public void MoveNext()
        {
            if (CurrentReadedChapter?.NextChapter != null)
            {
                CurrentReadedChapter = CurrentReadedChapter.NextChapter;
            }
        }

        public void MovePreviou()
        {
            if (CurrentReadedChapter.PreviousChapter != null)
            {
                CurrentReadedChapter = CurrentReadedChapter.PreviousChapter;
            }
        }

        public async void LoadChapter()
        {
            BookName = GetBookNameFromFilePath(path);
            Display = "加载中";
            await Task.Run(()=> 
            {
            FileStream fs = new FileStream(path, FileMode.Open);
            Size = Math.Round(fs.Length / 1024.0d / 1024.0d, 2);
            StreamReader rs = new StreamReader(fs);
            Chapter lastChapter = null;
            Chapter currentChaper = null;
                int index = 1;
            while (!rs.EndOfStream)
            {
                long pos = rs.BaseStream.Position;
                string content = rs.ReadLine();

                Array.ForEach(filter.ToArray(), (o) => 
                {
                    content.Replace(o, "");
                });

                if (string.IsNullOrEmpty(content))
                {
                    continue;
                }
                string cap = GetCapterFromString(content);
                if (!string.IsNullOrEmpty(cap))
                {
                    Chapter ch = new Chapter(content, pos);
                    currentChaper = ch;
                    Catalogs.Add(ch);
                     
                    if (lastChapter == null)
                    {
                        lastChapter = ch;
                    }
                    else
                    {
                        ch.PreviousChapter = lastChapter;
                        lastChapter.NextChapter = ch;
                    }
                    lastChapter = ch;
                    Display = cap;
                }
                else
                {
                    if (currentChaper != null)
                    {
                        currentChaper.Add(content);
                    }
                }
            }
            fs.Close();
            });
        }

        public void Serach(string captionstr,bool isMsg=false)
        {
            if (string.IsNullOrEmpty(captionstr))
            {
                return;
            }
            Chapter echapter = Catalogs.FirstOrDefault(o => o.Name.Contains(captionstr));
            if (echapter != null)
            {
                CurrentReadedChapter = echapter;
                if (isMsg)
                {
                    MessageBox.Show($"成功找到{echapter.Name}");
                }
                
            }
            else
            {
                if (isMsg)
                {
                    MessageBox.Show($"没有找到");
                }
            }
        }

        public void RemoveDuplicateChapters()
        {
            Catalogs = this.Catalogs.Distinct(new TestDuplicateDefine()).ToList().OrderBy(n=>n.Position).ToList();
            string NewBookName = Path.Insert(path.LastIndexOf('.'), "_New");
            using (FileStream fs = new FileStream(NewBookName, FileMode.Create))
            {
                StreamWriter ts = new StreamWriter(fs); 
                for (int i = 0; i < Catalogs.Count; i++)
                {
                    ts.WriteLine(Catalogs[i].Name);
                    ts.WriteLine(catalogs[i].ChapterConent);
                }
                ts.Close();
            }
            MessageBox.Show("去除重复完成！");
        }
    }

    public class TestDuplicateDefine : IEqualityComparer<Chapter>
    {
        public bool Equals(Chapter x, Chapter y)
        {
            return x.Name == y.Name;
        }
        public int GetHashCode(Chapter obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

}
