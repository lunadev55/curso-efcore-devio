
// See https://aka.ms/new-console-template for more information
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
// using CursoEFCore.Data;

namespace CursoEFCore;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("App Starts!");

        using var db = new Data.ApplicationContext();
        
        var pendingMigrations = db.Database.GetPendingMigrations().Any();

        if (pendingMigrations)
        {
            // FOLLOW WITH BUSINESS RULE
        }

        InserirDados();
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
