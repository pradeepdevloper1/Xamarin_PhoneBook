using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pradeep_PhoneBook.Model;
using Plugin.Media;
using Xamarin.Essentials;

namespace Pradeep_PhoneBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUser : ContentPage
    {
        public User userDetails;
        string sImagePath = "";

        User _selectedEmployee;
        public User SelectedUser
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                if (value != null)
                {
                    Navigation.PushAsync(new AddUser(SelectedUser));
                }
                OnPropertyChanged();
            }
        }
        public AddUser(User details)
        {
           
            InitializeComponent();
            this.Title = "Add Contact";
            DeleteUser.IsVisible = false;
            MakeCall.IsVisible = false;
            if (details != null)
            {
                DeleteUser.IsVisible = true;
                MakeCall.IsVisible = true;
                userDetails = details;
                PopulateDetails(userDetails);
            }
        }

        private void PopulateDetails(User details)
        {
            Name.Text = details.Name;
            Address.Text = details.Address;
            PhoneNumber.Text = details.PhoneNumber;
            Email.Text = details.Email;
            Password.Text = details.Password;
            Password.IsEnabled = false;
            sImagePath = details.ImagePath;
            myImage.Source = details.ImagePath;
            SaveButton.Text = "Update";
            this.Title = "Edit User";
        }

        private void SaveEmployee(object sender, EventArgs e)
        {
            if (SaveButton.Text == "Save")
            {
                User user = new User();
                user.Name = Name.Text;
                user.Address = Address.Text;
                user.PhoneNumber = PhoneNumber.Text;
                user.Email = Email.Text;
                user.Password = Password.Text;
                user.ImagePath = sImagePath;

                bool response = DependencyService.Get<ISQLite>().SaveUser(user);

                if (response)
                {
                    Navigation.PopAsync();
                }

                else
                {
                    DisplayAlert("Message", "Data Failed to Save", "OK");
                }
            }
            else
            {
                //update employee
                userDetails.Name = Name.Text;
                userDetails.Address = Address.Text;
                userDetails.PhoneNumber = PhoneNumber.Text;
                userDetails.Email = Email.Text;
                userDetails.ImagePath = sImagePath;

                bool response = DependencyService.Get<ISQLite>().UpdateUser(userDetails);

                if (response)
                {
                    Navigation.PopAsync();
                }

                else
                {
                    DisplayAlert("Message", "Data Failed to Save", "OK");
                }
            }
        }

        private void MakeCall_Clicked(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open(PhoneNumber.Text);

            }
            catch (Exception ex)
            {
                DisplayAlert("Message ", "Unable to Make Call", "Ok");
            }

            // DependencyService.Get<IPhoneCall>().makeCall(PhoneNumber.Text.ToString());

        }


        private async void btnCaptureImg_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            //var cameraStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            //var storageStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (!CrossMedia.Current.IsTakePhotoSupported && !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Message", "Photo captured or picked is not supported", "OK");
                return;
            }
            else
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Images",
                    Name = DateTime.Now + "_test.jpg"
                });
                if (file == null)
                    return;
                sImagePath = file.Path;
                await DisplayAlert("File Path", file.Path, "OK");
                myImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

            }

        }


        private async void btnGalleryImg_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Ops", " cannot access Gallery ", "OK");

                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            myImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            });
        }

        private void DeleteUser_Clicked(object sender, EventArgs e)
        {

      

            bool response = DependencyService.Get<ISQLite>().DeleteUser(userDetails.Id);

            if (response)
            {
                Navigation.PopAsync();
            }

            else
            {
                DisplayAlert("Message", "Data Failed to Delete", "OK");
            }

        }
    }
}