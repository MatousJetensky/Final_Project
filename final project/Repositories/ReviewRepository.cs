using System;
using System.Collections.Generic;
using final_project.Models;
using Npgsql;

namespace final_project.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly string _connectionString;

    public ReviewRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Review> GetByFilmId(Guid filmId)
    {
        var reviews = new List<Review>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            SELECT id, film_id, author, rating, content, created_at
            FROM review
            WHERE film_id = @filmId
            ORDER BY created_at DESC", connection);
        command.Parameters.AddWithValue("filmId", filmId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            reviews.Add(new Review
            {
                Id = reader.GetGuid(0),
                FilmId = reader.GetGuid(1),
                Author = reader.IsDBNull(2) ? null : reader.GetString(2),
                Rating = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Content = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = reader.IsDBNull(5) ? null : reader.GetDateTime(5)
            });
        }
        return reviews;
    }

    public void Add(Review review)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            INSERT INTO review (id, film_id, author, rating, content)
            VALUES (@id, @filmId, @author, @rating, @content)", connection);

        command.Parameters.AddWithValue("id", review.Id);
        command.Parameters.AddWithValue("filmId", review.FilmId);
        command.Parameters.AddWithValue("author", (object?)review.Author ?? DBNull.Value);
        command.Parameters.AddWithValue("rating", (object?)review.Rating ?? DBNull.Value);
        command.Parameters.AddWithValue("content", (object?)review.Content ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

    public void Update(Review review)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            UPDATE review SET author = @author, rating = @rating, content = @content
            WHERE id = @id", connection);

        command.Parameters.AddWithValue("id", review.Id);
        command.Parameters.AddWithValue("author", (object?)review.Author ?? DBNull.Value);
        command.Parameters.AddWithValue("rating", (object?)review.Rating ?? DBNull.Value);
        command.Parameters.AddWithValue("content", (object?)review.Content ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

    public void Delete(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            "DELETE FROM review WHERE id = @id", connection);
        command.Parameters.AddWithValue("id", id);
        command.ExecuteNonQuery();
    }
}