using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

using FileProviderSample.Droid;
using Java.IO;
using Xamarin.Forms;

using Uri = Android.Net.Uri;

[assembly: Dependency(typeof(SaveAndroid))]
namespace FileProviderSample.Droid
{
    public class SaveAndroid : ISave
    {

        public async Task Save(string fileName, string contentType, MemoryStream stream)
        {

            string exception = string.Empty;
            string root = null;
            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
                root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Java.IO.File myDir = new Java.IO.File(root + "/Atas");
            myDir.Mkdir();
            Java.IO.File file = new Java.IO.File(myDir, fileName);

            if (file.Exists()) file.Delete();


            try
            {
                FileOutputStream outs = new FileOutputStream(file);
                stream.Position = 0;
                outs.Write(stream.ToArray());
                outs.Flush();
                outs.Close();
            }

            catch (Exception e)
            {
                exception = e.ToString();
            }
            finally
            {
                stream.Dispose();
            }

            if (file.Exists())
            {
                string auth = "com.companyname.FileProviderSample.fileprovider";
                string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Uri.FromFile(file).ToString());
                string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
                if (mimeType == null)
                    mimeType = "*/*";

                Uri uri = FileProvider.GetUriForFile(Forms.Context, auth, file);

                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(uri, mimeType);
                intent.AddFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.NoHistory);

                var resInfoList = Forms.Context.PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
                foreach (var resolveInfo in resInfoList)
                {
                    var packageName = resolveInfo.ActivityInfo.PackageName;
                    Forms.Context.GrantUriPermission(packageName, uri, ActivityFlags.GrantWriteUriPermission | ActivityFlags.GrantPrefixUriPermission | ActivityFlags.GrantReadUriPermission);
                }

                Forms.Context.StartActivity(intent);
            }

        }

    }
}