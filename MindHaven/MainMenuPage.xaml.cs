using Microsoft.Maui.Controls;

namespace MindHaven
{
    public partial class MainMenuPage : FlyoutPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }
        private async void OnEmotionalDiaryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MindHaven.EmotionalDiaryPage());
        }
    }
}
