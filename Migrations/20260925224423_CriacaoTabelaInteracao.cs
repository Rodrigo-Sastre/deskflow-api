using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoTabelaInteracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interacoes_Chamados_ChamadoId",
                table: "Interacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interacoes",
                table: "Interacoes");

            migrationBuilder.DropColumn(
                name: "Autor",
                table: "Interacoes");

            migrationBuilder.RenameTable(
                name: "Interacoes",
                newName: "Tb_Interacoes");

            migrationBuilder.RenameIndex(
                name: "IX_Interacoes_ChamadoId",
                table: "Tb_Interacoes",
                newName: "IX_Tb_Interacoes_ChamadoId");

            migrationBuilder.AddColumn<string>(
                name: "QuemEscreveu",
                table: "Tb_Interacoes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tb_Interacoes",
                table: "Tb_Interacoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_Interacoes_Chamados_ChamadoId",
                table: "Tb_Interacoes",
                column: "ChamadoId",
                principalTable: "Chamados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Interacoes_Chamados_ChamadoId",
                table: "Tb_Interacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tb_Interacoes",
                table: "Tb_Interacoes");

            migrationBuilder.DropColumn(
                name: "QuemEscreveu",
                table: "Tb_Interacoes");

            migrationBuilder.RenameTable(
                name: "Tb_Interacoes",
                newName: "Interacoes");

            migrationBuilder.RenameIndex(
                name: "IX_Tb_Interacoes_ChamadoId",
                table: "Interacoes",
                newName: "IX_Interacoes_ChamadoId");

            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Interacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interacoes",
                table: "Interacoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interacoes_Chamados_ChamadoId",
                table: "Interacoes",
                column: "ChamadoId",
                principalTable: "Chamados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
