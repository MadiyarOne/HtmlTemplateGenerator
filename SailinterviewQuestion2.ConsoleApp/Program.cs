// See https://aka.ms/new-console-template for more information

using SailIntreviewQuestion2;


string templatePath = string.Empty;
string jsonPath = templatePath; 
string outputPath = jsonPath;

if (!args.Any())
{
    Console.WriteLine("Please provide the path to the template file.");
    templatePath = Console.ReadLine();
    Console.WriteLine("Please provide the path to the json file.");
    jsonPath = Console.ReadLine();
    Console.WriteLine("Please provide the path to the output file.");
    outputPath = Console.ReadLine();
}
else
{
    templatePath = args[0];
    jsonPath = args[1];
    outputPath = args[2];
}

if (string.IsNullOrWhiteSpace(templatePath) || string.IsNullOrWhiteSpace(jsonPath) || string.IsNullOrWhiteSpace(outputPath))
{
    throw new ArgumentNullException("Please provide the path to the template file, the json file, and the output file.");
}


ProcessFiles(templatePath, jsonPath, outputPath);

void ProcessFiles(string templatePath, string jsonPath, string outputPath)
{
    var htmlGenerator = new HtmlGenerator();
    var html = htmlGenerator.CreateHtml(
        templatePath, jsonPath);

    File.WriteAllText(
        outputPath, html);
}