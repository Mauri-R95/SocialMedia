﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SocialMediaContext _context; //visible por la misma clase y por las que la heredan
        protected DbSet<T> _entities;
        public BaseRepository(SocialMediaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        //public async Task<IEnumerable<T>> GetAll()
        //{
        //    return await _entities.ToListAsync();
        //}
        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
            //await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
            //await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
            //await _context.SaveChangesAsync();
        }

        
    }
}
