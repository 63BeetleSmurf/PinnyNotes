using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PinnyNotes.WpfUi.Helpers;

public static class VersionHelper
{
    private static Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new();

    public static async Task<bool> CheckForNewRelease(long? lastUpdateCheck, DateTimeOffset date)
    {
        if (lastUpdateCheck < date.AddDays(-7).ToUnixTimeSeconds())
        {
            if (CurrentVersion < await GetLatestGitHubReleaseVersion())
                MessageBox.Show(
                    $"A new version of Pinny Notes is available;{Environment.NewLine}https://github.com/63BeetleSmurf/PinnyNotes/releases/latest",
                    "Update available",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

            return true;
        }

        return false;
    }

    private static async Task<Version?> GetLatestGitHubReleaseVersion()
    {
        using HttpClient client = new();
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("PinnyNotes", CurrentVersion.ToString()));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            HttpResponseMessage response = await client.GetAsync("https://api.github.com/repos/63beetlesmurf/pinnynotes/releases/latest");
            if (!response.IsSuccessStatusCode)
                return null;

            using JsonDocument responseData = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (!responseData.RootElement.TryGetProperty("tag_name", out JsonElement tagNameElement))
                return null;

            string? releaseVersion = tagNameElement.GetString();
            if (string.IsNullOrWhiteSpace(releaseVersion))
                return null;

            if (Version.TryParse($"{releaseVersion[1..]}.0", out Version? parsedVersion)) // Remove v and add extra .0, v1.2.3 -> 1.2.3.0
                return parsedVersion;
        }
        catch
        {
        }

        return null;
    }
}
