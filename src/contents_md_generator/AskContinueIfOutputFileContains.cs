internal static partial class ContentsMdGenerator
{
    // Данный метод спрашивает у пользователя о необходимости перезаписать файл с контентом
    // Если он уже существует
    private static bool AskContinueIfOutputFileContains(FileInfo outputFile)
    {
        if (!outputFile.Exists)
            return true;

        Console.Write("Файл уже создан. Перезаписать? (Y/N): _\b");
        var answer = Console.ReadKey();
        ConsoleClearLine();

        if (answer.Key == ConsoleKey.Y)
            return true;
        else
            return false;

        // Локальная функция очистки одной строки консоли
        static void ConsoleClearLine() =>
            Console.Write('\r' + new string(' ', Console.WindowWidth) + '\r');
        // ----------------------------------------------
    }
}