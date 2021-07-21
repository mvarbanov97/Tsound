using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Plugin.Spotify.WebApi;
using TSound.Plugin.Spotify.WebApi.Authorization;
using TSound.Services.Models;
using TSound.Services.Tests.Infrastructure;
using Xunit;

namespace TSound.Services.Tests.Categories
{
    public class CategoriesServiceTests : BaseTest<CategoryService>
    {
        [Fact]
        public async Task GetCategoryByIdAsync_Shoud_Return_Correct_Entity()
        {
            using (this.dbContext)
            {
                // Arrange
                var category = new Category { Id = Guid.NewGuid(), Name = "Hip Hop", SpotifyId = "hiphop" };
                await this.unitOfWork.Categories.AddAsync(category);
                await this.unitOfWork.CompleteAsync();

                // Act
                var expectedCategory = this.unitOfWork.Categories.All().Where(x => x.Id == category.Id).FirstOrDefault();
                var actualCategory = await this.sut.Object.GetCategoryByIdAsync(category.Id);

                // Assert
                Assert.NotNull(actualCategory);
                Assert.Equal(expectedCategory.Id, actualCategory.Id);
                Assert.Equal(expectedCategory.Name, actualCategory.Name);
                Assert.Equal(expectedCategory.SpotifyId, actualCategory.SpotifyId);
                Assert.IsType<CategoryServiceModel>(actualCategory);
            }
            
        }

        [Fact]
        public async Task GetCategoryByIdAsync_Should_Throw_When_CategoryNotFound()
        {
            // Arrange
            var expectedMessage = "Category Not Found.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Object.GetCategoryByIdAsync(Guid.NewGuid()));
            Assert.Equal(expectedMessage, exception.ParamName);
        }

        [Fact]
        public async Task GetCategoryByPlaylistId_Should_Return_Collection_Of_Categories()
        {
            // Arrange
            var playlistOneId = this.dbContext.Playlists.Select(x => x.Id).FirstOrDefault();

            // Act
            var actualCategories = await sut.Object.GetCategoryByPlaylistIdAsync(playlistOneId);

            // Assert
            Assert.NotNull(actualCategories);
            Assert.Equal(2, actualCategories.Count());
            Assert.IsAssignableFrom<IEnumerable<CategoryServiceModel>>(actualCategories);
        }

        [Fact]
        public async Task GetCategoryByPlaylistId_Should_Throw_When_No_Such_Playlist_In_Db()
        {
            // Arrange
            var nonExistingPlaylistID = Guid.NewGuid();
            var expectedMessage = "There is no playlist with such Id in the database.";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => this.sut.Object.GetCategoryByPlaylistIdAsync(nonExistingPlaylistID));
            Assert.Equal(expectedMessage, exception.ParamName);
        }

        [Fact]
        public async Task GetAllCategories_Should_Return_Correct_Collection_Of_Categorires()
        {
            // Arrange
            // 3 Categories are already seeded when we inherit the base class (the db is initialized in the constructor for each test)

            // Act
            var categories = await this.sut.Object.GetAllCategoriesAsync();

            // Assert
            Assert.Equal(3, categories.Count());
        }

        [Fact]
        public async Task GetAllCategories_Should_Return_Empty_Collection_When_No_Collections_In_Db()
        {
            // Arrange -  Remove all the pre-seeded categories so the service is correctly tested
            var allSeededCategories = from c in this.dbContext.Categories select c;
            this.dbContext.Categories.RemoveRange(allSeededCategories);
            this.dbContext.SaveChanges();

            // Act 
            var categories = await this.sut.Object.GetAllCategoriesAsync();

            // Assert
            Assert.Empty(categories);
        }

        [Fact]
        public async Task LoadCategoriesInDbAsync_Should_Load_When_No_Categories_In_Db()
        {
            // Arrange -  Remove all the pre-seeded categories so the service is correctly tested
            var allSeededCategories = from c in this.dbContext.Categories select c;
            this.dbContext.Categories.RemoveRange(allSeededCategories);
            this.dbContext.SaveChanges();

            // Spotify Api wrapper - default value is 20
            var expectedCategoriesCount = 20;

            // Act
            await this.sut.Object.LoadCategoriesInDb();

            Assert.Equal(expectedCategoriesCount, this.dbContext.Categories.Count());
        }

        [Fact]
        public async Task LoadCategoriesInDbAsync_Should_NotLoad_When_Db_Has_Categories()
        {
            // Assert
            // 3 Categories are already seeded when we inherit the base class (the db is initialized in the constructor for each test)
            var expectedCategoriesCount = 3;

            // Act
            var actualCategories = await this.sut.Object.GetAllCategoriesAsync();
            await this.sut.Object.LoadCategoriesInDb();

            Assert.Equal(expectedCategoriesCount, actualCategories.Count());
        }
    }
}
