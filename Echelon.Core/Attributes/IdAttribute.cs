using System;

namespace Echelon.Core.Attributes
{
    public class IdAttribute : Attribute
    {
        public string Id;

        public IdAttribute(string id)
        {
            Id = id;
        }
    }
}