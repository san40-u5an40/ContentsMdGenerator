internal static partial class ContentsMdGenerator
{
    // Метод создания нового файла:
    // 
    // Также открывается stream для нового файла
    // По которому составленное оглавление записывается
    // Если создание текстового файла прошло успешно,
    // Возвращает соответствующее уведомление
    // В противном случае возвращает текст ошибки
    private static string WriteContents(string contents, FileInfo outputFile)
    {
        try
        {
            using var streamFile = outputFile.CreateText();

            streamFile.Write(contents);
            streamFile.Flush();

            return "Файл \"" + outputFile.Name + "\" успешно создан!";
        }
        catch (Exception ex)
        {
            return "Ошибка:\n" + ex.Message;
        }
    }
}