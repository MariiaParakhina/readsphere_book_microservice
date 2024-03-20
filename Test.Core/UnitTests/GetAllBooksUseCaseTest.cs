using Core.UseCases;
using Domains;
using Domains.Interfaces;
using NUnit.Framework;
using Test.Core.Helpers;


namespace Test.Core.UnitTests;

[TestFixture]
public class GetAllBooksUseCaseTest
{
    [Test]
    public void TestUseCase()
    {
        // Arrange
        IBookRepository repository = new BooksRepositoryHelper();
        var useCase = new GetAllBooksUseCase(repository);

        //Act 
        List<Book> books = useCase.Execute();

        //Assert  
        Assert.That(books.Count == 1);
    }
    public void TestUseCase_fail()
    {
        // Arrange
        IBookRepository repository = new BooksRepositoryHelper();
        var useCase = new GetAllBooksUseCase(repository);

        //Act 
        List<Book> books = useCase.Execute();

        //Assert  
        Assert.That(books.Count != 2);
    }
}