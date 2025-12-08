internal static partial class ContentsMdGenerator
{
    private const int LVL_LIMIT = 6;    // Лимит уровня заголовков, если не указан иной

    // Принцип работы метода получения оглавления из файла
    // 
    // Сначала он открывает stream и сохраняет весь текст из файла в строковую переменную
    // Если в ходе открытия stream возникли ошибки, возвращает соответствующий результат
    // Также и при отсутствии текста во входном файле
    // Если всё прошло хорошо, то ищет в полученном тексте все заголовки с помощью локальной функции SearchContents
    private static (string? Content, string? Error) GetContents(FileInfo file, string[] args)
    {
        string text;
        try
        {
            using var streamFile = file.OpenText();
            text = streamFile.ReadToEnd();
        }
        catch (Exception ex)
        {
            return (null, "Ошибка:\n" + ex.Message);
        }

        if (string.IsNullOrEmpty(text))
            return (null, "Ошибка: Файл \"" + file.Name + "\" не содержит текста!");

        int lvlLimit = GetLvlLimit(args);
        return SearchContents(text, lvlLimit);

        // Локальная функция получения лимита для уровня заголовка
        // Пробует получить лимит из аргументов,
        // Если лимит не указан, или указано некорректное значение
        // Возвращает максимальный лимит, указанный в файле конфигурации
        static int GetLvlLimit(string[] args)
        {
            if (args.Length > 1 &&
                int.TryParse(args[1], out int num) &&
                num >= 1 && num <= LVL_LIMIT)
            {
                return num;
            }
            else
                return LVL_LIMIT;
        }

        // Локальная функция поиска заголовков:
        // 
        // Для поиска заголовков она использует регулярное выражение,
        // Которое построчно анализирует текст на соответствие паттерну
        // Все найденные совпадения обрабатываются:
        // В начале строки добавляется табуляция
        // После которой следует элемент списка с оформлением заголовка в формате MarkDown
        // Все найденные строки возвращаются из функции,
        // Ну или информация об ошибках, если они возникли
        static (string? Content, string? Error) SearchContents(string text, int lvlLimit)
        {
            var matches = Regex.Matches(text,
                                    $"^(?<lvl>#{{1,{lvlLimit}}})\\s(?<content>.*?)\\r?$",
                                    RegexOptions.Compiled | RegexOptions.Multiline);

            var output = new StringBuilder();
            foreach (Match match in matches)
            {
                int lvl = match.Groups["lvl"].Value.Length;
                string tab = new string(' ', (lvl - 1) * 4);

                string content = match.Groups["content"].Value;

                output.AppendLine(tab + "- [" + content + "]()");
            }

            return (output.ToString(), null);
        }
    }
}