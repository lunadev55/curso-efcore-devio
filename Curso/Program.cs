
// See https://aka.ms/new-console-template for more information
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using System.Collections.Generic;
// using CursoEFCore.Data;

namespace CursoEFCore;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("App Starts!");

        using var db = new Data.ApplicationContext();
        
        // var pendingMigrations = db.Database.GetPendingMigrations().Any();
        // if (pendingMigrations) { /* FOLLOW WITH BUSINESS RULE */ }

        // InserirDados();
        // InserirDadosEmMassa();
        // InserirListaDeClientes();

        ConsultarClientes();
    }

    private static void ConsultarClientes()
    {
        using var db = new Data.ApplicationContext();

        // THE BELOW APPROACHES ALL WORK FOR RETRIEVING ENTITIES FROM THE DB

        // var clientes = db.Clientes.Select(p => p).ToList();
        // var clientes = db.Clientes.ToList();
        // var clientes = (from c in db.Clientes select c).ToList();

        // var clientesPorSintaxe = (from c in db.Clientes where c.Id > 0 select c).ToList();

        // var clientesPorMetodo = db.Clientes.Where(c => c.Id > 0).ToList();

        var clientesPorMetodoESemRastreamento = db.Clientes
            .AsNoTracking() // DISABLES DEFAULT ENTITY TRACKING
            .Where(c => c.Id > 0)
            .OrderBy(c => c.Id)
            .Take(10)
            .ToList();

        foreach (var cliente in clientesPorMetodoESemRastreamento)
        {
            Console.WriteLine($"Consultando Cliente: {cliente.Id}");
            // db.Clientes.Find(cliente.Id);
            
            db.Clientes.FirstOrDefault(c => c.Id == cliente.Id);
        }
    }

    // TODO - CREATE METHOD TO INSERT LIST OF 50 RANDOM CLIENTES IN THE DATABASE
    private static void InserirListaDeClientes()
    {
        var clientes = new List<Cliente>();

        for (int indice = 10; indice <= 60; indice++)
        {
            var cliente = new Cliente
            {
                Nome = "Cliente " + indice,
                Telefone = "819998877" + indice,
                CEP = "509203" + indice,
                Estado = "PE",
                Cidade = "Recife " + indice
            };

            clientes.Add(cliente);
        }

        using var db = new Data.ApplicationContext();

        db.Clientes.AddRange(clientes);
        var result = db.SaveChanges();

        Console.WriteLine($"{result} entities inserted in the database!");
    }

    // METHOD TO INSERT MULTIPLE DATA AT ONCE
    // THIS EXAMPLE INSERTS 2 ENTITIES (PRODUTO AND PEDIDO) IN THE SAME OPERATIONS - db.AddRange();
    private static void InserirDadosEmMassa()
    {
        var produto = new Produto
        {
            CodigoBarras = "11111111111111",
            Descricao = "Descricao de produto pra teste de insercao em massa",
            Valor = 123.56M,
            TipoProduto = TipoProduto.Embalagem,
            Ativo = true
        };

        var cliente = new Cliente
        {
            Nome = "Andre Luna Marinho",
            Telefone = "81991581301",
            CEP = "50920331",
            Estado = "PE",
            Cidade = "Recife"
        };

        using var db = new Data.ApplicationContext(); 
        db.AddRange(produto, cliente);
        var registros = db.SaveChanges();

        if (registros > 0)
            Console.WriteLine($"{registros} rows inserted in the database!");
    }

    private static void InserirDados()
    {
        var produto = new Produto
        {
            CodigoBarras = "01234567891011",
            Descricao = "Seda Papelito - King Size",
            Valor = 12.23M,
            TipoProduto = TipoProduto.MercadoriaParaRevenda,
            Ativo = true
        };

        using var db = new Data.ApplicationContext();

        // FIRST WAY I WROTE        
        db.Produtos.Add(produto);

        // SECOND APPROACH
        db.Set<Produto>().Add(produto);

        // THIRD APPROACH - TRACKING THE ENTITY STATE
        db.Entry(produto).State = EntityState.Added;

        // NEEDS TO KNOW THE ENTITY UNDER THE HOOD
        db.Add(produto);

        var saveChanges = db.SaveChanges();

        if (saveChanges > 0)
        {
            Console.WriteLine("Entity Saved Succesfully!");
        }
    }

    // SET THE METHOD NAME AS MAIN2 FOR IT NOT TO BE CALLED AT APPLICATION START
    static void Main2(string[] args)
    {
        // USUALLY MIGRATIONS ARE NOT APPLIED THIS WAY
        // NOT RECOMMENDED IN PROD ENV

        // THIS WAY MIGRATION WILL BE EXECUTEDF AT THE APP START 
        // DB1 IS DISPOSED AT THE END OF MAIN
        using var db1 = new Data.ApplicationContext();
        db1.Database.Migrate();

        // THIS WAY THE MIGRATIONS WILL BE EXECUTED AT THE APP START, AS WELL
        // DB2 IS DISPOSED AT THE END OF THE BLOCK
        using (var db2 = new Data.ApplicationContext())
        {
            db2.Database.Migrate();
        }
    }
}
