using System;
using System.Collections.Generic;
using final_project.Models;

namespace final_project.Repositories;

public interface IFilmRepository
{
    List<Film> GetAll();
    Film? GetById(Guid id);
    void Add(Film film);
    void Update(Film film);
    void Delete(Guid id);
    List<FilmStatus> GetAllStatuses();
}