using Domains.Interfaces;

namespace Core.UseCases;

public class DeleteUserDataUseCase(IBookRepository bookRepository)
{
    public async Task Execute(int userId)
    {
        await bookRepository.DeleteUserData(userId);
    }
}