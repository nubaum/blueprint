namespace Blueprint.Abstractions.Licensing;

public interface IActiproLicenseProvider
{
    string GetLicense();

    string GetLicensee();
}
