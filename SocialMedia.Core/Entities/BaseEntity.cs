using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Entities
{
    //manejar datos de auditoria, quien fue, fecha de creacion, fecha de modificacion,
    //todas las tablas deberian tener los mismos campos
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
