using System;
using System.Threading.Tasks;

namespace Utilities
{
    public static class TaskUtilities
    {    
        public static async Task WaitUntil(Func<bool> predicate)
        {
            while (!predicate())
            {
                await Task.Yield();
            }
        }
        
        public static async Task WaitWhile(Func<bool> predicate)
        {
            while (predicate())
            {
                await Task.Yield();
            }
        }
    }
}