using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Tools.Utils;

namespace Sortzam.Lib.ACRCloudSDK
{
    public class ACRCloudRecognizer
    {
        private string _host = "";
        private string _accessKey = "";
        private string _accessSecret = "";
        private int _timeout; // ms
        private int _audioLengthSecond;
        private ACRCloudExtrTool _acrTool = new ACRCloudExtrTool();

        public ACRCloudRecognizer(string host, string accessKey, string accessSecret, int timeoutSecond = 5)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(accessSecret))
                throw new Exception("ACRCloud API : Host, AccessKey and AccessSecret are required fields");

            _host = host;
            _accessKey = accessKey;
            _accessSecret = accessSecret;
            _timeout = 1000 * timeoutSecond;
            _audioLengthSecond = 15;
        }

        /// <summary>
        /// recognize by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <param name="wavAudioBuffer">wavAudioBuffer query audio buffer</param>
        /// <param name="wavAudioBufferLen">wavAudioBufferLen the length of wavAudioBuffer</param>
        /// <returns>result</returns>
        public dynamic Recognize(byte[] wavAudioBuffer, int wavAudioBufferLen)
        {
            byte[] fp = _acrTool.CreateFingerprint(wavAudioBuffer, wavAudioBufferLen, false);

            if (fp == null)
                return null;

            return JsonConvert.DeserializeObject(DoRecognize(fp));
        }

        /// <summary>
        /// Recognize by file path of (Audio/Video file)
        ///          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
        ///          Video: mp4, mkv, wmv, flv, ts, avi ...
        /// </summary>
        /// <param name="filePath">filePath query file path</param>
        /// <param name="startSeconds">startSeconds skip (startSeconds) seconds from from the beginning of (filePath)</param>
        /// <returns>result</returns>
        public dynamic RecognizeByFile(string filePath, int startSeconds)
        {
            if (string.IsNullOrEmpty(filePath) || !FileUtils.Exists(filePath))
                throw new FileNotFoundException("Audio file not found : " + filePath);

            byte[] fp = _acrTool.CreateFingerprintByFile(filePath, startSeconds, _audioLengthSecond, false);

            if (fp == null)
                return null;

            return JsonConvert.DeserializeObject(DoRecognize(fp));
        }

        /// <summary>
        /// recognize by buffer of (Audio/Video file)
        ///          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
        ///          Video: mp4, mkv, wmv, flv, ts, avi ...
        /// </summary>
        /// <param name="fileBuffer">fileBuffer query buffer</param>
        /// <param name="fileBufferLen">fileBufferLen the length of fileBufferLen </param>
        /// <param name="startSeconds">startSeconds skip (startSeconds) seconds from from the beginning of fileBuffer</param>
        /// <returns>result</returns>
        public dynamic RecognizeByFileBuffer(byte[] fileBuffer, int fileBufferLen, int startSeconds)
        {
            byte[] fp = _acrTool.CreateFingerprintByFileBuffer(fileBuffer, fileBufferLen, startSeconds, _audioLengthSecond, false);
            
            if (fp == null)
                return null;

            return JsonConvert.DeserializeObject(DoRecognize(fp));
        }

        private string DoRecognize(byte[] queryData)
        {
            string method = "POST";
            string httpURL = "/v1/identify";
            string dataType = "fingerprint";
            string sigVersion = "1";
            string timestamp = ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();

            string reqURL = "http://" + _host + httpURL;

            string sigStr = method + "\n" + httpURL + "\n" + _accessKey + "\n" + dataType + "\n" + sigVersion + "\n" + timestamp;
            string signature = EncryptByHMACSHA1(sigStr, _accessSecret);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("access_key", _accessKey);
            dict.Add("sample_bytes", queryData.Length.ToString());
            dict.Add("sample", queryData);
            dict.Add("timestamp", timestamp);
            dict.Add("signature", signature);
            dict.Add("data_type", dataType);

            dict.Add("signature_version", sigVersion);
            return PostHttp(reqURL, dict);
        }

        private string PostHttp(string url, IDictionary<string, Object> postParams)
        {
            string result = "";

            string BOUNDARYSTR = "acrcloud***copyright***2015***" + DateTime.Now.Ticks.ToString("x");
            string BOUNDARY = "--" + BOUNDARYSTR + "\r\n";
            byte[] ENDBOUNDARY = Encoding.ASCII.GetBytes("--" + BOUNDARYSTR + "--\r\n\r\n");

            string stringKeyHeader = BOUNDARY +
                           "Content-Disposition: form-data; name=\"{0}\"" +
                           "\r\n\r\n{1}\r\n";
            string filePartHeader = BOUNDARY +
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                            "Content-Type: application/octet-stream\r\n\r\n";

            MemoryStream memStream = new MemoryStream();

            foreach (KeyValuePair<string, object> item in postParams)
            {
                if (item.Value is string)
                {
                    string tmpStr = string.Format(stringKeyHeader, item.Key, item.Value);
                    byte[] tmpBytes = Encoding.UTF8.GetBytes(tmpStr);
                    memStream.Write(tmpBytes, 0, tmpBytes.Length);
                }
                else if (item.Value is byte[])
                {
                    var header = string.Format(filePartHeader, "sample", "sample");
                    var headerbytes = Encoding.UTF8.GetBytes(header);
                    memStream.Write(headerbytes, 0, headerbytes.Length);
                    byte[] sample = (byte[])item.Value;
                    memStream.Write(sample, 0, sample.Length);
                    memStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
                }
            }

            memStream.Write(ENDBOUNDARY, 0, ENDBOUNDARY.Length);

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream writer = null;
            StreamReader myReader = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = _timeout;
                request.Method = "POST";
                request.ContentType = "multipart/form-data; boundary=" + BOUNDARYSTR;

                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);

                writer = request.GetRequestStream();
                writer.Write(tempBuffer, 0, tempBuffer.Length);
                writer.Flush();
                writer.Close();
                writer = null;

                response = (HttpWebResponse)request.GetResponse();
                myReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = myReader.ReadToEnd();
            }
            finally
            {
                if (memStream != null)
                {
                    memStream.Close();
                    memStream = null;
                }

                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }

                if (myReader != null)
                {
                    myReader.Close();
                    myReader = null;
                }

                if (request != null)
                {
                    request.Abort();
                    request = null;
                }

                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }

            return result;
        }

        private string EncryptByHMACSHA1(string input, string key)
        {
            HMACSHA1 hmac = new HMACSHA1(System.Text.Encoding.UTF8.GetBytes(key));
            byte[] stringBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedValue = hmac.ComputeHash(stringBytes);

            return EncodeToBase64(hashedValue);
        }

        private string EncodeToBase64(byte[] input)
        {
            string res = Convert.ToBase64String(input, 0, input.Length);
            return res;
        }
    }
}
