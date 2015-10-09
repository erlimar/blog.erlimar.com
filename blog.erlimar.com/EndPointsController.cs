using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using Models;

[Route("api/" + EndPointsController.API_VERSION)]
public class EndPointsController : Controller
{
    ILogger _log;

    public EndPointsController(ILoggerFactory loggerfactory)
    {
        _log = loggerfactory.CreateLogger(this.GetType().Name);
    }

    const string API_VERSION = "v1";

    [FromServices]
    public BlogContext BlogContext { get; set; }

    [HttpGet("version")]
    public IActionResult ShowApiVersion()
    {
        _log.LogVerbose("[HttpGet(\"version\")]");
        return Ok(API_VERSION);
    }
    
    [HttpGet("blogs")]
    public IActionResult GetAllBlogs()
    {
        _log.LogVerbose("[HttpGet(\"blogs\")]");
        
        return Ok(BlogContext.Blogs);
    }

    /// <summary>
    /// http://blog.erlimar.com/api/v1/newblog/My%20%Blog%20%Name
    /// </summary>
    [HttpGet("newblog/{blogName}")]
    public IActionResult AddBlog(string blogName)
    {
        _log.LogVerbose("[HttpGet(\"newblog/{blogName}\")]");

        if (BlogContext.Blogs.Any(where => string.Compare(where.Name, blogName, true) == 0))
        {
            throw new Exception($"The blog '{ blogName } ' already exists.");
        }

        var blogUrl = blogName
            .Trim()
            .Replace(" ", "-")
            .ToLower();

        var blog = new Blog { Name = blogName, Url = blogUrl };

        BlogContext.Blogs.Add(blog);

        BlogContext.SaveChangesAsync();

        return Ok(blog);
    }
    
    /// <summary>
    /// http://blog.erlimar.com/api/v1/deleteblog/2
    /// </summary>
    [HttpGet("deleteblog/{blogId}")]
    public IActionResult DeleteBlog(int blogId)
    {
        _log.LogVerbose("[HttpGet(\"deleteblog/{blogId}\")]");
        
        var blog = BlogContext.Blogs.SingleOrDefault(b => b.BlogId == blogId);
        
        if(blog != null) {
            BlogContext.Blogs.Remove(blog);
            BlogContext.SaveChanges();
        }

        return Ok();
    }
    
    /// <summary>
    /// http://blog.erlimar.com/api/v1/my-blog-name/posts
    /// </summary>
    [HttpGet("{blogUrl}/posts")]
    public IActionResult GetAllPosts(string blogUrl)
    {
        _log.LogVerbose("[HttpGet(\"{blogUrl}/posts\")]");
        
        var blog = BlogContext.Blogs.SingleOrDefault(where => string.Compare(where.Url, blogUrl, true) == 0);

        if (blog == null)
        {
            return HttpNotFound($"Blog '{blogUrl}' not found!");
        }

        var posts = BlogContext.Posts
            .Where(w => string.Compare(w.Blog.Url, blogUrl, true) == 0);
        
        return Ok(posts);
    }

    /// <summary>
    /// http://blog.erlimar.com/api/v1/my-blog-name/newpost/My%20%Post%20%Title/Content%20%of%20%post...
    /// </summary>
    [HttpGet("{blogUrl}/newpost/{postTitle}/{postContent}")]
    public IActionResult AddPost(string blogUrl, string postTitle, string postContent)
    {
        _log.LogVerbose("[HttpGet(\"{blogUrl}/newpost/{postTitle}/{postContent}\")]");
        _log.LogVerbose($"blogUrl: {blogUrl}, postTitle: {postTitle}, postContent: {postContent}");

        var blog = BlogContext
            .Blogs
            .SingleOrDefault(where => string.Compare(where.Url, blogUrl, true) == 0);

        _log.LogVerbose($"Blog is null? => {blog == null}");

        if (blog == null)
        {
            _log.LogVerbose("Blog not found!");
        }
        else
        {
            _log.LogVerbose($"blog.id: {blog.BlogId}, blog.name: {blog.Name}, blog.url: {blog.Url}");
        }
        
        var now = DateTime.Now;
        
        var post = new Post
        {
            BlogId = blog.BlogId,
            Title = postTitle,
            Content = postContent,
            Created = now,
            LastUpdated = now
        };
        
        _log.LogVerbose($"New Post => blogId: {post.BlogId}, title: {post.Title}, content: {post.Content}, created: {post.Created}, lastUpdated: {post.LastUpdated}");
        
        _log.LogVerbose($"Blog posts is null?: {blog.Posts == null}");
        
        BlogContext.Posts.Add(post);

        BlogContext.SaveChanges();
        
        return Ok(post);
    }
}