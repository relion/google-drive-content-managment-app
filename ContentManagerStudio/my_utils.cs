using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Text;

namespace ContentManagerStudio
{
    static class my_utils
    {
        public static void serialize_to_gz_file(RootInfo _ri)
        {
            // Binary:
            FileStream file_write_stream = System.IO.File.Open(MainForm.ser_name, FileMode.Create, FileAccess.Write);
            (new BinaryFormatter()).Serialize(file_write_stream, _ri);
            file_write_stream.Close();
            //
            // GZip:
            FileStream file_read_stream = new FileStream(MainForm.ser_name, FileMode.Open, FileAccess.Read);
            string compressed_file_name = MainForm.ser_name + ".gz";
            string back_name = MainForm.ser_name + ".bak.gz";
            // int i = compressed_file_name.LastIndexOf(@".");
            if (System.IO.File.Exists(compressed_file_name))
            {
                //string back_name = compressed_file_name.Substring(0, i) + @"\" + DateTime.Now.ToString("yyMMddHHmm") + " " + compressed_file_name.Substring(i + 1);
                System.IO.File.Delete(back_name);
                System.IO.File.Move(compressed_file_name, back_name);
            }
            FileStream compressedFileStream = new FileStream(compressed_file_name, FileMode.Create);
            GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress);
            file_read_stream.CopyTo(compressionStream);
            compressionStream.Close();
            file_read_stream.Close();
        }

        public const string slash_char = @"/";

        public static string file_to_json(Google.Apis.Drive.v3.Data.File file)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms);
            (new JsonSerializer()).Serialize(writer, file);
            writer.Close();
            string file_json = System.Text.Encoding.Default.GetString(ms.ToArray());
            return file_json;
        }

        public static Google.Apis.Drive.v3.Data.File get_file_from_json(string json_str)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(json_str);
            if (jo == null) return null;
            Google.Apis.Drive.v3.Data.File res = jo.ToObject<Google.Apis.Drive.v3.Data.File>();
            return res;
        }

        public static void scroll_to_bottom(TextBox tb)
        {
            tb.SelectionStart = tb.Text.Length;
            tb.ScrollToCaret();
        }

        public static void add_and_count_key_to_ht(Hashtable ht, string key)
        {
            if (!ht.Contains(key))
            {
                ht.Add(key, 1);
            }
            else
            {
                ht[key] = (int)(ht[key]) + 1;
            }
        }

        public static void add_obj_to_ht(Hashtable ht, object key, object file_or_folder)
        {
            if (!ht.Contains(key))
            {
                ht.Add(key, new List<object>() { file_or_folder });
            }
            else
            {
                ((List<object>)(ht[key])).Add(file_or_folder);
            }
        }

        public static string bytes_to_formatted_string(long n_bytes)
        {
            string size_str;
            if (n_bytes < 1024)
            {
                size_str = n_bytes + " ";
            }
            else if (n_bytes < 1024 * 1024)
            {
                size_str = ((float)n_bytes / 1024).ToString("0.0") + " K";
            }
            else if (n_bytes < 1024 * 1024 * 1024)
            {
                size_str = ((float)n_bytes / 1024 / 1024).ToString("0.0") + " M";
            }
            else
            {
                size_str = ((float)n_bytes / 1024 / 1024 / 1024).ToString("0.0") + " G";
            }

            return size_str + "B";
        }

        public static string decode_heb(string txt)
        {
            if (txt == null)
            {
                return "";
            }

            //foreach (EncodingInfo ei in Encoding.GetEncodings())
            //{
            //    Encoding e = ei.GetEncoding();

            //    //Console.Write("{0,-6} {1,-25} ", ei.CodePage, ei.Name);
            //    //Console.Write("{0,-8} {1,-8} ", e.IsBrowserDisplay, e.IsBrowserSave);
            //    //Console.Write("{0,-8} {1,-8} ", e.IsMailNewsDisplay, e.IsMailNewsSave);
            //    //Console.WriteLine("{0,-8} {1,-8} ", e.IsSingleByte, e.IsReadOnly);
            //    foreach (EncodingInfo ei3 in Encoding.GetEncodings())
            //    {
            //        Encoding e23 = ei.GetEncoding();
            //        Console.WriteLine(e23.GetString(e.GetBytes(txt)));
            //    }

            //}






            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(txt));
        }

        public static string encode_heb(string txt)
        {
            //if (Decoder.De)
            string res = Encoding.Default.GetString(Encoding.UTF8.GetBytes(txt));
            return res;
        }
    }
}
