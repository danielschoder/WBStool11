using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Windows;
using WBStool11.Services;
using WBStool11.ViewModels;

namespace WBStool11;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IFileService, FileService>();
                services.AddSingleton<IProjectService, ProjectService>();
                services.AddSingleton<ProjectViewModel>();
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        CorrectMenuDropAlignment();

        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _host.Dispose();
        base.OnExit(e);
    }

    private static void CorrectMenuDropAlignment()
    {
        var menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
        setAlignmentValue();
        SystemParameters.StaticPropertyChanged += (sender, e) => { setAlignmentValue(); };

        void setAlignmentValue()
        {
            if (SystemParameters.MenuDropAlignment && menuDropAlignmentField != null) menuDropAlignmentField.SetValue(null, false);
        }
    }
}
