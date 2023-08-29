// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UtilityDAO.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Utility type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The utility.
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// The build contains expression.
        /// </summary>
        /// <param name="valueSelector">
        /// The value selector.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <typeparam name="TElement">
        /// </typeparam>
        /// <typeparam name="TValue">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
    Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector)
            {
                throw new ArgumentNullException("valueSelector");
            }

            if (null == values)
            {
                throw new ArgumentNullException("values");
            }

            ParameterExpression p = valueSelector.Parameters.Single();
            
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
    }
}