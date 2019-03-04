namespace EasyConfig.NetStandard
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A single setting from a configuration file
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Gets the name of the setting.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the raw value of the setting.
        /// </summary>
        public string RawValue { get; private set; }

        /// <summary>
        /// Gets whether or not the setting is an array.
        /// </summary>
        public bool IsArray { get; private set; }

        internal Setting(string name)
        {
            Name = name;
            RawValue = string.Empty;
            IsArray = false;
        }

        internal Setting(string name, string value, bool isArray)
        {
            Name = name;
            RawValue = value;
            IsArray = isArray;
        }

        /// <summary>
        /// Attempts to return the setting's value as an integer.
        /// </summary>
        /// <returns>An integer representation of the value</returns>
        public int GetValueAsInt()
        {
            return int.Parse(RawValue, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Attempts to return the setting's value as a float.
        /// </summary>
        /// <returns>A float representation of the value</returns>
        public float GetValueAsFloat()
        {
            return float.Parse(RawValue, CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Attempts to return the setting's value as a bool.
        /// </summary>
        /// <returns>A bool representation of the value</returns>
        public bool GetValueAsBool()
        {
            return bool.Parse(RawValue);
        }

        /// <summary>
        /// Attempts to return the setting's value as a string.
        /// </summary>
        /// <returns>A string representation of the value</returns>
        public string GetValueAsString()
        {
            if (!RawValue.StartsWith("\"") || !RawValue.EndsWith("\""))
            {
                throw new Exception("Cannot convert value to string.");
            }

            return RawValue.Substring(1, RawValue.Length - 2);
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of integers.
        /// </summary>
        /// <returns>An integer array representation of the value</returns>
        public int[] GetValueAsIntArray()
        {
            var parts = RawValue.Split(',');

            var valueParts = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                valueParts[i] = int.Parse(parts[i], CultureInfo.InvariantCulture.NumberFormat);
            }

            return valueParts;
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of floats.
        /// </summary>
        /// <returns>An float array representation of the value</returns>
        public float[] GetValueAsFloatArray()
        {
            var parts = RawValue.Split(',');

            var valueParts = new float[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                valueParts[i] = float.Parse(parts[i], CultureInfo.InvariantCulture.NumberFormat);
            }

            return valueParts;
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of bools.
        /// </summary>
        /// <returns>An bool array representation of the value</returns>
        public bool[] GetValueAsBoolArray()
        {
            var parts = RawValue.Split(',');

            var valueParts = new bool[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                valueParts[i] = bool.Parse(parts[i]);
            }

            return valueParts;
        }

        /// <summary>
        /// Attempts to return the setting's value as an array of strings.
        /// </summary>
        /// <returns>An string array representation of the value</returns>
        public string[] GetValueAsStringArray()
        {
            var match = Regex.Match(RawValue, "[\\\"][^\\\"]*[\\\"][,]*");

            var values = new List<string>();

            while (match.Success)
            {
                var value = match.Value;

                if (value.EndsWith(","))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                value = value.Substring(1, value.Length - 2);

                values.Add(value);

                match = match.NextMatch();
            }

            return values.ToArray();
        }

        /// <summary>
        /// Get Value as T
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <returns></returns>
        public T GetValueAs<T>()
        {
            try
            {
                return (T)Convert.ChangeType(RawValue, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Get array Value as T[]
        /// </summary>
        /// <typeparam name="T">array value type</typeparam>
        /// <returns></returns>
        public T[] GetValuesAs<T>()
        {
            var parts = RawValue.Split(',');

            var valueParts = new T[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                valueParts[i] = (T)Convert.ChangeType(parts[i], typeof(T));
            }

            return valueParts;
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(int value)
        {
            RawValue = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(float value)
        {
            RawValue = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(bool value)
        {
            RawValue = value.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="value">The new value to store.</param>
        public void SetValue(string value)
        {
            RawValue = AssertStringQuotes(value);
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="values">The new values to store.</param>
        public void SetValue(params int[] values)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < values.Length; i++)
            {
                builder.Append(values[i].ToString(CultureInfo.InvariantCulture.NumberFormat));

                if (i < values.Length - 1)
                {
                    builder.Append(",");
                }
            }

            RawValue = builder.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="values">The new values to store.</param>
        public void SetValue(params float[] values)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < values.Length; i++)
            {
                builder.Append(values[i].ToString(CultureInfo.InvariantCulture.NumberFormat));

                if (i < values.Length - 1)
                {
                    builder.Append(",");
                }
            }

            RawValue = builder.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="values">The new values to store.</param>
        public void SetValue(params bool[] values)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < values.Length; i++)
            {
                builder.Append(values[i]);

                if (i < values.Length - 1)
                {
                    builder.Append(",");
                }
            }

            RawValue = builder.ToString();
        }

        /// <summary>
        /// Sets the value of the setting.
        /// </summary>
        /// <param name="values">The new values to store.</param>
        public void SetValue(params string[] values)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < values.Length; i++)
            {
                builder.Append(AssertStringQuotes(values[i]));

                if (i < values.Length - 1)
                {
                    builder.Append(",");
                }
            }

            RawValue = builder.ToString();
        }

        private static string AssertStringQuotes(string value)
        {
            // make sure we have our surrounding quotations
            if (!value.StartsWith("\""))
            {
                value = "\"" + value;
            }

            if (!value.EndsWith("\""))
            {
                value = value + "\"";
            }

            return value;
        }
    }
}
