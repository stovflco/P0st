using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace P0st.Services
{
    public class NavigationService : INavigationService
    {
        // Dictionary with registered pages in the app:
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();

        // Navigation page where MainPage is hosted:
        private NavigationPage _navigation;

        // Get currently displayed page:
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = _navigation.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key.ToString()
                        : null;
                }
            }
        }

        // GoBack implementation (just pop page from the navigation stack):
        public void GoBack()
        {
            _navigation.PopAsync();
        }

        // NavigateTo method to navigate between pages without passing parameter:
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateAndCleanTo(string pageKey, object parameter = null)
        {
            NavigateTo(pageKey, parameter);

            var pages = _navigation.Pages.ToArray();
            for (var i = 0; i < pages.Length - 1; i++)
            {
                _navigation.Navigation.RemovePage(pages[i]);
            }
        }
        
        // NavigateTo method to navigate between pages with passing parameter:
        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {

                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                            parameter
                        };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;
                    _navigation.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey), nameof(pageKey));
                }
            }
        }

        // Register pages and add them to the dictionary:
        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        // Initialize first app page (navigation page):
        public void Initialize(NavigationPage navigation)
        {
            _navigation = navigation;
        }
    }
}