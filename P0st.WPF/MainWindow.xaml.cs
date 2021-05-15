using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using P0st.Services;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

namespace P0st.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
            Forms.Init();
            
            
            
            // Initializer
            SimpleIoc.Default.Register<IPreferences, PreferencesImplementation>();
            
            LoadApplication(new P0st.App());
        }
    }
}