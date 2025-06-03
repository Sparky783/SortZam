using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sortzam.Lib.Detectors
{
    public class DetectorConnector : IDetector
    {
        private IDetector _detector;

        public DetectorConnector(IDetector detector)
        {
            _detector = detector;
        }

        public IEnumerable<MusicDao> Recognize(string filePath)
        {
            if (_detector == null)
                return null;

            return _detector.Recognize(filePath);
        }
    }
}
