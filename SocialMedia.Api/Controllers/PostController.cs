using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    //al utilizar new alta dependencia, utilizar inyeccion de dependencia
    [Route("api/[controller]")]
    [ApiController] // nos permite trabajar con API RESTFul, agrega una cantidad de validaciones y features
    //ControllerBase para trabajar con API
    //Controller sirve para lo mismo pero agregar los feature para trabajar con MVC (para app web)
    public class PostController : ControllerBase
    {

        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK/*, Type = typeof(ApiResponse<IEnumerable<PostDto>>)*/)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest/*, Type = typeof(ApiResponse<IEnumerable<PostDto>>)*/)]
        //los ? los hace nulleables
        //si se manejan mas de 3 parametros encapsular esos valores dentro de un objeto
        //[FromQuery] para mappiar los datos que vienen en la URL
        public IActionResult GetPosts([FromQuery]PostQueryFilter filters)
        //public ActionResult<ApiResponse<IEnumerable<PostDto>>> GetPosts([FromQuery]PostQueryFilter filters)
        {
            var posts = _postService.GetPosts(filters);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            var response = new ApiResponse<IEnumerable<PostDto>>(postsDto);
            //return response;
            //paginacion
            var metadata = new
            {
                posts.TotalCount, //cantidad total de elementos
                posts.PageSize, 
                posts.CurrentPage,
                posts.TotalPages,
                posts.HasNextPage,
                posts.HasPreviousPage
               
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            var postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPost]
        //DTO protege contra overposting
        public async Task<IActionResult> Post(PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            await _postService.InsertPost(post);
            postDto = _mapper.Map<PostDto>(post);
            var response = new ApiResponse<PostDto>(postDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, PostDto postDto)
        {
            var post = _mapper.Map<Post>(postDto);
            post.Id = id;
            var result = await _postService.UpdatePost(post);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}
