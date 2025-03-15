using Microsoft.Maui.Controls;
using Syncfusion.Maui.Core.Hosting;

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
            Application.Current.MainPage = new EmotionalDiaryPage();
        }

        private async void OnReportsPageClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ReportsPage();
        }

        
    }

}
