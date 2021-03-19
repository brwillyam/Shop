using System.ComponentModel.DataAnnotations;
using Shop.Modelos;

namespace Shop.Modelos
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MaxLength(60, ErrorMessage = "O titulo deve conter entre 3 e 60 caracteres.")]
        [MinLength(3, ErrorMessage = "O titulo deve conter entre 3 e 60 caracteres.")]
        public string Titulo { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter no maximo 1024 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior qeu zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria invalida")]
        public int CategoriasId { get; set; }

        public Categorias Categorias { get; set; }
        
    }
}