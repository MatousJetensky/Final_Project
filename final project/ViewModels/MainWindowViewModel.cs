using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFilmRepository _filmRepository;
    private readonly IReviewRepository _reviewRepository;

    [ObservableProperty]
    private ViewModelBase _currentView;

    public MainWindowViewModel(IFilmRepository filmRepository, IReviewRepository reviewRepository)
    {
        _filmRepository = filmRepository;
        _reviewRepository = reviewRepository;
        _currentView = new FilmListViewModel(filmRepository, ShowFilmForm, ShowFilmDetail);
    }

    [RelayCommand]
    private void ShowFilmList()
    {
        CurrentView = new FilmListViewModel(_filmRepository, ShowFilmForm, ShowFilmDetail);
    }

    [RelayCommand]
    private void ShowFilmForm()
    {
        CurrentView = new FilmFormViewModel(_filmRepository, ShowFilmList, ShowFilmList);
    }

    [RelayCommand]
    private void ShowFilmEdit(Film film)
    {
        CurrentView = new FilmFormViewModel(_filmRepository, ShowFilmList, film, ShowFilmList);
    }

    [RelayCommand]
    private void ShowFilmDetail(Film film)
    {
        try
        {
            CurrentView = new FilmDetailViewModel(film, _reviewRepository, ShowFilmList);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CHYBA: {ex}");
            throw;
        }
    }

    [RelayCommand]
    private void FilterByStatus(string? status)
    {
        if (CurrentView is FilmListViewModel listVm)
            listVm.FilterByStatusCommand.Execute(status);
    }
    
}