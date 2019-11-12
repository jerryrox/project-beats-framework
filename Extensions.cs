﻿using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework
{
    public static class Extensions
    {

        /// <summary>
        /// Returns a new list containing the elements within the specified range.
        /// </summary>
        public static List<T> Slice<T>(this List<T> context, int startIndex, int length)
        {
            if(length < 0 || startIndex < 0 || startIndex >= context.Count || startIndex + length > context.Count)
            {
                throw new ArgumentOutOfRangeException(
                    $"Extensions.Slice - Invalid arguments: (ListCount: {context.Count}, startIndex: {startIndex}, length: {length})"
                );
            }

            var newList = new List<T>(length);
            for(int i = startIndex; i < startIndex + length; i++)
                newList.Add(context[i]);
            return newList;
        }

        /// <summary>
        /// Adds all elements from specified enumerable.
        /// </summary>
        public static void AddEnumerable<T>(this List<T> context, IEnumerable<T> collection)
        {
            foreach(var item in collection)
                context.Add(item);
        }

        /// <summary>
        /// Returns the last element contained in this array.
        /// </summary>
        public static T GetLast<T>(this List<T> context)
        {
            if(context == null || context.Count == 0)
                return default;
            return context[context.Count - 1];
        }

        /// <summary>
        /// Returns a new array containing the elements within the specified range.
        /// </summary>
        public static T[] Slice<T>(this T[] context, int startIndex, int length)
        {
            if(length < 0 || startIndex < 0 || startIndex >= context.Length || startIndex + length > context.Length)
            {
                throw new ArgumentOutOfRangeException(
                    $"Extensions.Slice - Invalid arguments: (ArrayLength: {context.Length}, startIndex: {startIndex}, length: {length})"
                );
            }

            var newArray = new T[length];
            Array.Copy(context, startIndex, newArray, 0, length);
            return newArray;
        }

        /// <summary>
        /// Performs a for loop on this array and executes the specified action.
        /// </summary>
        public static void ForEach<T>(this T[] context, Action<T> action)
        {
            for(int i = 0; i < context.Length; i++)
                action(context[i]);
        }

        /// <summary>
        /// Returns an enumerable using specified mapper.
        /// </summary>
        public static IEnumerable<TResult> GetEnumerable<TSource, TResult>(this IEnumerable<TSource> context, Func<TSource, TResult> mapper) where TResult : class
        {
            foreach(var item in context)
            {
                var result = mapper(item);
                if(result != null)
                    yield return result;
            }
        }

        /// <summary>
        /// Resets this transform's transform values except its rotation.
        /// </summary>
        public static void ResetTransform(this Transform context)
        {
            context.localPosition = Vector3.zero;
            context.localScale = Vector3.one;
            context.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// Sets the maintexture property of this UITexture and makes it pixel perfect.
        /// </summary>
        public static void SetMainTexture(this UITexture context, Texture2D texture)
        {
            context.mainTexture = texture;
            context.MakePixelPerfect();
        }

        /// <summary>
        /// Returns a single attribute instance of specified type.
        /// </summary>
        public static T GetCustomAttribute<T>(this MethodInfo context) where T : Attribute
        {
            var attributes = context.GetCustomAttributes(typeof(T), false);
            if(attributes == null || attributes.Length == 0)
                return null;
            return attributes[0] as T;
        }
    }
}