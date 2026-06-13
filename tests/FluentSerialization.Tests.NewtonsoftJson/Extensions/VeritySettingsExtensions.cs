using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace VerifyTests;

public static class VeritySettingsExtensions
{
    public static void UseCurrentMethodName(this VerifySettings settings, [CallerMemberName] string methodName = "")
    {
        settings.UseMethodName(methodName);
    }
}
