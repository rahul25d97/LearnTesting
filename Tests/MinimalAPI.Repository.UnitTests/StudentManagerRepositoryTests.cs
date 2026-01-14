using AutoFixture;
using FluentAssertions;
using MinimalAPI.Models;
using Moq;
using Moq.EntityFrameworkCore;

namespace MinimalAPI.Repository.Tests
{
    public class StudentManagerRepositoryTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<DatabaseContext> _dbContextMock;
        private readonly StudentManagerRepository _sut;

        private readonly List<Student> _students;

        public StudentManagerRepositoryTests()
        {
            _fixture = new Fixture();
            _dbContextMock = _fixture.Freeze<Mock<DatabaseContext>>();
            _sut = new StudentManagerRepository(_dbContextMock.Object);
            _students = new List<Student>
                    {
                        new Student { StudentId = 1, Name = "Rahul", Address = "Pune", Department = "IT", ContactNo = "123" },
                        new Student { StudentId = 2, Name = "Sneha", Address = "Mumbai", Department = "HR", ContactNo = "456" },
                        new Student { StudentId = 3, Name = "Amit", Address = "Pune", Department = "Finance", ContactNo = "789" }
                    };
        }

        
        [Fact]
        public async Task Get_AllStudent_ShouldReturnOKResponse_WhenDataFound()
        {
            // Arrange
            _dbContextMock.Setup(s => s.StudentDB).ReturnsDbSet(_students);

            string sSearchText = "Pune";
            int pageNumber = 1;
            int pageSize = 10;

            // Act
            var result = await _sut.Get_AllStudent(sSearchText, pageNumber, pageSize, "Name", "asc");

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(2); // Rahul and Amit match "Pune"
            result.All(r => r.Address.Contains("Pune")).Should().BeTrue();
            result.All(r => r.NOR == 2).Should().BeTrue(); // NOR = total count of filtered items

        }


        [Fact]
        public void Get_StudentById_ShouldReturnOKResponse_WhenDataFound()
        {
            // Arrange
            var id = 2;            
            _dbContextMock.Setup(s => s.StudentDB).ReturnsDbSet(_students);

            // Act
            var result = _sut.Get_SudentById(id);

            // Assert
            result.Should().NotBeNull();
        }


        [Fact]
        public void Save_Student_ShouldReturnOKResponse_WhenCreate()
        {
            var student = new Student { StudentId = 0, Name = "Pinky", Address = "Pune", Department = "IT", ContactNo = "123" };
            _dbContextMock.Setup(s => s.StudentDB).ReturnsDbSet(_students);
            _dbContextMock.Setup(x => x.SaveChanges()).Returns(1).Verifiable();

            // Act
            var result = _sut.Save_Student(student);

            // Assert
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());           

        }

        [Fact]
        public void Save_Student_ShouldReturnOKResponse_WhenUpdate()
        {
            var student = new Student { StudentId = 3, Name = "Pinky", Address = "Pune", Department = "IT", ContactNo = "123" };
            _dbContextMock.Setup(s => s.StudentDB).ReturnsDbSet(_students);
            _dbContextMock.Setup(x => x.SaveChanges()).Returns(1).Verifiable();

            // Act
            var result = _sut.Save_Student(student);

            // Assert
            result.Should().NotBeNull();
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Remove_Student_ShouldReturnOKResponse()
        {
            _dbContextMock.Setup(s => s.StudentDB).ReturnsDbSet(_students);
            _dbContextMock.Setup(x => x.SaveChanges()).Returns(1).Verifiable();

            // Act
            _sut.Remove_Student(1);

            // Assert
            _dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
        }

    }
}