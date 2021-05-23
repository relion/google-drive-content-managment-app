using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Text;
using System.Globalization;

namespace ContentManagerStudio
{
    public partial class MainForm : Form
    {
        Hashtable _1_ht = new Hashtable();


        public void one_level_expand_tv(TreeNode tn, FolderInfo fi, bool force = false)
        {
            if (tn == null)
            {
                if (googleDriveTreeView.Tag != null)
                {
                    return;
                }
                googleDriveTreeView.Tag = new List<object>() { true };
            }
            else if (!force && tn.Tag != null)
            {
                if ((bool)((List<object>)tn.Tag)[0] == true)
                {
                    return;
                }
                ((List<object>)tn.Tag)[0] = (object)true;
            }
            else
            {
                tn.Tag = new List<object>() { true };
            }
            //

            int i;
            TreeNode tnf;
            for (i = 0; i < fi.folders.Count; i++)
            {
                FolderInfo fi2 = fi.folders[i];
                tnf = new TreeNode(my_utils.decode_heb(fi2.file.Name));
                tnf.Tag = new List<object>() { false, fi2 };
                _1_ht.Add(fi2, tnf);
                tnf.BackColor = Color.Orange;
                Invoke(new MethodInvoker(delegate
                {
                    if (tn == null) googleDriveTreeView.Nodes.Add(tnf);
                    else tn.Nodes.Add(tnf);
                }));
            }
            List<Google.Apis.Drive.v3.Data.File> files_list = fi.files;
            bool sort = true;
            if (sort)
            {
                files_list.Sort((x, y) => get_file_date(y).CompareTo(get_file_date(x)));
            }
            for (i = 0; i < files_list.Count; i++)
            {
                Console.WriteLine($"Adding node {i + 1} of total {files_list.Count}.");
                Google.Apis.Drive.v3.Data.File fi3 = files_list[i];
                tnf = new TreeNode(my_utils.decode_heb(fi3.Name));
                tnf.Tag = new List<object>() { false, fi3 };
                _1_ht.Add(fi3, tnf);
                tnf.BackColor = Color.LightBlue;


                if (didnt_like(fi3))
                {
                    tnf.BackColor = Color.DarkRed;
                }
                else
                {
                    string[] dup_files;
                    bool already_shared_by_other_copy;
                    if (get_dup_files(null, fi3, out dup_files, out already_shared_by_other_copy))
                    {
                        // tnf.BackColor = Color.Violet;
                        if (already_shared_by_other_copy)
                        {
                            tnf.BackColor = Color.Pink;
                        }
                    }
                }



                if (fi3.Shared == true)
                {
                    mark_treenode(tnf, true, fi3.OriginalFilename != fi3.Name);
                }
                Invoke(new MethodInvoker(delegate
                {
                    if (tn == null) googleDriveTreeView.Nodes.Add(tnf);
                    else tn.Nodes.Add(tnf);
                }));
            }
            if (tn != null)
            {
                Invoke(new MethodInvoker(delegate
                {
                    tn.BackColor = Color.LightBlue;
                    tn.ForeColor = Color.Purple;
                    tn.Expand();
                }));
            }
        }

        private static bool didnt_like(Google.Apis.Drive.v3.Data.File f)
        {
            bool didnt_like;
            JObject jo = (JObject)global_user_cat_json[f.Md5Checksum];
            didnt_like = (jo != null && ((bool?)jo["דעתי/יחס/לא אהבתי 👎"] == true || (bool?)jo["דעתי/למחיקה 🗑️"] == true));
            return didnt_like;
        }

        private void mark_treenode(TreeNode tnf, bool bold, bool filename_changed)
        {
            tnf.ForeColor = bold ? filename_changed ? Color.Blue : Color.Green : googleDriveTreeView.ForeColor;
            // tnf.NodeFont = new Font(googleDriveTreeView.Font, filename_changed ? FontStyle.Bold : FontStyle.Regular);
        }

        //private void googleDriveTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    handle_googleDriveTreeView_Node_selected(e.Node, null, true);
        //}

        public void handle_googleDriveTreeView_Node_selected(TreeNode tn, object file_or_folder = null, bool decode_heb = false)
        {
            showDupsButton.Enabled = false;
            List<object> tag_list = (List<object>)tn.Tag;
            if (tag_list.Count > 2 && (bool)tag_list[2] == true)
            {
                decode_heb = false;
            }
            saveButton.Enabled = false;
            List<object> lo = (List<object>)tn.Tag;
            if (file_or_folder == null)
            {
                file_or_folder = lo[1];
            }
            nodeInfoTextBox.Clear();
            if (file_or_folder is Google.Apis.Drive.v3.Data.File)
            {
                if ((bool)lo[0] == false)
                {
                    lo[0] = true;
                    //nodeInfoTextBox.AppendText("todo: handle display File info.");
                }
                else
                {
                    //nodeInfoTextBox.AppendText("todo: re-display File info.");
                }
                //
                Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)file_or_folder;
                JObject json = (JObject)global_user_cat_json[f.Md5Checksum];
                if (json == null)
                {
                    json = new JObject();
                }
                object[] _args = { json.ToString() };
                while (true)
                {
                    object ret = null;
                    Invoke(new MethodInvoker(delegate
                    {
                        ret = categoriesWebBrowser.Document.InvokeScript("update_tree_menue_checkboxes", _args);
                    }));
                    if ((bool?)ret == true) break;
                    System.Threading.Thread.Sleep(1000);
                }
                JObject jo = (JObject)global_user_cat_json[f.Md5Checksum];
                if (jo == null)
                {

                }
                nodeInfoTextBox.AppendText($"Date: {get_file_date_str(f)}\r\n");
                string fn = decode_heb ? my_utils.decode_heb(f.Name) : f.Name;
                nodeInfoTextBox.AppendText($"File name: {fn}\r\n");
                titleTextBox.Text = fn;
                //
                string original_fn = decode_heb ? my_utils.decode_heb(f.OriginalFilename) : f.OriginalFilename;
                if (original_fn != fn)
                {
                    nodeInfoTextBox.AppendText($"Original File name: {original_fn}\r\n");
                }
                //
                string f_description = null;
                if (f.Description != null)
                {
                    f_description = decode_heb ? my_utils.decode_heb(f.Description) : f.Description;
                }
                nodeInfoTextBox.AppendText($"Description: {f_description}\r\n");
                descriptionTextBox.Text = f_description;
                //
                nodeInfoTextBox.AppendText($"Size: {my_utils.bytes_to_formatted_string((long)f.Size)}\r\n");
                nodeInfoTextBox.AppendText($"Id: {f.Id}\r\n");
                string mimeType = f.MimeType;
                nodeInfoTextBox.AppendText($"MimeType: {mimeType}\r\n");
                if (mimeType.StartsWith("video/") && f.VideoMediaMetadata != null)
                {
                    nodeInfoTextBox.AppendText($"Dim (w/h): {f.VideoMediaMetadata.Width} x {f.VideoMediaMetadata.Height}\r\n");
                }
                if (f.VideoMediaMetadata != null && f.VideoMediaMetadata.DurationMillis != null)
                {
                    string duration = (new TimeSpan((long)f.VideoMediaMetadata.DurationMillis * 10000)).ToString(@"hh\:mm\:ss");
                    nodeInfoTextBox.AppendText($"Duration: {duration}\r\n");
                }
                //nodeInfoTextBox.AppendText($"Version: {f.Version}\r\n");
                string[] dup_files;
                bool already_shared_by_other_copy;
                if (get_dup_files(null, f, out dup_files, out already_shared_by_other_copy))
                {
                    nodeInfoTextBox.AppendText($"Duplicated: {dup_files.Length - 1} more time" + (dup_files.Length == 1 ? "" : "s") + "." + (already_shared_by_other_copy /* && f.Shared == true */ ? " Already Shared!" : "") + "\r\n");
                    showDupsButton.Enabled = true;
                }
                else
                {
                    nodeInfoTextBox.AppendText($"Not Duplicated.\r\n");
                }
                if (f.Permissions != null)
                {
                    string permissions = string.Join(", ", f.Permissions.Select(x => x.DisplayName != null ? x.DisplayName : x.Id).ToArray());
                    nodeInfoTextBox.AppendText($"Permissions: {permissions}\r\n");
                }
                //nodeInfoTextBox.AppendText($"Md5Checksum: {f.Md5Checksum}\r\n");
                nodeInfoTextBox.AppendText($"Shared: {f.Shared}\r\n");
                titleTextBox.Enabled = clearButton.Enabled = descriptionTextBox.Enabled = descriptionClearButton.Enabled = (bool)f.Shared;
                List<object> los = (List<object>)(googleDriveTreeView.Tag);
                if (los.Count == 1) los.Add(f);
                else los[1] = f;
                //[2] = f;
            }
            else if (file_or_folder is FolderInfo)
            {
                titleTextBox.Text = "";
                FolderInfo fi = (FolderInfo)file_or_folder;

                if ((bool)lo[0] == false)
                {
                    System.Threading.Thread thread;
                    thread = new System.Threading.Thread(new ThreadStart((new ex_ex(this, tn, fi)).one_level_expand_tv));
                    thread.IsBackground = true;
                    thread.Start();
                }
                //
                nodeInfoTextBox.AppendText($"Date: {get_file_date_str(fi.file)}");
                nodeInfoTextBox.AppendText($"\r\nFolder name: {my_utils.decode_heb(fi.file.Name)}");
                nodeInfoTextBox.AppendText($"\r\nPath: {my_utils.decode_heb(fi.path)}");
                nodeInfoTextBox.AppendText($"\r\nSize: {my_utils.bytes_to_formatted_string(fi.total_size_bytes)}");
                nodeInfoTextBox.AppendText($" n_files: {fi.n_files}");
                //nodeInfoTextBox.AppendText($"depth: {fi.depth}\r\n");
                //nodeInfoTextBox.AppendText("\r\ntodo: display Folder info.");
            }
            else //if (tn.Parent != null) // root.
            {
                throw new Exception();
            }

            dupButton.Enabled = file_or_folder is FolderInfo;
        }

        private string get_file_date_str(Google.Apis.Drive.v3.Data.File f)
        {
            string res = get_file_date(f).ToString("dd'/'MM'/'yyyy hh:mm");
            return res;
        }

        private DateTime get_file_date(Google.Apis.Drive.v3.Data.File f)
        {
            DateTime res;
            if (f.CreatedTime != null)
            {

                //if (res.Ticks == 637429619080000000)
                if (f.Name == "VID-20200712-WA0027.mp4")
                {

                }

                //res = new DateTime(f.CreatedTime.Value.Ticks);
                //return res;

                res = (DateTime)f.CreatedTime;

              //if (true && !DateTime.TryParseExact(f.CreatedTimeRaw, "MM/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out res))
                if (true && !DateTime.TryParseExact(f.CreatedTimeRaw, "MM/dd/yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out res))
                {
                    if (true && DateTime.TryParseExact(f.CreatedTime.ToString(), "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out res))
                    {
                    }
                    else if (true && DateTime.TryParseExact(f.CreatedTime.ToString(), "dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out res))
                    {
                    }
                    else
                    {
                        MessageBox.Show("failed to Parse: " + f.CreatedTime.ToString());
                    }
                }
                else
                {
                    //MessageBox.Show("Successfully Parsed: " + f.CreatedTime.ToString() + " to: " + res.ToString());
                }
            }
            else
            {
                if ((string)properties_ht["timeFormat"] == "1")
                {
                    res = DateTime.ParseExact(f.CreatedTimeRaw, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    res = DateTime.ParseExact(f.CreatedTimeRaw, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }
            return res;
        }

        private static bool get_dup_files(GoogleFolder root_folder, Google.Apis.Drive.v3.Data.File f, out string[] dup_files, out bool already_shared_by_other_copy)
        {
            List<object> fl = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_Md5_ht"])[f.Md5Checksum];
            already_shared_by_other_copy = false;
            // check if shared:
            foreach (GoogleFolder gf in fl)
            {
                if (gf.file.Id != f.Id && gf.file.Shared == true)
                {
                    already_shared_by_other_copy = true;
                }
            }
            // dup_files = fl.Select(x => ((GoogleFolder)x).file.Id).ToArray();
            List<string> _dup_files = new List<string>();
            foreach(GoogleFolder gf in fl)
            {
                // check if f is under gf
                _dup_files.Add(gf.file.Id);
            }
            dup_files = _dup_files.ToArray();
            if (dup_files.Length == 0) throw new Exception();
            return dup_files.Length > 1;
        }
    }

    public class ex_ex
    {
        MainForm t;
        TreeNode tn;
        FolderInfo fi;
        public ex_ex(MainForm _t, TreeNode _tn, FolderInfo _fi)
        {
            t = _t;
            tn = _tn;
            fi = _fi;
        }
        public void one_level_expand_tv()
        {
            t.show_processing_image("one_level_expand_tv");
            t.one_level_expand_tv(tn, fi);
            t.Invoke(new MethodInvoker(delegate
            {
                if (tn.IsExpanded || fi.files.Count == 0)
                {
                    t.show_processing_image();
                }
            }));
        }
    }
}
