using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NProcessPipe
{
    /// <summary>
    /// There are several places where we need to iterate over an enumerable
    /// several times, but we cannot assume that it is safe to do so.
    /// This class will allow to safely use an enumerable multiple times, by caching
    /// the results after the first iteration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProcessEnumerable<T> : IEnumerable<T>, IEnumerator<T>
    {
        private bool? _isFirstTime = null;
        private IEnumerator<T> _internalEnumerator;
        private readonly LinkedList<T> _cache = new LinkedList<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessEnumerable&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public ProcessEnumerable(IEnumerable<T> inner)
        {
            _internalEnumerator = inner.GetEnumerator();
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_isFirstTime == null)
            {
                _isFirstTime = true;
            }
            else if (_isFirstTime.Value)
            {
                _isFirstTime = false;
                _internalEnumerator.Dispose();
                _internalEnumerator = _cache.GetEnumerator();
            }
            else
            {
                _internalEnumerator = _cache.GetEnumerator();
            }

            return this;
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        ///<summary>
        ///Gets the element in the collection at the current position of the enumerator.
        ///</summary>
        ///
        ///<returns>
        ///The element in the collection at the current position of the enumerator.
        ///</returns>
        ///
        T IEnumerator<T>.Current
        {
            get { return _internalEnumerator.Current; }
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            _internalEnumerator.Dispose();
        }

        ///<summary>
        ///Advances the enumerator to the next element of the collection.
        ///</summary>
        ///
        ///<returns>
        ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        ///</returns>
        ///
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            bool result = _internalEnumerator.MoveNext();
            if (result && _isFirstTime.Value)
                _cache.AddLast(_internalEnumerator.Current);
            return result;
        }

        ///<summary>
        ///Sets the enumerator to its initial position, which is before the first element in the collection.
        ///</summary>
        ///
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            _internalEnumerator.Reset();
        }

        ///<summary>
        ///Gets the current element in the collection.
        ///</summary>
        ///
        ///<returns>
        ///The current element in the collection.
        ///</returns>
        ///
        ///<exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.-or- The collection was modified after the enumerator was created.</exception><filterpriority>2</filterpriority>
        public object Current
        {
            get { return _internalEnumerator.Current; }
        }
    }
}
