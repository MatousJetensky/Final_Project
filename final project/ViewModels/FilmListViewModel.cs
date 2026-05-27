using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class FilmListViewModel : ViewModelBase
{
    private readonly IFilmRepository _repository;
    private readonly Action _onAddFilm;
    private readonly Action<Film> _onSelectFilm;
    private List<Film> _allFilms = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string? _selectedStatus = null;

    public ObservableCollection<Film> Films { get; } = new();

    public FilmListViewModel(IFilmRepository repository, Action onAddFilm, Action<Film> onSelectFilm)
    {
        _repository = repository;
        _onAddFilm = onAddFilm;
        _onSelectFilm = onSelectFilm;
        LoadFilms();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();
    partial void OnSelectedStatusChanged(string? value) => ApplyFilter();

    private void LoadFilms()
    {
        try
        {
            _allFilms = _repository.GetAll();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB chyba: {ex.Message}");
        }
    }

    private void ApplyFilter()
    {
        Films.Clear();
        var filtered = _allFilms.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
            filtered = filtered.Where(f =>
                f.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                (f.Director?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false));

        if (!string.IsNullOrWhiteSpace(SelectedStatus))
            filtered = filtered.Where(f => f.StatusName == SelectedStatus);

        foreach (var film in filtered)
            Films.Add(film);
    }

    [RelayCommand]
    private void ShowAddFilm() => _onAddFilm();

    [RelayCommand]
    private void FilterByStatus(string? status)
    {
        SelectedStatus = SelectedStatus == status ? null : status;
    }

    [RelayCommand]
    private void SelectFilm(Film film) => _onSelectFilm(film);
}