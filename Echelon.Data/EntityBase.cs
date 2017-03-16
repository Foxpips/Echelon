using System;

namespace Echelon.Data
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}