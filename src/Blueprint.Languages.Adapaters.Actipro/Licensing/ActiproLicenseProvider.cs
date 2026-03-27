using Blueprint.Abstractions.Licensing;
using Microsoft.Extensions.Configuration;

namespace Blueprint.Languages.Adapaters.Actipro.Licensing;

internal sealed class ActiproLicenseProvider(IConfiguration configuration) : IActiproLicenseProvider
{
    public string Licensee => configuration["Actipro:Licensee"] ?? string.Empty;

    public string LicenseKey => configuration["Actipro:LicenseKey"] ?? string.Empty;
}
