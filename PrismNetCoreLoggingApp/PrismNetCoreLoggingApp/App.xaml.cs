using Prism;
using Prism.Ioc;
using PrismNetCoreLoggingApp.ViewModels;
using PrismNetCoreLoggingApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PrismNetCoreLoggingApp
{
    using DryIoc;

    using Microsoft.Extensions.Logging;

    using PrismNetCoreLoggingApp.Interfaces;
    using PrismNetCoreLoggingApp.Services;

    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();

            // register services
            containerRegistry.RegisterSingleton<IMyService, MyService>();

            // create logger factory
            ILoggerFactory loggerFactory = new LoggerFactory().AddDebug();

            // get the container
            var container = containerRegistry.GetContainer();

            // register factory
            container.UseInstance(loggerFactory);

            // get factory method
            var loggerFactoryMethod = typeof(LoggerFactory).GetMethod("CreateLogger");

            // register ILogger<T>
            container.Register(
                typeof(ILogger<>),
                made: Made.Of(
                    req => loggerFactoryMethod.MakeGenericMethod(req.Parent.ImplementationType),
                    ServiceInfo.Of<LoggerFactory>()));

        }
    }
}
