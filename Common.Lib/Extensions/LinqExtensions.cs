using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.Extensions
{
    public static class LinqExtensions
    {

        public enum SortOption
        {
            DESC = 1,
            ASC = 2
        }

        /// <summary>
        /// Sorts the specified source.
        /// Example:
        ///     IEnumerable<Company> companies = CommonDB2.RetrieveObject<Company>("Call p_return_all_companies", null, DB2_SCHEMA, _db2Server).Sort("COMPANYNAME", LinqHelper.SortOption.ASC);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="option">The sort direction.</param>
        /// <returns></returns>
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, string columnName, SortOption option)
        {
            var param = Expression.Parameter(typeof(T), string.Empty);
            try
            {
                var property = Expression.Property(param, columnName);
                var sortLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

                if (option == SortOption.DESC)
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortLambda);

                return source.AsQueryable<T>().OrderBy<T, object>(sortLambda);
            }
            catch (ArgumentException)
            {
                return source;
            }
        }

        /// <summary>
        /// Gets the duplicates by key.
        /// Example Usage: var query = cars.GetDuplicatesByKey(x => new { x.Color, x.Length, x.Width });
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> GetDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector)
                .Where(g => g.Skip(1).Any())
                .SelectMany(g => g);
        }


        /// <summary>
        /// Gets the duplicates.
        /// Example:
        ///     foreach (var car in cars.GetDuplicates("Color"))
        ///         Console.WriteLine(car.SerializeObjectToString());
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source, string columnName)
        {
            //var results = from Item a in MyCollection
            //group a by a.Id into g
            //   where g.Count() > 1
            //select g;

            //foreach (var group in results)
            //foreach (var item in group)
            //yield return item;

            var param = Expression.Parameter(typeof(T), string.Empty);
            IQueryable<IGrouping<object, T>> groups = null;
            try
            {
                var property = Expression.Property(param, columnName);

                var groupByLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);
                groups = source.AsQueryable<T>().GroupBy<T, object>(groupByLambda).Where(g => g.Count() > 1);

            }
            catch (Exception) { }

            if (groups != null)
                foreach (var group in groups)
                    foreach (var item in @group)
                        yield return item;
        }

        /// <summary>
        /// Example
        /// foreach (var car in cars.Distinct(new LinqHelper.GenericComparer<Car>(new List<string> { "Color", "Length" })))
        ///    Console.WriteLine(car.SerializeObjectToString());
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GenericComparer<T> : IEqualityComparer<T> where T : class
        {
            private List<PropertyInfo> _properties = new List<PropertyInfo>();

            /// <summary>
            /// Creates a new instance of PropertyComparer.
            /// </summary>
            /// <param name="propertyNames">The name of the properties in type T 
            /// to perform the comparison on.</param>
            public GenericComparer(IEnumerable<string> propertyNames)
            {
                //store a reference to the property info object for use during the comparison
                foreach (var propertyName in propertyNames)
                {
                    PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName,
                                                                       BindingFlags.GetProperty | BindingFlags.Instance |
                                                                       BindingFlags.Public);
                    if (propertyInfo == null)
                        throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName,
                                                                  typeof(T)));

                    _properties.Add(propertyInfo);
                }
            }

            public bool Equals(T x, T y)
            {
                bool equal = false;
                foreach (var property in _properties)
                {
                    //get the current value of the comparison property of x and of y
                    object xValue = null;
                    object yValue = null;
                    if (x != null)
                        xValue = property.GetValue(x, null);
                    if (y != null)
                        yValue = property.GetValue(y, null);

                    //if the xValue is null then we consider them equal if and only if yValue is null
                    if (xValue == null)
                        equal = yValue == null;
                    else
                        equal = xValue.Equals(yValue);

                    //If already not equal break because found a property that is not equal.
                    if (!equal)
                        return false;
                }

                //use the default comparer for whatever type the comparison property is.
                return equal;
            }

            public int GetHashCode(T obj)
            {
                string value = string.Empty;
                foreach (var property in _properties)
                {
                    //get the value of the comparison property out of obj
                    object propertyValue = property.GetValue(obj, null);

                    if (propertyValue != null)
                        value += propertyValue.ToString();
                }

                return value.GetHashCode();
            }
        }
    }
}



