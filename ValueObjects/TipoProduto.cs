using System.ComponentModel;

namespace CursoEFCore.ValueObjects;

public enum TipoProduto
{
    [Description("Mercadoria para Revenda")]
    MercadoriaParaRevenda,

    [Description("Embalagem")]
    Embalagem,

    [Description("Servico")]
    Servico
}