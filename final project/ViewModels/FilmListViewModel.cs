using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class FilmListViewModel : ViewModelBase
{
    private readonly IFilmRepository _repository;

    public ObservableCollection<Film> Films { get; } = new();

    public FilmListViewModel(IFilmRepository repository)
    {
        _repository = repository;
        LoadFilms();
    }

    private void LoadFilms()
    {
        Films.Clear();
        foreach (var film in _repository.GetAll())
            Films.Add(film);
    }
}