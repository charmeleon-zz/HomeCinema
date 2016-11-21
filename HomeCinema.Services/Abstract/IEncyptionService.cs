namespace HomeCinema.Services
{
    public interface IEncyptionService
    {
        string CreateSalt();
        string EncryptPassword(string password, string salt);
    }
}
