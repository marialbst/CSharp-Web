namespace StudentSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;

    class Program
    {
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            using (StudentDbContext ctx = new StudentDbContext())
            {
                ctx.Database.Migrate();

                SeedData(ctx);
            }
        }

        private static void SeedData(StudentDbContext ctx)
        {
            const int totalStudents = 25;
            const int totalCourses = 10;
            DateTime currentDate = DateTime.Now;
            List<Course> courses = new List<Course>();
            //Add students
            for (int i = 0; i < totalStudents; i++)
            {
                ctx.Students.Add(new Student
                {
                    Name = $"Student {i}",
                    RegistrationDate = currentDate.AddDays(i),
                    Birthday = currentDate.AddYears(-8 * i).AddDays(i * 2),
                    Phone = $"Some phone {i}"
                });
            }

            ctx.SaveChanges();

            //Add courses
            for (int i = 0; i < totalCourses; i++)
            {
                Course currCourse = new Course
                {
                    Name = $"Course {i}",
                    Price = 100 * (i % 3),
                    Description = $"Some description {i}",
                    StartDate = currentDate.AddDays(i),
                    EndDate = currentDate.AddMonths(i * 2)
                };

                courses.Add(currCourse);
                ctx.Courses.Add(currCourse);
            }

            ctx.SaveChanges();

            //Students in courses
            var studentIds = ctx.Students
                .Select(s => s.Id)
                .ToList();

            for (int i = 0; i < totalCourses; i++)
            {
                Course currCourse = courses[i];
                int studentsinCourse = rnd.Next(2, totalStudents / 2);

                for (int j = 0; j < studentsinCourse; j++)
                {
                    var studentId = studentIds[rnd.Next(0, studentIds.Count)];
                    currCourse.Students.Add(new StudentCourse { StudentId = studentId });
                }
            }
        }

        private static void PrepareDb(StudentDbContext ctx)
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }
}