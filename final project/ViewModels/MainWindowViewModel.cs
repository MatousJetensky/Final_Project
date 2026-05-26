// ViewModels/MainWindowViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace final_project.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentView;

    public MainWindowViewModel()
    {
        _currentView = new FilmListViewModel();
    }

    [RelayCommand]
    private void ShowFilmList()
    {
        CurrentView = new FilmListViewModel();
    }
}