using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;

namespace Domain.Base;

[ExcludeFromCodeCoverage]

public static class Extensions
{

    public static DateTime BrazilDateTime(this DateTime dataHora)
    {
        TimeZoneInfo timeZone = GetTimeZone();
        return TimeZoneInfo.ConvertTime(dataHora, timeZone);
    }
    private static TimeZoneInfo GetTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        }
        catch (Exception)
        {
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
    }
    public static string GetDescription(this Enum GenericEnum)
    {
        Type genericEnumType = GenericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
        if ((memberInfo != null && memberInfo.Length > 0))
        {
            var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (_Attribs?.Any() ?? false)
            {
                return (_Attribs.FirstOrDefault() as DescriptionAttribute)?.Description;
            }
        }
        return GenericEnum.ToString();
    }

    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
