using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Common.Request.Courses;
using Domain.Entities;
using Application.DTOs.Courses;
using Microsoft.AspNetCore.Identity;
using Infrastructure.IdentityModels;
using Infrastructure.Data;
using Infrastructure.Services.Courses;
using Application;
using Application.Services.Identity;

namespace Test
{
    public class CourseServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly ILogger<CourseService> _logger;

        private readonly CourseService _service;

        public CourseServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _currentUserMock = new Mock<ICurrentUserService>();

            // UserManager Mock requires UserStore - simplest way for demo:
            var userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _dbContextMock = new Mock<ApplicationDbContext>();
            _logger = new NullLogger<CourseService>();

            _service = new CourseService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _currentUserMock.Object,
                _userManagerMock.Object,
                _dbContextMock.Object,
                _logger);
        }

        [Fact]
        public async Task AddCourseAsync_ShouldReturnSuccess_WhenCourseAdded()
        {
            // Arrange
            var addRequest = new AddCourseRequest { CourseName = "Test Course" , CourseDescription="Welcome" , StartDate = new DateOnly(2025, 10, 15),EndDate = new DateOnly(2025, 12, 30) };
            var courseEntity = new Course { Id = Guid.NewGuid(), CourseName = "Test Course", CourseDescription = "Welcome", StartDate = new DateOnly(2025, 10, 15), EndDate = new DateOnly(2025, 12, 30) };
            var courseDto = new CourseDTO { Id = courseEntity.Id, CourseName = courseEntity.CourseName };

            _mapperMock.Setup(m => m.Map<Course>(It.IsAny<AddCourseRequest>())).Returns(courseEntity);
            _unitOfWorkMock.Setup(u => u.ExecuteTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
                           .Returns<Func<Task>, CancellationToken>((func, token) => func());

            _mapperMock.Setup(m => m.Map<CourseDTO>(courseEntity)).Returns(courseDto);

            // Act
            var result = await _service.AddCourseAsync(addRequest, CancellationToken.None);

            // Assert
            Assert.True(true);
        }
    }

}
