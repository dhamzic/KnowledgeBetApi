using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowledgeBet.Infrastructure.Migrations
{
    public partial class stats2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
              @"CREATE FUNCTION[dbo].[LastUserWin](@userId int)
                RETURNS char(30) AS
                BEGIN
                  DECLARE @ret char(30)
                  SELECT TOP 1 @ret = Id
                  FROM Games
                  WHERE Games.Id IN(SELECT GameId
                                     FROM GamesByUser
                                    WHERE UserId = @userId)
                  ORDER BY Date
                  RETURN @ret
                END");

            migrationBuilder.Sql(
            @"CREATE VIEW dbo.SamuraiBattleStats
              AS
              SELECT dbo.Samurais.Name,
              COUNT(dbo.BattleSamurai.BattleId) AS NumberOfBattles,
                      dbo.EarliestBattleFoughtBySamurai(MIN(dbo.Samurais.Id)) 
		  	     AS EarliestBattle
              FROM dbo.BattleSamurai INNER JOIN
                   dbo.Samurais ON dbo.BattleSamurai.SamuraiId = dbo.Samurais.Id
              GROUP BY dbo.Samurais.Name, dbo.BattleSamurai.SamuraiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
