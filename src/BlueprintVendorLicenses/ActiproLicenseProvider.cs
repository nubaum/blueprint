using System.Globalization;
using System.Text;
using Blueprint.Abstractions.Licensing;
using Microsoft.Extensions.Configuration;

namespace BlueprintVendorLicenses;

internal sealed class ActiproLicenseProvider : IActiproLicenseProvider
{
    private readonly int[] _licenseeParts = [0x74, 0x66, 0x6F, 0x73, 0x6D, 0x65, 0x52];
    private readonly string[] _parts;

    public ActiproLicenseProvider()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets(typeof(ActiproLicenseProvider).Assembly, optional: true)
            .Build();

        _parts = LoadParts(configuration, "Actipro", "VENDOR_LICENSE_KEY_PART", 5);
    }

    public string GetLicensee()
    {
        StringBuilder sb = new();
        for (int i = 6; i >= 0; i--)
        {
            sb.Append(((char)_licenseeParts[i]).ToString(CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }

    public string GetLicense()
    {
        return string.Join('-', _parts);
    }

    private static string[] LoadParts(
        IConfiguration configuration,
        string sectionName,
        string environmentPrefix,
        int count)
    {
        string[] parts = new string[count];

        for (int i = 0; i < count; i++)
        {
            string? value =
                configuration[$"{sectionName}:LicenseKey{i + 1}"] ??
                configuration[$"{environmentPrefix}{i + 1}"];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Missing vendor license key.");
            }

            parts[i] = value;
        }

        return parts;
    }
}
