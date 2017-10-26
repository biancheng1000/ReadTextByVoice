using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTextByVoice
{
    /// <summary>
    /// 读书的信息
    /// </summary>
    public  class book
    {
        string bookName;
        double progress;
        string path;
        double size;

        public string BookName
        {
            get
            {
                return bookName;
            }

            set
            {
                bookName = value;
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

        public double Size { get => size; set => size = value; }
    }
}
