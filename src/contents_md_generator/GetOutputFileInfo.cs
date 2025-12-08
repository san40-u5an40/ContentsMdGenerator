internal static partial class ContentsMdGenerator
{
    private const string DEFAULT_PREFIX = "content";

    // Для получения нового выходного файла программа берёт префикс из конфигурации
    // И объединяет его с названием файла
    // Если префикс не указан использует стандартный из константы
    private static FileInfo GetOutputFileInfo(IConfigurationRoot config, FileInfo file)
    {
        string prefix = config.GetSection("AppSettings")["Prefix"] ?? DEFAULT_PREFIX;
        string outputFilePath = Path.Combine(Environment.CurrentDirectory, $"{prefix}_{file.Name}");
        return new FileInfo(outputFilePath);
    }
}