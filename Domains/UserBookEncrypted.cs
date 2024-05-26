namespace Domains;

public class UserBookEncrypted
{
    private byte[] UserId { get; set; }
    private byte[] BookId { get; set; }

    public UserBookEncrypted(int userId, int bookId)
    {
        UserId = StringEncryptor.Encrypt(userId.ToString());
        BookId = StringEncryptor.Encrypt(bookId.ToString());
    }
}