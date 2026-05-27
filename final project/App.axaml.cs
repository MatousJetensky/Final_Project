using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using final_project.ViewModels;
using final_project.Views;
using Microsoft.Extensions.DependencyInjection;

namespace final_project;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            try
            {
                ServiceProvider = Services.ServiceCollection();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>(),
                };
            }
            catch (Exception ex)
            {
                var errorWindow = new Window
                {
                    Title = "Chyba připojení",
                    Width = 400,
                    Height = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new StackPanel
                    {
                        Margin = new Thickness(20),
                        Spacing = 12,
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "Nepodařilo se připojit k databázi.",
                                FontSize = 16,
                                FontWeight = FontWeight.Bold
                            },
                            new TextBlock
                            {
                                Text = "Zkontroluj že Docker běží a kontejner je spuštěný.",
                                TextWrapping = TextWrapping.Wrap,
                                Opacity = 0.7
                            },
                            new TextBlock
                            {
                                Text = $"Detail: {ex.Message}",
                                FontSize = 11,
                                Opacity = 0.5,
                                TextWrapping = TextWrapping.Wrap
                            }
                        }
                    }
                };

                desktop.MainWindow = errorWindow;
            }
        }  // ← tato závorka chyběla

        base.OnFrameworkInitializationCompleted();
    }  // ← konec OnFrameworkInitializationCompleted

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}