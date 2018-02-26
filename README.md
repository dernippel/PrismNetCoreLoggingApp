# PrismNetCoreLoggingApp

This Sample demonstrates the usage of **Microsoft.Extensions.Logging** in a **Xamarin.Forms** App (with **Prism** and **DryIoc** Container) in the same way as in a ASP.NET Core Web-Application.

## Usage
You can use any constructor (e.g. ViewModel, Service, ...) to get the logger injected:
```C#
public MyService(ILogger<MyService> logger)
{
  this.logger = logger;
}
```

## Setup

### Installing Microsoft.Extensions.Logging

Due to a bug (see [here](https://developercommunity.visualstudio.com/content/problem/152947/xamarin-android-run-error-could-not-load-assembly.html) it is not just enought to install the **Microsoft.Extensions.Logging** nuget package.
You had to set the package **System.Runtime.CompilerServices.Unsafe** to a special version and disable a nuget warning in the main project .csproj like that:

```XML
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <NoWarn>$(NoWarn);NU1605</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="2.0.0" />
    <PackageReference Include="Xamarin.Forms" Version="2.5.0.280555" />
    <PackageReference Include="Prism.DryIoc.Forms" Version="7.0.0.396" />
    <!-- Due to a bug in version 4.4.0 use 4.3.0-->
    <PackageReference Include="System.Runtime.CompilerServices.Unsafe" Version="4.3.0" />
  </ItemGroup>

</Project>
```

After that you are able to use the **Microsoft.Extensions.Logging** Package e.g. with the **Microsoft.Extensions.Logging.Debug** package (for further information about the usage look [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x))

### Register Logger in DryIoc Container

At next you had to create the logger factory and register the logger with the container. This shows the registration in the **App.xaml.cs**:
```C#
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

  // get the factory method
  var loggerFactoryMethod = typeof(LoggerFactoryExtensions).GetMethod("CreateLogger", new Type[] { typeof(ILoggerFactory) });

  // register logger with factory method
  container.Register(typeof(ILogger<>), made: Made.Of(req => loggerFactoryMethod.MakeGenericMethod(req.Parent.ImplementationType)));
}
```

## Hint
This is running on **Android** and **WUP**, but I couldn't test **iOS**. If you are able to run this sample successfully on **iOS** please let me know so that I can update this readme.

**Special Thanks** to **[dadhi](https://stackoverflow.com/users/2492669/)** for getting this running at [https://stackoverflow.com/questions/48911710/register-iloggerfactory-in-dryioc-container/](https://stackoverflow.com/questions/48911710/register-iloggerfactory-in-dryioc-container/)
