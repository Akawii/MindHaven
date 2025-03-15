using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace MindHaven
{
    public partial class EmotionalDiaryPage : FlyoutPage
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler());
        private string selectedEmotion;

        public EmotionalDiaryPage()
        {
            InitializeComponent();
            dateLabel.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void OnEmotionSelected(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                selectedEmotion = button.StyleId;

                foreach (var child in emotionButtons.Children)
                {
                    if (child is Button btn)
                    {
                        btn.Opacity = btn.StyleId == selectedEmotion ? 1 : 0.5;
                        btn.Scale = btn.StyleId == selectedEmotion ? 1.1 : 1;
                    }
                }
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string note = noteEditor.Text;
            string date = dateLabel.Text;
            int userId = Preferences.Get("UserId", 0);

            if (string.IsNullOrWhiteSpace(selectedEmotion) || string.IsNullOrWhiteSpace(note))
            {
                await DisplayAlert("Error", "Please select an emotion and write a note.", "OK");
                return;
            }

            if (userId == 0)
            {
                await DisplayAlert("Error", "User not logged in. Please log in again.", "OK");
                return;
            }

            var emotionData = new
            {
                user_id = userId,
                emotion = selectedEmotion,
                note,
                date
            };

            string jsonData = JsonSerializer.Serialize(emotionData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost/mindhaven/emotionaldiary.php", content);
                string result = await response.Content.ReadAsStringAsync();

                var jsonResponse = JsonSerializer.Deserialize<ResponseData>(result);

                if (jsonResponse != null && jsonResponse.status == "success")
                {
                    await DisplayAlert("Success", "Your emotion has been recorded!", "OK");
                    noteEditor.Text = string.Empty;
                    selectedEmotion = null;
                    foreach (var child in emotionButtons.Children)
                    {
                        if (child is Button btn)
                        {
                            btn.Opacity = 1;
                            btn.Scale = 1;
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", jsonResponse?.message ?? "Failed to save entry.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainMenuPage();
        }

        private class ResponseData
        {
            public string status { get; set; }
            public string message { get; set; }
        }
    }
}