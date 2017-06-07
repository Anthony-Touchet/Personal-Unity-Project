using System;
using UnityEngine;

namespace Other
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomAttributte : Attribute
    {
        public readonly string propertyName;

        public CustomAttributte(string s)
        {
            propertyName = s;
        }
    }
}
