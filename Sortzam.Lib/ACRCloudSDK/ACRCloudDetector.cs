using Sortzam.Lib.DataAccessObjects;
using Sortzam.Lib.Detectors;
using System;
using System.Collections.Generic;

namespace Sortzam.Lib.ACRCloudSDK
{
    /// <summary>
    /// Music Tag ID3 and meta datas loader from CRCloud API
    /// </summary>
    public class ACRCloudDetector : IDetector
    {
        private readonly string _apiKey;
        private readonly string _apiHost;
        private readonly string _apiSecretKey;


        /// <summary>
        /// Instance the detector using CRCloud API
        /// </summary>
        /// <param name="apiHost">host of CRCloud api</param>
        /// <param name="apiKey">api Key to authenticate API</param>
        /// <param name="apiSecretKey">secret Key to authenticate API</param>
        public ACRCloudDetector(string apiHost, string apiKey, string apiSecretKey)
        {
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiHost) || string.IsNullOrEmpty(apiSecretKey))
                throw new Exception("API Key and Api Host and Api Secret Key are required");

            _apiHost = apiHost;
            _apiSecretKey = apiSecretKey;
            _apiKey = apiKey;
        }


        /// <summary>
        /// Recognize tag ID3 and metas datas music, from an audio file path
        ///          Audio accepted: mp3, wav, m4a, flac, aac, amr, ape, ogg, wma ...
        /// </summary>
        /// <param name="filePath">path to the audio file</param>
        /// <returns></returns>
        public IEnumerable<MusicDao> Recognize(string filePath)
        {
            // TODO : check duration file before start to 30s
            ACRCloudRecognizer recognizer = new ACRCloudRecognizer(_apiHost, _apiKey, _apiSecretKey);
            dynamic stuff = recognizer.RecognizeByFile(filePath, 100);
            dynamic code = int.Parse(stuff.status?.code?.ToString() ?? "0");

            // If match and no error code
            if (stuff != null && stuff.metadata != null && code == 0)
                return Map(stuff.metadata);

            // If no match
            if (code == 1001)
                return null;

            // If an  other error occurs
            dynamic error = int.Parse(stuff.status.code.ToString()) switch
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
            List<MusicDao> result = new List<MusicDao>();

            foreach (dynamic i in jsonResult.music)
                result.Add(new MusicDao().MapJson(i));

            return result;
        }
    }
}
