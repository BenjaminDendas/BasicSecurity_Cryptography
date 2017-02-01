using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Program
{
    public class BestandenFiles
    {
        public string loadPath { get; set; }
        public string savePath { get; set; }
        public string User { get; set; }

        public BestandenFiles(string gebruiker)
        {
            this.User = gebruiker;
        }

        public void OpenDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();
            loadPath = open.FileName;
        }

        public void SaveDialog()
        {
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == true)
            {
                
                    savePath = save.FileName;            
            }
        } 

    }
}
