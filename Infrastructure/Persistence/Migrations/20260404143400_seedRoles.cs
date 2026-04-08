using Common;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns : new [] {"Id","Name","NormalizedName","ConcurrencyStamp"},
                values: new object[] {Guid.NewGuid().ToString(),Roles.Admin,Roles.Admin.ToUpper(),Guid.NewGuid().ToString()}
                );
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns : new [] {"Id","Name","NormalizedName","ConcurrencyStamp"},
                values: new object[] {Guid.NewGuid().ToString(),Roles.Restaurant,Roles.Restaurant.ToUpper(),Guid.NewGuid().ToString()}
            );
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns : new [] {"Id","Name","NormalizedName","ConcurrencyStamp"},
                values: new object[] {Guid.NewGuid().ToString(),Roles.Employee,Roles.Employee.ToUpper(),Guid.NewGuid().ToString()}
            );
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns : new [] {"Id","Name","NormalizedName","ConcurrencyStamp"},
                values: new object[] {Guid.NewGuid().ToString(),Roles.Customer,Roles.Customer.ToUpper(),Guid.NewGuid().ToString()}
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
