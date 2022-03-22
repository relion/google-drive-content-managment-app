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
using System.Linq;
using System.Text.RegularExpressions;

namespace ContentManagerStudio
{
    class read_google_drive
    {
        MainForm form;

        public read_google_drive(MainForm form)
        {
            this.form = form;
        }

        public static string ApplicationName = "CMStudio"; // https://console.developers.google.com/apis/credentials
        public UserCredential credential;

        int n_total_obj = 0;
        long n_total_bytes = 0;
        public int n_total_requests = 0;

        public int tree_depth = 0;
        Hashtable all_my_mime_types = new Hashtable();
        Hashtable missing_mime_types = new Hashtable();

        Stopwatch runtime_sw;

        public void read_google_drive_info()
        {
            StringWriter all_output = new StringWriter();
            FolderInfo root_folder_info = read_all_tree(all_output, out runtime_sw);

            RootInfo _ri = new RootInfo();
            _ri.root = root_folder_info;
            my_utils.serialize_to_gz_file(_ri);

            Console.Write(all_output);
            if (root_folder_info.total_size_bytes != n_total_bytes)
            {
                // debug.
            }
            Console.WriteLine($"\r\nTotal Tree Objects: {n_total_obj}\r\nTotal Size: {my_utils.bytes_to_formatted_string(n_total_bytes)}\r\nTotal Processing Time: {runtime_sw.Elapsed.ToString(@"mm\:ss")}\r\nTotal Requests: {n_total_requests}");
            Console.WriteLine($"\r\n{all_my_mime_types.Count} all my mime types:");
            foreach (DictionaryEntry mime_type in all_my_mime_types)
            {
                Console.WriteLine($"{mime_type.Key} ({mime_type.Value})");
            }
            Console.WriteLine($"\r\n{missing_mime_types.Count} missing mime_types:");
            foreach (DictionaryEntry mime_type in missing_mime_types)
            {
                Console.WriteLine($"{mime_type.Key} ({mime_type.Value})");
            }
            //
            MainForm.global_ri = _ri;
            //
            form.show_processing_image("processing google tree...");
            MainForm.rescan_tree_from_google();

            MessageBox.Show("google-drive Rescanned.\r\nPlease Restart the application.");
            MainForm.this_form.run_load_google_drive_info();
        }

        private FolderInfo read_all_tree(StringWriter all_output, out Stopwatch runtime_sw)
        {
            credential = get_credential();

            runtime_sw = new Stopwatch();
            runtime_sw.Start();
            FolderInfo root_folder_info = read_tree("root", null, all_output, "");
            form.Invoke(new MethodInvoker(delegate
            {
                form.outputTextBox.AppendText("\r\nDone.");
            }));
            runtime_sw.Stop();
            return root_folder_info;
        }

        public static UserCredential get_credential()
        {
            // https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth
            FileStream stream = new FileStream("client_secret_62536932498-91hvdg9m9s0rk26qd3gt7r4b0j441c0c.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read);
            string credPath = "token.json";
            UserCredential c = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new string[] { DriveService.Scope.Drive }, // DriveReadonly. note if the browser is stucked.. enter in chrome console: document.getElementsByClassName("RveJvd snByac")[0].click();
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
            Console.WriteLine(c.Token.Scope);
            return c;
        }

        private FolderInfo read_tree(string root_id, Google.Apis.Drive.v3.Data.File root, StringWriter output, string path)
        {
            // if (tree_depth > 3) return; // (new StackTrace(true)).FrameCount > 32) return;

            if (form.pauseCheckBox.Checked)
            {
                do
                {
                    Thread.Sleep(200);
                    if (!form.pauseCheckBox.Checked)
                    {
                        break;
                    }
                }
                while (true);
            }

            FolderInfo folder_info = new FolderInfo(root, my_utils.slash_char);
            if (!string.IsNullOrEmpty(form.excluded_folders_re) && Regex.IsMatch(path.Trim('/'), "^(" + form.excluded_folders_re + ")$"))
            {
                form.Invoke(new MethodInvoker(delegate
                {
                    form.traceTextBox.AppendText("Folder excluded: " + path + "\r\n");
                    my_utils.scroll_to_bottom(form.traceTextBox);
                }));
                return null; // skeep.
            }

            form.Invoke(new MethodInvoker(delegate
            {
                form.outputTextBox.Text = $"tree_depth: {tree_depth}\r\nn_total_requests/folders: {n_total_requests}\r\nn_total_obj: {n_total_obj}\r\nTotal Size: {my_utils.bytes_to_formatted_string(n_total_bytes)}\r\nRunTime: {runtime_sw.Elapsed.ToString(@"mm\:ss")}\r\nPath: {path}";
                form.traceTextBox.AppendText("".PadRight(tree_depth, '*') + $" {(root != null ? root.Name : root_id)}\r\n");
                my_utils.scroll_to_bottom(form.traceTextBox);
            }));
            //

            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            tree_depth++;
            n_total_requests++;
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 100;
            listRequest.Q = $"'{root_id}' in parents";
            listRequest.Fields = "*"; // "nextPageToken, files(id, name)"

            // Loop.
            do
            {
                FileList fileList;
                int n_retries = 4;
                while (true)
                try {
                    fileList = listRequest.Execute();
                        break;
                }
                catch (Exception ex) {
                        if (--n_retries == 0)
                            throw ex;
                        Thread.Sleep(2000);
                        output.WriteLine($"retrying.. {n_retries}/{4}");

                    }
                IList<Google.Apis.Drive.v3.Data.File> files = fileList.Files;
                if (files == null || files.Count == 0) break;
                foreach (var file in files)
                {
                    n_total_obj++;
                    ggg:
                    bool is_folder = false;
                    switch (file.MimeType)
                    {
                        case "text/plain":
                        case "text/html":
                        case "text/css":
                        case "application/x-javascript":
                        case "text/xml":
                        case "text/x-csharp":
                        case "text/x-sql":
                        case "application/json":
                            break;
                        case "image/jpeg":
                        case "image/gif":
                        case "application/ogg":
                        case "image/tiff":
                        case "image/png":
                        case "image/bmp":
                        case "image/x-icon":
                        case "application/x-iso9660-image":
                        case "image/webp":
                            break;
                        case "text/x-url":
                        case "application/x-mpegurl":
                        case "audio/x-mpegurl":
                        case "application/x-bittorrent":
                            break;
                        case "application/vnd.google-apps.site":
                            break;
                        case "image/x-photoshop":
                        case "image/vnd.adobe.photoshop":
                            break;
                        case "application/vnd.google-apps.document":
                            break;
                        case "application/zip":
                        case "application/x-zip":
                        case "application/x-gzip":
                        case "application/x-zip-compressed":
                            break;
                        case "audio/mpeg":
                        case "audio/mp3":
                        case "audio/wav":
                        case "audio/mp4":
                        case "audio/aac":
                        case "audio/x-wav":
                        case "audio/amr":
                            break;
                        case "audio/mid":
                        case "audio/x-mod":
                        case "audio/prs.sid":
                            break;
                        case "video/mpeg":
                        case "video/avi":
                        case "video/mp4":
                        case "video/x-msvideo":
                        case "video/x-matroska":
                        case "video/webm":
                        case "video/flv":
                        case "video/x-flv":
                        case "video/3gpp":
                        case "video/x-ms-wmv":
                        case "video/x-ms-asf":
                        case "video/quicktime":
                            break;
                        case "application/x-subrip":
                            break;
                        case "application/octet-stream":
                            break;
                        case "application/pdf":
                        case "application/rtf":
                            break;
                        case "application/vnd.google-apps.spreadsheet":
                            break;
                        case "application/msword":
                        case "application/vnd.ms-excel":
                        case "application/vnd.ms-powerpoint":
                        case "application/x-shockwave-flash":
                        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                        case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                            break;
                        case "application/x-msdos-program":
                        case "application/x-dosexec":
                        case "application/x-ms-shortcut":
                            break;
                        case "application/vnd.google-apps.folder":
                            //string folderName = file.Name;
                            is_folder = true;
                            break;
                        default:
                            my_utils.add_and_count_key_to_ht(missing_mime_types, file.MimeType);

                            break; // goto ggg; // Debug.
                    }

                    my_utils.add_and_count_key_to_ht(all_my_mime_types, file.MimeType);

                    output.Write("".PadRight(((tree_depth - 1) * 2))); // indent.
                    string mark_folder = is_folder ? "*" : "";
                    output.Write($"{mark_folder}{file.Name}{mark_folder}");
                    if (file.Size != null)
                    {
                        n_total_bytes += (long)file.Size;
                        folder_info.total_size_bytes += (long)file.Size;
                        output.WriteLine($" ({my_utils.bytes_to_formatted_string((long)file.Size)})");
                        folder_info.n_files++;
                        folder_info.add_file(file);
                    }
                    else if (is_folder)
                    {
                        folder_info.n_folders++;
                        StringWriter sub_tree_output = new StringWriter();
                        FolderInfo sub_folder_info = read_tree(file.Id, file, sub_tree_output, path + "/" + file.Name);
                        if (sub_folder_info == null)
                        {
                            continue;
                        }
                        output.WriteLine($" ({my_utils.bytes_to_formatted_string(sub_folder_info.total_size_bytes)}, {sub_folder_info.n_files} files, {sub_folder_info.n_folders} foldders)");
                        output.Write(sub_tree_output);
                        folder_info.total_size_bytes += sub_folder_info.total_size_bytes;
                        folder_info.n_files += sub_folder_info.n_files;
                        folder_info.n_folders += sub_folder_info.n_folders;
                        folder_info.folders.Add(sub_folder_info);
                    }
                    else
                    {
                        output.WriteLine(" (?)");
                    }
                }

                string next_page_token = fileList.NextPageToken;
                if (next_page_token == null) break;
                listRequest.PageToken = next_page_token;
            }
            while (true);
            //
            tree_depth--;
            //
            return folder_info;
        }
    }
}
