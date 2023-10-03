using BlogPessoal.Model;

namespace BlogPessoal.Security;

public interface IAuthService
{
    Task<UserLogin?> Autenticar(UserLogin userLogin);
}