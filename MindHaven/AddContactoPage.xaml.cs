using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using PhoneNumbers;

namespace MindHaven
{
    public partial class AddContactoPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler());
        private string selectedRelationship;
        private string selectedCountryPrefix;
        private List<CountryInfo> countryList;
        private bool isMenuOpen = false;

        public List<CountryInfo> CountryList
        {
            get => countryList;
            set => countryList = value;
        }

        public AddContactoPage()
        {
            InitializeComponent();
            LoadCountries();
            BindingContext = this; // Set the binding context for the CountryPicker
        }

        private void LoadCountries()
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var regions = phoneNumberUtil.GetSupportedRegions();

            countryList = regions
                .Select(region =>
                {
                    int countryCode = phoneNumberUtil.GetCountryCodeForRegion(region);
                    return new CountryInfo
                    {
                        RegionCode = region,
                        CountryCode = $"+{countryCode}",
                        DisplayName = $"{GetCountryName(region)} (+{countryCode})"
                    };
                })
                .OrderBy(c => c.DisplayName)
                .ToList();

            // Optionally set a default selection (e.g., United States)
            var defaultCountry = countryList.FirstOrDefault(c => c.RegionCode == "US");
            if (defaultCountry != null)
            {
                CountryPicker.SelectedItem = defaultCountry;
            }
        }

        private string GetCountryName(string regionCode)
        {
            try
            {
                var regionInfo = new System.Globalization.RegionInfo(regionCode);
                return regionInfo.EnglishName;
            }
            catch
            {
                return regionCode; // Fallback to region code if the name isn't found
            }
        }

        private void OnCountrySelected(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem is CountryInfo selectedCountry)
            {
                selectedCountryPrefix = selectedCountry.CountryCode;
                PhoneEntry.Placeholder = $"Enter phone number (e.g., 1234567890 for {selectedCountryPrefix})";
                PhoneEntry.Text = selectedCountryPrefix; // Prepend the prefix
            }
        }

        private void OnRelationshipSelected(object sender, EventArgs e)
        {
            if (sender is Picker picker && picker.SelectedItem != null)
            {
                selectedRelationship = picker.SelectedItem.ToString();
            }
        }

        private async void OnMenuButtonClicked(object sender, EventArgs e)
        {
            if (MenuPopup.IsVisible)
            {
                await MenuPopup.TranslateTo(-250, 0, 250, Easing.Linear);
                MenuPopup.IsVisible = false;
            }
            else
            {
                MenuPopup.IsVisible = true;
                await MenuPopup.TranslateTo(0, 0, 250, Easing.Linear);
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

        private async void OnReportsClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new ReportsPage();
        }

        private async void OnMainMenuClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new MainMenuPage();
        }

        private async void OnPasswordClicked(object sender, EventArgs e)
        {
            await CloseMenu();
            Application.Current.MainPage = new ChangePasswordPage();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string name = NameEntry.Text;
            string phone = PhoneEntry.Text;
            string relationship = selectedRelationship;
            int userId = Preferences.Get("UserId", 0);

            // Validate input
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Error", "Please enter a name.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                await DisplayAlert("Error", "Please enter a phone number.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedCountryPrefix))
            {
                await DisplayAlert("Error", "Please select a country.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(relationship))
            {
                await DisplayAlert("Error", "Please select a relationship.", "OK");
                return;
            }

            if (userId == 0)
            {
                await DisplayAlert("Error", "User not logged in. Please log in again.", "OK");
                return;
            }

           
            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                PhoneNumber number = phoneNumberUtil.Parse(phone, selectedCountryPrefix.Substring(1)); // Substring remove o '+' do prefixo

                // Verificar se o número é válido
                if (!phoneNumberUtil.IsValidNumber(number))
                {
                    await DisplayAlert("Error", "The phone number is invalid.", "OK");
                    return;
                }

                // Formatar o número no formato E.164
                string formattedPhone = phoneNumberUtil.Format(number, PhoneNumberFormat.E164);

                // Prepare the data to send to the API
                var contactData = new
                {
                    user_id = userId,
                    name,
                    phone = formattedPhone, // Usar o número formatado
                    relationship,
                    created_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                string jsonData = JsonSerializer.Serialize(contactData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    // Send the data to the API
                    var response = await client.PostAsync("http://172.20.10.2/mindhaven/emergencycontacts.php", content);
                    string result = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JsonSerializer.Deserialize<ResponseData>(result);

                    if (jsonResponse != null && jsonResponse.status == "success")
                    {
                        await DisplayAlert("Success", "Emergency contact added successfully!", "OK");
                        // Clear the form
                        NameEntry.Text = string.Empty;
                        PhoneEntry.Text = string.Empty;
                        CountryPicker.SelectedItem = null;
                        selectedCountryPrefix = null;
                        RelationshipPicker.SelectedItem = null;
                        selectedRelationship = null;
                        // Navigate back to the previous page
                        Application.Current.MainPage = new DataUser();
                    }
                    else
                    {
                        await DisplayAlert("Error", jsonResponse?.message ?? "Failed to save contact.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
            catch (NumberParseException)
            {
                await DisplayAlert("Error", "Invalid phone number format.", "OK");
            }
        }

        public class CountryInfo
        {
            public string RegionCode { get; set; }
            public string CountryCode { get; set; }
            public string DisplayName { get; set; }
        }

        public class ResponseData
        {
            public string status { get; set; }
            public string message { get; set; }
        }
    }
}
