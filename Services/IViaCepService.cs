namespace TesteDevCSharp.Services
{
    public interface IViaCepService
    {
        Task<string?> BuscarCepAsync(string cep);
    }
}