using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace MindHaven
{
    public partial class ReportsPage : ContentPage
    {
        public ObservableCollection<EmotionEntry> EmotionData { get; set; } = new();
        private static readonly HttpClient client = new();

        public ReportsPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadEmotionData();
        }

        private async void LoadEmotionData()
        {
            int userId = Preferences.Get("UserId", 0);
            if (userId == 0)
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            try
            {
                var requestData = new { user_id = userId };
                var response = await client.PostAsJsonAsync("http://localhost/mindhaven/reports.php", requestData); // Replace with your server's IP

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse); // Debugging output

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", $"Server error: {jsonResponse}", "OK");
                    return;
                }

                // Handle potential error message in the JSON
                if (jsonResponse.Contains("error"))
                {
                    await DisplayAlert("Error", jsonResponse, "OK");
                    return;
                }

                var emotions = JsonSerializer.Deserialize<List<EmotionEntry>>(jsonResponse);
                if (emotions == null || emotions.Count == 0)
                {
                    await DisplayAlert("Info", "No emotion data found.", "OK");
                    return;
                }

                EmotionData.Clear();
                foreach (var emotion in emotions)
                {
                    EmotionData.Add(new EmotionEntry
                    {
                        Date = emotion.Date,
                        Emotion = emotion.Emotion,
                        Intensity = MapEmotionToIntensity(emotion.Emotion)
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        private int MapEmotionToIntensity(string emotion)
        {
            if (string.IsNullOrEmpty(emotion))
            {
                return 3; // Default intensity if emotion is null or empty
            }

            return emotion.ToLower() switch
            {
                "Happy" => 10,
                "Neutral" => 5,
                "Sad" => 1,
                _ => 3
            };
        }

    }

    public class EmotionEntry
    {
        public string Date { get; set; }
        public string Emotion { get; set; }
        public int Intensity { get; set; }
    }
}
