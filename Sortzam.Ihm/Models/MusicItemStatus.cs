using System;
using System.Collections.Generic;
using System.Text;

namespace Sortzam.Ihm.Models
{
    public enum MusicItemStatus
    {
        Loaded,     // When metadata comes to be read.
        Analysed,   // When the file comes to be analysed by the API.
        Modified,   // When metadata comes to be changed by new values.
        Saved,      // When the modified file is saved.
        Error       // When the analyze failed.
    }
}
