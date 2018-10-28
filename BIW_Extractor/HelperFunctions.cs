using System;
namespace BIW_Extractor
{
    public static class HelperFunctions
    {

            public static string AddQuotesIfRequired(string path)
            {
                return !string.IsNullOrEmpty(path) ?
                    path.Contains(" ") ? "\"" + path + "\"" : path
                    : string.Empty;
            }
    }
}
