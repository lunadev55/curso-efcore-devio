using System.ComponentModel;

namespace CursoEFCore.ValueObjects;

public enum StatusPedido
{
    [Description("Pedido em Analise")]
    Analise,

    [Description("Pedido Finalizado")]
    Finalizado,

    [Description("Pedido Entregue ao Destinatario")]
    Entregue
}