using System;

namespace GroceryList.Models
{
    public class GroceryException : Exception
    {
        public GroceryException(string message)
            : base(message)
        {
        }
    }
}