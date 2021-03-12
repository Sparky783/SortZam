using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Text;

namespace Sortzam.Ihm.Models
{
    public class AnalyzeResult : Notifier
    {
        public AnalyzeResult(MusicDao music)
        {
            Music = music;
            ColorLevel = new SolidColorBrush(Colors.Orange);
        }

        #region Properties
        public MusicDao Music { get; set; }

        /// <summary>
        /// Music title.
        /// </summary>
        public string Title
        {
            get
            {
                if (Music != null)
                    return Music.Title;

                return "";
            }
        }

        private int matchLevel;
        public int MatchLevel
        {
            get { return matchLevel; }
            set
            {
                matchLevel = value;
            }
        }

        private Brush colorLevel;
        public Brush ColorLevel
        {
            get { return colorLevel; }
            set
            {
                colorLevel = value;
            }
        }
        #endregion
    }
}
