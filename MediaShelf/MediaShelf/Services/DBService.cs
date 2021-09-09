using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MediaShelf.Services
{
    public static class DBService
    {
        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db != null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, Constants.DBName);

            db = new SQLiteAsyncConnection(dbPath);

            CopyDB(dbPath);
        }

        public static void CopyDB(string dbPath)
        {
            var embeddedResourceDb = Assembly.GetExecutingAssembly().GetManifestResourceNames().First(s => s.Contains(Constants.DBName));
            var embeddedResourceDbStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceDb);

            //if (!File.Exists(dbPath))
            //{
            //    File.Delete(dbPath);

            //    using (var br = new BinaryReader(embeddedResourceDbStream))
            //    {
            //        using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
            //        {
            //            var buffer = new byte[2048];
            //            int len;
            //            while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
            //            {
            //                bw.Write(buffer, 0, len);
            //            }
            //        }
            //    }
            //}

            File.Delete(dbPath);

            using (var br = new BinaryReader(embeddedResourceDbStream))
            {
                using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                {
                    var buffer = new byte[2048];
                    int len;
                    while ((len = br.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        bw.Write(buffer, 0, len);
                    }
                }
            }
        }



    }
}
