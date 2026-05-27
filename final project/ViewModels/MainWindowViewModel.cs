using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFilmRepository _repository;

    [ObservableProperty]
    private ViewModelBase _currentView;

    public MainWindowViewModel(IFilmRepository repository)
    {
        _repository = repository;
        _currentView = new FilmListViewModel(repository, ShowFilmForm, ShowFilmEdit);
    }

    [RelayCommand]
    private void ShowFilmList()
    {
        CurrentView = new FilmListViewModel(_repository, ShowFilmForm, ShowFilmEdit);
    }

    [RelayCommand]
    private void ShowFilmForm()
    {
        CurrentView = new FilmFormViewModel(_repository, ShowFilmList, ShowFilmList);
    }

    [RelayCommand]
    private void ShowFilmEdit(Film film)
    {
        CurrentView = new FilmFormViewModel(_repository, ShowFilmList, film, ShowFilmList);
    }

    [RelayCommand]
    private void FilterByStatus(string? status)
    {
        if (CurrentView is FilmListViewModel listVm)
            listVm.FilterByStatusCommand.Execute(status);
    }
}