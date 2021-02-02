using Sortzam.Lib.ACRCloudSDK;
using Sortzam.Lib.DataAccessObjects;
using System;
using System.Collections.Generic;

namespace Sortzam.Lib.Detectors
{
    public class MusicTagDetector
    {
        private readonly string _apiKey;
        private readonly string _apiHost;
        private readonly string _apiSecretKey;

        public MusicTagDetector(string apiHost, string apiKey, string apiSecretKey)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiHost) || string.IsNullOrEmpty(apiSecretKey))
                throw new Exception("API Key and Api Host and Api Secret Key are required");
            _apiHost = apiHost;
            _apiSecretKey = apiSecretKey;
            _apiKey = apiKey;
        }
        public IEnumerable<MusicDao> Search(string search)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Recognize by file path of Audio file
        ///          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
        /// </summary>
        /// <param name="filePath">path to the audio file</param>
        /// <returns></returns>
        public IEnumerable<MusicDao> Recognize(string filePath)
        {
            // TODO : check duration file before start to 30s
            var recognizer = new ACRCloudRecognizer(_apiHost, _apiKey, _apiSecretKey);
            var stuff = recognizer.RecognizeByFile(filePath, 30);
            var code = int.Parse(stuff.status?.code?.ToString() ?? "0");

            // If match and no error code
            if (stuff != null && stuff.metadata != null && code == 0)
                return Map(stuff.metadata);

            // If no match
            if (code == 1001)
                return null;

            // If an  other error occurs
            var error = int.Parse(stuff.status.code.ToString()) switch
            {
                3001 => "Missing/Invalid Access Key",
                3002 => "Invalid ContentType. valid Content-Type is multipart/form-data",
                3003 => "Limit exceeded",
                3006 => "Invalid parameters",
                3014 => "Invalid Signature",
                3015 => "Could not generate fingerprint",
                _ => stuff?.status?.msg?.ToString() ?? "Unknow Error",
            };
            throw new Exception(error);
        }
        private IEnumerable<MusicDao> Map(dynamic jsonResult)
        {
            var result = new List<MusicDao>();
            foreach (var i in jsonResult.music)
                result.Add(new MusicDao().Map(i));
            return result;
        }
    }
}
