using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 6, nullable: true),
                    CountryID = table.Column<Guid>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), "China" },
                    { new Guid("7cf40db8-7f7c-b531-f0a5-abf2d7cd778b"), "USA" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("06b68984-41ee-4e3b-af4f-e57493198042"), "577 7th Terrace", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1993, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "dlevensky1@barnesandnoble.com", "Female", "Devan", false },
                    { new Guid("304b8efd-f866-4566-a279-5b72f7f358c2"), "11866 Graceland Way", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1986, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "dearsman6@cbsnews.com", "Male", "Dilly", true },
                    { new Guid("3aef3959-0c4e-4860-a66f-74c5d0563488"), "348 Anniversary Avenue", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1990, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "fstrother5@vimeo.com", "Male", "Frannie", false },
                    { new Guid("621f8b35-8618-4c4f-b088-e961525a8e15"), "6 Parkside Hill", new Guid("7cf40db8-7f7c-b531-f0a5-abf2d7cd778b"), new DateTime(2000, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "tribbens2@blogger.com", "Female", "Thomasin", false },
                    { new Guid("6776db70-e3d6-49df-ba18-12dc86c968f2"), "80 4th Street", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1996, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "tcampana3@hud.gov", "Male", "Todd", false },
                    { new Guid("aa0f0d65-2e9b-4b4f-91e9-9b93f5dc3280"), "40 Old Gate Junction", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1988, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "bmersh8@aboutads.info", "Female", "Bobby", false },
                    { new Guid("b7660feb-aa9b-4664-8190-f2b52e2681ad"), "4 Transport Center", new Guid("7cf40db8-7f7c-b531-f0a5-abf2d7cd778b"), new DateTime(1985, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hbroad0@marketwatch.com", "Male", "Harv", false },
                    { new Guid("dfd67610-631e-4723-97fa-dbd92aab4526"), "68 Oakridge Crossing", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1983, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "amedlar7@umn.edu", "Male", "Alphonso", true },
                    { new Guid("e27ea869-7a6b-49d2-8a96-718ea4d8e8b0"), "1 Daystar Hill", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1991, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahyatt4@bbb.org", "Male", "Aloin", false },
                    { new Guid("f26fda3f-a537-473f-8c6b-b47caff2cb86"), "161 Grasskamp Way", new Guid("384b362a-0f7b-da3c-69bd-36e615484632"), new DateTime(1983, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "jlearoid9@marketwatch.com", "Female", "Jehanna", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
