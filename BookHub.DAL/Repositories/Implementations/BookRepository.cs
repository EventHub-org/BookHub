﻿using BookHub.DAL.Entities;
using BookHub.DAL.Repositories.Interfaces;
using BookHub.DAL.DataAccess;
using Microsoft.EntityFrameworkCore;
using BookHub.DAL.DTO;

namespace BookHub.DAL.Repositories.Implementations
{
    public class BookRepository : Repository<BookEntity>, IBookRepository
    {
        private AppDbContext _context;

        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        

        public async Task<(List<BookEntity> Items, long TotalCount)> GetPagedAsync(Pageable pageable)
        {
            var totalCount = await _context.Books.CountAsync();

            var items = await _context.Books
                .Skip((pageable.Page - 1) * pageable.Size)
                .Take(pageable.Size)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<BookEntity> Items, long TotalCount)> GetPagedBooksByCollectionAsync(int collectionId, Pageable pageable)
        {
            var query = _context.Collections
                .Where(c => c.Id == collectionId)
                .SelectMany(c => c.Books);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageable.Page - 1) * pageable.Size)
                .Take(pageable.Size)
                .ToListAsync();

            return (items, totalCount);
        }


    }
}
