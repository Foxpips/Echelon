using System;

namespace Echelon.Data.Entities
{
    public abstract class EntityBase
    {
        private string _id;

        public virtual string Id
        {
            get { return _id ?? (_id = Guid.NewGuid().ToString()); }
            set { _id = value; }
        }
    }
}