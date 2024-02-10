using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SailIntreviewQuestion2.Models;

namespace SailIntreviewQuestion2;

public class HtmlGenerator
{
    public string CreateHtml(string template, string jsonData)
    {
        var (textFromTemplateFile, textFromTemplateJsonData) = ReadFiles(template, jsonData);
        var products = SerializeProducts(textFromTemplateJsonData);
        return ProcessTemplate(textFromTemplateFile, products);
    }

    private static ProductList SerializeProducts(string textFromTemplateJsonData)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var products = JsonSerializer.Deserialize<ProductList>(textFromTemplateJsonData, jsonSerializerOptions);
        if (products is null)
        {
            throw new ArgumentNullException(nameof(textFromTemplateJsonData));
        }

        return products;
    }

    private (string textFromTemplateFile, string textFromTemplateJsonData) ReadFiles(string template, string jsonData)
    {
        var textFromTemplateFile = File.ReadAllText(template);
        var textFromTemplateJsonData = File.ReadAllText(jsonData);
        return (textFromTemplateFile, textFromTemplateJsonData);
    }

    private static string ProcessTemplate(string template, ProductList productList)
    {
        // Process loops first
        const string loopPattern = @"{% for product in products %}(.*?){% endfor %}";
        var loopMatch = Regex.Match(template, loopPattern, RegexOptions.Singleline);

        if (!loopMatch.Success)
            return template;
        
        var loopResult = new StringBuilder();
        var loopTemplate = loopMatch.Groups[1].Value;

        foreach (var processedLoop in ProcessedLoops(productList, loopTemplate))
        {
            loopResult.AppendLine(processedLoop);
        }

        template = Regex.Replace(template, loopPattern, loopResult.ToString(), RegexOptions.Singleline);
        
        return template;
    }

    private static IEnumerable<string> ProcessedLoops(ProductList productList, string loopTemplate)
    {
        return productList.Products.Select(product => loopTemplate
            .Replace("{{product.name}}", product.Name)
            .Replace("{{product.price | price }}", $"${product.Price}")
            .Replace("{{product.description | paragraph }}", product.Description));
    }
}