EasyConfig
=====

EasyConfig is a simple configuration file parsing library created by Nick Gravelyn (http://nickgravelyn.com) for use with .NET applications and games using C#.

EasyConfig supports files that follow the following format:

    [GroupName]
    name = value #comment
    #comment

Configuration files consist of one or more groups designated by using the [] brackets with a name inside. Each group consists of one or more settings. A setting is set up as a name and a value separated by an equals sign (=).

Setting values can be integers, floating point numbers, booleans, or strings. Strings are designated with quotes like the following:

    aStringSetting = "SomeString"

Booleans can be set using any of the following: on, off, true, false, yes, no. Booleans are also not case sensitive so the following works:

    aBoolSetting = yEs

Settings can also be arrays which are defined simply by using a comma-separated list of values of the same type. You could make an array of booleans like so:

    aBoolArray = no, yes, false, true, on, off, off

You can also place comments in your configuration files using the pound sign (#). Any contents of a line following a pound sign (#) will be ignored by the parser.

EasyConfig parses these files and converts all the groups and settings into easy to navigate classes for easy access to these properties in your application without you having to manually parse the files. The following is a sample of how to parse a window size for an XNA Game Studio game:

The configuration file:

    [Video]
    Width = 1280
    Height = 720

The code:

    graphics.PreferredBackBufferWidth = config.SettingGroups["Video"].Settings["Width"].GetValueAsInt();
    graphics.PreferredBackBufferHeight = config.SettingGroups["Video"].Settings["Height"].GetValueAsInt();
    graphics.ApplyChanges();

Note that this example is for XNA Game Studio, however the library is a plain C# class library and as such can be used by just about any .NET platform.
