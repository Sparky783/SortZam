using System;
using System.Runtime.InteropServices;

namespace Sortzam.Lib.ACRCloudSDK
{
    public class ACRCloudExtrTool
    {
        public ACRCloudExtrTool()
        {
            try
            {
                ACRCloudNative.acr_init();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("0x8007000B"))
                    throw new Exception("libacrcloud_extr_tool.dll is not 64bits or AnyCpu compatible. Try to build in x86 mode");
                
                throw new Exception("Error", e);
            }
        }

        /**
          *
          *  create "ACRCloud Fingerprint" by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz) 
          *
          *  @param pcmBuffer query audio buffer
          *  @param pcmBufferLen the length of wavAudioBuffer
          *  @param isDB   If it is True, it will create db frigerprint; 
          *  
          *  @return result "ACRCloud Fingerprint"
          *
          **/
        public byte[] CreateFingerprint(byte[] pcmBuffer, int pcmBufferLen, bool isDB)
        {
            byte[] fpBuffer = null;

            if (pcmBuffer == null || pcmBufferLen <= 0)
                return fpBuffer;

            if (pcmBufferLen > pcmBuffer.Length)
                pcmBufferLen = pcmBuffer.Length;

            byte tIsDB = (isDB) ? (byte)1 : (byte)0;
            IntPtr pFpBuffer = IntPtr.Zero;
            int fpBufferLen = ACRCloudNative.create_fingerprint(pcmBuffer, pcmBufferLen, tIsDB, ref pFpBuffer);

            if (fpBufferLen <= 0)
                return fpBuffer;

            fpBuffer = new byte[fpBufferLen];
            Marshal.Copy(pFpBuffer, fpBuffer, 0, fpBufferLen);
            ACRCloudNative.acr_free(pFpBuffer);

            return fpBuffer;
        }

        /**
          *
          *  create "ACRCloud Fingerprint" by file path of (Audio/Video file)
          *          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
          *          Video: mp4, mkv, wmv, flv, ts, avi ...
          *
          *  @param filePath query file path
          *  @param startTimeSeconds skip (startSeconds) seconds from from the beginning of (filePath)
          *  @param audioLenSeconds Length of audio data you need. if you create recogize frigerprint, default is 12 seconds, if you create db frigerprint, it is not usefully; 
          *  @param isDB   If it is True, it will create db frigerprint; 
          *  
          *  @return result "ACRCloud Fingerprint"
          *
          **/
        public byte[] CreateFingerprintByFile(string filePath, int startTimeSeconds, int audioLenSeconds, bool isDB)
        {
            byte[] fpBuffer = null;
            byte tIsDB = (isDB) ? (byte)1 : (byte)0;
            IntPtr pFpBuffer = IntPtr.Zero;
            int fpBufferLen = ACRCloudNative.create_fingerprint_by_file(filePath, startTimeSeconds, audioLenSeconds, tIsDB, ref pFpBuffer);
            
            switch (fpBufferLen)
            {
                case -1:
                    throw new Exception(filePath + " is not readable!");
                case -2:
                    throw new Exception(filePath + " can not be decoded audio data!");
            }

            if (fpBufferLen == 0)
                return fpBuffer;

            fpBuffer = new byte[fpBufferLen];
            Marshal.Copy(pFpBuffer, fpBuffer, 0, fpBufferLen);
            ACRCloudNative.acr_free(pFpBuffer);

            return fpBuffer;
        }

        /**
          *
          *  create "ACRCloud Fingerprint" by file buffer of (Audio/Video file)
          *          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
          *          Video: mp4, mkv, wmv, flv, ts, avi ...
          *
          *  @param fileBuffer data buffer of input file
          *  @param fileBufferLen  length of fileBuffer
          *  @param startTimeSeconds skip (startSeconds) seconds from from the beginning of (filePath)
          *  @param audioLenSeconds Length of audio data you need. if you create recogize frigerprint, default is 12 seconds, if you create db frigerprint, it is not usefully; 
          *  @param isDB   If it is True, it will create db frigerprint; 
          *  
          *  @return result "ACRCloud Fingerprint"
          *
          **/
        public byte[] CreateFingerprintByFileBuffer(byte[] fileBuffer, int fileBufferLen, int startTimeSeconds, int audioLenSeconds, bool isDB)
        {
            byte[] fpBuffer = null;

            if (fileBufferLen > fileBuffer.Length)
                fileBufferLen = fileBuffer.Length;

            byte tIsDB = (isDB) ? (byte)1 : (byte)0;
            IntPtr pFpBuffer = IntPtr.Zero;
            int fpBufferLen = ACRCloudNative.create_fingerprint_by_filebuffer(fileBuffer, fileBufferLen, startTimeSeconds, audioLenSeconds, tIsDB, ref pFpBuffer);
            
            switch (fpBufferLen)
            {
                case -1:
                    throw new Exception("fileBuffer is not audio/video data!");
                case -2:
                    throw new Exception("fileBuffer can not be decoded audio data!");
            }

            if (fpBufferLen == 0)
                return fpBuffer;

            fpBuffer = new byte[fpBufferLen];
            Marshal.Copy(pFpBuffer, fpBuffer, 0, fpBufferLen);
            ACRCloudNative.acr_free(pFpBuffer);

            return fpBuffer;
        }

        /**
          *
          *  decode audio from file path of (Audio/Video file)
          *          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
          *          Video: mp4, mkv, wmv, flv, ts, avi ...
          *
          *  @param filePath query file path
          *  @param startTimeSeconds skip (startSeconds) seconds from from the beginning of (filePath)
          *  @param audioLenSeconds Length of audio data you need, if it is 0, will decode all the audio;  
          *  
          *  @return result audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
          *
          **/
        public byte[] DecodeAudioByFile(string filePath, int startTimeSeconds, int audioLenSeconds)
        {
            byte[] audioBuffer = null;
            IntPtr pAudioBuffer = IntPtr.Zero;
            int fpBufferLen = ACRCloudNative.decode_audio_by_file(filePath, startTimeSeconds, audioLenSeconds, ref pAudioBuffer);

            switch (fpBufferLen)
            {
                case -1:
                    throw new Exception(filePath + " is not readable!");
                case -2:
                    throw new Exception(filePath + " can not be decoded audio data!");
            }

            if (fpBufferLen == 0)
                return audioBuffer;

            audioBuffer = new byte[fpBufferLen];
            Marshal.Copy(pAudioBuffer, audioBuffer, 0, fpBufferLen);
            ACRCloudNative.acr_free(pAudioBuffer);

            return audioBuffer;
        }

        /**
          *
          *  decode audio from file buffer of (Audio/Video file)
          *          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
          *          Video: mp4, mkv, wmv, flv, ts, avi ...
          *
          *  @param fileBuffer data buffer of input file
          *  @param fileBufferLen  length of fileBuffer
          *  @param startTimeSeconds skip (startSeconds) seconds from from the beginning of (filePath)
          *  @param audioLenSeconds Length of audio data you need, if it is 0, will decode all the audio;  
          *  
          *  @return result audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
          *
          **/
        public byte[] DecodeAudioByFileBuffer(byte[] fileBuffer, int fileBufferLen, int startTimeSeconds, int audioLenSeconds)
        {
            byte[] audioBuffer = null;

            if (fileBufferLen > fileBuffer.Length)
                fileBufferLen = fileBuffer.Length;

            IntPtr pAudioBuffer = IntPtr.Zero;
            int fpBufferLen = ACRCloudNative.decode_audio_by_filebuffer(fileBuffer, fileBufferLen, startTimeSeconds, audioLenSeconds, ref pAudioBuffer);
            
            switch (fpBufferLen)
            {
                case -1:
                    throw new Exception("fileBuffer is not audio/video data!");
                case -2:
                    throw new Exception("fileBuffer can not be decoded audio data!");
            }

            if (fpBufferLen == 0)
                return audioBuffer;

            audioBuffer = new byte[fpBufferLen];
            Marshal.Copy(pAudioBuffer, audioBuffer, 0, fpBufferLen);
            ACRCloudNative.acr_free(pAudioBuffer);

            return audioBuffer;
        }

        /**
          *
          *  get duration from file buffer of (Audio/Video file)
          *          Audio: mp3, wav, m4a, flac, aac, amr, ape, ogg ...
          *          Video: mp4, mkv, wmv, flv, ts, avi ...
          *
          *  @param filePath query file path 
          *  
          *  @return duration ms
          *
          **/
        public int GetDurationMillisecondByFile(string filePath)
        {
            return ACRCloudNative.get_duration_ms_by_file(filePath);
        }
    }
}
