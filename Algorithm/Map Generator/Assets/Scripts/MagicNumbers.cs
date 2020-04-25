using System.ComponentModel;
/*
    Steps to Use : 
    Step 1: Create an Enum under MagicEnums
    Step 2: Provide the string as Description 
    Step 3: Provide the instance variable name of enum as assign an integer, make sure to follow the order 
    Step 4: In you file call the Enum using syntax  MagicEnums.<Your Enum instance variable>.ToDescriptionString()
       
*/

public enum MagicEnums
{
    [Description("C:/Users/Mayank PC/AppData/Local/Programs/Python/Python38-32/python.exe")]
    PythonPath = 1,
    [Description("String 2")]
    V2 = 2
}

public static class MagicWords
{
    public static string ToDescriptionString(this MagicEnums val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
           .GetType()
           .GetField(val.ToString())
           .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
