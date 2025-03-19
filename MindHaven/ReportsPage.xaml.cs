using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Syncfusion.Maui.Charts;
using System.Text.Json.Serialization;

namespace MindHaven
{
    public partial class ReportsPage : ContentPage
    {
        private bool isMenuOpen = false;

        public ObservableCollection<EmotionEntry> EmotionData { get; set; } = new();
        public ObservableCollection<NoteEntry> NotesData { get; set; } = new();
        private static readonly HttpClient client = new();

        public ReportsPage()
        {
            InitializeComponent();
            BindingContext = this;
            LoadEmotionData();
            LoadNotesData();
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
                var response = await client.PostAsJsonAsync("http://localhost/mindhaven/reports.php", requestData);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var emotions = JsonSerializer.Deserialize<List<EmotionEntry>>(jsonResponse, options);

                if (emotions != null)
                {
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
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }

        private int MapEmotionToIntensity(string emotion)
        {
            return emotion switch
            {
                "Excited" => 10,
                "Happy" => 8,
                "Neutral" => 6,
                "Sad" => 4,
                "Angry" => 2
            };
        }

        private async void LoadNotesData()
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
                var response = await client.PostAsJsonAsync("http://localhost/mindhaven/get_notes.php", requestData);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var notes = JsonSerializer.Deserialize<List<NoteEntry>>(jsonResponse, options);

                if (notes != null)
                {
                    NotesData.Clear();
                    foreach (var note in notes)
                    {
                        NotesData.Add(note);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load notes: {ex.Message}", "OK");
            }
        }

        private void OnLabelCreated(object sender, ChartAxisLabelEventArgs e)
        {
            if (int.TryParse(e.Label, out int value)) // Try to parse the label to an integer
            {
                e.Label = value switch
                {
                    10 => "Excited",
                    8 => "Happy",
                    6 => "Neutral",
                    4 => "Sad",
                    2 => "Angry",
                    _ => e.Label 
                };
            }
        }

        public class EmotionEntry
        {
            public string Date { get; set; }
            public string Emotion { get; set; }
            public int Intensity { get; set; }
        }

        public class NoteEntry
        {
            public string Date { get; set; }
            [JsonPropertyName("note")] public string Content { get; set; }
        }

        private async void OnEmotionalDiaryClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new EmotionalDiaryPage();
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new MainMenuPage();
        }


        private async void OnMenuButtonClicked(object sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                await CloseMenu();
            }
            else
            {
                MenuPopup.IsVisible = true;
                await MenuPopup.TranslateTo(0, 0, 250, Easing.CubicIn);
                isMenuOpen = true;
            }
        }


        private async Task CloseMenu()
        {
            if (isMenuOpen)
            {
                await MenuPopup.TranslateTo(-250, 0, 250, Easing.CubicOut);
                MenuPopup.IsVisible = false;
                isMenuOpen = false;
            }
        }
    }
}