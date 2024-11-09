using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWebStore.UnitTest.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using BussinessLogic.Interface;
    using BussinessLogic.Service;
    using DataAccess.Interface;
    using Entity.Models;
    using Entity.Query;
    using FakeItEasy;
    using Xunit;

    public class AuthorServiceTest
    {
        private readonly Fixture _fixture;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorService _sut;
        private readonly IAuthorRepository _authorRepository;

        public AuthorServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
               .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _unitOfWork = A.Fake<IUnitOfWork>();
            _authorRepository = A.Fake<IAuthorRepository>();

            A.CallTo(() => _unitOfWork.Author).Returns(_authorRepository);
            _sut = new AuthorService(_unitOfWork);
        }

        // Test methods go here...
        [Fact]
        public async Task GetAllAuthorsAsync_WhenCalled_ShouldReturnAllAuthors()
        {
            // Arrange
            var authors = _fixture.CreateMany<Author>(3);
            A.CallTo(() => _authorRepository.ListAllAsync(A<QueryOptions<Author>>._)).Returns(Task.FromResult(authors));

            // Act
            var result = await _sut.GetAllAuthorsAsync();

            // Assert
            A.CallTo(() => _authorRepository.ListAllAsync(A<QueryOptions<Author>>._)).MustHaveHappenedOnceExactly();
            Assert.Equal(authors, result);
        }
        [Fact]
        public async Task GetAuthorByIdAsync_WhenCalled_ShouldReturnAuthorWithBooks()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var expectedAuthor = _fixture.Build<Author>()
                                         .With(a => a.AuthorId, authorId)
                                         .Create();

            // Set up the query options to match the expected options in the service method
            var options = new QueryOptions<Author>
            {
                Where = a => a.AuthorId == authorId,
                Includes = "Books"
            };

            A.CallTo(() => _authorRepository.GetAsync(A<QueryOptions<Author>>._))
                .Returns(expectedAuthor);

            // Act
            var result = await _sut.GetAuthorByIdAsync(authorId);

            // Assert
            A.CallTo(() => _authorRepository.GetAsync(A<QueryOptions<Author>>._)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedAuthor, result);
        }
        [Fact]
        public async Task GetAuthorsByTermAsync_WhenCalled_ShouldReturnFilteredAuthors()
        {
            string searchTerm = "John";
            var authors = _fixture.CreateMany<Author>(5).ToList();

            // Setup query options to match the filtering logic
            var options = new QueryOptions<Author>
            {
                Where = a => a.FirstName.Contains(searchTerm) || a.LastName.Contains(searchTerm)
            };

            A.CallTo(() => _authorRepository.ListAllAsync(A<QueryOptions<Author>>._))
                .Returns(authors);

            // Act
            var result = await _sut.GetAuthorsByTermAsync(searchTerm);

            // Assert
            A.CallTo(() => _authorRepository.ListAllAsync(A<QueryOptions<Author>>._)).MustHaveHappenedOnceExactly();

            Assert.Equal(authors, result);
        }
        [Fact]
        public async Task AddAuthorAsync_WhenCalled_ShouldAddAuthorAndSave()
        {
            // Arrange
            var author = _fixture.Create<Author>();

            // Act
            await _sut.AddAuthorAsync(author);

            // Assert
            A.CallTo(() => _authorRepository.Add(author)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
        }
        [Fact]
        public async Task RemoveAuthorAsync_WhenCalled_ShouldRemoveAuthorAndSave()
        {
            // Arrange
            var author = _fixture.Create<Author>();

            // Act
            await _sut.RemoveAuthorAsync(author);

            // Assert
            A.CallTo(() => _authorRepository.Remove(author)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _unitOfWork.SaveAsync()).MustHaveHappenedOnceExactly();
        }
    }


}
