using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBFramework.Data
{
	/// <summary>
	/// Custom implementation of SortedList for one type parameter only.
	/// Also, buffer will not be reset or trimmed until Trim() is called explicitly.
	/// </summary>
	public class SortedList<T> : IList<T> where T : IComparable<T>
	{
		private T[] buffer;
		private int baseCapacity;
		private int realCount;


		public T this[int index]
		{
			get
			{
				if (index > realCount)
					throw new IndexOutOfRangeException("Argument 'index' is greater than list count.");
				else if (index < 0)
					throw new IndexOutOfRangeException("Argument 'index' must be greater than or equal to 0.");
				return buffer[index];
			}
			set
			{
				if (index > realCount)
					throw new IndexOutOfRangeException("Argument 'index' is greater than list count.");
				else if (index < 0)
					throw new IndexOutOfRangeException("Argument 'index' must be greater than or equal to 0.");
				buffer[index] = value;
			}
		}

		public int Count { get { return realCount; } }

		public bool IsReadOnly { get { return false; } }


		public SortedList() : this(0) { }

		public SortedList(int capacity)
		{
			if (capacity < 0)
				throw new ArgumentOutOfRangeException("capacity");

			buffer = new T[Math.Max(1, capacity)];
			baseCapacity = 0;
			realCount = 0;
		}

		public int IndexOf(T item)
		{
			for (int i = 0; i < realCount; i++)
			{
				if (buffer[i].Equals(item))
					return i;
			}
			return -1;
		}

		public void Insert(int index, T item)
		{
			// This should not be supported.
			return;
		}

		public void RemoveAt(int index)
		{
			if (index > realCount)
				throw new IndexOutOfRangeException("Argument 'index' is greater than list count.");
			else if (index < 0)
				throw new IndexOutOfRangeException("Argument 'index' must be greater than or equal to 0.");

			// Nullify item at index
			buffer[index] = default(T);

			// Shift back all items after this index.
			for (int i = index + 1; i < realCount; i++)
				buffer[i - 1] = buffer[i];

			realCount--;
		}

		public void Add(T item)
		{
			// Reallocate buffer as necessary.
			if (realCount + 1 > buffer.Length)
				ReallocateBuffer();

			// Search for insertion index.
			int index = BinarySearch(item, 0, realCount);

			// If add
			if (index >= realCount)
			{
				// Simply assign the item to the last.
				buffer[realCount] = item;
			}
			// If insert
			else
			{
				// Shift all elements starting from 'index' to forward.
				for (int i = realCount - 1; i >= index; i--)
					buffer[i + 1] = buffer[i];

				// Insert item
				buffer[index] = item;
			}

			realCount++;
		}

		public void Clear()
		{
			// Remove all items in buffer
			for (int i = 0; i < realCount; i++)
				buffer[i] = default(T);

			realCount = 0;
		}

		public bool Contains(T item)
		{
			return IndexOf(item) >= 0;
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			int copyableCount = array.Length - arrayIndex;
			if (copyableCount < realCount)
				throw new Exception("Array length is not enough for copy.");

			for (int i = 0; i < realCount; i++)
				array[arrayIndex + 1] = buffer[i];
		}

		public bool Remove(T item)
		{
			// Find removal index.
			int removeIndex = -1;
			for (int i = 0; i < realCount; i++)
			{
				if (buffer[i].Equals(item))
				{
					// Remove item and store index
					buffer[i] = default(T);
					removeIndex = i;
					break;
				}
			}

			// If remove point found
			if (removeIndex >= 0)
			{
				// Shift back all items after removeIndex
				for (int i = removeIndex + 1; i < realCount; i++)
					buffer[i - 1] = buffer[i];

				realCount--;
				return true;
			}

			// No item found.
			return false;
		}

		public void Trim()
		{
			// If real count is 0, just remove the buffer and recreate it.
			if (realCount == 0)
			{
				buffer = new T[Math.Min(1, baseCapacity)];
				return;
			}

			// Determine size of new buffer
			int newSize = realCount;
			if (baseCapacity == 0)
				newSize *= 2;
			else
				newSize += baseCapacity;

			// Create new array with new length.
			T[] newBuffer = new T[newSize];

			// Copy over items
			for (int i = 0; i < realCount; i++)
				newBuffer[i] = buffer[i];

			// Store buffer
			buffer = newBuffer;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < realCount; i++)
				yield return buffer[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator)(this as SortedList<T>).GetEnumerator();
		}

		int BinarySearch(T item, int startIndex, int end)
		{
			int midIndex = 0;
			while (startIndex < end)
			{
				// Get mid point
				midIndex = (end + startIndex) >> 1;

				// Re enumerate with changed indexes.
				int compare = item.CompareTo(buffer[midIndex]);
				if (compare < 0)
					end = midIndex;
				else if (compare > 0)
					startIndex = midIndex + 1;
				else
					return midIndex;
			}
			return startIndex;
		}

		void ReallocateBuffer()
		{
			// Determine size for the new buffer
			int newCount = 0;
			if (baseCapacity == 0)
				newCount = buffer.Length * 2;
			else
				newCount = buffer.Length + baseCapacity;

			// Create new buffer
			T[] newBuffer = new T[newCount];

			// Copy over existing data
			for (int i = 0; i < buffer.Length; i++)
				newBuffer[i] = buffer[i];

			// Rewrite buffer
			buffer = newBuffer;
		}
	}
}

