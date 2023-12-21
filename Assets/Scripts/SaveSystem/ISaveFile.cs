using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SaveSystem
{
    public interface ISaveFile
    {
        public void SetSystemHeaders(string fileName, string filePath);
    }
}
