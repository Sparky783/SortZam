using System;

namespace Sortzam.Lib.DataAccessObjects
{
    /// <summary>
    /// Represent a music
    /// </summary>
    public class MusicDao
    {
        #region Properties
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public string Album { get; set; }
        public string Comment { get; set; }
        public int? Year { get; set; }
        #endregion


        /// <summary>
        /// Map a Json object into a MusicDao object
        /// </summary>
        /// <param name="jsonResult">json object</param>
        /// <returns></returns>
        public MusicDao MapJson(dynamic jsonObject)
        {
            string artist = "";

            if (jsonObject.artists != null)
            {
                for (var i = 0; i < jsonObject.artists.Count; i++)
                    artist += string.Format("{0}{1}", i == 0 ? "" : "/", jsonObject.artists[i].name);
            }

            string genre = "";

            if (jsonObject.genres != null)
            {
                for (var i = 0; i < jsonObject.genres.Count; i++)
                    genre += string.Format("{0}{1}", i == 0 ? "" : "/", jsonObject.genres[i].name);
            }

            int year;
            int.TryParse(jsonObject.release_date?.ToString()?.Split('-')[0], out year);

            return new MusicDao()
            {
                Album = string.IsNullOrEmpty(jsonObject.album?.name?.ToString()) ? null : jsonObject.album?.name?.ToString(),
                Artist = string.IsNullOrEmpty(artist) ? null : artist,
                Kind = string.IsNullOrEmpty(genre) ? null : genre,
                Title = string.IsNullOrEmpty(jsonObject.title?.ToString()) ? null : jsonObject.title?.ToString(),
                Year = year
            };
        }
    }
}
