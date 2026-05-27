using System;
using System.Linq;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class FilmFormViewModel : ViewModelBase
{
    private readonly IFilmRepository _repository;
    private readonly Action _onSaved;
    private readonly Guid? _editId;

    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _director = string.Empty;
    [ObservableProperty] private string _year = string.Empty;
    [ObservableProperty] private string _genre = string.Empty;
    [ObservableProperty] private FilmStatus? _selectedStatus;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private bool _isEditMode;

    public ObservableCollection<FilmStatus> Statuses { get; } = new();

    // Konstruktor pro PŘIDÁNÍ
    public FilmFormViewModel(IFilmRepository repository, Action onSaved)
    {
        _repository = repository;
        _onSaved = onSaved;
        IsEditMode = false;
        LoadStatuses();
    }

    // Konstruktor pro EDITACI
    public FilmFormViewModel(IFilmRepository repository, Action onSaved, Film film)
    {
        _repository = repository;
        _onSaved = onSaved;
        _editId = film.Id;
        IsEditMode = true;

        Title = film.Title;
        Director = film.Director ?? string.Empty;
        Year = film.Year?.ToString() ?? string.Empty;
        Genre = film.Genre ?? string.Empty;

        LoadStatuses();
        SelectedStatus = Statuses.FirstOrDefault(s => s.Id == film.StatusId);
    }

    private void LoadStatuses()
    {
        foreach (var s in _repository.GetAllStatuses())
            Statuses.Add(s);
        if (SelectedStatus == null && Statuses.Count > 0)
            SelectedStatus = Statuses[0];
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            ErrorMessage = "Název filmu je povinný.";
            return;
        }
        if (SelectedStatus == null)
        {
            ErrorMessage = "Vyber status.";
            return;
        }

        var film = new Film
        {
            Id = _editId ?? Guid.NewGuid(),
            Title = Title,
            Director = string.IsNullOrWhiteSpace(Director) ? null : Director,
            Year = int.TryParse(Year, out var y) ? y : null,
            Genre = string.IsNullOrWhiteSpace(Genre) ? null : Genre,
            StatusId = SelectedStatus.Id
        };

        if (IsEditMode)
            _repository.Update(film);
        else
            _repository.Add(film);

        _onSaved();
    }

    [RelayCommand]
    private void Delete()
    {
        if (_editId.HasValue)
            _repository.Delete(_editId.Value);
        _onSaved();
    }
}