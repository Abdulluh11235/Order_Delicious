using Common;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seedRolesAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns : new [] {"Id","Name","NormalizedName","ConcurrencyStamp"},
                values: new object[] {Guid.NewGuid().ToString(),Roles.User,Roles.User.ToUpper(),Guid.NewGuid().ToString()}
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] " +
                                 $"WHERE NAME ='{Roles.User}'");
        }
    }
}
