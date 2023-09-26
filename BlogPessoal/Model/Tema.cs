using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPessoal.Model;

public class Tema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    [Column(TypeName = "varchar")]
    [StringLength(255)]
    public string Descricao { get; set; } = string.Empty;
}