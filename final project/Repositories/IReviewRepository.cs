using System;
using System.Collections.Generic;
using final_project.Models;

namespace final_project.Repositories;

public interface IReviewRepository
{
    List<Review> GetByFilmId(Guid filmId);
    void Add(Review review);
    void Update(Review review);
    void Delete(Guid id);
}