namespace Blueprint.Application.Abstractions.Languages;

public interface IActiproLicenseProvider
{
    string Licensee { get; }

    string LicenseKey { get; }
}
