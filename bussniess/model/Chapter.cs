using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTextByVoice
{
    /// <summary>
    /// 章信息
    /// </summary>
   public class Chapter
    {

        public Chapter(string name, long position)
        {
            this.name = name;
            this.position = position;
        }
        Chapter previousChapter;
        Chapter nextChapter;
        string name;
        long position;
        StringBuilder contaiter=new StringBuilder ();
        string chapterContent;

        public Chapter PreviousChapter { get => previousChapter; set => previousChapter = value; }
        public Chapter NextChapter { get => nextChapter; set => nextChapter = value; }
        public string Name { get => name; set => name = value; }
        public long Position { get => position; set => position = value; }
        
        /// <summary>
        ///   chapter content
        /// </summary>
        public string ChapterConent
        {
            get
            {
                if (string.IsNullOrEmpty(chapterContent))
                {
                    chapterContent = contaiter.ToString();
                }
                return chapterContent ;
            }
            set
            {
                chapterContent = value;
            }
        }

        public void Add(string econtent)
        {
            contaiter.Append(econtent);
        }
    }
}
