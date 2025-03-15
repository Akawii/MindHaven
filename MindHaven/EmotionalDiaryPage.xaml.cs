using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage; // Needed for Preferences

namespace MindHaven
{
    public partial class EmotionalDiaryPage : FlyoutPage
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler());

        public EmotionalDiaryPage()
        {
            InitializeComponent();
            dateLabel.Text = DateTime.Now.ToString("yyyy-MM-dd"); // Auto-fill date
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string emotion = emotionEntry.Text;
            string note = noteEditor.Text;
            string date = dateLabel.Text;

            // Get stored User ID from login
            int userId = Preferences.Get("UserId", 0);

            if (string.IsNullOrWhiteSpace(emotion) || string.IsNullOrWhiteSpace(note))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
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
                emotion,
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
                    
                    emotionEntry.Text = string.Empty;
                    noteEditor.Text = string.Empty;
                    
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
