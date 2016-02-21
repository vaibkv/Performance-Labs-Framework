// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PerformanceLabs.AllTests.SampleProfilingProject.Models;

namespace PerformanceLabs.AllTests.SampleProfilingProject
{
    public class TestDBBlogContext : DbContext
    {
        static TestDBBlogContext()
        {
            Database.SetInitializer<TestDBBlogContext>(new SampleDBSeedInitializer());
        }

        public TestDBBlogContext()
            : base("Name=TestDBBlogContext")
        {
            this.Database.Initialize(true);
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}