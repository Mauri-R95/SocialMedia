﻿using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        //private readonly IRepository<Post> _postRepository;
        //private readonly IRepository<User> _userRepository;

        //public PostService(IRepository<Post> postRepository, IRepository<User> userRepository)
        //{
        //    _postRepository = postRepository;
        //    _userRepository = userRepository;
        //}

        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = options.Value;
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        //public IEnumerable<Post> GetPosts(PostQueryFilter filters)
        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? _paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize= filters.PageSize == 0 ? _paginationOptions.DefaultPageSize : filters.PageSize;
            var posts = _unitOfWork.PostRepository.GetAll();
            if (filters.UserId != null)
                posts = posts.Where(x => x.UserId == filters.UserId);
            if (filters.Date != null)
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            if (filters.Description != null)
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            //paginacion
            var pagedPosts = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);
            return pagedPosts;
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if(user == null)
            {
                throw new BusinessException("User Doesn´t existe");
            }
            var userPost = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if(userPost.Count() < 10)
            {
                var lastPost = userPost.OrderByDescending(x => x.Date).FirstOrDefault();
                if((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("You are not able to publish the post");
                }
            }
            if (post.Description.Contains("sexo"))
            {
                throw new BusinessException("Contenido no permitido");
            }
            await _unitOfWork.PostRepository.Add(post);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var existingpost = await _unitOfWork.PostRepository.GetById(post.Id);
            existingpost.Image = post.Image;
            existingpost.Description = post.Description;
            _unitOfWork.PostRepository.Update(existingpost);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
