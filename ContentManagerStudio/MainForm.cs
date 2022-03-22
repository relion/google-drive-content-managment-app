using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;


using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

using Google.Apis.Services;

using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;

using Google.Apis.Util.Store;



using Coe.WebSocketWrapper;


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
using System.Net;
using System.Text.RegularExpressions;
using System.Linq;
using System.Drawing;
using CefSharp;
using CefSharp.WinForms;

namespace ContentManagerStudio
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class MainForm : Form
    {
        public static MainForm this_form;
        public Hashtable properties_ht = new Hashtable();
        public string excluded_folders_re;

        ContextMenu contextMenu = new ContextMenu();

        public MainForm()
        {
            InitializeComponent();
            this.Text = read_google_drive.ApplicationName;
            this_form = this;
            //
            string[] config_lines = System.IO.File.ReadAllLines("config.txt");
            foreach (string line in config_lines)
            {
                string[] key_val = line.Split(':');
                string key = key_val[0].Trim();
                if (key.StartsWith("#"))
                {
                    continue;
                }
                string val = key_val[1].Trim();
                properties_ht[key] = val;
            }

            string ex_re = (string)properties_ht["excluded_folders_re"];
            if (string.IsNullOrEmpty(ex_re))
            {
                excluded_folders_re = null;
            }
            else
            {
                excluded_folders_re = ex_re.Replace("*", @".*").Replace("/", @"\/").Replace("$", @"\$"); // .Split(',').Select(f => f.Trim()).ToArray();
            }
        }

        private void check_user_cat(JArray cat_ar)
        {
            foreach (KeyValuePair<string, JToken> id_value_pair in global_user_cat_json)
            {
                string id = id_value_pair.Key;
                foreach (JProperty user_path_property in id_value_pair.Value)
                {
                    bool found = false;
                    string[] path_items = user_path_property.Name.Split('/');
                    JArray cat_pointer = cat_ar;
                    int i;
                    string last_type_found = null;
                    for (i = 0; i < path_items.Length; i++)
                    {
                        found = false;
                        foreach (object cat_item in cat_pointer)
                        {
                            if (cat_item is JValue && ((JValue)cat_item).Value.Equals(path_items[i]))
                            {
                                if (i < path_items.Length - 1)
                                {
                                    cat_pointer = (JArray)cat_pointer["content"];
                                }
                                found = true;
                                last_type_found = "file";
                                break;
                            }
                            else if (cat_item is JObject && (string)((JObject)cat_item)["dname"] == path_items[i])
                            {
                                cat_pointer = (JArray)((JObject)cat_item)["content"];
                                found = true;
                                last_type_found = "folder";
                                break;
                            }
                            else
                            {
                                //
                            }
                        }
                        if (!found)
                        {
                            break;
                        }
                    }
                    if (!found)
                    {
                        string err_string = "Checkbox Not found\r\nId: " + id + "\r\npath: " + user_path_property.Name;
                        MessageBox.Show(err_string); // Console.WriteLine()
                    }
                    else if (last_type_found != "file")
                    {
                        MessageBox.Show(user_path_property.Name + " is a folder (not a file/leaf).");
                    }
                }
            }
        }

        private void Run(object sender, EventArgs e)
        {
            MainForm_SizeChanged(sender, e);
            //    string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + "accesstoken" + "";
            //    WebRequest request = WebRequest.Create(url);
            //    request.Credentials = CredentialCache.DefaultCredentials;
            //    WebResponse response = request.GetResponse();
            //    Stream dataStream = response.GetResponseStream();
            //    StreamReader reader = new StreamReader(dataStream);
            //    string responseFromServer = reader.ReadToEnd();
            //    reader.Close();
            //    response.Close();
            //    JavaScriptSerializer js = new JavaScriptSerializer();
            //    Userclass userinfo = js.Deserialize<Userclass>(responseFromServer);
            //    imgprofile.ImageUrl = userinfo.picture;
            //    lblid.Text = userinfo.id;
            //    lblgender.Text = userinfo.gender;
            //    lbllocale.Text = userinfo.locale;
            //    lblname.Text = userinfo.name;
            //    hylprofile.NavigateUrl = userinfo.link;
            //}



            //var service = new DiscoveryService(new BaseClientService.Initializer
            //{
            //    ApplicationName = "Discovery Sample",
            //    ApiKey = "[YOUR_API_KEY_HERE]",
            //});

            //Google.Apis.Drive //DirectoryService 

            DiscoveryService service = null;
            //service = new DiscoveryService(new BaseClientService.Initializer
            //{
            //    ApplicationName = "CMStudio",
            //    ApiKey = "...",
            //});

            //var x = service.Users.Messages.Get("", messageId).Execute();

            if (false)
            {
                var s = service.Apis.List();
                DirectoryList r = s.Execute();
                foreach (var ii in r.Items)
                {
                    Console.WriteLine(ii.Kind + " " + ii.Name);
                    if (ii.Name == "drive#permission")
                    {
                    }
                }

            }

            //service.Apis

            //var service = new Peo(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "M_Test",
            //});

            //service.Apis.List.get

            //UserCredential credential = read_google_drive.get_credential();

            //var Service = new Big(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    //ApplicationName = "BigQueryDemoApp"
            //});




            //Google.Apis.Auth.OAuth2.sc. v2.Oauth2Service.Scope.UserinfoProfile



            //DriveService service = new DriveService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = read_google_drive.ApplicationName
            //});

            //service.li



            //PeopleResource.GetRequest peopleRequest = service.People.Get("people/me");
            //peopleRequest.RequestMaskIncludeField = "person.names";
            //Person profile = peopleRequest.Execute();

            //return;


            if (false)
            {
                global_user_cat_json = new JObject();
            }
            else if (true)
            {
                //JArray categories_json = (JArray)((JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(categories_filename)));
                ////global_user_cat_json = (JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText("categories/user_categories.json"));
                ////////
                //handle_cats(categories_json, "cat");
                run_load_google_drive_info();
            }
            else
            {

            }

            if (false)
            {
                check_user_cat(((JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(categories_filename))));
            }

            MenuItem showDuplicatesMenuItem = new MenuItem("Show Duplicates");
            contextMenu.MenuItems.Add(showDuplicatesMenuItem);
            showDuplicatesMenuItem.Click += new EventHandler(menuItem_Click);
            //
            MenuItem excludeFolderMenuItem = new MenuItem("Exclude Folder");
            contextMenu.MenuItems.Add(excludeFolderMenuItem);
            excludeFolderMenuItem.Click += new EventHandler(menuItem_Click);
            //
            MenuItem deleteMenuItem = new MenuItem("Delete");
            contextMenu.MenuItems.Add(deleteMenuItem);
            deleteMenuItem.Click += new EventHandler(menuItem_Click);
            //
            MenuItem rescanMenuItem = new MenuItem("Rescan");
            contextMenu.MenuItems.Add(rescanMenuItem);
            rescanMenuItem.Click += new EventHandler(menuItem_Click);

            var debugLocation = categories_ChromiumWebBrowser.Location;

            if (!Cef.IsInitialized)
            {
                CefSettings settings = new CefSettings();
                settings.CefCommandLineArgs.Add("disable-gpu", "1");
                Cef.Initialize(settings);
            }


            categories_ChromiumWebBrowser.LoadingStateChanged += (sender2, args) =>
            {
                //Wait for the Page to finish loading
                if (args.IsLoading == false)
                {
                    categories_ChromiumWebBrowser.SetZoomLevel(float.Parse((string)properties_ht["cat_ZoomLevel"]));

                    JArray categories_json = (JArray)((JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(categories_filename)));
                    global_user_cat_json = (JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText("categories/user_categories.json"));
                    //
                    handle_cats(categories_json, "cat");
                }
            };
            //categoriesWebBrowser.ObjectForScripting = this;
            string test_tree_menue_path = $"file://{Directory.GetCurrentDirectory()}/browser/test_tree_menue.htm";
            categories_ChromiumWebBrowser.Load(test_tree_menue_path);

            //filterCategoriesWebBrowser.ObjectForScripting = this;
            //filterCategoriesWebBrowser.Navigate($"file://{cur_dir}/browser/test_tree_menue.htm");

            filter_ChromiumWebBrowser.LoadingStateChanged += (sender2, args) =>
            {
                //Wait for the Page to finish loading

                if (args.IsLoading == false)
                {
                    filter_ChromiumWebBrowser.SetZoomLevel(float.Parse((string)properties_ht["filter_ZoomLevel"]));

                    JArray categories_json = (JArray)((JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(categories_filename)));
                    ////global_user_cat_json = (JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText("categories/user_categories.json"));
                    ////
                    handle_cats(categories_json, "filter");
                    ////chromiumWebBrowser1.EvaluateScriptAsync("alert('page loaded');");
                }
            };
            filter_ChromiumWebBrowser.Load(test_tree_menue_path);

            //string[] fb_feeling_activity_json_lines = System.IO.File.ReadAllLines("fb_feeling_activity.json");
            //foreach (var line in fb_feeling_activity_json_lines)
            //{
            //    if (line.Contains("\"text\":"))
            //    {
            //        outputTextBox.AppendText(line + "\r\n"); // Regex.Match(line, ":\s+\"").Groups[0].Value
            //    }
            //}

        }

        const string categories_filename = @"categories/categories.json";

        class load_cat_tree_class
        {
            object wb;
            MainForm f;
            bool enable_edit, enable_files_checkbox, enable_folders_checkbox, use_trippleCheckbox, add_clear_buttton;
            string backgroundColor;
            JArray jo;

            public load_cat_tree_class(object _wb, MainForm _f, bool _enable_edit, bool _enable_files_checkbox, bool _enable_folders_checkbox, bool _use_trippleCheckbox, bool _add_clear_buttton, string _backgroundColor, JArray _jo)
            {
                wb = _wb;
                f = _f;
                enable_edit = _enable_edit;
                enable_files_checkbox = _enable_files_checkbox;
                enable_folders_checkbox = _enable_folders_checkbox;
                use_trippleCheckbox = _use_trippleCheckbox;
                add_clear_buttton = _add_clear_buttton;
                backgroundColor = _backgroundColor;
                jo = _jo;
            }

            public void chromium_cat_has_checked(string path, int _checked)
            {
                f.cat_has_checked(path, _checked);
            }

            public void chromium_cat_ctrl_plus_minus(double f)
            {
                ((ChromiumWebBrowser)wb).SetZoomLevel(((ChromiumWebBrowser)wb).GetZoomLevelAsync().Result + f);
            }

            //private void CefBrowser_KeyPress(object sender, KeyPressEventArgs e)
            //{

            //}
            private async System.Threading.Tasks.Task run_ChromiumWebBrowser_create_tree_menue(ChromiumWebBrowser wb, object[] args)
            {
                wb.JavascriptObjectRepository.Register("callbackObj", this, isAsync: true, options: BindingOptions.DefaultBinder);
                //
                wb.JavascriptObjectRepository.ObjectBoundInJavascript += (sender, e) =>
                {
                    var name = e.ObjectName;
                    int retry_count = 5;
                    while (!wb.CanExecuteJavascriptInMainFrame && retry_count-- > 0)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    if (retry_count == 0)
                    {
                        throw new Exception("Can't Execute Javascript In MainFrame");
                    }
                    System.Threading.Tasks.Task res_task =  load_cat_tree_class.run_ChromiumWebBrowser_create_tree_menue2(wb, args);
                    res_task.Wait(); // lilo
                };
                JavascriptResponse res = await wb.EvaluateScriptAsync(@"(async function() {await CefSharp.BindObjectAsync();})();"); // \"callbackObj\"
            }

            private static async System.Threading.Tasks.Task run_ChromiumWebBrowser_create_tree_menue2(ChromiumWebBrowser wb, object[] args)
            {
                JavascriptResponse res = await wb.EvaluateScriptAsync("create_tree_menue", args);
                if (!res.Success || !(bool)res.Result) throw new Exception("failed to run javascript in browser.");
            }

            public void load_cat_tree()
            {
                string json_str = jo.ToString();
                object[] args = { json_str, enable_edit, enable_files_checkbox, enable_folders_checkbox, use_trippleCheckbox, add_clear_buttton, backgroundColor };
                while (true)
                {
                    object ret = null;

                    int i;
                    for (i = 0; i < 5; i++)
                    {
                        try
                        {
                            bool res;
                            //f.Invoke(new MethodInvoker(delegate
                            //{
                                if (wb.GetType() == typeof(ChromiumWebBrowser))
                                {
                                    System.Threading.Tasks.Task re7 = this.run_ChromiumWebBrowser_create_tree_menue((ChromiumWebBrowser)wb, args);
                                    //ret = wb.GetMainFrame().EvaluateScriptAsync("create_tree_menue").Result.Message;(, args);
                                    //if (re7.)
                                    ret = true;
                                }
                                else
                                {
                                    ret = ((WebBrowser)wb).Document.InvokeScript("create_tree_menue", args);
                                }
                            //}));
                            break;
                        }
                        catch (Exception ex) { }
                        System.Threading.Thread.Sleep(400);
                    }
                    if (i == 5) throw new Exception("failed to InvokeScript: create_tree_menue");

                    if ((bool?)ret == true) break;
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }


        public void run_load_google_drive_info()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(load_google_drive_info));
            thread.IsBackground = true;
            thread.Start();
        }

        public static string ser_name = "serialized\\my-gd.ser";

        public static RootInfo global_ri = new RootInfo();

        public static JObject global_user_cat_json = null;

        void load_google_drive_info()
        {
            show_processing_image("loading google-drive info from the local-drive...");

            Invoke(new MethodInvoker(delegate
            {
                mainTabControl.SelectTab(0);
                treeViewTabControl.SelectTab(1);
            }));

            Stopwatch sw = new Stopwatch();

            sw.Start();
            global_ri = read_root_info_from_serialized_gz_file();
            if (global_ri == null)
            {
                show_processing_image();
                return;
            }
            sw.Stop();
            Console.WriteLine("it took: " + sw.Elapsed.ToString(@"hh\:mm\:ss") + " to Load the data from the serialized zipped file.");

            bool do_process_google_tree = false;
            if (do_process_google_tree)
            {
                show_processing_image("processing google tree...");
                rescan_tree_from_google();
            }

            /* */

            FolderInfo start_dir = global_ri.root;
            string start_dir_str = (string)properties_ht["start_dir"];
            if (start_dir_str != null)
            {
                string[] folders_path_ar = start_dir_str.Split('/');
                for (int i = 0; i < folders_path_ar.Length; i++)
                {
                    string folder_name = folders_path_ar[i];
                    bool found = false;
                    foreach (FolderInfo fi in start_dir.folders)
                    {
                        if (fi.file.Name == folder_name)
                        {
                            found = true;
                            start_dir = fi;
                            break;
                        }
                    }
                    if (!found) throw new Exception();
                }
            }

            one_level_expand_tv(null, start_dir);

            show_processing_image();

            Invoke(new MethodInvoker(delegate
            {
                treeViewTabControl.Controls[treeViewTabControl.SelectedIndex].Controls[0].Controls[0].Focus();
            }));

            return;

            /* */

            List<string> res2 = get_tree_public_GIds(global_ri);


            //re_scan_tree(global_ri);

            List<GoogleFolder> ffii10 = (List<GoogleFolder>)((Hashtable)global_ri.by_ht_name_ht["file_by_MimeType_ht"])["video/mp4"];



            List<GoogleFolder> ffii1 = (List<GoogleFolder>)((Hashtable)global_ri.by_ht_name_ht["file_by_Id_ht"])["1bYwTyLi9NuqLc5A62q022fJpp6TPIhwL"];
            Google.Apis.Drive.v3.Data.File.VideoMediaMetadataData file_vmd = ffii1[0].file.VideoMediaMetadata;
            string file_name = ffii1[0].file.Name;
            List<GoogleFolder> ffii2 = (List<GoogleFolder>)((Hashtable)global_ri.by_ht_name_ht["file_by_Name_ht"])[file_name];
            string file_name2 = ffii1[0].file.Name;
            if (file_name != ffii1[0].file.Name) throw new Exception("unexpected - should be similar. check it.");

            report_duplicate_Md5_files();

            /* */

            Google.Apis.Drive.v3.Data.File test_file = global_ri.root.folders[0].folders[0].file;
        }

        public static void rescan_tree_from_google()
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            update_tree(global_ri);
            sw.Stop();
            Console.WriteLine("it took: " + sw.Elapsed.ToString(@"hh\:mm\:ss") + " to Update the Hashes data.");
            //
            sw.Restart();
            my_utils.serialize_to_gz_file(global_ri);
            sw.Stop();
            Console.WriteLine("it took: " + sw.Elapsed.ToString(@"hh\:mm\:ss") + " to Save the data.");
        }

        public void show_processing_image(string processing_purpose = "")
        {
            bool is_processing = processing_purpose != "";
            Invoke(new MethodInvoker(delegate
            {
                //processingPictureBox.BringToFront();
                processing_purpose_TextBox.Text = processing_purpose;
                processingPictureBox.Visible = is_processing;
                splitContainer2.Enabled = !is_processing;
            }));
        }

        private static void update_tree(RootInfo _ri)
        {
            _ri.by_ht_name_ht["file_by_Md5_ht"] = new Hashtable();
            _ri.by_ht_name_ht["file_by_Id_ht"] = new Hashtable();
            _ri.by_ht_name_ht["file_by_Name_ht"] = new Hashtable();
            _ri.by_ht_name_ht["file_by_MimeType_ht"] = new Hashtable();
            //
            _ri.by_ht_name_ht["folder_by_Id_ht"] = new Hashtable();
            //
            //_ri.by_ht_name_ht["categories_ht"] = new Hashtable() { { "מצחיק", 1 }, { "שירה", 2 }, { "מוזיקה", 3 }, { "לא מומלץ", 4 } };
            //_ri.by_ht_name_ht["comments_by_Id_ht"] = new Hashtable();
            //
            _update_tree(_ri.root, my_utils.slash_char);
        }

        private static void _update_tree(FolderInfo folder_info, string path)
        {
            foreach (Google.Apis.Drive.v3.Data.File file in folder_info.files)
            {
                GoogleFolder google_file_folder = new GoogleFolder(file, path + file.Name);
                if (file.Md5Checksum == null) continue;
                my_utils.add_obj_to_ht(((Hashtable)global_ri.by_ht_name_ht["file_by_Md5_ht"]), file.Md5Checksum, google_file_folder);
                my_utils.add_obj_to_ht(((Hashtable)global_ri.by_ht_name_ht["file_by_Id_ht"]), file.Id, google_file_folder);
                my_utils.add_obj_to_ht((Hashtable)global_ri.by_ht_name_ht["file_by_Name_ht"], file.Name, google_file_folder);
                my_utils.add_obj_to_ht((Hashtable)global_ri.by_ht_name_ht["file_by_MimeType_ht"], file.MimeType, google_file_folder);
            }
            foreach (FolderInfo folder in folder_info.folders)
            {
                ((Hashtable)(global_ri.by_ht_name_ht["folder_by_Id_ht"])).Add(folder.file.Id, folder);
                my_utils.add_obj_to_ht((Hashtable)global_ri.by_ht_name_ht["file_by_Name_ht"], folder.file.Name, folder);
                folder.path = path;
                _update_tree(folder, path + folder.file.Name + my_utils.slash_char);
            }
        }

        private List<string> get_tree_public_GIds(RootInfo _ri)
        {
            List<string> res = new List<string>();
            Hashtable file_by_Id_ht = (Hashtable)_ri.by_ht_name_ht["file_by_Id_ht"];
            foreach (DictionaryEntry gd in file_by_Id_ht)
            {
                string gid = (string)gd.Key;

                foreach (GoogleFolder gf in (List<GoogleFolder>)file_by_Id_ht[gd.Key])
                {
                    if (gf.file.Permissions == null)
                    {
                        // debug.
                        continue;
                    }
                    foreach (Permission per in gf.file.Permissions)
                    {
                        if (per.EmailAddress == "aryeh.tuchfeld@gmail.com")
                        {
                            continue;
                        }
                        if ((new List<string>() { "relion.man@gmail.com", "yechezkelbr@gmail.com", "hana.tuchfeld@gmail.com", "yishaygoldman@gmail.com", "cashgold3@yahoo.com", "ofekkm@gmail.com", "wsite.man@gmail.com" }).Contains(per.EmailAddress))
                        {
                            continue;
                        }
                        if (per.Id == "anyoneWithLink" || per.Id == "anyone")
                        {
                            res.Add(my_utils.decode_heb(gf.path));
                            continue;
                        }
                        throw new Exception("undetected user with permissions. email: " + per.EmailAddress);
                    }
                }

            }
            res.Sort();
            return res;
        }

        private void report_duplicate_Md5_files()
        {
            int dups_count = 0;
            long dup_total_size = 0;
            Hashtable by_Md5_ht = (Hashtable)global_ri.by_ht_name_ht["file_by_Md5_ht"];
            //Hashtable _ht = new Hashtable();
            foreach (DictionaryEntry md5 in by_Md5_ht)
            {
                string md5_Key = (string)md5.Key;
                report_duplicate_Md5_files_helper(ref dups_count, ref dup_total_size, by_Md5_ht, md5_Key);
            }
            Console.WriteLine("dups_count: " + dups_count + " dup_total_size: " + my_utils.bytes_to_formatted_string(dup_total_size) + ".");
            //return _ht;
        }

        private static void report_duplicate_Md5_files_helper(ref int dups_count, ref long dup_total_size, Hashtable by_Md5_ht, string md5_Key)
        {
            List<GoogleFolder> files_list = (List<GoogleFolder>)by_Md5_ht[md5_Key];
            if (files_list.Count > 1)
            {
                dups_count++;
                long file_size = 0;
                List<string> paths = new List<string>();
                foreach (GoogleFolder google_file in files_list)
                {
                    paths.Add(my_utils.decode_heb(google_file.path));
                    if (file_size == 0)
                    {
                        file_size = (long)google_file.file.Size;
                    }
                    else if (file_size != google_file.file.Size)
                    {
                        throw new Exception("list of files doesn't have the same size!!. check this.");
                    }
                }
                //_ht.Add(md5.Key, new Hashtable() {
                //    {"file_size", file_size},
                //    {"paths", paths}
                //});
                dup_total_size += file_size;
                string files_str = string.Join("\t", paths);
                //Console.WriteLine($"{md5.Key} ({my_utils.bytes_to_formatted_string(file_size)}) " + files_str);
                Console.WriteLine($"{md5_Key}\t{file_size}\t" + files_str); // my_utils.bytes_to_formatted_string(file_size)
            }
        }

        private RootInfo read_root_info_from_serialized_gz_file()
        {
            string file_name = ser_name + ".gz";
            if (!System.IO.File.Exists(file_name))
            {
                MessageBox.Show("First You need to Scan the Drive.");
                return null;
            }
            FileStream file_read_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
            GZipStream decompressionStream = new GZipStream(file_read_stream, CompressionMode.Decompress);
            RootInfo ri = (RootInfo)(new BinaryFormatter()).Deserialize(decompressionStream);
            file_read_stream.Close();
            return ri;
        }

        private void scanGoogleDriveButton_Click(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                mainTabControl.SelectTab(3);
            }));
            System.Threading.Thread thread;
            thread = new System.Threading.Thread(new ThreadStart((new read_google_drive(this)).read_google_drive_info));
            thread.IsBackground = true;
            thread.Start();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            // https://docs.microsoft.com/en-us/windows/win32/wmp/axwmplib-axwindowsmediaplayer-playstatechange
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsPlaying)
            {

                show_processing_image();
                if (true && (bool)axWindowsMediaPlayer1.Tag)
                {
                    //System.Threading.Thread.Sleep(2000);
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                }
            }
        }

        private void play(object sender, EventArgs e)
        {
            Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)((List<object>)googleDriveTreeView.Tag)[1];
            if (f.Shared != true)
            {
                MessageBox.Show("the File is Not Shared. Ignoring.");
                return;
            }
            show_processing_image("Fetching Data to Play...");
            axWindowsMediaPlayer1.URL = f.WebContentLink; // http://10.0.0.26:4747/video
            axWindowsMediaPlayer1.Tag = (new List<string>() { "jpg", "gif", "png", "bmp" }).Contains(f.FileExtension);
        }

        private void shareButton_Click(object sender, EventArgs e)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(handle_shareButton_Click));
            thread.IsBackground = true;
            thread.Start();
        }

        private void handle_shareButton_Click()
        {
            object possibly_File_node = null;
            Invoke(new MethodInvoker(delegate
            {
                possibly_File_node = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
            }));
            if (!(possibly_File_node is Google.Apis.Drive.v3.Data.File))
            {
                MessageBox.Show("Only File can be Shared. Ignoring.");
                return;
            }
            Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)possibly_File_node;
            if (f.Shared != true)
            {
                show_processing_image("Sharing File...");

                DriveService service = get_service();

                Permission perm = new Permission();
                perm.Role = "reader";
                perm.Type = "anyone";
                var x = service.Permissions.Create(perm, f.Id); //  .List(
                var y = x.Execute();
                //
                do_update_file_in_tree(service, f);
            }

            //
            if (!play_after_share_CheckBox.Checked)
            {
                show_processing_image();
            }
            else
            {
                play(null, null);
            }
        }

        private void do_update_file_in_tree(DriveService service, Google.Apis.Drive.v3.Data.File f)
        {
            FilesResource.GetRequest getRequest = service.Files.Get(f.Id);
            getRequest.Fields = "*";
            Google.Apis.Drive.v3.Data.File updated_file = getRequest.Execute();


            // update file in tree:
            if (f.Id != updated_file.Id) throw new Exception();
            update_file_in_tree(updated_file);


            update_file_in_hts(f.Name, updated_file);

            // need to rescan the tree..

            //f.Shared = true;
            //List<GoogleFolder> ffii1 = (List<GoogleFolder>)((Hashtable)global_ri.by_ht_name_ht["file_by_Id_ht"])[f.Id];
            //if (ffii1.Count != 1) throw new Exception();
            //GoogleFolder gff = ffii1[0];
            //Google.Apis.Drive.v3.Data.File fff = gff.file; //.Shared = true;


            //if (currenf_file.Id != f.Id) throw new Exception();
            //replace_file_in_tree(currenf_file, "file_by_Id_ht", f.Id);
            //replace_file_in_tree(currenf_file, "file_by_Md5_ht", f.Md5Checksum);
            //replace_file_in_tree(currenf_file, "file_by_Name_ht", f.Name);
            //replace_file_in_tree(currenf_file, "file_by_MimeType_ht", f.MimeType);

            //foreach (var key in global_ri.by_ht_name_ht)
            //{
            //    (Hashtable)(global_ri.by_ht_name_ht[key])[]
            //}

            my_utils.serialize_to_gz_file(global_ri);

            //string name = Encoding.ASCII.GetString(Encoding.Default.GetBytes(updated_file.Name));
            updated_file.Name = f.Name;
            ((List<object>)googleDriveTreeView.Tag)[1] = updated_file;
            Invoke(new MethodInvoker(delegate
            {
                ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1] = updated_file;
                handle_googleDriveTreeView_Node_selected(googleDriveTreeView.SelectedNode); // refresh file details.
            }));
        }

        private static DriveService get_service()
        {
            DriveService service;
            UserCredential credential = read_google_drive.get_credential();
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = read_google_drive.ApplicationName,
            });
            return service;
        }

        private void unShareButton_Click(object sender, EventArgs e)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(handle_unShareButton_Click));
            thread.IsBackground = true;
            thread.Start();
        }

        private void handle_unShareButton_Click()
        {
            object possibly_File_node = null;
            Invoke(new MethodInvoker(delegate
            {
                possibly_File_node = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
            }));
            if (!(possibly_File_node is Google.Apis.Drive.v3.Data.File))
            {
                MessageBox.Show("Only File can be UnShared. Ignoring.");
                return;
            }
            Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)possibly_File_node;
            //                
            if (f.Shared != true)
            {
                MessageBox.Show("the File is Not Shared. Ignoring.");
                return;
            }
            show_processing_image("UnSharing File...");
            DriveService service = get_service();
            int n_deleted = 0;
        a:
            PermissionsResource.ListRequest l = service.Permissions.List(f.Id);
            PermissionList y = l.Execute();
            foreach (Permission p in y.Permissions)
            {
                if (p.Type == "anyone")
                {
                    PermissionsResource.DeleteRequest dr = service.Permissions.Delete(f.Id, "anyoneWithLink");
                    n_deleted++;
                    bool success = false;
                    try
                    {
                        dr.Execute();
                        success = true;
                    }
                    catch (Exception e2) { }; // lilo: actually it works..
                    goto a;
                }
            }
            if (n_deleted != 1) throw new Exception();
            //
            do_update_file_in_tree(service, f);
            update_tree(global_ri);
            //
            show_processing_image();
        }

        private void update_file_in_tree(Google.Apis.Drive.v3.Data.File updated_file)
        {
            string file_path = ((GoogleFolder)((List<object>)((Hashtable)(global_ri.by_ht_name_ht["file_by_Id_ht"]))[updated_file.Id])[0]).path;
            FolderInfo fff9 = global_ri.root;
            string[] ddd9 = file_path.Substring(1).Split('/');
            bool found;
            for (int i11 = 0; i11 < ddd9.Length - 1; i11++)
            {
                found = false;
                for (int j9 = 0; j9 < fff9.folders.Count; j9++)
                {
                    if (fff9.folders[j9].file.Name == ddd9[i11])
                    {
                        fff9 = fff9.folders[j9];
                        found = true;
                        break;
                    }
                }
                if (!found) throw new Exception();
            }
            List<Google.Apis.Drive.v3.Data.File> fi99 = fff9.files;
            found = false;
            for (int k9 = 0; k9 < fi99.Count; k9++)
            {
                if (fi99[k9].Name == ddd9[ddd9.Length - 1])
                {
                    // fi99[k9] = updated_file;
                    fff9.update_folder_file(updated_file, k9);
                    //fff9.recache_files();
                    found = true;
                    break;
                }
            }
            if (!found) throw new Exception();

            Invoke(new MethodInvoker(delegate
            {
                mark_treenode(googleDriveTreeView.SelectedNode, updated_file.Shared == true, updated_file.OriginalFilename != updated_file.Name);
            }));
        }

        private static void unused_replace_file_in_tree(Google.Apis.Drive.v3.Data.File currenf_file, string ht_name_key, string ht_field_value)
        {
            List<GoogleFolder> list_of_GoogleFolder = (List<GoogleFolder>)((Hashtable)(global_ri.by_ht_name_ht[ht_name_key]))[ht_field_value];
            bool found = false;
            for (int i = 0; i < list_of_GoogleFolder.Count; i++)
            {
                if (list_of_GoogleFolder[i].file.Id == currenf_file.Id)
                {
                    if (found) throw new Exception("already found.");
                    found = true;
                    GoogleFolder googleFolder = new GoogleFolder(currenf_file, list_of_GoogleFolder[i].path);
                    list_of_GoogleFolder[i] = googleFolder;
                }
            }
            if (!found) throw new Exception("not found.");
        }

        // https://stackoverflow.com/questions/21198668/treenode-selected-backcolor-while-treeview-not-focused
        // Observe that this only works if HideSelection is True(the default)!
        public TreeNode previousSelectedNode = null;
        Color previouse_ForeColor;
        Color previouse_BackColor;
        private void googleDriveTreeView_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (googleDriveTreeView.SelectedNode == null) return;
            previouse_ForeColor = googleDriveTreeView.SelectedNode.ForeColor;
            previouse_BackColor = googleDriveTreeView.SelectedNode.BackColor;
            previousSelectedNode = googleDriveTreeView.SelectedNode;
            googleDriveTreeView.SelectedNode.BackColor = SystemColors.Highlight;
            googleDriveTreeView.SelectedNode.ForeColor = Color.White;
        }

        private void googleDriveTreeView_NodeMouseClick(object sender, TreeViewEventArgs e)
        {
            handle_googleDriveTreeView_Node_selected(e.Node, null, true);
            if (previousSelectedNode != null)
            {
                if (previousSelectedNode.BackColor == SystemColors.Highlight)
                {
                    previousSelectedNode.BackColor = previouse_BackColor;
                }
                if (previousSelectedNode.ForeColor == Color.White)
                {
                    previousSelectedNode.ForeColor = previouse_ForeColor;
                }
            }
            if (!googleDriveTreeView.Focused)
            {
                googleDriveTreeView_Validating(sender, null);
                //googleDriveTreeView.SelectedNode.BackColor = SystemColors.Highlight;
                //googleDriveTreeView.SelectedNode.ForeColor = Color.White;
            }
            googleDriveTreeView.SelectedNode = e.Node;
            //if (click_to_play_CheckBox.Checked)
            //{
            //    if (false && ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1] is Google.Apis.Drive.v3.Data.File)
            //    {
            //        //sahreAndPalyButton.Click()...; // todo
            //        shareButton_Click(sender, e);
            //    }
            //}
        }

        private void getLinkButton_Click(object sender, EventArgs e)
        {
            Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)(((List<object>)googleDriveTreeView.SelectedNode.Tag)[1]);
            if (f.Shared != true)
            {
                MessageBox.Show("The File is not Shared. You need to Share it first.");
                return;
            }
            Clipboard.SetText(f.WebViewLink.Substring(0, f.WebViewLink.IndexOf("?")));
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            processingPictureBox.Location = new System.Drawing.Point(this.Size.Width / 2 - processingPictureBox.Size.Width / 2, this.Size.Height / 2 - processingPictureBox.Size.Height / 2);
        }

        private void dupButton_Click(object sender, EventArgs e)
        {
            FolderInfo fi = null;
            switch ((string)dupsFolderComboBox.SelectedItem)
            {
                case "Root":
                    fi = global_ri.root;
                    break;
                case "Selected Node":
                    if (googleDriveTreeView.SelectedNode == null)
                    {
                        MessageBox.Show("Please select a Node.");
                        return;
                    }
                    object o = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
                    if (o is Google.Apis.Drive.v3.Data.File)
                    {
                        MessageBox.Show("only Folders can be selected for duplication analysis.");
                        return;
                    }
                    fi = (FolderInfo)o;
                    break;
                default:
                    MessageBox.Show("Please select a Root Type.");
                    return;
            }

            int min_size_bytes = int.Parse(minTextBox.Text);

            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(new dup_thread_under_folder(this, fi, min_size_bytes).run));
            thread.IsBackground = true;
            thread.Start();
        }

        protected class dup_thread_under_folder
        {
            MainForm form;
            FolderInfo fi;
            int min_size_bytes;

            public dup_thread_under_folder(MainForm _form, FolderInfo _fi, int _min_size_bytes = 1)
            {
                form = _form;
                fi = _fi;
                min_size_bytes = _min_size_bytes;
            }

            public void run()
            {
                string title;

                form.Invoke(new MethodInvoker(delegate
                {
                    string name = fi.file == null ? "" : my_utils.decode_heb(fi.file.Name);
                    title = "dups for folder: " + fi.path + name;
                    //form.tabControl1.SelectTab(0);
                    form.analysisResultsTextBox.Text = title + "\r\n";
                    form.show_processing_image(title);
                }));

                long total_dup_size = 0;
                int n_dups = 0;
                form.scan_folder_files_for_dups(fi, min_size_bytes, ref n_dups, ref total_dup_size);

                form.Invoke(new MethodInvoker(delegate
                {
                    form.analysisResultsTextBox.AppendText($"Found: {n_dups} duplicated files, Taking: {my_utils.bytes_to_formatted_string(total_dup_size)} bytes.");
                }));

                form.show_processing_image();
            }
        }

        void scan_folder_files_for_dups(FolderInfo fi, int min_size_bytes, ref int n_dups, ref long total_dup_size)
        {
            Hashtable _ht = new Hashtable();
            foreach (Google.Apis.Drive.v3.Data.File f in fi.files)
            {
                if (_ht.Contains(f.Id))
                {
                    continue; // already handled.
                }

                string[] dup_files;
                bool already_shared_by_other_copy;
                //
                GoogleFolder gf = get_GoogleFolder_from_Googlefile(f);
                //
                if (get_dup_files(gf, f, out dup_files, out already_shared_by_other_copy))
                {
                    if (gf.file.Size < min_size_bytes)
                    {
                        continue;
                    }
                    Invoke(new MethodInvoker(delegate
                    {
                        analysisResultsTextBox.AppendText(my_utils.decode_heb(gf.path) + "\t" + ((bool)gf.file.Shared ? "" : "Not ") + "Shared" + "\t" + "" + "\t" + gf.file.Size + "\t" + my_utils.bytes_to_formatted_string((long)f.Size)); //  + "\t" + already_shared_by_other_copy
                    }));
                    total_dup_size += (long)gf.file.Size;
                    n_dups++;
                    foreach (string file_id in dup_files)
                    {
                        List<object> _fi2 = (List<object>)((Hashtable)(global_ri.by_ht_name_ht["file_by_Id_ht"]))[file_id];
                        if (_fi2.Count > 1)
                        {
                            throw new Exception();
                        }
                        GoogleFolder gf2 = (GoogleFolder)_fi2[0];

                        if (gf.file.Id == gf2.file.Id)
                        {
                            continue; // throw new Exception();
                        }

                        if (_ht.Contains(gf2.file.Id))
                        {
                            continue; // already handled.
                        }

                        bool is_same_file = gf.path == gf2.path;

                        Invoke(new MethodInvoker(delegate
                        {
                            analysisResultsTextBox.AppendText("\t" + (is_same_file ? $"same file {gf2.file.Id}" : my_utils.decode_heb(gf2.path)) + "\t" + ((bool)gf2.file.Shared ? "" : "Not ") + "Shared");
                        }));
                        _ht.Add(gf2.file.Id, null);
                    }
                    Invoke(new MethodInvoker(delegate
                    {
                        analysisResultsTextBox.AppendText("\r\n");
                    }));
                }
            }
            foreach (FolderInfo f in fi.folders)
            {
                scan_folder_files_for_dups(f, min_size_bytes, ref n_dups, ref total_dup_size);
            }
        }

        private static GoogleFolder get_GoogleFolder_from_Googlefile(Google.Apis.Drive.v3.Data.File f)
        {
            List<object> _fi = (List<object>)((Hashtable)(global_ri.by_ht_name_ht["file_by_Id_ht"]))[f.Id];
            if (_fi.Count > 1)
            {
                throw new Exception();
            }
            return (GoogleFolder)_fi[0];
        }

        private void button_do_analyse_different_video_files(object sender, EventArgs e)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(do_analyse_different_video_files));
            thread.IsBackground = true;
            thread.Start();
        }

        private void do_analyse_different_video_files()
        {
            show_processing_image("Analysing different_video_files...");

            Hashtable file_by_Md5_ht = (Hashtable)(global_ri.by_ht_name_ht["file_by_Md5_ht"]);
            int n_dups_mp4 = 0;
            long dups_mp4_bytes = 0;
            int total_mp4_n = 0;
            long? total_mp4_ms = 0;
            long total_mp4_bytes = 0;
            foreach (DictionaryEntry de in file_by_Md5_ht)
            {
                List<object> google_folder_list = (List<object>)de.Value;
                Google.Apis.Drive.v3.Data.File google_file = ((GoogleFolder)google_folder_list[0]).file;
                if (google_file.MimeType.StartsWith("video/") && google_file.VideoMediaMetadata != null)
                {
                    total_mp4_bytes += (long)google_file.Size;
                    //
                    int n_files_in_tree = google_folder_list.Count;
                    if (n_files_in_tree > 1)
                    {
                        n_dups_mp4 += (n_files_in_tree - 1);
                        dups_mp4_bytes += ((long)google_file.Size * (n_files_in_tree - 1));
                    }
                    //
                    total_mp4_n++;
                    total_mp4_ms += google_file.VideoMediaMetadata.DurationMillis;
                }
            }

            Invoke(new MethodInvoker(delegate
            {
                analysisResultsTextBox.Text = $"n_different_video_files: {total_mp4_n}\r\navg. time: {(new TimeSpan((long)total_mp4_ms * 10000 / total_mp4_n)).ToString(@"mm\:ss")}\r\navg. size: {my_utils.bytes_to_formatted_string(total_mp4_bytes / total_mp4_n)}\r\nn_dups_mp4: {n_dups_mp4}\r\ndups_mp4_bytes: {my_utils.bytes_to_formatted_string(dups_mp4_bytes)}";
            }));

            show_processing_image();
        }

        public void cat_has_changed(string json_str)
        {
            //MessageBox.Show("Called from javascript!!! " + json_str);
            JArray json = JArray.Parse(json_str);
            System.IO.File.WriteAllText(categories_filename, json.ToString());
        }

        //const string user_categories_filename = @"categories/user_categories.json";
        Hashtable filterCategories_ht = new Hashtable();
        Hashtable filterOutCategories_ht = new Hashtable();

        public void cat_tree_was_cleared()
        {
            filterCategories_ht.Clear();
        }

        public void cat_has_checked(string path, int _checked)
        {
            Invoke(new MethodInvoker(delegate
            {
            if (categories_ChromiumWebBrowser.Focused) // googleDriveTreeView.Focused || 
            {
                object file_or_folder = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
                if (file_or_folder is Google.Apis.Drive.v3.Data.File)
                {
                    if (googleDriveTreeView.SelectedNode == null)
                    {
                        MessageBox.Show("נא לבחור קודם קובץ");
                        return;
                    }
                    Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)file_or_folder;
                    if (global_user_cat_json[f.Md5Checksum] == null)
                    {
                        global_user_cat_json[f.Md5Checksum] = new JObject();
                    }
                    ((JObject)global_user_cat_json[f.Md5Checksum])[path] = _checked;
                    var x = JsonConvert.SerializeObject(global_user_cat_json);
                    System.IO.File.Delete("categories/user_categories.json.bak");
                    System.IO.File.Copy("categories/user_categories.json", "categories/user_categories.json.bak");
                    System.IO.File.WriteAllText("categories/user_categories.json", x);
                    //
                    handle_file_cat_changed(f, googleDriveTreeView.Nodes);
                }
            }
            else if (filter_ChromiumWebBrowser.Focused) // filterCategoriesWebBrowser.Focused
            {
                if (_checked == 1)
                {
                    filterOutCategories_ht.Remove(path);
                    filterCategories_ht.Add(path, null);
                }
                else if (_checked == 2)
                {
                    filterCategories_ht.Remove(path);
                    filterOutCategories_ht.Add(path, null);
                }
                else
                {
                    filterCategories_ht.Remove(path);
                    filterOutCategories_ht.Remove(path);
                }
            }
            else
            {
                who_is_focused(this.Controls);
                throw new Exception();
            }
            }));
        }

        void handle_file_cat_changed(Google.Apis.Drive.v3.Data.File f, TreeNodeCollection nc)
        {
            foreach (TreeNode n in nc)
            {
                object o = ((List<object>)n.Tag)[1];
                if (o is Google.Apis.Drive.v3.Data.File)
                {
                    Google.Apis.Drive.v3.Data.File _f = (Google.Apis.Drive.v3.Data.File)o;
                    if (_f.Md5Checksum == f.Md5Checksum)
                    {
                        n.BackColor = didnt_like(f) ? Color.DarkRed : Color.LightBlue; // f.Shared == true
                    }
                }
                handle_file_cat_changed(f, n.Nodes);
            }
        }


        private void who_is_focused(Control.ControlCollection cc)
        {
            foreach (Control c in cc) // this.Controls
            {
                if (c.Focused)
                {
                    throw new Exception("focused Control not handled: " + c);
                }
                who_is_focused(c.Controls);
            }
        }

        //private void testButton_Click(object sender, EventArgs e)
        //{
        //    object[] json = { "[ \"f0\", \"f1\", { \"dname\": \"d1\", \"content\": [\"f11\", \"f12\", { \"dname\": \"d2\", \"content\": [\"f21\", \"f22\"] }] }, \"f2\", \"f3\"]" };
        //    categoriesWebBrowser.Document.InvokeScript("create_tree_menue", json);
        //}

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13) // Enter
            {
                return;
            }
            do_search();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            do_search();
        }

        private void do_search()
        {
            System.Threading.Thread _do_search_thread = new System.Threading.Thread(new ThreadStart(do_search_thread));
            _do_search_thread.IsBackground = true;
            _do_search_thread.Start();
        }

        private void do_search_thread()
        {
            show_processing_image("searching...");
            bool search_folders = foldersCheckBox.Checked;
            bool search_files = filesCheckBox.Checked;
            string files_ext = null;
            Invoke(new MethodInvoker(delegate
            {
                files_ext = (string)filesExtComboBox.SelectedItem;
                searchListView.Items.Clear();
                searchResultsTextBox.Text = "";
            }));

            int n_results = 0;
            if (!SearchGIdCheckBox.Checked)
            {
                string search_str = searchTextBox.Text.ToLower().Trim();
                Hashtable _ht = (Hashtable)global_ri.by_ht_name_ht["file_by_Name_ht"];
                foreach (var fname in _ht.Keys)
                {
                    if ((my_utils.decode_heb((string)fname)).ToLower().Contains(search_str) && (files_ext == null || Regex.IsMatch((string)fname, $"\\.({files_ext})$")))
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            List<object> google_folder_list = (List<object>)_ht[fname];
                            foreach (object file_or_folder in google_folder_list)
                            {
                                bool found = false;
                                bool is_folder = file_or_folder is FolderInfo;
                                if (is_folder)
                                {
                                    if (!search_folders) continue;
                                    found = true;
                                }
                                else // File
                            {
                                    if (!search_files)
                                    {
                                        continue;
                                    }
                                    if (olnySharedCheckBox.Checked && !(bool)((GoogleFolder)file_or_folder).file.Shared)
                                    {
                                        continue;
                                    }

                                    found = false;

                                    if (filterOutCategories_ht.Count == 0 && filterCategories_ht.Count == 0) // !byFilterCheckBox.Checked)
                                {
                                        found = true;
                                    }
                                    else
                                    {
                                        JObject jo = (JObject)global_user_cat_json[((GoogleFolder)file_or_folder).file.Md5Checksum];
                                        if (jo == null)
                                        {
                                            continue;
                                        }

                                        if (found_cat(jo, filterOutCategories_ht, false))
                                        {
                                            continue;
                                        }

                                        found = found_cat(jo, filterCategories_ht, categoriesByAndOrCheckBox.Checked);
                                        if (categoriesByAndOrCheckBox.Checked && !found)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (!found)
                                {
                                    continue;
                                }
                                n_results++;
                                searchResultsTextBox.Text = "Found " + n_results + " results.";
                                string res_line_txt = (is_folder ? "(dir) " + my_utils.decode_heb(((FolderInfo)file_or_folder).path + ((FolderInfo)file_or_folder).file.Name) : my_utils.decode_heb(((GoogleFolder)file_or_folder).path)) + (is_folder ? $" ({my_utils.bytes_to_formatted_string(((FolderInfo)file_or_folder).total_size_bytes)})" : "");
                                string[] res_line_ar = new string[4];
                                res_line_ar[0] = is_folder ? my_utils.decode_heb(((FolderInfo)file_or_folder).path + ((FolderInfo)file_or_folder).file.Name) : get_path(my_utils.decode_heb(((GoogleFolder)file_or_folder).path));
                                res_line_ar[1] = is_folder ? "" : get_file(my_utils.decode_heb(((GoogleFolder)file_or_folder).path));
                                if (res_line_ar[1] == "")
                                { // lilo
                                res_line_ar[1] = is_folder ? "" : my_utils.decode_heb(((GoogleFolder)file_or_folder).file.Name);
                                }
                                res_line_ar[2] = is_folder ? $"{my_utils.bytes_to_formatted_string(((FolderInfo)file_or_folder).total_size_bytes)}" : my_utils.bytes_to_formatted_string((long)((GoogleFolder)file_or_folder).file.Size);
                                ListViewItem lvi = new ListViewItem(res_line_ar);
                                if (is_folder) lvi.BackColor = Color.Orange;
                                if (file_or_folder is FolderInfo && ((FolderInfo)file_or_folder).file.Shared == true || file_or_folder is GoogleFolder && ((GoogleFolder)file_or_folder).file.Shared == true)
                                {
                                    lvi.ForeColor = Color.Green;
                                }
                                lvi.Tag = file_or_folder;
                                searchListView.Items.Add(lvi);
                            }
                        }));
                    }
                }
            }
            else if (SearchGIdCheckBox.Checked)
            {
                List<object> o = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_Id_ht"])[searchTextBox.Text.Trim()];
                n_results = o.Count;
                if (o.Count > 0)
                {
                    GoogleFolder gf = (GoogleFolder)o[0];
                    // lilo: duplicated code..
                    string[] res_line_ar = new string[4];
                    res_line_ar[0] = get_path(my_utils.decode_heb((gf).path));
                    res_line_ar[1] = my_utils.decode_heb((gf).file.Name);
                    res_line_ar[2] = my_utils.bytes_to_formatted_string((long)(gf).file.Size);
                    ListViewItem lvi = new ListViewItem(res_line_ar);
                    if ((gf).file.Shared == true)
                    {
                        lvi.ForeColor = Color.Green;
                    }
                    lvi.Tag = gf;
                    Invoke(new MethodInvoker(delegate
                    {
                        searchListView.Items.Add(lvi);
                    }));
                }
            }
            Invoke(new MethodInvoker(delegate
            {
                searchResultsTextBox.Text = "Found " + n_results + " results.";
            }));
            show_processing_image();
        }

        private bool found_cat(JObject jo, Hashtable __ht, bool and)
        {
            int k_n = 0;
            bool found = false;
            foreach (string k in __ht.Keys)
            {
                k_n++;
                IEnumerator<KeyValuePair<string, JToken>> e = jo.GetEnumerator();
                while (e.MoveNext())
                {
                    string kk = e.Current.Key;
                    if (kk.StartsWith(k) && (bool)jo[kk] == true) //  Regex.IsMatch(kk, k)
                    {
                        found = true;
                        break;
                    }
                }
                if (and)
                {
                    if (!found)
                    {
                        break;
                    }
                    found = k_n == __ht.Keys.Count;
                }
            }

            return found;
        }

        string get_path(string full_path)
        {
            return full_path.Substring(0, full_path.LastIndexOf("/"));
        }
        string get_file(string full_path)
        {
            return full_path.Substring(full_path.LastIndexOf('/') + 1);
        }

        private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            if (googleDriveTreeView.SelectedNode == null)
            {
                return;
            }
            object possibly_File_node = null;
            Invoke(new MethodInvoker(delegate
            {
                possibly_File_node = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
            }));
            string name;
            string description;
            if ((possibly_File_node is Google.Apis.Drive.v3.Data.File))
            {
                name = ((Google.Apis.Drive.v3.Data.File)possibly_File_node).Name;
                description = ((Google.Apis.Drive.v3.Data.File)possibly_File_node).Description;

            }
            else if (possibly_File_node is FolderInfo)
            {
                name = (((FolderInfo)possibly_File_node).file).Name;
                description = (((FolderInfo)possibly_File_node).file).Description;

            }
            else
            {
                MessageBox.Show("Only File can be Shared. Ignoring.");
                return;
            }
            saveButton.Enabled = titleTextBox.Text != my_utils.decode_heb(name) && titleTextBox.Text.Length > 2 || descriptionTextBox.Text != my_utils.decode_heb(description);
        }

        const string regex1 = @"^קול ([\d]+)_sd([\-]+[\d]+).*$";
        const string regex2 = @"^קול ([\d]+).*$";

        private void clearButton_Click(object sender, EventArgs e)
        {
            string t = "";
            if (googleDriveTreeView.SelectedNode != null)
            {
                string x = googleDriveTreeView.SelectedNode.Text; // "קול 466.m4a"
                if (Regex.IsMatch(x, regex1))
                {
                    t = Regex.Replace(x, regex1, "$1$2") + " ";
                }
                else if (Regex.IsMatch(x, regex2))
                {
                    t = Regex.Replace(x, regex2, "$1") + " ";
                }
            }
            titleTextBox.Text = t;
            titleTextBox.Focus();
            titleTextBox.SelectionStart = titleTextBox.Text.Length;
            titleTextBox.SelectionLength = 0;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            System.Threading.Thread read_google_drive_thread = new System.Threading.Thread(new ThreadStart(do_update_name_and_details));
            read_google_drive_thread.IsBackground = true;
            read_google_drive_thread.Start();
        }

        private void do_update_name_and_details()
        {
            string process_title = "Saving Name and Details...";
            show_processing_image(process_title);

            object possibly_File_node = null;
            Invoke(new MethodInvoker(delegate
            {
                possibly_File_node = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
            }));
            if (!(possibly_File_node is Google.Apis.Drive.v3.Data.File))
            {
                throw new Exception();

            }

            string Id = ((Google.Apis.Drive.v3.Data.File)possibly_File_node).Id;
            Hashtable id_ht = ((Hashtable)(global_ri.by_ht_name_ht["file_by_Id_ht"]));
            List<object> fi = (List<object>)id_ht[Id];
            if (fi.Count != 1)
            {
                throw new Exception();
            }
            GoogleFolder old_f = (GoogleFolder)fi[0];
            string old_f_name = my_utils.decode_heb(old_f.file.Name);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            string path = old_f.path;
            path = path.Substring(0, path.LastIndexOf('/') + 1);

            Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            file.Name = titleTextBox.Text; // my_utils.encode_heb(titleTextBox.Text); 
            file.Description = descriptionTextBox.Text; // my_utils.encode_heb(descriptionTextBox.Text);
            DriveService service = get_service();
            FilesResource.UpdateRequest request = service.Files.Update(file, Id);
            request.Fields = "*";
            Google.Apis.Drive.v3.Data.File updated_file = request.Execute();
            //
            Invoke(new MethodInvoker(delegate
            {
                traceTextBox.AppendText("updating file name and description on google-drive took: " + sw.Elapsed.ToString(@"mm\:ss"));
            }));

            sw.Restart();
            //
            FolderInfo _1fi;
            _1fi = (FolderInfo)get_folder_info_from_path(path, old_f_name, updated_file);

            Invoke(new MethodInvoker(delegate
            {
                traceTextBox.AppendText("\r\nfind the path took: " + sw.Elapsed.ToString(@"mm\:ss"));
            }));
            sw.Restart();

            // update_tree(global_ri);
            //
            update_file_in_hts(old_f.file.Name, updated_file);

            Invoke(new MethodInvoker(delegate
            {
                traceTextBox.AppendText("\r\nupdate_tree took: " + sw.Elapsed.ToString(@"mm\:ss"));
            }));
            sw.Restart();

            my_utils.serialize_to_gz_file(global_ri);
            //
            Invoke(new MethodInvoker(delegate
            {
                traceTextBox.AppendText("\r\nserialize_to_gz_file took: " + sw.Elapsed.ToString(@"mm\:ss"));
                googleDriveTreeView.SelectedNode.Text = file.Name;
                googleDriveTreeView.SelectedNode.ForeColor = Color.Blue;
                ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1] = updated_file;
                ((List<object>)googleDriveTreeView.SelectedNode.Tag).Add(true); // special case of decoding
                //one_level_expand_tv(googleDriveTreeView.SelectedNode, _1fi, true);
                handle_googleDriveTreeView_Node_selected(googleDriveTreeView.SelectedNode, updated_file, false);
            }));

            show_processing_image();
        }

        private static void update_file_in_hts(string old_file_name, Google.Apis.Drive.v3.Data.File updated_file)
        {
            string old_path = ((GoogleFolder)((List<object>)((Hashtable)(global_ri.by_ht_name_ht["file_by_Id_ht"]))[updated_file.Id])[0]).path;
            //string x = Encoding.Unicode.GetString(Encoding.Default.GetBytes(updated_file.Name));
            //string y = Encoding.Default.GetString(Encoding.Unicode.GetBytes(x));
            string path = old_path.Substring(0, old_path.LastIndexOf("/")) + "/" + my_utils.encode_heb(updated_file.Name);
            //string path = old_path.Substring(0, old_path.LastIndexOf("/")) + "/" + updated_file.Name;
            //string z = my_utils.decode_heb(Encoding.Default.GetString(Encoding.Unicode.GetBytes(updated_file.Name)));
            GoogleFolder update_google_folder = new GoogleFolder(updated_file, path);
            List<object> fl = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_Md5_ht"])[updated_file.Md5Checksum];
            update_ht_list(fl, updated_file.Id, update_google_folder);
            fl = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_Id_ht"])[updated_file.Id];
            update_ht_list(fl, updated_file.Id, update_google_folder);
            Hashtable _ht7 = (Hashtable)global_ri.by_ht_name_ht["file_by_Name_ht"];
            fl = (List<object>)(_ht7)[my_utils.decode_heb(old_file_name)];
            if (fl == null)
            {
                fl = (List<object>)(_ht7)[old_file_name];
            }
            if (fl == null)
            {
                throw new Exception("check...");
                fl = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_Name_ht"])[my_utils.decode_heb(updated_file.Name)];
            }

            update_ht_list(fl, updated_file.Id, null, true);
            fl = (List<object>)_ht7[updated_file.Name];
            if (fl == null)
            {
                fl = new List<object>();
                _ht7[updated_file.Name] = fl;
            }
            fl.Add(update_google_folder);
            fl = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_MimeType_ht"])[updated_file.MimeType];
            update_ht_list(fl, updated_file.Id, update_google_folder);
        }

        private static object get_folder_info_from_path(string path, string f_name, Google.Apis.Drive.v3.Data.File updated_file)
        {
            FolderInfo _1fi;
            string[] path_items = path.Trim('/').Split('/');
            _1fi = global_ri.root;
            bool found = false;
            for (int i = 0; i < path_items.Length; i++)
            {
                string path_item = path_items[i];
                found = false;
                for (int k = 0; k < _1fi.folders.Count; k++)
                {
                    FolderInfo _2fi = _1fi.folders[k];
                    string fname = my_utils.decode_heb(_2fi.file.Name);
                    if (fname == path_item || fname == my_utils.decode_heb(path_item)) // lilo
                    {
                        found = true;
                        _1fi = _2fi;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception();
                }
            }

            if (!string.IsNullOrEmpty(f_name))
            {
                found = false;
                List<Google.Apis.Drive.v3.Data.File> files = _1fi.files; // must for performance!
                for (int j = 0; j < files.Count; j++)
                {
                    Google.Apis.Drive.v3.Data.File _3fi = files[j];
                    if (_3fi.Name == f_name || my_utils.decode_heb(_3fi.Name) == f_name) // lilo
                    {
                        if (updated_file == null)
                        {
                            return _3fi;
                        }
                        _1fi.update_folder_file(updated_file, j);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception();
                }
            }
            return _1fi;
        }

        private static void update_ht_list(List<object> files_list, string updated_file_id, GoogleFolder update_google_folder, bool remove = false)
        {
            if (files_list.Count == 0)
            {
                throw new Exception("a non empty list was expected.");
            }
            bool found2 = false;
            for (int i = 0; i < files_list.Count; i++)
            {
                if (((GoogleFolder)files_list[i]).file.Id == updated_file_id)
                {
                    if (remove)
                    {
                        files_list.RemoveAt(i);
                    }
                    else
                    {
                        files_list[i] = update_google_folder;
                    }
                    found2 = true;
                    break;
                }

            }
            if (!found2)
            {
                throw new Exception("not found.");
            }
        }

        private void titleTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && saveButton.Enabled)
            {
                saveButton_Click(sender, e);
                e.SuppressKeyPress = true;
                //e.Handled = true;
            }
        }

        private void searchListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //locate_search_result();
            //return;
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            System.Threading.Thread read_google_drive_thread = new System.Threading.Thread(locate_search_result);
            read_google_drive_thread.IsBackground = true;
            read_google_drive_thread.Start();
        }

        void locate_search_result()
        {
            string path = null;
            string name = null;
            Invoke(new MethodInvoker(delegate
            {
                path = searchListView.SelectedItems[0].SubItems[0].Text.Trim('/');
                name = searchListView.SelectedItems[0].SubItems[1].Text;
            }));

            show_processing_image("Locating Search Result...");
            Invoke(new MethodInvoker(delegate
            {
                mainTabControl.SelectTab(0);
                treeViewTabControl.SelectTab(1);
                //googleDriveTreeView.Focus();
            }));
            int i = path.IndexOf('/');
            string _path1 = path.Substring(0, i);
            string _path2 = path.Substring(i + 1) + "/";
            FolderInfo fi;
            bool done = false;
            while (true)
            {
                while (true)
                {
                    fi = (FolderInfo)get_folder_info_from_path(_path1, null, null);
                    one_level_expand_tv((TreeNode)_1_ht[fi], fi);
                    i = _path2.IndexOf('/');
                    if (i < 1)
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            Invoke(new MethodInvoker(delegate
                            {
                                googleDriveTreeView.SelectedNode = ((TreeNode)_1_ht[fi]);
                            }));
                        }
                        else
                        {
                            bool _f = false;
                            foreach (TreeNode tn7 in ((TreeNode)_1_ht[fi]).Nodes)
                            {
                                if (tn7.Text == name)
                                {
                                    Invoke(new MethodInvoker(delegate
                                    {
                                        googleDriveTreeView.SelectedNode = tn7;
                                    }));
                                    _f = true;
                                    break;
                                }
                            }
                            if (!_f)
                            {
                                throw new Exception();
                            }
                        }
                        done = true;
                        break;
                    }
                    _path1 += "/" + _path2.Substring(0, i);
                    _path2 = _path2.Substring(i + 1);
                }
                if (done)
                {
                    //Invoke(new MethodInvoker(delegate
                    //{
                    //    //googleDriveTreeView.Select();
                    //    //googleDriveTreeView.SelectedNode.EnsureVisible();
                    //    //googleDriveTreeView_NodeMouseClick(null, new TreeViewEventArgs(googleDriveTreeView.SelectedNode));
                    //}));
                    break;
                }
                i = _path1.LastIndexOf('/');
                _path2 = _path1.Substring(i + 1) + _path2;
                _path1 = _path1.Substring(0, i);
            }
            show_processing_image();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var x = e;
            TabControl tc = (TabControl)sender;
            try
            {
                tc.Controls[tc.SelectedIndex].Controls[0].Controls[0].Focus(); // on the ChromiumBrowser
            }
            catch (Exception) { }
        }

        // https://www.codeproject.com/Questions/144813/Tab-control-background-color-in-visual-Studio-2005
        private void tabControl_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Font f;
            Brush backBrush;
            Brush foreBrush;
            TabControl tc = (TabControl)sender;

            if (e.Index == tc.SelectedIndex)
            {
                //f = e.Font;
                f = new Font(e.Font, FontStyle.Regular | FontStyle.Bold);
                backBrush = new SolidBrush(e.Index != 2 ? Color.LightBlue : Color.LightGreen); //new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.Blue, Color.Red, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                foreBrush = Brushes.Black;
            }
            else
            {
                f = e.Font;
                backBrush = new SolidBrush(e.BackColor);
                foreBrush = new SolidBrush(e.ForeColor);
            }

            TabPage tp = tc.TabPages[e.Index];
            string tabName = tp.Text;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            Rectangle r = e.Bounds;
            r = new Rectangle(r.X, r.Y + 4, r.Width, r.Height - 3);
            e.Graphics.DrawString(tabName, f, foreBrush, r, sf);

            sf.Dispose();
            if (e.Index == tc.SelectedIndex)
            {
                f.Dispose();
                backBrush.Dispose();
            }
            else
            {
                backBrush.Dispose();
                foreBrush.Dispose();
            }
        }

        private void axWindowsMediaPlayer1_ErrorEvent(object sender, EventArgs e)
        {
            string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;
            // MessageBox.Show("axWindowsMediaPlayer1_ErrorEvent");
            // show_processing_image();
        }

        private void showDupsButton_Click(object sender, EventArgs e)
        {
            searchResultsTextBox.Text = "Searching Duplicated...";
            searchListView.Items.Clear();
            Google.Apis.Drive.v3.Data.File f = (Google.Apis.Drive.v3.Data.File)((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
            mainTabControl.SelectTab(1);
            List<object> fl = (List<object>)((Hashtable)global_ri.by_ht_name_ht["file_by_Md5_ht"])[f.Md5Checksum];
            foreach (object _f2 in fl)
            {
                GoogleFolder f2 = (GoogleFolder)_f2;
                string[] res_line_ar = new string[4];
                string full_path = my_utils.decode_heb((f2).path);
                res_line_ar[0] = get_path(full_path);
                res_line_ar[1] = get_file(full_path);
                if (res_line_ar[1] == "")
                { // lilo
                    res_line_ar[1] = my_utils.decode_heb(((GoogleFolder)f2).file.Name);
                }
                res_line_ar[2] = my_utils.bytes_to_formatted_string((long)f2.file.Size);
                ListViewItem lvi = new ListViewItem(res_line_ar);
                if (f2.file.Shared == true)
                {
                    lvi.ForeColor = Color.Green;
                }
                lvi.Tag = f2;
                searchListView.Items.Add(lvi);
            }
            searchResultsTextBox.Text = "Found " + fl.Count + " Duplications.";
        }


        void menuItem_Click(object sender, EventArgs e)
        {
            string menu_txt = ((MenuItem)sender).Text;
            switch (menu_txt)
            {
                case "Show Duplicates":
                    mainTabControl.SelectTab(2);
                    dupsFolderComboBox.SelectedItem = "Selected Node";
                    dupButton_Click(null, null);
                    return;
                case "Exclude Folder":
                    // todo: first implement config.txt as json with {} of folders to exclude.
                    break;
                case "Delete":
                    // todo
                    break;
                case "Rescan":
                    // todo
                    break;
                default:
                    throw new Exception("unhandled: " + menu_txt);
            }
            MessageBox.Show("todo: " + ((MenuItem)sender).Text + ": " + googleDriveTreeView.SelectedNode.Text);
        }

        private void googleDriveTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            googleDriveTreeView.SelectedNode = ((TreeView)sender).GetNodeAt(new Point(e.X, e.Y));
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            if (e.Button == MouseButtons.Right)
            {
                object o = ((List<object>)googleDriveTreeView.SelectedNode.Tag)[1];
                bool is_file = o is Google.Apis.Drive.v3.Data.File;
                contextMenu.MenuItems[0].Enabled = !is_file;
                contextMenu.MenuItems[1].Enabled = !is_file;
                contextMenu.MenuItems[2].Enabled = is_file;
                contextMenu.MenuItems[3].Enabled = !is_file;
                contextMenu.Show(googleDriveTreeView, e.Location);
            }
        }

        WebSocketWrapper ws = null;
        WebSocketWrapper ws1 = null;
        Hashtable ws_ht = new Hashtable();
        int ws_i = 0;

        private void connect_to_ws_Button_Click(object sender, EventArgs e)
        {
            if (ws == null)
            {
                ws = WebSocketWrapper.Create("ws://localhost:3030");

                ws.OnConnect((WebSocketWrapper c) =>
                {
                    ws1 = c;
                    Console.WriteLine("connected to websocket server.");
                });

                ws.OnMessage((string msg, WebSocketWrapper c) =>
                {
                    if (Regex.IsMatch(msg, @"^[a-zA-Z0-9\+/]*={0,2}$"))
                    {
                        msg = Encoding.UTF8.GetString(System.Convert.FromBase64String(msg));
                    }
                    JObject jo = (JObject)JsonConvert.DeserializeObject(msg);
                    string op = (string)jo["op"];
                    string browser_id = (string)jo["browser_id"];
                    switch (op)
                    {
                        case "ws_connected":
                            Invoke(new MethodInvoker(delegate
                            {
                                connect_to_ws_Button.Text = "Connected";
                                sendCatButton.Enabled = true;
                                sendUserCatButton.Enabled = true;
                            }));

                            //JArray cat_json = (JArray)((JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(categories_filename)));
                            JObject jo2 = new JObject();
                            jo2.Add("op", "client_connected");
                            jo2.Add("username", (string)properties_ht["username"]);
                            ws1.SendMessage(jo2.ToString());

                            break;

                        case "client_details":
                            // ws_ht[c] = browser_id;

                            JArray categories_json = (JArray)jo["categories"];
                            global_user_cat_json = (JObject)jo["user_categories"];

                            handle_cats(categories_json, "cat");

                            break;
                        default:
                            throw new Exception("unrecognized op: " + op);
                    }
                    Console.WriteLine("got message: " + msg);
                });
                ws.Connect();
                connect_to_ws_Button.Enabled = false;
                connect_to_ws_Button.Text = "Connecting..";
            }
            else
            {
                throw new Exception("connect cannot be called more that once.");
                JObject jo = (JObject)JsonConvert.DeserializeObject($"{{ op: 'client_did_something', x: {++ws_i} }}");
                ws.SendMessage(jo.ToString());
            }
        }

        private void handle_cats(JArray categories_json, string _type)
        {
            if (_type == "filter")
            {
                System.Threading.Thread load_filter_cat_tree_thread = new System.Threading.Thread(new ThreadStart((new load_cat_tree_class(filter_ChromiumWebBrowser, this, false, true, true, true, true, "#d7ffb0", categories_json)).load_cat_tree));
                load_filter_cat_tree_thread.IsBackground = true;
                load_filter_cat_tree_thread.Start();
            }
            else if (_type == "cat")
            {
                System.Threading.Thread load_cat_tree_thread = new System.Threading.Thread(new ThreadStart((new load_cat_tree_class(categories_ChromiumWebBrowser, this, false, true, false, false, false, "#fff7b0", categories_json)).load_cat_tree));
                load_cat_tree_thread.IsBackground = true;
                load_cat_tree_thread.Start();
                //
                check_user_cat(categories_json);
            }
            else
            {
                throw new Exception("undetected _type: " + _type);
            }
        }

        private void sendCatButton_Click(object sender, EventArgs e)
        {
            //string browser_id = (string)ws_ht[ws1];
            JArray cat_json = (JArray)((JArray)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(categories_filename)));
            JObject jo = new JObject();
            jo.Add("op", "client_sent_cats");
            jo.Add("username", (string)properties_ht["username"]);
            jo.Add("categories", cat_json);
            ws1.SendMessage(jo.ToString());
        }

        private void sendUserCatButton_Click(object sender, EventArgs e)
        {
            //string browser_id = (string)ws_ht[ws1];
            JObject user_cat_json = (JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText("categories/user_categories.json")); // "{ op: 'client_did_something'}");
            JObject jo = new JObject();
            jo.Add("op", "client_sent_user_cats");
            jo.Add("username", (string)properties_ht["username"]);
            jo.Add("user_categories", user_cat_json);
            ws1.SendMessage(jo.ToString());
        }
    }
}
