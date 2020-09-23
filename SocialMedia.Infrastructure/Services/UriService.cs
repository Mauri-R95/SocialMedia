using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;

namespace SocialMedia.Infrastructure.Services
{
    public class UriService : IUriService
    {
        //se encarga de generar las URL siguiendo el principio de responsabilidad unica
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            string baseUrl = $"{_baseUri}{actionUrl}";
            return new Uri(baseUrl);
        }


    }
}
