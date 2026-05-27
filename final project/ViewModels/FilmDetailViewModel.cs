using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using final_project.Models;
using final_project.Repositories;

namespace final_project.ViewModels;

public partial class FilmDetailViewModel : ViewModelBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly Action _onBack;

    public Film Film { get; }

    [ObservableProperty] private string _author = string.Empty;
    [ObservableProperty] private string _rating = string.Empty;
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private Review? _selectedReview;

    public ObservableCollection<Review> Reviews { get; } = new();

    public FilmDetailViewModel(Film film, IReviewRepository reviewRepository, Action onBack)
    {
        Film = film;
        _reviewRepository = reviewRepository;
        _onBack = onBack;
        LoadReviews();
    }

    private void LoadReviews()
    {
        Reviews.Clear();
        foreach (var r in _reviewRepository.GetByFilmId(Film.Id))
            Reviews.Add(r);
    }

    partial void OnSelectedReviewChanged(Review? value)
    {
        if (value == null) return;
        Author = value.Author ?? string.Empty;
        Rating = value.Rating?.ToString() ?? string.Empty;
        Content = value.Content ?? string.Empty;
    }

    [RelayCommand]
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(Author))
        {
            ErrorMessage = "Autor je povinný.";
            return;
        }
        if (!int.TryParse(Rating, out var r) || r < 1 || r > 10)
        {
            ErrorMessage = "Hodnocení musí být číslo 1–10.";
            return;
        }

        ErrorMessage = string.Empty;

        if (SelectedReview == null)
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                FilmId = Film.Id,
                Author = Author,
                Rating = r,
                Content = string.IsNullOrWhiteSpace(Content) ? null : Content
            };
            _reviewRepository.Add(review);
        }
        else
        {
            SelectedReview.Author = Author;
            SelectedReview.Rating = r;
            SelectedReview.Content = string.IsNullOrWhiteSpace(Content) ? null : Content;
            _reviewRepository.Update(SelectedReview);
            SelectedReview = null;
        }

        ClearForm();
        LoadReviews();
    }

    [RelayCommand]
    private void Delete()
    {
        if (SelectedReview == null) return;
        _reviewRepository.Delete(SelectedReview.Id);
        SelectedReview = null;
        ClearForm();
        LoadReviews();
    }

    [RelayCommand]
    private void ClearForm()
    {
        Author = string.Empty;
        Rating = string.Empty;
        Content = string.Empty;
        ErrorMessage = string.Empty;
        SelectedReview = null;
    }

    [RelayCommand]
    private void GoBack() => _onBack();
}