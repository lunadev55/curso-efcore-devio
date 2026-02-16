
// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("App Starts!");
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
