using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Aetherium.Helpers
{
    public static class EnumExtensions
    {
        public static IEnumerable<(int Value, string DisplayName)> GetEnumDisplayValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(value =>
            {
                var member = typeof(T).GetMember(value.ToString()).FirstOrDefault();
                var display = member?.GetCustomAttribute<DisplayAttribute>()?.Name ?? value.ToString();
                return ((int)(object)value, display);
            });
        }
    }
}
