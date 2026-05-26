using System;
using DotNetEnv;
using final_project.Repositories;
using final_project.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace final_project;

public class Services
{
    public static IServiceProvider ServiceCollection()
    {
        Env.TraversePath().Load();
        var services = new ServiceCollection();

        var connectionString =
            $"Host={Env.GetString("HOST")};Port={Env.GetString("PORT")};Database={Env.GetString("DATABASE")};Username={Env.GetString("USERNAME")};Password={Env.GetString("PASSWORD")};SSL Mode=Disable";

        services.AddSingleton<IFilmRepository>(new FilmRepository(connectionString));
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<FilmListViewModel>();

        return services.BuildServiceProvider();
    }
}