using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MottoMap.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NET_C3_Filial",
                columns: table => new
                {
                    IdFilial = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    Cidade = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    Estado = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    CEP = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_C3_Filial", x => x.IdFilial);
                });

            migrationBuilder.CreateTable(
                name: "NET_C3_Funcionario",
                columns: table => new
                {
                    IdFuncionario = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    IdFilial = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Funcao = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_C3_Funcionario", x => x.IdFuncionario);
                    table.ForeignKey(
                        name: "FK_NET_C3_Funcionario_NET_C3_Filial_IdFilial",
                        column: x => x.IdFilial,
                        principalTable: "NET_C3_Filial",
                        principalColumn: "IdFilial",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NET_C3_Motos",
                columns: table => new
                {
                    IdMoto = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Marca = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Modelo = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Placa = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: false),
                    IdFilial = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Cor = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                    Quilometragem = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_C3_Motos", x => x.IdMoto);
                    table.ForeignKey(
                        name: "FK_NET_C3_Motos_NET_C3_Filial_IdFilial",
                        column: x => x.IdFilial,
                        principalTable: "NET_C3_Filial",
                        principalColumn: "IdFilial",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NET_C3_Funcionario_Email",
                table: "NET_C3_Funcionario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NET_C3_Funcionario_IdFilial",
                table: "NET_C3_Funcionario",
                column: "IdFilial");

            migrationBuilder.CreateIndex(
                name: "IX_NET_C3_Motos_IdFilial",
                table: "NET_C3_Motos",
                column: "IdFilial");

            migrationBuilder.CreateIndex(
                name: "IX_NET_C3_Motos_Placa",
                table: "NET_C3_Motos",
                column: "Placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NET_C3_Funcionario");

            migrationBuilder.DropTable(
                name: "NET_C3_Motos");

            migrationBuilder.DropTable(
                name: "NET_C3_Filial");
        }
    }
}
