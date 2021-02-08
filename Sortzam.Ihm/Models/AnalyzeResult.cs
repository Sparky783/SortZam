using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sortzam.Ihm.Models
{
    public class AnalyzeResult : Notifier
    {
        public AnalyzeResult(MusicDao music)
        {
            Music = music;
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
        #endregion
    }
}
