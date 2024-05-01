namespace Domains.Interfaces;

public interface IOpenLibraryRepository
{
    Task<bool> VerifyBook(Book book);
}