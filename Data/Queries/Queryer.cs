using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace PBFramework.Data.Queries
{
    public class Queryer<T> : IQueryer<T> where T : class, IQueryableData {

		/// <summary>
		/// Table of token handlers that start with specific string value.
		/// </summary>
		private Dictionary<string, QueryHandler<T>> specialHandlers = new Dictionary<string, QueryHandler<T>>();


        public IEnumerable<T> Query(IEnumerable<T> objects, string queryString = "")
        {
			if(string.IsNullOrEmpty(queryString)) return objects;
			if(objects == null) return null;

            foreach (var token in queryString.Split(','))
            {
				// Perform query with last objects.
                var result = QueryWithToken(objects, token);

                // Premature null result.
                if(result == null) return null;

                // Intersection
                objects = result;
            }
            return objects;
        }

		public void SetSpecialHandler(string delimiter, QueryHandler<T> handler)
		{
            // Make sure the delimiter is valid.
            delimiter = StandardizeToken(delimiter);
            if(string.IsNullOrEmpty(delimiter))
                throw new ArgumentException($"The delimiter ({delimiter}) is invalid!");

            specialHandlers[delimiter] = handler;
		}

		/// <summary>
		/// Handles default querying process using IQueryableData interface.
		/// </summary>
        protected virtual IEnumerable<T> HandleDefault(IEnumerable<T> objects, string token)
        {
            // Standardize token.
            token = StandardizeToken(token);

			// No need to filter anything.
            if(token.Length == 0) return objects;

            // Search within queryable data.
            return objects.Where(o => {
                return o.GetQueryables().Any(d => d != null && d.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0);
            });
        }

        /// <summary>
        /// Finds and returns all matching data for specified token.
        /// </summary>
        private IEnumerable<T> QueryWithToken(IEnumerable<T> objects, string token)
        {
			// Nothing to search for!
			if(objects == null) return null;

            // Standardize the token.
            token = StandardizeToken(token);

			// No need to filter anything.
			if(token.Length == 0) return objects;

            // Process special handlers if applicable.
            foreach (var key in specialHandlers.Keys)
            {
                int keyIndex = token.IndexOf(key, StringComparison.OrdinalIgnoreCase);
                if (keyIndex >= 0)
                {
					return specialHandlers[key](objects, token.Substring(keyIndex + key.Length).Trim());
                }
            }

            // Process default action.
			return HandleDefault(objects, token);
        }

		/// <summary>
		/// Makes the token standardized for querying.
		/// </summary>
        private string StandardizeToken(string token)
        {
            return token.Trim();
        }
    }
}
