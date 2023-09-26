using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPessoal.Model;

public class Postagem : Auditable
{
    [Key] //PrimaryKey (Id)
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //AutoIncrement
    public long Id { get; set; }
    
    [Column(TypeName = "varchar")]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar")]
    [StringLength(1000)]
    public string Texto { get; set; } = string.Empty;
    
    public virtual Tema? Tema { get; set; }
}