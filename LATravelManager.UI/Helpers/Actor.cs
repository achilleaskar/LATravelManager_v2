using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public class Actor<T, V>
    {
        // Optional delegate to use to pass back the
        public delegate void After(object sender, V e);

        // use this delegate to represent a method that can be passed
        // as a parameter without explicitly declaring a custom delegate.
        private readonly Func<T, V> job;

        public Actor(Func<T, V> f)
        {
            job = f;
        }

        // This method passes T as the parameter to Func<T, V> job and
        // returns V.  The callback delegate is invoked once the Task has
        // ran to completion.
        public virtual async Task Act(T t, After a)
        {
            V v = await Task.Run(() =>
            {
                return job(t);
            });

            a?.Invoke(this, v);
        }

        // This method is the same as above but without invoking a
        // callback delegate. The response is returned directly to
        // to the calling application.
        public virtual async Task<V> Act(T t)
        {
            return await Task.Run(() =>
            {
                return job(t);
            });
        }
    }
}