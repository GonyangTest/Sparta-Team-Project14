using System.ComponentModel;
using System.Reflection;

namespace SpartaDungeon.Core.Utils
{
    public static class EnumUtils
    {
        public static string GetDescription(this Enum value)
        {
            // 열거형 값의 필드 정보 가져오기
            FieldInfo field = value.GetType().GetField(value.ToString());

            // DescriptionAttribute 특성 가져오기
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            // 특성이 있으면 설명 반환, 없으면 원래 이름 반환
            return attribute?.Description ?? value.ToString();
        }
    }
}
