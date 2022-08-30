using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace EFDataApp.Infrastructure.ExtensionsMethods
{
    public static class DynamicOrder
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> sourse, string propertyName, bool desc)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(T), "x");
            //Expression propertyExp = Expression.Property(parameterExp, propertyName);
            Expression propertyExp = propertyName.Split('.').Aggregate((Expression)parameterExp, Expression.Property);
            LambdaExpression LambdaExp = Expression.Lambda(propertyExp, parameterExp);

            Type queryableType = typeof(Queryable); // получаем метаданные из типа Queryable
            MethodInfo[] methods = queryableType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            MethodInfo method = methods // получаем методы Queryable из его метаданных
                .Where(m =>
                      m.Name == (desc ? "OrderByDescending" : "OrderBy")
                   && m.IsGenericMethodDefinition
                   && m.GetGenericArguments().Length == 2
                   && m.GetParameters().Length == 2)
                .First() // получем только один метод из найденных
                .MakeGenericMethod(typeof(T), propertyExp.Type); // преобразуем метод в обобщенный

            return (IQueryable<T>)method.Invoke(null, new object[] { sourse, LambdaExp });
            // вызываем полученный метод и возвращаем результат его действия
            
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> sourse, string propertyName, string serchTerm)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(T), "x"); // x
            Expression propertyExp = propertyName.Split('.').Aggregate((Expression)parameterExp, Expression.Property); // x.propertyName
            MethodCallExpression metodCallExp = Expression.Call(propertyExp, "Contains", Type.EmptyTypes, Expression.Constant(serchTerm, typeof(string))); // выражение вызова метода Contains(serchTerm) у свойства propertyName - x.propertyName.Contains(serchTerm)
            Expression<Func<T, bool>> lambdaExp = Expression.Lambda<Func<T, bool>>(metodCallExp, parameterExp); // x => x.propertyName.Contains(serchTerm)

            return sourse.Where(lambdaExp);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> sourse, string propertyName, decimal serchCount)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(T), "x"); // x
            Expression propertyExp = propertyName.Split('.').Aggregate((Expression)parameterExp, Expression.Property); // x.propertyName
            MethodCallExpression metodCallExp = Expression.Call(propertyExp, "Equals", Type.EmptyTypes, Expression.Constant(serchCount, typeof(decimal))); // выражение вызова метода Contains(serchTerm) у свойства propertyName - x.propertyName.Contains(serchTerm)
            Expression<Func<T, bool>> lambdaExp = Expression.Lambda<Func<T, bool>>(metodCallExp, parameterExp); // x => x.propertyName.Contains(serchTerm)

            return sourse.Where(lambdaExp);
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> sourse, string propertyName, decimal? min, decimal? max)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(T), "x"); // x
            Expression propertyExp = propertyName.Split('.').Aggregate((Expression)parameterExp, Expression.Property); // x.propertyName
            ConstantExpression maxExp;
            ConstantExpression minExp;
            BinaryExpression CompareExp;
            if (min == null && max == null)
            {
                return sourse;
            }
            else if (min == null)
            {
                maxExp = Expression.Constant(max); // max
                CompareExp = Expression.LessThanOrEqual(propertyExp, maxExp); // x.propertyName <= max
            }
            else if (max == null)
            {
                minExp = Expression.Constant(min); // min
                CompareExp = Expression.GreaterThanOrEqual(propertyExp, minExp); // x.propertyName >= min
            }
            else
            {
                maxExp = Expression.Constant(max); // max
                minExp = Expression.Constant(min); // min
                BinaryExpression GreaterExp = Expression.GreaterThanOrEqual(propertyExp, minExp); // x.propertyName >= min
                BinaryExpression LessExp = Expression.LessThanOrEqual(propertyExp, maxExp); // x.propertyName <= max
                CompareExp = Expression.And(GreaterExp, LessExp); // x.propertyName >= min && x.propertyName <= max
            }
            Expression<Func<T, bool>> lambdaExp = Expression.Lambda<Func<T, bool>>(CompareExp, parameterExp); // x => x.propertyName >= min && x.propertyName >= max

            return sourse.Where(lambdaExp);
        }

        //public static decimal Max<T>(this IQueryable<T> sourse, string propertyName)
        //{
        //    ParameterExpression parameterExp = Expression.Parameter(typeof(T), "x"); // x
        //    Expression propertyExp = propertyName.Split('.').Aggregate((Expression)parameterExp, Expression.Property); // x.propertyName
        //    Expression<Func<T, decimal>> lambdaExp = Expression.Lambda<Func<T, decimal>>(propertyExp, parameterExp); // x => x.propertyName

        //    return sourse.Max(lambdaExp);
        //}

        //public static decimal Min<T>(this IQueryable<T> sourse, string propertyName)
        //{
        //    ParameterExpression parameterExp = Expression.Parameter(typeof(T), "x"); // x
        //    Expression propertyExp = propertyName.Split('.').Aggregate((Expression)parameterExp, Expression.Property); // x.propertyName
        //    Expression<Func<T, decimal>> lambdaExp = Expression.Lambda<Func<T, decimal>>(propertyExp, parameterExp); // x => x.propertyName

        //    return sourse.Min(lambdaExp);
        //}
    }

    
}
