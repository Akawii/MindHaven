using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace MindHaven
{
    public partial class MainMenuPage : ContentPage
    {
        private bool isMenuOpen = false;

        public MainMenuPage()
        {
            InitializeComponent();
        }

        private async void OnEmotionalDiaryClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new EmotionalDiaryPage();
        }

        private async void OnUserProfilePageClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new UserProfilePage();
        }


        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new ReportsPage();
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
