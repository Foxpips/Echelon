using System;

namespace Echelon.Data
{
    public abstract class EntityBase
    {
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();
    }
}