using System;
using System.Collections.Generic;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Framework.DependencyInjection;

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
			
			blog.Property(b => b.BlogId)
				.HasColumnName("blog_id")
				.ValueGeneratedOnAdd();
				
			blog.Property(b => b.Name)
				.Required();
				
			blog.Property(b => b.Url)
				.Required();
				
			var post = builder.Entity<Post>().ToTable(name: "posts");
			
			post.Property(p => p.PostId)
				.HasColumnName("post_id")
				.ValueGeneratedOnAdd();
				
			post.Property(p => p.Blogid)
				.HasColumnName("blog_id");
				
			post.Property(p => p.Created)
				.ValueGeneratedOnAdd();
				
			post.Property(p => p.LastUpdated)
				.ValueGeneratedOnAddOrUpdate();
			
			post.Property(p => p.Title)
				.Required();
				
			post.Property(p => p.Content)
				.Required();
		}
	}

    public class Blog
    {
		public int BlogId { get; set; }
		public string Url { get; set; }
		public string Name { get; set; }
		
		public List<Post> Posts { get; set; }
    }
	
    public class Post
    {
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		
		public DateTime Created { get; set; }
		public DateTime LastUpdated { get; set; }
		
		public int Blogid { get; set; }
		public Blog Blog { get; set; }
    }
}
