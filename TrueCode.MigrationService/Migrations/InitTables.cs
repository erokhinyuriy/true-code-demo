using FluentMigrator;

namespace TrueCode.MigrationService.Migrations;

[Migration(202609291254)]
public class InitTables : Migration
{
    public override void Up()
    {
        Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");

        Create.Table("currency")
            .WithColumn("id")
                .AsGuid()
                .PrimaryKey()
                .WithDefault(SystemMethods.NewGuid)
            .WithColumn("name")
                .AsString(10)
                .NotNullable()
                .Unique()
            .WithColumn("rate")
                .AsDecimal(18, 4)
                .NotNullable();

        Create.Table("user")
            .WithColumn("id")
                .AsGuid()
                .PrimaryKey()
                .WithDefault(SystemMethods.NewGuid)
            .WithColumn("name")
                .AsString(100)
                .NotNullable()
                .Unique()
            .WithColumn("password")
                .AsString(200)
                .NotNullable();

        Create.Table("user_favorite_currency")
            .WithColumn("user_id")
                .AsGuid()
                .NotNullable()
            .WithColumn("currency_id")
                .AsGuid()
                .NotNullable();

        Create.PrimaryKey("PK_user_favorite_currency")
            .OnTable("user_favorite_currency")
            .Columns("user_id", "currency_id");

        Create.ForeignKey("FK_user_favorite_currency_user")
            .FromTable("user_favorite_currency")
                .ForeignColumn("user_id")
            .ToTable("user")
                .PrimaryColumn("id");

        Create.ForeignKey("FK_user_favorite_currency_currency")
            .FromTable("user_favorite_currency")
                .ForeignColumn("currency_id")
            .ToTable("currency")
                .PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table("user_favorite_currency");
        Delete.Table("user");
        Delete.Table("currency");
    }
}