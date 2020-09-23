using SocialMedia.Core.CustomEntities;

namespace SocialMedia.Api.Responses
{
    // La que siempre la que vamos a retornar para manejar una estructura estandar en todo
    public class ApiResponse<T>
    {

        public ApiResponse(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Metadata Meta { get; set; }

    }
}
