using System.ComponentModel;

namespace CursoEFCore.ValueObjects;

public enum TipoFrete
{
    [Description("Remetente Paga")]
    CIF, 

    [Description("Destinatario Paga")]
    FOB,

    [Description("Sem Frete")]
    SemFrete
}