using Api;

Console.WriteLine("Hello!");

var filePathsList = new List<string>();

filePathsList.AddRange(Directory.EnumerateFiles(".", "*.txt").ToArray());
filePathsList.AddRange(Directory.EnumerateFiles(".", "*.log").ToArray());
var filePaths = filePathsList.ToArray();

if (filePaths.Length == 0)
{
    Console.WriteLine("No files found.");
    Console.ReadKey();
    return;
}

Console.WriteLine("Files Found:");

foreach (var filePath in filePaths)
{
    Console.WriteLine(Path.GetFileName(filePath));
}

Console.WriteLine("Continue? [Y] / N");

var keyResponse = Console.ReadKey();
Console.WriteLine("");
switch (keyResponse.Key)
{
    case ConsoleKey.Y:
        break;
    case ConsoleKey.N:
        Console.WriteLine("Bye");
        Console.ReadKey();
        return;
    default:
        Console.WriteLine("That doesn't sounds right");
        Console.ReadKey();
        return;
}

foreach (var filePath in filePaths)
{
    Console.WriteLine("---");
    Console.WriteLine("Parsing: " + filePath);
    FileParser.Parse(filePath);
    Console.WriteLine("Done.");
}

Console.ReadKey();