internal static partial class ContentsMdGenerator
{
    // Принцип работы программы
    // 
    // Сначала она проверяет наличие аргументов, если они не указаны, выводит соответствующую подсказку
    // Затем проверяет в целом существование файла, если его нет, выводит соответствующее уведомление
    // Затем проверяет наличие файла с оглавлением
    // Если он есть, то спрашивает о необходимости перезаписать содержимое
    // На основе указанного файла и лимита заголовка, который может быть указан в аргументах, получает из него оглавление
    // Которое затем записывает в отдельный файл с префиксом "contents" (при желании можно поменять в файле конфигурации)
    internal static void Run(string[] args)
    {
        var getConfigurationResult = GetConfiguration();
        if (getConfigurationResult.Error != null)
        {
            Console.WriteLine(getConfigurationResult.Error);
            return;
        }

        if (args.Length == 0 || args.Any(string.IsNullOrEmpty))
        {
            Console.WriteLine("Ошибка: В аргументе необходимо указать название md-файла для формирования его оглавления.");
            return;
        }

        var file = new FileInfo(Path.Combine(Environment.CurrentDirectory, args[0]));
        if (!file.Exists || file.Extension != ".md")
        {
            Console.WriteLine("Ошибка: По указанному пути md-файл не найден!");
            return;
        }

        var outputFile = GetOutputFileInfo(getConfigurationResult.Configuration!, file);
        if (!AskContinueIfOutputFileContains(outputFile))
            return;
        
        var getContentsResult = GetContents(file, args);
        if (getContentsResult.Error != null)
        {
            Console.WriteLine(getContentsResult.Error);
            return;
        }

        string resultMessage = WriteContents(getContentsResult.Content!, outputFile);
        Console.WriteLine(resultMessage);
    }
}