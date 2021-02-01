using System;

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
        public MusicDao() { }
        public MusicDao Map(dynamic jsonResult)
        {
            var artist = "";
            for (var i = 0; i < jsonResult.artists.Count; i++)
                artist += string.Format("{0}{1}", i == 0 ? "" : " & ", jsonResult.artists[i].name);

            var genre = "";
            for (var i = 0; i < jsonResult.genres.Count; i++)
                genre += string.Format("{0}{1}", i == 0 ? "" : " & ", jsonResult.genres[i].name);

            int year;
            var t = int.TryParse(jsonResult.release_date?.ToString()?.Split('-')[0], out year);

            return new MusicDao()
            {
                Album = jsonResult.album?.name,
                Artist = artist,
                Kind = genre,
                Title = jsonResult.title,
                Year = year
            };
        }
    }
}
