using System;
using System.Collections.Generic;
using System.Collections;

namespace PBFramework.Data
{
	/// <summary>
	/// Stack implementation with a limited capacity where first-in objects are removed on excessive push.
	/// </summary>
	public class LimitedStack<T> : IEnumerable<T>, ICollection {

		private T[] buffer;

		private int count = 0;


		public T this[int index]
		{
			get
			{
				if(index < 0 || index >= count)
					throw new ArgumentOutOfRangeException("index");
				return buffer[index];
			}
		}

		public int Count { get { return count; } }

		public bool IsSynchronized { get { return false; } }

		public object SyncRoot { get { return null; } }


		public LimitedStack (int capacity)
		{
			if(capacity <= 0)
				throw new ArgumentException("capacity must be greater than 0!");
			
			buffer = new T[capacity];
		}

		/// <summary>
		/// Pushes the specified value to the top.
		/// </summary>
		public void Push(T value)
		{
			if(count >= buffer.Length)
				PopBottom();
			buffer[count] = value;
			count ++;
		}

		/// <summary>
		/// Pops and returns the items at the top.
		/// </summary>
		public T Pop()
		{
			if(count <= 0)
				throw new Exception("There are no more items in the stack!");
			var value = buffer[count-1];
            buffer[count-1] = default(T);
			count --;
            return value;
		}

		/// <summary>
		/// Returns the item at the top.
		/// </summary>
		public T Peek()
		{
			if(count <= 0)
				throw new Exception("There are no more items in the stack!");
			return buffer[count-1];
		}

		/// <summary>
		/// Pops and returns the item at the bottom.
		/// </summary>
		public T PopBottom()
		{
			if(count <= 0)
				throw new Exception("There are no more items in the stack!");
			var value = buffer[0];
			for(int i=0; i<count-1; i++)
				buffer[i] = buffer[i+1];
			buffer[count-1] = default(T);
			count--;
            return value;
		}

		/// <summary>
		/// Returns the item at the bottom.
		/// </summary>
		public T PeekBottom()
		{
			if(count <= 0)
				throw new Exception("There are no more items in the stack!");
			return buffer[0];
		}

		/// <summary>
		/// Clears all entries in the buffer.
		/// </summary>
		public void Clear()
		{
			count = 0;
			for(int i=0; i<buffer.Length; i++)
				buffer[i] = default(T);
		}

		public IEnumerator<T> GetEnumerator ()
		{
			for(int i=0; i<count; i++)
				yield return buffer[i];
		}

		IEnumerator IEnumerable.GetEnumerator () { return (IEnumerator)GetEnumerator(); }

		public void CopyTo (Array array, int index) { throw new NotImplementedException (); }
	}
}

