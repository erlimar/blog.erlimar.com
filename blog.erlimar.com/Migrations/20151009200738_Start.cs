using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace blog.erlimar.com.Migrations
{
    public partial class Start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    blog_id = table.Column<int>(isNullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(isNullable: false),
                    Url = table.Column<string>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.blog_id);
                });
            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    post_id = table.Column<int>(isNullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    blog_id = table.Column<int>(isNullable: false),
                    Content = table.Column<string>(isNullable: false),
                    Created = table.Column<DateTime>(isNullable: false),
                    LastUpdated = table.Column<DateTime>(isNullable: false),
                    Title = table.Column<string>(isNullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_Post_Blog_BlogId",
                        column: x => x.blog_id,
                        principalTable: "blogs",
                        principalColumn: "blog_id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("posts");
            migrationBuilder.DropTable("blogs");
        }
    }
}
