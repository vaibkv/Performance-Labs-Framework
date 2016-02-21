// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerformanceLabs.AllTests.SampleProfilingProject.Models;

namespace PerformanceLabs.AllTests.SampleProfilingProject
{
    public class SampleDBSeedInitializer : DropCreateDatabaseAlways<TestDBBlogContext>
    {
        protected override void Seed(TestDBBlogContext context)
        {
            //context.Database.ExecuteSqlCommand(Constants.MakeLoginCredentialsTestDB);

            Category cat1 = new Category { Id = Guid.NewGuid(), Name = ".NET Framework" };
            Category cat2 = new Category { Id = Guid.NewGuid(), Name = "SQL Server" };
            Category cat3 = new Category { Id = Guid.NewGuid(), Name = "jQuery" };

            context.Categories.Add(cat1);
            context.Categories.Add(cat2);
            context.Categories.Add(cat3);

            BlogPost post1 = new BlogPost { Category = cat1, CategoryId = cat1.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "Task Parallel Library in .Net" };
            BlogPost post2 = new BlogPost { Category = cat1, CategoryId = cat1.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "Customizing CLR" };
            BlogPost post3 = new BlogPost { Category = cat2, CategoryId = cat2.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "Inside Sql Server 2008" };
            BlogPost post4 = new BlogPost { Category = cat2, CategoryId = cat2.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "Fundamentals of T-Sql" };
            BlogPost post5 = new BlogPost { Category = cat3, CategoryId = cat3.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "JQuery by John Resig" };
            BlogPost post6 = new BlogPost { Category = cat3, CategoryId = cat3.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "JQuery Ninjas by John Resig" };

            context.BlogPosts.Add(post1);
            context.BlogPosts.Add(post2);
            context.BlogPosts.Add(post3);
            context.BlogPosts.Add(post4);
            context.BlogPosts.Add(post5);
            context.BlogPosts.Add(post6);

            for (Int32 i = 0; i < 500; i++)
            {
                Category catx = new Category { Id = Guid.NewGuid(), Name = "Category_" + i };
                BlogPost postx = new BlogPost { Category = cat1, CategoryId = cat1.Id, Content = "some content", Id = Guid.NewGuid(), PublishDate = DateTime.UtcNow, Title = "Post_" + i };

                context.Categories.Add(catx);
                context.BlogPosts.Add(postx);
            }
            context.SaveChanges();
        }
    }
}