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
    [Serializable]
    public class RootInfo
    {
        public FolderInfo root;
        //public Hashtable file_by_Id_ht = new Hashtable();
        //public Hashtable folder_by_Id_ht = new Hashtable();
        //public Hashtable by_Md5_ht = new Hashtable();
        //public Hashtable file_by_Name_ht = new Hashtable();
        public Hashtable by_ht_name_ht = new Hashtable();
    }

    [Serializable]
    public class GoogleFolder
    {
        private string _path;
        private string folder_file_json;

        public string path
        {
            get { return _path; }
        }

        public GoogleFolder(Google.Apis.Drive.v3.Data.File folder_file, string path)
        {
            folder_file_json = my_utils.file_to_json(folder_file);
            _path = path;
        }

        public Google.Apis.Drive.v3.Data.File file
        {
            get { return my_utils.get_file_from_json(folder_file_json); }
        }
    }

    [Serializable]
    public class FolderInfo
    {
        public string path;

        private string folder_file_json;

        public FolderInfo(Google.Apis.Drive.v3.Data.File folder_file, string path)
        {
            this.path = path;
            folder_file_json = my_utils.file_to_json(folder_file);
        }

        public void update_folder_file(Google.Apis.Drive.v3.Data.File updated_file, int i)
        {
            //Google.Apis.Drive.v3.Data.File fff = file;
            _files[i] = my_utils.file_to_json(updated_file);
            //this.files[i] = updated_file;
            //folder_file_json = my_utils.file_to_json(folder);
        }

        public Google.Apis.Drive.v3.Data.File file
        {
            get { return my_utils.get_file_from_json(folder_file_json); }
        }

        public long total_size_bytes = 0;
        public int n_files = 0;
        public int n_folders = 0;

        private List<string> _files = new List<string>();

        public void add_file(Google.Apis.Drive.v3.Data.File file)
        {
            _files.Add(my_utils.file_to_json(file));
            //recache_files();
        }

        [NonSerialized]
        List<Google.Apis.Drive.v3.Data.File> files_cached = null;

        public List<Google.Apis.Drive.v3.Data.File> files
        {
            get
            {
                bool check_this = false;
                if (check_this && files_cached != null)
                {
                    return files_cached;
                }
                else
                {
                    recache_files();
                    return files_cached;
                }
            }
        }

        public void recache_files()
        {
            List<Google.Apis.Drive.v3.Data.File> res = new List<Google.Apis.Drive.v3.Data.File>();
            foreach (string file_json in _files)
            {
                res.Add(my_utils.get_file_from_json(file_json));
            }
            files_cached = res;
        }

        public List<FolderInfo> folders = new List<FolderInfo>();
    }
}
