using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class FilmFormViewModel : ViewModelBase
{
    private readonly IFilmRepository _repository;
    private readonly System.Action _onSaved;

    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _director = string.Empty;
    [ObservableProperty] private string _year = string.Empty;
    [ObservableProperty] private string _genre = string.Empty;
    [ObservableProperty] private FilmStatus? _selectedStatus;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public ObservableCollection<FilmStatus> Statuses { get; } = new();

    public FilmFormViewModel(IFilmRepository repository, System.Action onSaved)
    {
        _repository = repository;
        _onSaved = onSaved;
        LoadStatuses();
    }

    private void LoadStatuses()
    {
        foreach (var s in _repository.GetAllStatuses())
            Statuses.Add(s);
        if (Statuses.Count > 0)
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
            Id = System.Guid.NewGuid(),
            Title = Title,
            Director = string.IsNullOrWhiteSpace(Director) ? null : Director,
            Year = int.TryParse(Year, out var y) ? y : null,
            Genre = string.IsNullOrWhiteSpace(Genre) ? null : Genre,
            StatusId = SelectedStatus.Id
        };

        _repository.Add(film);
        _onSaved();
    }
}