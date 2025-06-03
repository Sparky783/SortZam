using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sortzam.Lib.Detectors
{
    public interface IDetector
    {
        IEnumerable<MusicDao> Recognize(string filePath);
    }
}
