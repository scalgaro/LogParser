using System.Text.RegularExpressions;

namespace Api;

public static class FileParser
{
    const string Separator  = ";;;";

    public static void Parse(string filePath)
    {
        var contents = File.ReadAllText(filePath);

        // Acomodando los saltos de linea y tabs
        contents = Regex.Replace(contents, "\\n[^2022]", " ");
        contents = Regex.Replace(contents, "(?:(?!\\n)\\s)+", " ");
        contents = Regex.Replace(contents, "\\t", " ");

        var lines = contents.Split('\n');

        var resultLines = new List<string>()
        {
            $"Time{Separator}ID{Separator}Command{Separator}Argument{Separator}"
        };

        foreach (var line in lines)
        {
            var resultLine = TreatLine(line);
            if (resultLine != null) resultLines.Add(resultLine);
        }

        Directory.CreateDirectory("./Processed");

        var newPath = "./Processed/" + Path.GetFileNameWithoutExtension(filePath) + ".csv";

        if (File.Exists(newPath))
        {
            File.Delete(newPath);
        }

        File.WriteAllLines(newPath, resultLines);

        File.Delete(filePath);
    }

    private static string? TreatLine(string line)
    {
        if (!line.StartsWith("2022"))
        {
            return null;
        }
        
        var result = line;

        // Separando la fecha que está en ISO
        result = result.ReplaceForComma(27);

        // Separando el ID
        result = result.ReplaceForComma(39);

        // Separando comandos
        var comandos = new[]
        {
            "Query",
            "Quit",
            "Connect",
            "Init DB",
        };

        foreach (var comando in comandos)
        {
            result = result.Replace(Separator + comando, Separator + comando + Separator + "\"");
        }

        // Agregando el final de linea
        result += "\"" + Separator;


        return result;
    }

    private static string ReplaceForComma(this string text, int index)
    {
        return text.Remove(index, 1).Insert(index, Separator);
    }
}