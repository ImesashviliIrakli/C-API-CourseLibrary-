using CourseLibrary.DbContexts;
using CourseLibrary.Entities;
using CourseLibrary.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseLibrary.Services
{
    public class CourseLibraryRepository : ICourseLibraryRepository
    {
        #region DependencyInjection
        private readonly CourseLibraryContext _context;

        public CourseLibraryRepository(CourseLibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Author
        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(i => i.Id == authorId);
        }
        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList();
        }
        public IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorsResourceParameters)
        {
            if (authorsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(authorsResourceParameters));
            }

            if (string.IsNullOrWhiteSpace(authorsResourceParameters.MainCategory)
                && string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
            {
                return GetAuthors();
            }

            var collection = _context.Authors as IQueryable<Author>;

            if (!string.IsNullOrWhiteSpace(authorsResourceParameters.MainCategory))
            {
                string mainCategory = authorsResourceParameters.MainCategory;
                collection = collection.Where(m => m.MainCategory == mainCategory);
            }

            if (!string.IsNullOrWhiteSpace(authorsResourceParameters.SearchQuery))
            {
                string searchQuery = authorsResourceParameters.SearchQuery;
                collection = collection.Where(a => a.MainCategory.Contains(searchQuery)
                || a.FirstName.Contains(searchQuery)
                || a.LastName.Contains(searchQuery));
            }

            return collection.ToList();
        }
        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }
        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(i => i.Id == authorId);
        }
        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Authors.Add(author);
        }
        public void UpdateAuthor(Author author)
        {
            //no code needed
        }
        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
        }
        #endregion

        #region Course
        public Course GetCourse(Guid authorId, Guid courseId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Courses.Where(
                a => a.Id == courseId
                && a.AuthorId == authorId)
                .FirstOrDefault();
        }
        public IEnumerable<Course> GetCourses(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Courses.Where(a => a.AuthorId == authorId)
                .OrderBy(b => b.Title).ToList();
        }
        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            course.AuthorId = authorId;
            _context.Courses.Add(course);
        }
        public void UpdateCourse(Course course)
        {
            //no code needed

        }
        public void DeleteCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            _context.Courses.Remove(course);
        }
        #endregion

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
