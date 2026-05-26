using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        _currentView = new FilmListViewModel(repository);
    }

    [RelayCommand]
    private void ShowFilmList()
    {
        CurrentView = new FilmListViewModel(_repository);
    }
}