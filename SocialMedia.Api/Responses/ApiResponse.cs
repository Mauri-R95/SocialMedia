using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    }
}
