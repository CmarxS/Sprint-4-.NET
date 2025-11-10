using MottoMap.Models;
using MottoMap.DTOs.Funcionario;
using MottoMap.DTOs.Common;

namespace MottoMap.Mappers
{
    /// <summary>
    /// Mapper para conversões entre FuncionarioEntity e DTOs
    /// </summary>
    public static class FuncionarioMapper
    {
        /// <summary>
        /// Converte CreateFuncionarioDto para FuncionarioEntity
        /// </summary>
        /// <param name="dto">DTO de criação</param>
        /// <returns>Entidade do funcionário</returns>
        public static FuncionarioEntity ToEntity(CreateFuncionarioDto dto)
        {
            return new FuncionarioEntity
            {
                Nome = dto.Nome,
                Email = dto.Email,
                IdFilial = dto.IdFilial,
                Funcao = dto.Funcao
            };
        }

        /// <summary>
        /// Atualiza uma FuncionarioEntity existente com dados do UpdateFuncionarioDto
        /// </summary>
        /// <param name="entity">Entidade existente</param>
        /// <param name="dto">DTO de atualização</param>
        public static void UpdateEntity(FuncionarioEntity entity, UpdateFuncionarioDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Email = dto.Email;
            entity.IdFilial = dto.IdFilial;
            entity.Funcao = dto.Funcao;
        }

        /// <summary>
        /// Converte FuncionarioEntity para FuncionarioResponseDto
        /// </summary>
        /// <param name="entity">Entidade do funcionário</param>
        /// <returns>DTO de resposta</returns>
        public static FuncionarioResponseDto ToResponseDto(FuncionarioEntity entity)
        {
            var dto = new FuncionarioResponseDto
            {
                IdFuncionario = entity.IdFuncionario,
                Nome = entity.Nome,
                Email = entity.Email,
                IdFilial = entity.IdFilial,
                Funcao = entity.Funcao
            };

            // Mapear filial se estiver carregada
            if (entity.Filial != null)
            {
                dto.Filial = new FilialSummaryDto
                {
                    IdFilial = entity.Filial.IdFilial,
                    Nome = entity.Filial.Nome,
                    Cidade = entity.Filial.Cidade,
                    Estado = entity.Filial.Estado
                };
            }

            return dto;
        }

        /// <summary>
        /// Converte lista de FuncionarioEntity para lista de FuncionarioResponseDto
        /// </summary>
        /// <param name="entities">Lista de entidades</param>
        /// <returns>Lista de DTOs de resposta</returns>
        public static List<FuncionarioResponseDto> ToResponseDtoList(IEnumerable<FuncionarioEntity> entities)
        {
            return entities.Select(ToResponseDto).ToList();
        }

        /// <summary>
        /// Converte DataPage de FuncionarioEntity para PagedResponseDto de FuncionarioResponseDto
        /// </summary>
        /// <param name="dataPage">Página de dados das entidades</param>
        /// <returns>Resposta paginada de DTOs</returns>
        public static PagedResponseDto<FuncionarioResponseDto> ToPagedResponseDto(DataPage<FuncionarioEntity> dataPage)
        {
            var dtos = ToResponseDtoList(dataPage.Data);
            
            var pagedResponse = new PagedResponseDto<FuncionarioResponseDto>(
                dtos,
                dataPage.PageNumber,
                dataPage.PageSize,
                dataPage.TotalItems
            );

            // Copiar links HATEOAS se existirem
            foreach (var link in dataPage.Links)
            {
                pagedResponse.Links[link.Key] = link.Value;
            }

            return pagedResponse;
        }

        /// <summary>
        /// Converte FuncionarioEntity para FuncionarioSummaryDto (para uso em outras entidades)
        /// </summary>
        /// <param name="entity">Entidade do funcionário</param>
        /// <returns>DTO resumido</returns>
        public static DTOs.Filial.FuncionarioSummaryDto ToSummaryDto(FuncionarioEntity entity)
        {
            return new DTOs.Filial.FuncionarioSummaryDto
            {
                IdFuncionario = entity.IdFuncionario,
                Nome = entity.Nome,
                Email = entity.Email,
                Funcao = entity.Funcao
            };
        }

        /// <summary>
        /// Adiciona links HATEOAS ao DTO de resposta do funcionário
        /// </summary>
        /// <param name="dto">DTO de resposta</param>
        /// <param name="baseUrl">URL base da API</param>
        public static void AddHateoasLinks(FuncionarioResponseDto dto, string baseUrl)
        {
            dto.Links["self"] = $"{baseUrl}/funcionarios/{dto.IdFuncionario}";
            dto.Links["update"] = $"{baseUrl}/funcionarios/{dto.IdFuncionario}";
            dto.Links["delete"] = $"{baseUrl}/funcionarios/{dto.IdFuncionario}";
            dto.Links["filial"] = $"{baseUrl}/filiais/{dto.IdFilial}";
            dto.Links["all"] = $"{baseUrl}/funcionarios";
        }

        /// <summary>
        /// Adiciona links HATEOAS a uma lista de DTOs
        /// </summary>
        /// <param name="dtos">Lista de DTOs</param>
        /// <param name="baseUrl">URL base da API</param>
        public static void AddHateoasLinks(IEnumerable<FuncionarioResponseDto> dtos, string baseUrl)
        {
            foreach (var dto in dtos)
            {
                AddHateoasLinks(dto, baseUrl);
            }
        }
    }
}