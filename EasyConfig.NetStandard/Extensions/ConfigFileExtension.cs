namespace EasyConfig.NetStandard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ConfigFile 扩展
    /// </summary>
    public static class ConfigFileExtension
    {
        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public static T ToObject<T>(this ConfigFile configFile) where T : class, new()
        {
            var obj = new T();

            foreach (var item in obj.GetType().GetProperties())
            {
                if (item.GetValue(obj, null) == null)
                {
                    item.SetValue(obj, Activator.CreateInstance(item.PropertyType), null);
                }

                var settings = configFile.SettingGroups[item.Name].Settings.Select(m => m.Value).AsEnumerable();

                var propertyObj = GetObject(item.PropertyType, settings);

                item.SetValue(obj, propertyObj, null);
            }

            return obj;
        }

        private static object GetObject(Type objType, IEnumerable<Setting> settings)
        {
            var properties = objType.GetProperties();

            var obj = Activator.CreateInstance(objType);

            foreach (var setting in settings)
            {
                var property = properties.SingleOrDefault(m => m.Name == setting.Name);

                // 未知属性直接跳过
                if (property == null)
                {
                    continue;
                }

                var value = default(object);

                if (setting.IsArray)
                {
                    var array = setting.RawValue.Split(',');

                    if (new[] { typeof(IEnumerable<string>) }.Contains(property.PropertyType))
                    {
                        value = array.Select(m => m.Replace("\"", string.Empty)).AsEnumerable();
                    }
                    else if (new[] { typeof(IEnumerable<bool>), typeof(IEnumerable<bool?>) }.Contains(property.PropertyType))
                    {
                        var vals = array.Select(m => m.ToBool());

                        if (property.PropertyType == typeof(IEnumerable<bool>))
                        {
                            value = vals.Select(m => m.GetValueOrDefault()).AsEnumerable();
                        }
                        else
                        {
                            value = vals.AsEnumerable();
                        }
                    }
                    else if (new[] { typeof(IEnumerable<int>), typeof(IEnumerable<int?>) }.Contains(property.PropertyType))
                    {
                        var vals = array.Select(m => m.ToInt());

                        if (property.PropertyType == typeof(IEnumerable<int>))
                        {
                            value = vals.Select(m => m.GetValueOrDefault()).AsEnumerable();
                        }
                        else
                        {
                            value = vals.AsEnumerable();
                        }
                    }
                    else if (new[] { typeof(IEnumerable<float>), typeof(IEnumerable<float?>) }.Contains(property.PropertyType))
                    {
                        var vals = array.Select(m => m.ToFloat());

                        if (property.PropertyType == typeof(IEnumerable<float>))
                        {
                            value = vals.Select(m => m.GetValueOrDefault()).AsEnumerable();
                        }
                        else
                        {
                            value = vals.AsEnumerable();
                        }
                    }
                    else if (new[] { typeof(IEnumerable<double>), typeof(IEnumerable<double?>) }.Contains(property.PropertyType))
                    {
                        var vals = array.Select(m => m.ToDouble());

                        if (property.PropertyType == typeof(IEnumerable<double>))
                        {
                            value = vals.Select(m => m.GetValueOrDefault()).AsEnumerable();
                        }
                        else
                        {
                            value = vals.AsEnumerable();
                        }
                    }
                    else if (new[] { typeof(IEnumerable<decimal>), typeof(IEnumerable<decimal?>) }.Contains(property.PropertyType))
                    {
                        var vals = array.Select(m => m.ToDecimal());

                        if (property.PropertyType == typeof(IEnumerable<decimal>))
                        {
                            value = vals.Select(m => m.GetValueOrDefault()).AsEnumerable();
                        }
                        else
                        {
                            value = vals.AsEnumerable();
                        }
                    }
                    else if (new[] { typeof(IEnumerable<DateTime>), typeof(IEnumerable<DateTime?>) }.Contains(property.PropertyType))
                    {
                        var vals = array.Select(m => m.ToDateTime());

                        if (property.PropertyType == typeof(IEnumerable<DateTime>))
                        {
                            value = vals.Select(m => m.GetValueOrDefault()).AsEnumerable();
                        }
                        else
                        {
                            value = vals.AsEnumerable();
                        }
                    }
                }
                else
                {
                    if (new[] { typeof(string) }.Contains(property.PropertyType))
                    {
                        value = setting.RawValue.ToString();
                    }
                    else if (new[] { typeof(bool), typeof(bool?) }.Contains(property.PropertyType))
                    {
                        var val = setting.RawValue.ToBool();

                        value = property.PropertyType == typeof(bool) ? val.GetValueOrDefault() : val;
                    }
                    else if (new[] { typeof(int), typeof(int?) }.Contains(property.PropertyType))
                    {
                        var val = setting.RawValue.ToInt();

                        value = property.PropertyType == typeof(int) ? val.GetValueOrDefault() : val;
                    }
                    else if (new[] { typeof(float), typeof(float?) }.Contains(property.PropertyType))
                    {
                        var val = setting.RawValue.ToFloat();

                        value = property.PropertyType == typeof(float) ? val.GetValueOrDefault() : val;
                    }
                    else if (new[] { typeof(double), typeof(double?) }.Contains(property.PropertyType))
                    {
                        var val = setting.RawValue.ToDouble();

                        value = property.PropertyType == typeof(double) ? val.GetValueOrDefault() : val;
                    }
                    else if (new[] { typeof(decimal), typeof(decimal?) }.Contains(property.PropertyType))
                    {
                        var val = setting.RawValue.ToDecimal();

                        value = property.PropertyType == typeof(decimal) ? val.GetValueOrDefault() : val;
                    }
                    else if (new[] { typeof(DateTime), typeof(DateTime?) }.Contains(property.PropertyType))
                    {
                        var val = setting.RawValue.ToDateTime();

                        value = property.PropertyType == typeof(DateTime) ? val.GetValueOrDefault() : val;
                    }
                }

                if (value != null)
                {
                    property.SetValue(obj, value, null);
                }
            }

            return obj;
        }
    }
}
