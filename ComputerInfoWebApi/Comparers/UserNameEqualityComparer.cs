using ComputerInfo.Models.Models;
using System.Collections.Generic;

namespace ComputerInfoWebApi.Comparers
{
    public class UserNameEqualityComparer : IEqualityComparer<User>
    {
        public int GetHashCode(User obj)
        {
            return (obj == null) ? 0 : obj.Name.GetHashCode();
        }

        public bool Equals(User x, User y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Name == y.Name;
        }
    }
}