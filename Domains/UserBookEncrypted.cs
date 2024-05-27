namespace Domains;

public class UserBookEncrypted
{
    public byte[] UserId { get; set; }
    public byte[] BookId { get; set; }

    public UserBookEncrypted(int userId, int bookId)
    {
        UserId = StringEncryptor.Encrypt(userId.ToString());
        BookId = StringEncryptor.Encrypt(bookId.ToString());
    }
}