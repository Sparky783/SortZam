using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Sortzam.Lib.ACRCloudSDK
{
    public class ACRCloudNative
    {
        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern int create_fingerprint(byte[] pcm_buffer, int pcm_buffer_len, byte is_db_fingerprint, ref IntPtr fps_buffer);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern int create_fingerprint_by_file(string file_path, int start_time_seconds, int audio_len_seconds, byte is_db_fingerprint, ref IntPtr fps_buffer);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern int create_fingerprint_by_filebuffer(byte[] file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, byte is_db_fingerprint, ref IntPtr fps_buffer);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern int decode_audio_by_file(string file_path, int start_time_seconds, int audio_len_seconds, ref IntPtr audio_buffer);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern int decode_audio_by_filebuffer(byte[] file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, ref IntPtr audio_buffer);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern void acr_free(IntPtr buffer);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern int get_duration_ms_by_file(string file_path);

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern void acr_set_debug();

        [DllImport("libacrcloud_extr_tool.dll")]
        public static extern void acr_init();
    }
}
