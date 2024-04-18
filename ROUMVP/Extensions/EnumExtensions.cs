using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace ROUMVP.Extensions;

public static class EnumExtensions
{
    private static readonly Dictionary<string, Dictionary<int,string>> Cache = new();
	
    public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
    {
        Type type = typeof(TEnum);
		
        string fullName = type.FullName ?? throw new UnreachableException();
		
        if (!Cache.TryGetValue(fullName, out var dict))
        {
            dict = new Dictionary<int, string>();
			
            var fields = type.GetFields().Where(field => field.FieldType.FullName != typeof(Int32).FullName).ToArray();
            for (int i = 0; i < fields.Length; i++)
            {
                var description = fields[i].GetCustomAttribute<DescriptionAttribute>();
                dict.Add(i, description?.Description ?? fields[i].Name);
            }
			
            Cache[fullName] = dict;
        }
		
        return dict[Convert.ToInt32(value)];
    }
}