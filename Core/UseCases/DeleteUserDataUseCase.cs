using Domains.Interfaces;

namespace Core.UseCases;

public class DeleteUserDataUseCase(IBookRepository? bookRepository)
{
    public async Task Execute(int userId)
    {
        try
        {
            Console.WriteLine("Im going to delete user data in use case");
            if (bookRepository != null)
            {
                await bookRepository.DeleteUserData(userId);
            }
            else
            {
                Console.WriteLine("bookRepository is null");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}