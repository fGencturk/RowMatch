using System.Threading.Tasks;
using DG.Tweening;

namespace Utilities.Extensions
{
    public static class TweenExtensions
    {
        public static async Task ToTask(this Tween tween)
        {
            await TaskUtilities.WaitWhile(tween.IsActive);
        }
    }
}