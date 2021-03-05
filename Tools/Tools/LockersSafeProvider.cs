using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Tools
{
    /// <summary>
    /// Get safe lockers for singleton using string key
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class LockersSafeProvider<TKey> : Dictionary<TKey, object>
    {
        private static object keyLocker = new object();
        public new object this[TKey key]
        {
            get
            {
                if (!base.ContainsKey(key))
                {
                    lock (keyLocker)
                    {
                        if (!ContainsKey(key))
                        {
                            Add(key, new object());
                        }
                    }
                }
                return base[key];
            }
        }
    }
}
