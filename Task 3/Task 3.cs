using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient Client = new HttpClient();

    static async Task Main()
    {
        try
        {
            string url = GetApiBaseUrl();
            using HttpResponseMessage response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await using Stream responseStream = await response.Content.ReadAsStreamAsync();
            using JsonDocument document = await JsonDocument.ParseAsync(responseStream);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException("The API response was not a JSON array.");
            }

            foreach (JsonElement post in document.RootElement.EnumerateArray())
            {
                if (post.TryGetProperty("title", out JsonElement title) &&
                    title.ValueKind == JsonValueKind.String)
                {
                    Console.WriteLine(title.GetString());
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"API request failed: {ex.Message}");
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"Could not parse the API response: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"Could not read configuration or response data: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine($"Configuration error: {ex.Message}");
        }
    }

    private static string GetApiBaseUrl()
    {
        string? url = Environment.GetEnvironmentVariable("API_BASE_URL");
        if (string.IsNullOrWhiteSpace(url))
        {
            url = ReadDotEnvValue("API_BASE_URL");
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            throw new InvalidOperationException(
                "API_BASE_URL was not found in the environment or a .env file.");
        }

        url = url.Trim();
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException("API_BASE_URL must be an absolute HTTP or HTTPS URL.");
        }

        return uri.ToString();
    }

    private static string? ReadDotEnvValue(string variableName)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] possiblePaths =
        {
            Path.Combine(currentDirectory, ".env"),
            Path.Combine(currentDirectory, "Task 3", ".env")
        };

        foreach (string path in possiblePaths)
        {
            if (!File.Exists(path))
            {
                continue;
            }

            foreach (string line in File.ReadLines(path))
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.Length == 0 || trimmedLine.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                int separatorIndex = trimmedLine.IndexOf('=');
                if (separatorIndex < 0 ||
                    !trimmedLine[..separatorIndex].Trim().Equals(variableName, StringComparison.Ordinal))
                {
                    continue;
                }

                return trimmedLine[(separatorIndex + 1)..].Trim().Trim('"', '\'');
            }
        }

        return null;
    }
}
