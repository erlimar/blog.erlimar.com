using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Models;

namespace blog.erlimar.com.Migrations
{
    [DbContext(typeof(BlogContext))]
    partial class BlogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Annotation("ProductVersion", "7.0.0-beta7-15540");

            modelBuilder.Entity("Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .Annotation("Relational:ColumnName", "blog_id");

                    b.Property<string>("Name")
                        .Required();

                    b.Property<string>("Url")
                        .Required();

                    b.Key("BlogId");

                    b.Annotation("Relational:TableName", "blogs");
                });

            modelBuilder.Entity("Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .Annotation("Relational:ColumnName", "post_id");

                    b.Property<int>("BlogId")
                        .Annotation("Relational:ColumnName", "blog_id");

                    b.Property<string>("Content")
                        .Required();

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Title")
                        .Required();

                    b.Key("PostId");

                    b.Annotation("Relational:TableName", "posts");
                });

            modelBuilder.Entity("Models.Post", b =>
                {
                    b.Reference("Models.Blog")
                        .InverseCollection()
                        .ForeignKey("BlogId");
                });
        }
    }
}
