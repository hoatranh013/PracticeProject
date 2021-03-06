using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIWeapon.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddFriendNotis",
                columns: table => new
                {
                    AddFriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheSender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheReceiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandleOrNot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddFriendNotis", x => x.AddFriId);
                });

            migrationBuilder.CreateTable(
name: "dbo.SuggestionClass",
        columns: table => new
        {
            SuggestionClassId = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            SuggestionClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            SuggestionClassDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            SuggestionClassAttribute = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            SuggestionClassWeapon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            SuggestionClassEffect = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_dbo.SuggestionClass", x => x.SuggestionClassId);
        });

            migrationBuilder.CreateTable(
name: "dbo.SuggestionGame",
        columns: table => new
        {
            SuggestionGameId = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            SuggestionGameDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            SuggestionGamePros = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            SuggestionGameCons = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            SuggestionGameRate = table.Column<int>(type: "int", maxLength: 50, nullable: false),
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_dbo.SuggestionGame", x => x.SuggestionGameId);
        });

            migrationBuilder.CreateTable(
name: "dbo.SuggestionWeapon",
columns: table => new
{
SuggestWeaponId = table.Column<int>(type: "int", nullable: false)
    .Annotation("SqlServer:Identity", "1, 1"),
SuggestWeaponName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
    SuggestWeaponEffect = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
    SuggestWeaponAttribute = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
    SuggestWeaponDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
},
constraints: table =>
{
table.PrimaryKey("PK_dbo.SuggestionWeapon", x => x.SuggestWeaponId);
});

            migrationBuilder.CreateTable(
                name: "dbo.CharacterModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Class = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.CharacterModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dbo.WeaponModels",
                columns: table => new
                {
                    WeaponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WeaponDefense = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    WeaponAttack = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    WeaponAttribute = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WeaponOwner = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.WeaponModels", x => x.WeaponId);
                });

            migrationBuilder.CreateTable(
                name: "FriendLists",
                columns: table => new
                {
                    FriendListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheOwnered = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FriendName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLists", x => x.FriendListId);
                });

            migrationBuilder.CreateTable(
                name: "LoginModels",
                columns: table => new
                {
                    LoginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginModels", x => x.LoginId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationModels",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TheSender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheReceiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeaponTrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandleOrNot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationModels", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "PreviousPasswordModels",
                columns: table => new
                {
                    DeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreviousToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Handle = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousPasswordModels", x => x.DeId);
                });

            migrationBuilder.CreateTable(
                name: "ResettingPasswordModels",
                columns: table => new
                {
                    RsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RsCharacterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RsClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RsRule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RsGmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResettingPasswordModels", x => x.RsId);
                });

            migrationBuilder.CreateTable(
                name: "ResettingTokens",
                columns: table => new
                {
                    TkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResettingTokens", x => x.TkId);
                });

            migrationBuilder.CreateTable(
                name: "SearchingFriends",
                columns: table => new
                {
                    SearchingFriendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchingFriends", x => x.SearchingFriendId);
                });

            migrationBuilder.CreateTable(
                name: "SearchingWeapons",
                columns: table => new
                {
                    SearchingWeaponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchingWeaponName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchingWeaponDefense = table.Column<int>(type: "int", nullable: false),
                    SearchingWeaponAttack = table.Column<int>(type: "int", nullable: false),
                    SearchingWeaponAttribute = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchingWeapons", x => x.SearchingWeaponId);
                });
            migrationBuilder.InsertData(
    table: "dbo.CharacterModels",
    columns: new[] { "Id", "CharacterName", "Password", "Gmail", "Class", "Rule", "Token" },
    values: new object[] { 4214124, "Terenas Menathil", "bb0f7e021d52a4e31613d463fc0525d8", "chaunguyengiang2000@gmail.com", "Paladin", "King", "" });
            migrationBuilder.InsertData(
table: "dbo.WeaponModels",
columns: new[] { "WeaponId", "WeaponName", "WeaponAttack", "WeaponDefense", "WeaponAttribute", "WeaponOwner" },
values: new object[] { 4134322, "Stormbreaker - Thunder Hummer", 1500, 1800, "Light", "Terenas Menathil" });
            migrationBuilder.CreateIndex(
                name: "IX_dbo.CharacterModels_CharacterName",
                table: "dbo.CharacterModels",
                column: "CharacterName",
                unique: true);
        }

/// <inheritdoc />
protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddFriendNotis");

            migrationBuilder.DropTable(
                name: "dbo.CharacterModels");

            migrationBuilder.DropTable(
                name: "dbo.WeaponModels");

            migrationBuilder.DropTable(
                name: "FriendLists");

            migrationBuilder.DropTable(
                name: "LoginModels");

            migrationBuilder.DropTable(
                name: "NotificationModels");

            migrationBuilder.DropTable(
                name: "PreviousPasswordModels");

            migrationBuilder.DropTable(
                name: "ResettingPasswordModels");

            migrationBuilder.DropTable(
                name: "ResettingTokens");

            migrationBuilder.DropTable(
                name: "SearchingFriends");

            migrationBuilder.DropTable(
                name: "SearchingWeapons");
        }
    }
}
