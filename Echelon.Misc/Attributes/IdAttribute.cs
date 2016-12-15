using System;

namespace Echelon.Misc.Attributes
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