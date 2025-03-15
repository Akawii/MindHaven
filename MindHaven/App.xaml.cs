namespace MindHaven
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Ensure NavigationPage is wrapped correctly
            MainPage = new NavigationPage(new LoginPage());

        }
    }
}