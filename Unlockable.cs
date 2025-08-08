using System.Collections.Generic;

namespace SPToolkits
{
    /// <summary>
    /// Holds a catalog of unlokcable values.
    /// </summary>
    public sealed class Unlockable<T>
    {
        private List<T> _unlockedValues;
        public event System.Action<T> onValueUnlocked;
         
        public Unlockable()
        {
            _unlockedValues = new List<T>();
        }

        /// <summary>
        /// Mark the given values as unlocked.
        /// <br>Already unlocked values are ignored.</br>
        /// </summary>
        /// <param name="values">The values to unlock.</param>
        /// <returns>The number of values successfully unlocked.</returns>
        public uint Unlock(params T[] values) 
        {
            if (values == null)
                return 0;

            uint count = 0;
            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];
                if (!_unlockedValues.Contains(value))
                {
                    _unlockedValues.Add(value);
                    onValueUnlocked?.Invoke(value);
                    count++;
                }
            }
            return count;
        }


        /// <summary>
        /// Mark the given values as unlocked but do not invoke any callbacks. 
        /// <br>Already unlocked values are ignored.</br>
        /// </summary>
        /// <param name="values">The values to unlock.</param>
        /// <returns>The number of values successfully unlocked.</returns>
        public uint UnlockWithoutNotify(params T[] values)
        {
            if (values == null)
                return 0;
            uint count = 0;
            for (int i = 0; i < values.Length; i++)
            {
                T value = values[i];
                if (!_unlockedValues.Contains(value))
                {
                    _unlockedValues.Add(value);
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Force invokes onNewlyUnlocked listeners for all unlocked values.
        /// </summary>
        public void ForceNotify() 
        {
            foreach (T value in _unlockedValues)
                    onValueUnlocked?.Invoke(value);
        }

        /// <param name="value">Check whether this value is unlocked or not.</param>
        /// <returns>True, if the value is unlocked.</returns>
        public bool IsUnlocked(T value)
            => _unlockedValues.Contains(value);

        /// <returns>Array of unlocked values.</returns>
        public T[] GetUnlockedValues()
            => _unlockedValues.ToArray();
    }
}