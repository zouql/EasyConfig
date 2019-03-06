namespace EasyConfig.NetStandard
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a configuration file.
    /// </summary>
    public class ConfigFile
    {
        /// <summary>
        /// Gets the groups found in the configuration file.
        /// </summary>
        public IDictionary<string, SettingsGroup> SettingGroups { get; private set; }

        /// <summary>
        /// Creates a blank configuration file.
        /// </summary>
        public ConfigFile()
        {
            SettingGroups = new Dictionary<string, SettingsGroup>();
        }

        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="file">The filename where the configuration file can be found.</param>
        /// <param name="encoding">Encoding</param>
        public ConfigFile(string file, Encoding encoding = null)
        {
            Load(file, encoding);
        }

        /// <summary>
        /// Loads a configuration file.
        /// </summary>
        /// <param name="file">The stream from which to load the configuration file.</param>
        public ConfigFile(Stream stream)
        {
            Load(stream);
        }

        /// <summary>
        /// Adds a new settings group to the configuration file.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        /// <returns>The newly created SettingsGroup.</returns>
        public SettingsGroup AddSettingsGroup(string groupName)
        {
            if (SettingGroups.ContainsKey(groupName))
            {
                throw new Exception($"Group already exists with name '{groupName}'");
            }

            var group = new SettingsGroup(groupName);

            SettingGroups.Add(groupName, group);

            return group;
        }

        /// <summary>
        /// Deletes a settings group from the configuration file.
        /// </summary>
        /// <param name="groupName">The name of the group to delete.</param>
        public void DeleteSettingsGroup(string groupName)
        {
            SettingGroups.Remove(groupName);
        }

        /// <summary>
        /// Loads the configuration from a file.
        /// </summary>
        /// <param name="file">The file from which to load the configuration.</param>
        /// <param name="encoding">Encoding</param>
        public void Load(string file, Encoding encoding = null)
        {
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                Load(stream, encoding);
            }
        }

        /// <summary>
        /// Loads the configuration from a stream.
        /// </summary>
        /// <param name="stream">The stream from which to read the configuration.</param>
        /// <param name="encoding"></param>
        public void Load(Stream stream, Encoding encoding = null)
        {
            // track line numbers for exceptions
            var lineNumber = 0;

            // groups found
            var groups = new List<SettingsGroup>();

            // current group information
            var currentGroupName = default(string);

            var settings = default(List<Setting>);

            using (var reader = new StreamReader(stream, encoding ?? Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    lineNumber++;

                    // strip out comments
                    if (line.Contains("#"))
                    {
                        if (line.IndexOf("#") == 0)
                        {
                            continue;
                        }

                        line = line.Substring(0, line.IndexOf("#"));
                    }

                    // trim off any extra whitespace
                    line = line.Trim();

                    // try to match a group name
                    var match = Regex.Match(line, "\\[[a-zA-Z\\d\\s]+\\]");

                    // found group name
                    if (match.Success)
                    {
                        // if we have a current group we're on, we save it
                        if (settings != null && currentGroupName != null)
                        {
                            groups.Add(new SettingsGroup(currentGroupName, settings));
                        }

                        // make sure the name exists
                        if (match.Value.Length == 2)
                        {
                            throw new Exception(string.Format($"Group must have name (line {lineNumber})"));
                        }

                        // set our current group information
                        currentGroupName = match.Value.Substring(1, match.Length - 2);

                        settings = new List<Setting>();
                    }

                    // no group name, check for setting with equals sign
                    else if (line.Contains("="))
                    {
                        // split the line
                        var parts = line.Split(new[] { '=' }, 2);

                        // if we have any more than 2 parts, we have a problem
                        if (parts.Length != 2)
                        {
                            throw new Exception(string.Format($"Settings must be in the format 'name = value' (line {lineNumber})"));
                        }

                        // trim off whitespace
                        parts[0] = parts[0].Trim();

                        parts[1] = parts[1].Trim();

                        // figure out if we have an array or not
                        var isArray = false;

                        var inString = false;

                        // go through the characters
                        foreach (char c in parts[1])
                        {
                            // any comma not in a string makes us creating an array
                            if (c == ',' && !inString)
                            {
                                isArray = true;
                            }

                            // flip the inString value each time we hit a quote
                            else if (c == '"')
                            {
                                inString = !inString;
                            }
                        }

                        // if we have an array, we have to trim off whitespace for each item and
                        // do some checking for boolean values.
                        if (isArray)
                        {
                            // split our value array
                            var pieces = parts[1].Split(',');

                            // need to build a new string
                            var builder = new StringBuilder();

                            for (int i = 0; i < pieces.Length; i++)
                            {
                                // trim off whitespace
                                var s = pieces[i].Trim();

                                // convert to lower case
                                var t = s.ToLower();

                                // check for any of the true values
                                if (t == "on" || t == "yes" || t == "true")
                                {
                                    s = "true";
                                }

                                // check for any of the false values
                                else if (t == "off" || t == "no" || t == "false")
                                {
                                    s = "false";
                                }

                                // append the value
                                builder.Append(s);

                                // if we are not on the last value, add a comma
                                if (i < pieces.Length - 1)
                                {
                                    builder.Append(",");
                                }
                            }

                            // save the built string as the value
                            parts[1] = builder.ToString();
                        }

                        // if not an array
                        else
                        {
                            // make sure we are not working with a string value
                            if (!parts[1].StartsWith("\""))
                            {
                                // convert to lower
                                var t = parts[1].ToLower();

                                // check for any of the true values
                                if (t == "on" || t == "yes" || t == "true")
                                {
                                    parts[1] = "true";
                                }

                                // check for any of the false values
                                else if (t == "off" || t == "no" || t == "false")
                                {
                                    parts[1] = "false";
                                }
                            }
                        }

                        // add the setting to our list making sure, once again, we have stripped
                        // off the whitespace
                        settings.Add(new Setting(parts[0].Trim(), parts[1].Trim(), isArray));
                    }
                }
            }

            // make sure we save off the last group
            if (settings != null && currentGroupName != null)
            {
                groups.Add(new SettingsGroup(currentGroupName, settings));
            }

            // create our new group dictionary
            SettingGroups = new Dictionary<string, SettingsGroup>();

            // add each group to the dictionary
            foreach (var group in groups)
            {
                SettingGroups.Add(group.Name, group);
            }
        }

        /// <summary>
        /// Saves the configuration to a file
        /// </summary>
        /// <param name="filename">The filename for the saved configuration file.</param>
        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                Save(stream);
            }
        }

        /// <summary>
        /// Saves the configuration to a stream.
        /// </summary>
        /// <param name="stream">The stream to which the configuration will be saved.</param>
        private void Save(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                foreach (var groupValue in SettingGroups)
                {
                    writer.WriteLine($"[{groupValue.Key}]");

                    foreach (var settingValue in groupValue.Value.Settings)
                    {
                        writer.WriteLine($"{settingValue.Key} = {settingValue.Value.RawValue}");
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
