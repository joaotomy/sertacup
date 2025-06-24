using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SertaCup_site.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    equipa = table.Column<int>(type: "int", nullable: false),
                    grupo = table.Column<int>(type: "int", nullable: false),
                    pontos = table.Column<int>(type: "int", nullable: true),
                    jogos_feitos = table.Column<int>(type: "int", nullable: true),
                    vitorias = table.Column<int>(type: "int", nullable: true),
                    derrotas = table.Column<int>(type: "int", nullable: true),
                    empate = table.Column<int>(type: "int", nullable: true),
                    golos_marcados = table.Column<int>(type: "int", nullable: true),
                    golos_sofridos = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classificacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "equipas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Completo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sigla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cor_Principal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cor_Secundaria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Simbolo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "grupos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grupo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    equipa1 = table.Column<byte>(type: "tinyint", nullable: false),
                    equipa2 = table.Column<byte>(type: "tinyint", nullable: false),
                    equipa3 = table.Column<byte>(type: "tinyint", nullable: false),
                    equipa4 = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "jogadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equipa_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "jogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Chave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    hora_prevista = table.Column<DateTime>(type: "datetime2", nullable: false),
                    equipa1 = table.Column<int>(type: "int", nullable: false),
                    equipa2 = table.Column<int>(type: "int", nullable: false),
                    golos_equipa1 = table.Column<byte>(type: "tinyint", nullable: false),
                    golos_equipa2 = table.Column<byte>(type: "tinyint", nullable: false),
                    hora_inicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    hora_inicio_2parte = table.Column<DateTime>(type: "datetime2", nullable: true),
                    primeira_parte_terminada = table.Column<bool>(type: "bit", nullable: false),
                    começado = table.Column<bool>(type: "bit", nullable: false),
                    terminado = table.Column<bool>(type: "bit", nullable: false),
                    tempo_pausado_1 = table.Column<int>(type: "int", nullable: false),
                    tempo_pausado_2 = table.Column<int>(type: "int", nullable: false),
                    interrompido = table.Column<bool>(type: "bit", nullable: false),
                    campo = table.Column<int>(type: "int", nullable: false),
                    grupo = table.Column<int>(type: "int", nullable: true),
                    situacao_precaria = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jogos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "marcadores_jogo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    jogo_id = table.Column<int>(type: "int", nullable: false),
                    marcador = table.Column<int>(type: "int", nullable: false),
                    equipa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marcadores_jogo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classificacoes");

            migrationBuilder.DropTable(
                name: "equipas");

            migrationBuilder.DropTable(
                name: "grupos");

            migrationBuilder.DropTable(
                name: "jogadores");

            migrationBuilder.DropTable(
                name: "jogos");

            migrationBuilder.DropTable(
                name: "marcadores_jogo");
        }
    }
}
