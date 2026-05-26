using System;
using System.Collections.Generic;
using final_project.Models;
using Npgsql;

namespace final_project.Repositories;

public class FilmRepository : IFilmRepository
{
    private readonly string _connectionString;

    public FilmRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Film> GetAll()
    {
        var films = new List<Film>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            SELECT f.id, f.title, f.director, f.year, f.genre, f.status_id, s.name
            FROM film f
            LEFT JOIN film_status s ON f.status_id = s.id", connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            films.Add(new Film
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(1),
                Director = reader.IsDBNull(2) ? null : reader.GetString(2),
                Year = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Genre = reader.IsDBNull(4) ? null : reader.GetString(4),
                StatusId = reader.GetGuid(5),
                StatusName = reader.IsDBNull(6) ? null : reader.GetString(6)
            });
        }
        return films;
    }

    public Film? GetById(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            SELECT f.id, f.title, f.director, f.year, f.genre, f.status_id, s.name
            FROM film f
            LEFT JOIN film_status s ON f.status_id = s.id
            WHERE f.id = @id", connection);
        command.Parameters.AddWithValue("id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Film
            {
                Id = reader.GetGuid(0),
                Title = reader.GetString(1),
                Director = reader.IsDBNull(2) ? null : reader.GetString(2),
                Year = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                Genre = reader.IsDBNull(4) ? null : reader.GetString(4),
                StatusId = reader.GetGuid(5),
                StatusName = reader.IsDBNull(6) ? null : reader.GetString(6)
            };
        }
        return null;
    }

    public void Add(Film film)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            INSERT INTO film (id, title, director, year, genre, status_id)
            VALUES (@id, @title, @director, @year, @genre, @statusId)", connection);

        command.Parameters.AddWithValue("id", film.Id);
        command.Parameters.AddWithValue("title", film.Title);
        command.Parameters.AddWithValue("director", (object?)film.Director ?? DBNull.Value);
        command.Parameters.AddWithValue("year", (object?)film.Year ?? DBNull.Value);
        command.Parameters.AddWithValue("genre", (object?)film.Genre ?? DBNull.Value);
        command.Parameters.AddWithValue("statusId", film.StatusId);

        command.ExecuteNonQuery();
    }

    public void Update(Film film)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(@"
            UPDATE film SET title = @title, director = @director, year = @year,
            genre = @genre, status_id = @statusId WHERE id = @id", connection);

        command.Parameters.AddWithValue("id", film.Id);
        command.Parameters.AddWithValue("title", film.Title);
        command.Parameters.AddWithValue("director", (object?)film.Director ?? DBNull.Value);
        command.Parameters.AddWithValue("year", (object?)film.Year ?? DBNull.Value);
        command.Parameters.AddWithValue("genre", (object?)film.Genre ?? DBNull.Value);
        command.Parameters.AddWithValue("statusId", film.StatusId);

        command.ExecuteNonQuery();
    }

    public void Delete(Guid id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            "DELETE FROM film WHERE id = @id", connection);
        command.Parameters.AddWithValue("id", id);
        command.ExecuteNonQuery();
    }

    public List<FilmStatus> GetAllStatuses()
    {
        var statuses = new List<FilmStatus>();
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new NpgsqlCommand(
            "SELECT id, name FROM film_status", connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            statuses.Add(new FilmStatus
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1)
            });
        }
        return statuses;
    }
}