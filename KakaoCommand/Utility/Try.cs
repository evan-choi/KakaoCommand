using System;

namespace KakaoCommand.Utility
{
    static class Try
    {
        public static void Invoke(Action action) => Invoke(action, null);

        public static void Invoke(Action action, Action<Exception> error)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                error?.Invoke(e);
            }
        }

        public static TResult Invoke<TResult>(Func<TResult> func) => Invoke(func, null);

        public static TResult Invoke<TResult>(Func<TResult> func, Action<Exception> error)
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception e)
            {
                error?.Invoke(e);
            }

            return default(TResult);
        }
    }
}
