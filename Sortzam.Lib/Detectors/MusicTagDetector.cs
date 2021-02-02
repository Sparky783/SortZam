using Newtonsoft.Json;
using RestSharp;
using Sortzam.Lib.DataAccessObjects;
using Sortzam.Lib.SDK;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tools.Utils;

namespace Sortzam.Lib.Detectors
{
    public class MusicTagDetector
    {
        private readonly string _apiKey;
        private readonly string _apiHost;
        private readonly string _apiSecretKey;
        private Dictionary<string, object> _configuration
        {
            get
            {
                return new Dictionary<string, object> {
                { "host", _apiHost.Trim() },
                { "access_key", _apiKey.Trim() },
                { "access_secret", _apiSecretKey.Trim() },
                { "timeout", 10 } // seconds
            };
            }
        }

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
            //var client = new RestClient(string.Format(_urlApi, search));
            //var request = new RestRequest(_method);
            //request.AddHeader("x-rapidapi-key", _rapidApiKey);
            //request.AddHeader("x-rapidapi-host", _rapidApiHost);
            //var response = client.Execute(request);

            //var recognizer = new ACRCloudRecognizer(new ACRCloudOptions(_apiHost, _apiKey, _apiSecretKey));
            //recognizer.RecognizeByFileAsync()

            return null;
        }
        public IEnumerable<MusicDao> Recognize(string filePath)
        {
            if (!FileUtils.Exists(filePath))
                throw new KeyNotFoundException(string.Format("File not found : `{0}`", filePath));

            // TODO : check duration file before start to 30s
            var recognizer = new ACRCloudRecognizer(_configuration);
            string result = recognizer.RecognizeByFile(filePath, 30);
            dynamic stuff;
            try
            {
                stuff = JsonConvert.DeserializeObject(result);
            }
            catch (Exception) { stuff = JsonConvert.DeserializeObject(ACRCloudStatusCode.JSON_ERROR); }

            int code;
            try { code = stuff.status.code; } catch (Exception) { code = 1001; }

            // If match
            if (code == 0)
                return Map(stuff.metadata);

            // If no matchs
            if (code == 1001)
                return null;

            // If an  other error occurs
            Exception(code, stuff?.status?.msg?.ToString());
            return null;
        }
        private IEnumerable<MusicDao> Map(dynamic jsonResult)
        {
            var result = new List<MusicDao>();
            foreach (var i in jsonResult.music)
                result.Add(new MusicDao().Map(i));
            return result;
        }
        private void Exception(int code, string defaultMessage)
        {
            var error = code switch
            {
                3001 => "Missing/Invalid Access Key",
                3002 => "Invalid ContentType. valid Content-Type is multipart/form-data",
                3003 => "Limit exceeded",
                3006 => "Invalid parameters",
                3014 => "Invalid Signature",
                3015 => "Could not generate fingerprint",
                _ => defaultMessage ?? "Unknow Error",
            };
            throw new Exception(error);
        }
    }
}
