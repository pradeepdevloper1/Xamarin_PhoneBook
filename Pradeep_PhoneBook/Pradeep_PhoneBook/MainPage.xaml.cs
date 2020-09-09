using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Pradeep_PhoneBook.Model;

namespace Pradeep_PhoneBook
{
    public partial class MainPage : ContentPage
    {
        public User user;
        public MainPage()
        {
            this.Title = "Pradeep PoneBook";
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            PopulateEmployeeList();
        }
        public void PopulateEmployeeList()
        {
            EmployeeList.ItemsSource = null;
            EmployeeList.ItemsSource = DependencyService.Get<ISQLite>().GetUser();
        }
        public async void AddEmployee(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new AddUser(null));

        }
        private void EditEmployee(object sender, ItemTappedEventArgs e)
        {
            User details = e.Item as User;
            if (details != null)
            {
                Navigation.PushAsync(new AddUser(details));
            }
        }
        private async void DeleteEmployee(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Message", "Do you want to delete employee?", "Ok", "Cancel");
            if (res)
            {
                var menu = sender as MenuItem;
                User details = menu.CommandParameter as User;
                DependencyService.Get<ISQLite>().DeleteUser(details.Id);
                PopulateEmployeeList();
            }
        }

        private void callPerson_Clicked(object sender, EventArgs e)
        {
            var data = (Xamarin.Forms.Button)sender;

            string PhoneNumber = (data.CommandParameter).ToString();
            try
            {
                PhoneDialer.Open(PhoneNumber);

            }
            catch (Exception ex)
            {
                DisplayAlert("Message ", "Unable to Make Call", "Ok");
            }
        }


    }
}
