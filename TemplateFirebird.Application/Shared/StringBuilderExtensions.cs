using System.Text;

namespace TemplateFirebird.Application.Shared;
public static class StringBuilderExtensions
{
    public static void AppendSql(this StringBuilder stringBuilder, string sql)
    {
        stringBuilder.Append(" " + sql);
    }

    public static void AppendParamRoute(this StringBuilder stringBuilder, string sql)
    {
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Append("&" + sql);
        }
        else
        {
            stringBuilder.Append("?" + sql);
        }
    }
}