﻿using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookHub.DAL.Repositories.Implementations
{
    public class ReviewRepository : IRepository<ReviewEntity>, IReviewRepository<ReviewEntity>
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReviewEntity entity)
        {
            _context.Reviews.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(ReviewEntity entity)
        {
            _context.Reviews.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(ReviewEntity entity)
        {
            _context.Reviews.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<ReviewEntity> GetByIdAsync(int id)
        {
            return _context.Reviews.Find(id);
        }

        public async Task<IEnumerable<ReviewEntity>> GetAllAsync()
        {
            return _context.Reviews.ToList();
        }

        public async Task<(List<ReviewEntity> Items, long TotalCount)> GetPagedAsync(int pageSize, int pageNumber)
        {
            var totalCount = await _context.Reviews.CountAsync();

            var items = await _context.Reviews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
