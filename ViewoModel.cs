﻿using Microsoft.Practices.Prism.Commands;
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

namespace ReadTextByVoice
{
   public  class ViewoModel:BindableBase
    {
        public ViewoModel()
        {
            reader = new SpeechSynthesizer();
        }

        SpeechSynthesizer reader;

        string targetText;
        string filePath;
        int bookmarker;
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
    }
}