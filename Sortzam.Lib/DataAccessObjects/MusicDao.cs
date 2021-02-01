namespace Sortzam.Lib.DataAccessObjects
{
    public class MusicDao
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public string Album { get; set; }
        public string Comment { get; set; }
        public int? Year { get; set; }
        public int? TrackNumber { get; set; }
        public MusicDao() { }
    }
}
