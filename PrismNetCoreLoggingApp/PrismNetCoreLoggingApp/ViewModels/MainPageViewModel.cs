using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismNetCoreLoggingApp.ViewModels
{
    using PrismNetCoreLoggingApp.Interfaces;

    public class MainPageViewModel : ViewModelBase
    {
        private readonly IMyService myService;

        private int counter;

        public MainPageViewModel(INavigationService navigationService, IMyService myService) 
            : base (navigationService)
        {
            this.myService = myService;
            Title = "Main Page";
        }

        public int Counter
        {
            get => this.counter;
            set => this.SetProperty(ref this.counter, value);
        }

        public DelegateCommand DoSomethingCommand => new DelegateCommand(this.OnDoSomething);

        private void OnDoSomething()
        {
            this.Counter++;

            this.myService.DoSomething(this.Counter);
        }
    }
}
