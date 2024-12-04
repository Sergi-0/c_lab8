using System;
using System.IO;
using System.IO.Compression;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите путь для поиска: ");
        string searchPath = Console.ReadLine();

        Console.Write("Введите имя файла для поиска: ");
        string fileName = Console.ReadLine();

        // Поиск файла
        string filePath = FindFile(searchPath, fileName);

        if (filePath != null)
        {
            Console.WriteLine($"Файл найден: {filePath}");
            Console.WriteLine("Содержимое файла:");

            // Вывод содержимого файла на консоль
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }

            // Сжатие файла
            Console.Write("Хотите сжать файл? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                string compressedFilePath = CompressFile(filePath);
                Console.WriteLine($"Файл сжат и сохранен как: {compressedFilePath}");
            }
        }
        else
        {
            Console.WriteLine("Файл не найден.");
        }
    }

    static string FindFile(string path, string fileName)
    {
        try
        {
            // Поиск файла во всех поддиректориях
            foreach (string file in Directory.GetFiles(path, fileName, SearchOption.AllDirectories))
            {
                return file; // Возвращаем первый найденный файл
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при поиске файла: {ex.Message}");
        }

        return null; // Если файл не найден
    }

    static string CompressFile(string filePath)
    {
        string compressedFilePath = Path.ChangeExtension(filePath, ".zip");

        using (FileStream originalFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (FileStream compressedFileStream = File.Create(compressedFilePath))
        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
        {
            originalFileStream.CopyTo(compressionStream);
        }

        return compressedFilePath;
    }
}