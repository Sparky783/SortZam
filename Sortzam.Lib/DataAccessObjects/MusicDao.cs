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

        /// <summary>
        /// Map a Json object into a MusicDao object
        /// </summary>
        /// <param name="jsonResult">json object</param>
        /// <returns></returns>
        public MusicDao MapJson(dynamic jsonObject)
        {
            var artist = "";
            if (jsonObject.artists != null)
                for (var i = 0; i < jsonObject.artists.Count; i++)
                    artist += string.Format("{0}{1}", i == 0 ? "" : " & ", jsonObject.artists[i].name);

            var genre = "";
            if (jsonObject.genres != null)
                for (var i = 0; i < jsonObject.genres.Count; i++)
                    genre += string.Format("{0}{1}", i == 0 ? "" : " & ", jsonObject.genres[i].name);

            int year;
            var t = int.TryParse(jsonObject.release_date?.ToString()?.Split('-')[0], out year);

            return new MusicDao()
            {
                Album = jsonObject.album?.name,
                Artist = artist,
                Kind = genre,
                Title = jsonObject.title,
                Year = year
            };
        }
    }
}
