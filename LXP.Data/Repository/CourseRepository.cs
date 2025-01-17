﻿using LXP.Common;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using Microsoft.AspNetCore.Http;
using LXP.Data.DBContexts;

namespace LXP.Data.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _contextAccessor;
        public CourseRepository(LXPDbContext lXPDbContext, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _lXPDbContext = lXPDbContext;
            _environment = environment;
            _contextAccessor = httpContextAccessor;
        }
        public Course GetCourseDetailsByCourseId(Guid CourseId)
        {
            return _lXPDbContext.Courses.Find(CourseId);
        }

        public Course FindCourseid(Guid courseid)
        {
            return _lXPDbContext.Courses.Find(courseid);

        }

        public Enrollment FindEntrollmentcourse(Guid Courseid)
        {
            return _lXPDbContext.Enrollments.FirstOrDefault(Course => Course.CourseId == Courseid);
        }

        public async Task Deletecourse(Course course)
        {
            _lXPDbContext.Courses.Remove(course);
            await _lXPDbContext.SaveChangesAsync();
        }

        public async Task Changecoursestatus(Course course)
        {
             _lXPDbContext.Courses.Update(course);
            await _lXPDbContext.SaveChangesAsync();
        }
        

        public async Task Updatecourse(Course course)
        {
            _lXPDbContext.Courses.Update(course);
            await _lXPDbContext.SaveChangesAsync();
        }

        public IEnumerable<CourseViewModel> GetAllCourse()
        {
            return _lXPDbContext.Courses
                      .Select(c => new CourseViewModel
                      {
                          CourseId = c.CourseId,
                          Title = c.Title,
                          Level = c.Level.Level,
                          Category = c.Category.Category,
                          Duration = c.Duration,
                          CreatedAt = c.CreatedAt,
                      })

                      .ToList();

        }

        public IEnumerable<CourseViewModel> GetLimitedCourse()
        {
            return _lXPDbContext.Courses
              .OrderByDescending(c => c.CreatedAt)
              .Select(c => new CourseViewModel
              {
                  CourseId = c.CourseId,
                  Title = c.Title,
                  Level = c.Level.Level,
                  Category = c.Category.Category,
                  Duration = c.Duration,
                  Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                             _contextAccessor.HttpContext.Request.Scheme,
                             _contextAccessor.HttpContext.Request.Host,
                             _contextAccessor.HttpContext.Request.PathBase,
                             c.Thumbnail)

              })
              .Take(9)
              .ToList();
        }



       

    }
}
