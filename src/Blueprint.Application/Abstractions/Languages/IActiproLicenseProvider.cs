namespace Blueprint.Abstractions.Licensing;

public interface IActiproLicenseProvider
{
    string Licensee { get; }

    string LicenseKey { get; }
}
