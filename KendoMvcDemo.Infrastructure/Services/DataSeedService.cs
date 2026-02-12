using ClosedXML.Excel;
using KendoMvcDemo.Core.Entities;
using KendoMvcDemo.Core.Models.Seed;
using KendoMvcDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace KendoMvcDemo.Infrastructure.Services
{
    public class DataSeedService
    {
        private readonly KaustDbContext db;

        public DataSeedService(KaustDbContext db)
        {
            this.db = db;
        }

        public async Task SeedAsync()
        {
            await SeedCoursesAsync();
            await SeedStudentsAndAssignToCoursesAsync();
        }

        private async Task SeedCoursesAsync()
        {
            IEnumerable<CourseSeedModel> coursesWithTeachers = ReadCoursesAndTeachersFromExcel();

            if (await db.Courses.AnyAsync())
            {
                return;
            }

            foreach (CourseSeedModel courseModel in coursesWithTeachers)
            {
                Course existingCourse = await db.Courses
                    .FirstOrDefaultAsync(c => c.Name == courseModel.Name);

                if (existingCourse != null)
                {
                    continue;
                }

                Course newCourse = new()
                {
                    Code = courseModel.Code,
                    Name = courseModel.Name,
                    Section = courseModel.Section,
                    Start = courseModel.StartDate,
                    End = courseModel.EndDate
                };

                foreach (TeacherSeedModel teacher in courseModel.Teachers)
                {
                    Teacher dbTeacher = await db.Teachers.FirstOrDefaultAsync(x => x.Name == teacher.Name);

                    if (dbTeacher == null)
                    {
                        dbTeacher = new()
                        {
                            Name = teacher.Name
                        };

                        await db.Teachers.AddAsync(dbTeacher);

                        await db.SaveChangesAsync();
                    }
                    newCourse.Teachers.Add(new CourseTeacher
                    {
                        Course = newCourse,
                        TeacherId = dbTeacher.Id
                    });
                }

                await db.Courses.AddAsync(newCourse);
            }

            await db.SaveChangesAsync();
        }

        private async Task SeedStudentsAndAssignToCoursesAsync()
        {
            IEnumerable<StudentSeedModel> students = ReadStudentsFromJsonFile();

            if (await db.Students.AnyAsync())
            {
                return;
            }

            List<Course> courses = await db.Courses.ToListAsync();
            Random random = new();

            foreach (StudentSeedModel studentModel in students)
            {
                Student existingStudent = await db.Students
                    .FirstOrDefaultAsync(s => s.Id == studentModel.Id);

                if (existingStudent != null)
                {
                    continue;
                }

                Student newStudent = new()
                {
                    Name = studentModel.Name,
                    StudentNumber = studentModel.StudentNumber
                };

                // Get random courses for the student
                int coursesCount = random.Next(10, 20);

                List<Course> randomCourses = courses
                    .OrderBy(_ => random.Next())
                    .Take(coursesCount)
                    .ToList();

                foreach (Course randomCourse in randomCourses)
                {
                    newStudent.Enrollments.Add(new Enrollment
                    {
                        CourseId = randomCourse.Id,
                        DateEnrolled = DateTime.UtcNow,
                    });
                }

                await db.Students.AddAsync(newStudent);
            }

            await db.SaveChangesAsync();
        }

        private IEnumerable<CourseSeedModel> ReadCoursesAndTeachersFromExcel()
        {
            using Stream excelStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("KendoMvcDemo.Infrastructure.KaustCourses.xlsx");

            List<CourseSeedModel> courses = [];

            using XLWorkbook workbook = new(excelStream);
            IXLWorksheet sheet = workbook.Worksheet(1);

            foreach (IXLRow row in sheet.RowsUsed().Skip(1))
            {
                CourseSeedModel course = new()
                {
                    Code = row.Cell("A").GetString(),
                    Name = row.Cell("B").GetString(),
                    Section = row.Cell("C").GetString(),
                    StartDate = row.Cell("H").GetDateTime(),
                    EndDate = row.Cell("I").GetDateTime()
                };

                List<TeacherSeedModel> teachers = row.Cell("E").GetString()
                    .Split("/", StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => new TeacherSeedModel
                    {
                        Name = t.Trim()
                    }).ToList();

                course.Teachers = teachers;

                courses.Add(course);
            }

            return courses;
        }

        private IEnumerable<StudentSeedModel> ReadStudentsFromJsonFile()
        {
            using Stream studentsStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("KendoMvcDemo.Infrastructure.StudentsSeed.json");

            using MemoryStream ms = new();
            studentsStream.CopyToAsync(ms);

            string studentsJson = System.Text.Encoding.UTF8.GetString(ms.ToArray());

            IEnumerable<StudentSeedModel> students = JsonSerializer.Deserialize<IEnumerable<StudentSeedModel>>(studentsJson);

            return students;
        }
    }
}
