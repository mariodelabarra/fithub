using FitHub.Platform.Workout.Domain;
using FluentMigrator;
using System.Text.Json;

namespace FitHub.Platform.Workout.Repository.Migrations
{
    [Migration(1)]
    public class CreateExerciseTable : Migration
    {
        public override void Up()
        {
            Create.Table("Exercises")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CreatedOn").AsDateTime()
                .WithColumn("ModifiedOn").AsDateTime().Nullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString()
                .WithColumn("Type").AsInt32().Nullable()
                .WithColumn("DifficultyLevel").AsInt32().Nullable()
                .WithColumn("Instructions").AsString().Nullable()
                .WithColumn("Categories").AsString().Nullable()
                .WithColumn("MuscleGroups").AsCustom("JSON").Nullable();

            Insert.IntoTable("Exercises").Row(new
            {
                Id = 123,
                Name = "Push Ups",
                Description = "Test description",
                CreatedOn = DateTime.UtcNow,
                Type = 1,
                DifficultyLevel = 1,
                MuscleGroups = JsonSerializer.Serialize(MuscleGroup.Core),
                Instructions = string.Empty,
                Categories = string.Empty,
            });
        }

        public override void Down()
        {
            Delete.Table("Exercises");
        }
    }
}
