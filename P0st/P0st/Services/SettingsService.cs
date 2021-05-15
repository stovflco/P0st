using System;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;

namespace P0st.Services
{
    public class SettingsService
    {
        private IPreferences _preferences;
        
        public SettingsService()
        {
            if (SimpleIoc.Default.IsRegistered<IPreferences>())
            {
                this._preferences = SimpleIoc.Default.GetInstance<IPreferences>();
            }

            if (this._preferences == null)
            {
                throw new Exception("Preferences");
            }
        }

        public string Me
        {
            get => this._preferences.Get("Me", string.Empty);
            set => this._preferences.Set("Me", value);
        }
    }
}