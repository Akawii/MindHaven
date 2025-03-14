using Microsoft.Maui.Controls;
using System;

namespace MindHaven
{
    public partial class EmotionalDiaryPage : FlyoutPage
    {
        public EmotionalDiaryPage()
        {
            InitializeComponent();
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MindHaven.MainMenuPage());
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Saved", "Your emotion has been recorded!", "OK");
        }
    }
}
