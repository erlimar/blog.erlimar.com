using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using Newtonsoft.Json;

namespace Models
{
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var appPath = CallContextServiceLocator
                .Locator
                .ServiceProvider
                .GetRequiredService<IApplicationEnvironment>()
                .ApplicationBasePath;

            optionsBuilder.UseSqlite($"Data Source={ appPath }/blog.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var blog = builder.Entity<Blog>().ToTable(name: "blogs");
            
            blog.Key(b => b.BlogId);
            
            blog.Property(b => b.BlogId)
                .HasColumnName("blog_id")
                .ValueGeneratedOnAdd();

            blog.Property(b => b.Name)
                .Required();

            blog.Property(b => b.Url)
                .Required();
                

            
            blog.Collection(b => b.Posts);
                
            var post = builder.Entity<Post>().ToTable(name: "posts");
            
            post.Key(p => p.PostId);
            
            post.Reference(p => p.Blog)
                .InverseCollection(b => b.Posts)
                .ForeignKey(b => b.BlogId);

            post.Property(p => p.PostId)
                .HasColumnName("post_id")
                .ValueGeneratedOnAdd();

            post.Property(p => p.BlogId)
                .HasColumnName("blog_id");

            post.Property(p => p.Title)
                .Required();

            post.Property(p => p.Content)
                .Required();
                               
            base.OnModelCreating(builder);
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        public int BlogId { get; set; }
        
        [JsonIgnore]
        public Blog Blog { get; set; }
    }
}
