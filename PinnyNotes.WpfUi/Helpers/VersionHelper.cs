using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

using PinnyNotes.WpfUi.Properties;

namespace PinnyNotes.WpfUi.Helpers;

public static class VersionHelper
{
    private static Version CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version ?? new();

    public static async void CheckForNewRelease()
    {
        DateTimeOffset date = DateTimeOffset.UtcNow;

        if (Settings.Default.CheckForUpdates && Settings.Default.LastUpdateCheck < date.AddDays(-7).ToUnixTimeSeconds())
        {
            Settings.Default.LastUpdateCheck = date.ToUnixTimeSeconds();
            Settings.Default.Save();

            if (CurrentVersion < await GetLatestGitHubReleaseVersion())
                MessageBox.Show(
                    $"A new version of Pinny Notes is available;{Environment.NewLine}https://github.com/63BeetleSmurf/PinnyNotes/releases/latest",
                    "Update available",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
        }
    }

    private static async Task<Version?> GetLatestGitHubReleaseVersion()
    {
        using HttpClient client = new HttpClient();
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
