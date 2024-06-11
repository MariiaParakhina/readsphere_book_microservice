using Domains.Interfaces;

namespace Core.UseCases;

public class UpdateBookPrivacyUseCase(IBookRepository bookRepository)
{
    public async Task Execute(int userId, int bookId, bool isHidden)
    {
        // call to update the record for user book to set ishidden as varianble isHidden
        try
        {
            await bookRepository.UpdateBookPrivacy(userId, bookId, isHidden);    
        }catch(Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Error in updating the privacy of the book");
        }
        
    }

}