using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Compare
/// </summary>
namespace Affine.Utils.Compare
{


    public class Comparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        public Comparer(Func<T, T, bool> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            _comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}