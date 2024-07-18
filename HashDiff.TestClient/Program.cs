// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using static System.Console;

/// <summary>
/// Simple 
/// </summary>
public class Program
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost:8001/"),
    };

    public static async Task<int> Main(string[] args)
    {
        var currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        WriteLine($"Trying to find Docker compose file up the directory tree. Starting with {currentFolder}");
        var folderWithDockerCompose = TryToFindFolderContainingDockerComposeFile(currentFolder);
        if (folderWithDockerCompose == null)
        {
            WriteLine($"Could not find Docker compose file. Please make sure it is in some of these folders {currentFolder}");
            return -1;
        }
        else
            WriteLine($"Docker compose file found at {folderWithDockerCompose}");

        
        WriteLine("Cleaning up!");
        //StopDockerCompose(folderWithDockerCompose);
        WriteLine("Starting the application");
        StartDockerCompose(folderWithDockerCompose);
        while (!await MakeSureAppIsRunning(sharedClient))
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
        WriteLine("Application is running!\n");

        var anotherRound = true;
        while (anotherRound)
        {
            var idInserted = false;
            var id = 0;
            while (!idInserted)
            {
                Write("Please insert diff ID: ");
                idInserted = int.TryParse(ReadLine(), out id);
            }

            bool leftPartIsOk = false;
            while (!leftPartIsOk)
            {
                WriteLine("\nPlease insert left part of the diff: ");
                var leftPart = ReadLine();
                var results = await PostDiff(sharedClient, id, leftPart, isLeft: true);
                WriteLine(results.Item2);
                leftPartIsOk = results.Item1 == HttpStatusCode.Created;
            }
        
            bool rightPartIsOk = false;
            while (!rightPartIsOk)
            {
                WriteLine("\nPlease insert right part of the diff: ");
                var rightPart = ReadLine();
                var results = await PostDiff(sharedClient, id, rightPart, isLeft: false);
                WriteLine(results.Item2);
                rightPartIsOk = results.Item1 == HttpStatusCode.Created;
            }
        
            WriteLine("\nGetting diffs: ");
            WriteLine(await GetDiffResult(sharedClient, id));
            
            WriteLine("\nTry again? (y/n)");
            anotherRound = ReadLine()?.ToLowerInvariant() == "y";
        }
        return 0;
    }

    private static string? TryToFindFolderContainingDockerComposeFile(string? startingPath)
    {
        var containsDockerComposeFile = Directory.GetFiles(startingPath, "*.yml")
            .Count(filePath => filePath.Contains("docker-compose.TestClient")) == 1;
        if(containsDockerComposeFile)
            return startingPath;
		
        var oneUpFolder = Directory.GetParent(startingPath)?.FullName;
        if(!containsDockerComposeFile && oneUpFolder != null)
            return TryToFindFolderContainingDockerComposeFile(oneUpFolder);
	
        return null;
    }
    
    private static void StopDockerCompose(string workingDir)
    {
        var processInfo = new ProcessStartInfo("docker-compose", "-f docker-compose.TestClient.yml down");
	
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.WorkingDirectory = workingDir;

        using var process = new Process();
        process.StartInfo = processInfo;
        process.Start();
    }
    
    private static void StartDockerCompose(string workingDir)
    {
        var processInfo = new ProcessStartInfo("docker-compose", "-f docker-compose.TestClient.yml up --detach");
	
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.WorkingDirectory = workingDir;

        using var process = new Process();
        process.StartInfo = processInfo;
        process.Start();
    }
    
    private static async Task<bool> MakeSureAppIsRunning(HttpClient httpClient)
    {
        var result = false;
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync("system/check");
            result = response.StatusCode == HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            // ignored
        }

        return result;
    }
    
    static async Task<(HttpStatusCode, string)> PostDiff(HttpClient httpClient, int id, string contents, bool isLeft)
    {
        var content = new StringContent(contents, Encoding.UTF8, "application/custom");

        using HttpResponseMessage response = await httpClient.PostAsync(
            isLeft ? $"v1/diff/{id}/left" : $"v1/diff/{id}/right",
            content);

        
        // using HttpResponseMessage response = await httpClient.PostAsJsonAsync(
        //     isLeft ? $"v1/diff/{id}/left" : $"v1/diff/{id}/right",
        //     contents);

        response.EnsureSuccessStatusCode();

        return (response.StatusCode, await response.Content.ReadAsStringAsync());
    }
    
    static async Task<string> GetDiffResult(HttpClient httpClient, int id)
    {
        using HttpResponseMessage response = await httpClient.GetAsync($"v1/diff/{id}");
        return await response.Content.ReadAsStringAsync();
    }
}
