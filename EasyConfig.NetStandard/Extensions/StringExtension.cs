namespace EasyConfig.NetStandard
{
    using System;

    /// <summary>
    /// String扩展
    /// </summary>
    internal static class StringExtension
    {
        /// <summary>
        /// 转为bool类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool? ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (bool.TryParse(value, out var outValue))
            {
                return outValue;
            }

            return null;
        }

        /// <summary>
        /// 转为int类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static int? ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (int.TryParse(value, out var outValue))
            {
                return outValue;
            }

            return null;
        }

        /// <summary>
        /// 转为float类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static float? ToFloat(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (float.TryParse(value, out var outValue))
            {
                return outValue;
            }

            return null;
        }

        /// <summary>
        /// 转为float类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static double? ToDouble(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (double.TryParse(value, out var outValue))
            {
                return outValue;
            }

            return null;
        }

        /// <summary>
        /// 转为decimal类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (decimal.TryParse(value, out var outValue))
            {
                return outValue;
            }

            return null;
        }

        /// <summary>
        /// 转为DateTime类型
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (DateTime.TryParse(value, out var outValue))
            {
                return outValue;
            }

            return null;
        }
    }
}
