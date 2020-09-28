using Dapper;
using Microsoft.Data.SqlClient;
using SocialMedia.Api.Application.Model;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Api.Application.Queries
{
    public class PostQueries : IPostQueries
    {
        private string _connectionString = string.Empty;

        public PostQueries(SqlConfiguration sql)
        {

            var constr = sql.ConnectionString;
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<PostDto> GetPostById(int id) 
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var sql = @"SELECT * FROM Publicacion WHERE IdPublicacion = @id;";
                var result = await conn.QueryAsync<dynamic>(sql.ToString(), new { id });
                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();
                return MapPost(result);
            }
        
        }

        private PostDto MapPost(dynamic result)
        {
            var post = new PostDto
            {
                Id = result[0].IdPublicacion,
                Date = result[0].Fecha,
                Description = result[0].Descripcion,
                Image = result[0].Imagen,
                UserId = result[0].IdUsuario
            };
            return post;
        }
    }
}
