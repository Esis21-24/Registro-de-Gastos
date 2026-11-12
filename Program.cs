using System;
using System.Collections.Generic;
using System.Globalization;

class Program
{

    // Categorías: diccionario número -> nombre
    static Dictionary<int, string> categorias = new Dictionary<int, string>();
    static int siguienteCategoria = 1;

    // Gastos: una lista por cada dato (la posición i es el mismo gasto)
    static List<string> gastoDescripciones = new List<string>();
    static List<decimal> gastoMontos = new List<decimal>();
    static List<int> gastoCategorias = new List<int>();
    static List<DateTime> gastoFechas = new List<DateTime>();

    // Ingresos: una lista por cada dato (la posición i es el mismo ingreso)
    static List<string> ingresoDescripciones = new List<string>();
    static List<decimal> ingresoMontos = new List<decimal>();
    static List<DateTime> ingresoFechas = new List<DateTime>();

    // ===================== MAIN =====================
    static void Main()
    {
        CargarCategoriasIniciales();
        bool salir = false;

        while (!salir)
        {
            try
            {
                Console.WriteLine("\n===== REGISTRO DE GASTOS PERSONALES =====");
                Console.WriteLine("1. Gastos");
                Console.WriteLine("2. Ingresos");
                Console.WriteLine("3. Categorías");
                Console.WriteLine("4. Balance");
                Console.WriteLine("0. Salir");

                switch (LeerEntero("Opción: "))
                {
                    case 1: MenuGastos(); break;
                    case 2: MenuIngresos(); break;
                    case 3: MenuCategorias(); break;
                    case 4: MostrarBalance(); break;
                    case 0: salir = Confirmar("¿Seguro que desea salir? (s/n): "); break;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado: " + ex.Message);
                Console.WriteLine("El programa continuará ejecutándose.");
            }
        }

        Console.WriteLine("¡Hasta luego!");
    }

    static void CargarCategoriasIniciales()
    {
        string[] iniciales = { "Comida", "Transporte", "Servicios", "Ocio", "Salud" };

        foreach (string nombre in iniciales)
        {
            categorias.Add(siguienteCategoria, nombre);
            siguienteCategoria++;
        }
    }

    static void MostrarMenuCrud(string titulo)
    {
        Console.WriteLine("\n--- " + titulo + " ---");
        Console.WriteLine("1. Agregar");
        Console.WriteLine("2. Listar");
        Console.WriteLine("3. Buscar");
        Console.WriteLine("4. Modificar");
        Console.WriteLine("5. Eliminar");
        Console.WriteLine("0. Volver");
    }

    static void MenuGastos()
    {
        bool volver = false;

        while (!volver)
        {
            try
            {
                MostrarMenuCrud("GASTOS");

                switch (LeerEntero("Opción: "))
                {
                    case 1: AgregarGasto(); break;
                    case 2: ListarGastos(); break;
                    case 3: BuscarGastos(); break;
                    case 4: ModificarGasto(); break;
                    case 5: EliminarGasto(); break;
                    case 0: volver = true; break;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al trabajar con los gastos: " + ex.Message);
            }
        }
    }

    static void AgregarGasto()
    {
        if (categorias.Count == 0)
        {
            Console.WriteLine("Primero debe crear al menos una categoría.");
            return;
        }

        ListarCategorias();
        int categoria = LeerEntero("Número de la categoría: ");

        if (!categorias.ContainsKey(categoria))
        {
            Console.WriteLine("Esa categoría no existe.");
            return;
        }

        gastoDescripciones.Add(LeerTexto("Descripción: "));
        gastoMontos.Add(LeerMonto("Monto: "));
        gastoCategorias.Add(categoria);
        gastoFechas.Add(LeerFecha("Fecha (dd/mm/aaaa): "));

        Console.WriteLine("Gasto agregado.");
    }

    static void MostrarGasto(int i)
    {
        Console.WriteLine((i + 1) + ". " + gastoFechas[i].ToString("dd/MM/yyyy")
            + " | " + gastoDescripciones[i]
            + " | " + gastoMontos[i].ToString("N2")
            + " | " + NombreCategoria(gastoCategorias[i]));
    }

    static void ListarGastos()
    {
        if (gastoDescripciones.Count == 0)
        {
            Console.WriteLine("No hay gastos registrados.");
            return;
        }

        Console.WriteLine("\n# | Fecha | Descripción | Monto | Categoría");

        for (int i = 0; i < gastoDescripciones.Count; i++)
        {
            MostrarGasto(i);
        }
    }

    static void BuscarGastos()
    {
        Console.WriteLine("Buscar por: 1. Descripción   2. Categoría   3. Rango de monto   4. Fecha");
        int opcion = LeerEntero("Opción: ");
        int encontrados = 0;

        if (opcion == 1)
        {
            string texto = LeerTexto("Texto a buscar: ").ToLower();

            for (int i = 0; i < gastoDescripciones.Count; i++)
            {
                if (gastoDescripciones[i].ToLower().Contains(texto))
                {
                    MostrarGasto(i);
                    encontrados++;
                }
            }
        }
        else if (opcion == 2)
        {
            ListarCategorias();
            int categoria = LeerEntero("Número de la categoría: ");

            if (!categorias.ContainsKey(categoria))
            {
                Console.WriteLine("Esa categoría no existe.");
                return;
            }

            for (int i = 0; i < gastoCategorias.Count; i++)
            {
                if (gastoCategorias[i] == categoria)
                {
                    MostrarGasto(i);
                    encontrados++;
                }
            }
        }
        else if (opcion == 3)
        {
            decimal minimo = LeerMonto("Monto mínimo: ");
            decimal maximo = LeerMonto("Monto máximo: ");

            if (minimo > maximo)
            {
                Console.WriteLine("El monto mínimo no puede ser mayor que el monto máximo.");
                return;
            }

            for (int i = 0; i < gastoMontos.Count; i++)
            {
                if (gastoMontos[i] >= minimo && gastoMontos[i] <= maximo)
                {
                    MostrarGasto(i);
                    encontrados++;
                }
            }
        }
        else if (opcion == 4)
        {
            DateTime fecha = LeerFecha("Fecha (dd/mm/aaaa): ");

            for (int i = 0; i < gastoFechas.Count; i++)
            {
                if (gastoFechas[i].Date == fecha)
                {
                    MostrarGasto(i);
                    encontrados++;
                }
            }
        }
        else
        {
            Console.WriteLine("Opción no válida.");
            return;
        }

        if (encontrados == 0)
        {
            Console.WriteLine("No se encontraron gastos.");
        }
    }

    static void ModificarGasto()
    {
        if (gastoDescripciones.Count == 0)
        {
            Console.WriteLine("No hay gastos para modificar.");
            return;
        }

        ListarGastos();
        int i = LeerEntero("Número del gasto a modificar: ") - 1;

        if (i < 0 || i >= gastoDescripciones.Count)
        {
            Console.WriteLine("Ese gasto no existe.");
            return;
        }

        Console.WriteLine("(Deje vacío y presione Enter para mantener el valor actual)");

        Console.Write("Descripción [" + gastoDescripciones[i] + "]: ");
        string descripcion = LeerLinea().Trim();

        if (descripcion != "")
        {
            gastoDescripciones[i] = descripcion;
        }

        Console.Write("Monto [" + gastoMontos[i].ToString("N2") + "]: ");
        string montoTexto = LeerLinea().Trim();

        if (montoTexto != "")
        {
            decimal nuevoMonto;

            if (TryParseMonto(montoTexto, out nuevoMonto))
            {
                gastoMontos[i] = nuevoMonto;
            }
            else
            {
                Console.WriteLine("Monto inválido, se mantiene el anterior.");
            }
        }

        Console.Write("Número de categoría [" + gastoCategorias[i] + " - "
            + NombreCategoria(gastoCategorias[i]) + "]: ");
        string categoriaTexto = LeerLinea().Trim();

        if (categoriaTexto != "")
        {
            int nuevaCategoria;

            if (int.TryParse(categoriaTexto, out nuevaCategoria)
                && categorias.ContainsKey(nuevaCategoria))
            {
                gastoCategorias[i] = nuevaCategoria;
            }
            else
            {
                Console.WriteLine("Categoría inválida, se mantiene la anterior.");
            }
        }

        Console.Write("Fecha [" + gastoFechas[i].ToString("dd/MM/yyyy") + "]: ");
        string fechaTexto = LeerLinea().Trim();

        if (fechaTexto != "")
        {
            DateTime nuevaFecha;

            if (DateTime.TryParseExact(fechaTexto, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out nuevaFecha))
            {
                gastoFechas[i] = nuevaFecha.Date;
            }
            else
            {
                Console.WriteLine("Fecha inválida, se mantiene la anterior.");
            }
        }

        Console.WriteLine("Gasto modificado.");
    }

    static void EliminarGasto()
    {
        if (gastoDescripciones.Count == 0)
        {
            Console.WriteLine("No hay gastos para eliminar.");
            return;
        }

        ListarGastos();
        int i = LeerEntero("Número del gasto a eliminar: ") - 1;

        if (i < 0 || i >= gastoDescripciones.Count)
        {
            Console.WriteLine("Ese gasto no existe.");
            return;
        }

        if (Confirmar("¿Está seguro de eliminarlo? (s/n): "))
        {
            gastoDescripciones.RemoveAt(i);
            gastoMontos.RemoveAt(i);
            gastoCategorias.RemoveAt(i);
            gastoFechas.RemoveAt(i);
            Console.WriteLine("Gasto eliminado.");
        }
    }

    static void MenuIngresos()
    {
        bool volver = false;

        while (!volver)
        {
            try
            {
                MostrarMenuCrud("INGRESOS");

                switch (LeerEntero("Opción: "))
                {
                    case 1: AgregarIngreso(); break;
                    case 2: ListarIngresos(); break;
                    case 3: BuscarIngresos(); break;
                    case 4: ModificarIngreso(); break;
                    case 5: EliminarIngreso(); break;
                    case 0: volver = true; break;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al trabajar con los ingresos: " + ex.Message);
            }
        }
    }

    static void AgregarIngreso()
    {
        ingresoDescripciones.Add(LeerTexto("Descripción: "));
        ingresoMontos.Add(LeerMonto("Monto: "));
        ingresoFechas.Add(LeerFecha("Fecha (dd/mm/aaaa): "));
        Console.WriteLine("Ingreso agregado.");
    }

    static void MostrarIngreso(int i)
    {
        Console.WriteLine((i + 1) + ". " + ingresoFechas[i].ToString("dd/MM/yyyy")
            + " | " + ingresoDescripciones[i]
            + " | " + ingresoMontos[i].ToString("N2"));
    }

    static void ListarIngresos()
    {
        if (ingresoDescripciones.Count == 0)
        {
            Console.WriteLine("No hay ingresos registrados.");
            return;
        }

        Console.WriteLine("\n# | Fecha | Descripción | Monto");

        for (int i = 0; i < ingresoDescripciones.Count; i++)
        {
            MostrarIngreso(i);
        }
    }

    static void BuscarIngresos()
    {
        Console.WriteLine("Buscar por: 1. Descripción   2. Rango de monto   3. Fecha");
        int opcion = LeerEntero("Opción: ");
        int encontrados = 0;

        if (opcion == 1)
        {
            string texto = LeerTexto("Texto a buscar: ").ToLower();

            for (int i = 0; i < ingresoDescripciones.Count; i++)
            {
                if (ingresoDescripciones[i].ToLower().Contains(texto))
                {
                    MostrarIngreso(i);
                    encontrados++;
                }
            }
        }
        else if (opcion == 2)
        {
            decimal minimo = LeerMonto("Monto mínimo: ");
            decimal maximo = LeerMonto("Monto máximo: ");

            if (minimo > maximo)
            {
                Console.WriteLine("El monto mínimo no puede ser mayor que el monto máximo.");
                return;
            }

            for (int i = 0; i < ingresoMontos.Count; i++)
            {
                if (ingresoMontos[i] >= minimo && ingresoMontos[i] <= maximo)
                {
                    MostrarIngreso(i);
                    encontrados++;
                }
            }
        }
        else if (opcion == 3)
        {
            DateTime fecha = LeerFecha("Fecha (dd/mm/aaaa): ");

            for (int i = 0; i < ingresoFechas.Count; i++)
            {
                if (ingresoFechas[i].Date == fecha)
                {
                    MostrarIngreso(i);
                    encontrados++;
                }
            }
        }
        else
        {
            Console.WriteLine("Opción no válida.");
            return;
        }

        if (encontrados == 0)
        {
            Console.WriteLine("No se encontraron ingresos.");
        }
    }

    static void ModificarIngreso()
    {
        if (ingresoDescripciones.Count == 0)
        {
            Console.WriteLine("No hay ingresos para modificar.");
            return;
        }

        ListarIngresos();
        int i = LeerEntero("Número del ingreso a modificar: ") - 1;

        if (i < 0 || i >= ingresoDescripciones.Count)
        {
            Console.WriteLine("Ese ingreso no existe.");
            return;
        }

        Console.WriteLine("(Deje vacío y presione Enter para mantener el valor actual)");

        Console.Write("Descripción [" + ingresoDescripciones[i] + "]: ");
        string descripcion = LeerLinea().Trim();

        if (descripcion != "")
        {
            ingresoDescripciones[i] = descripcion;
        }

        Console.Write("Monto [" + ingresoMontos[i].ToString("N2") + "]: ");
        string montoTexto = LeerLinea().Trim();

        if (montoTexto != "")
        {
            decimal nuevoMonto;

            if (TryParseMonto(montoTexto, out nuevoMonto))
            {
                ingresoMontos[i] = nuevoMonto;
            }
            else
            {
                Console.WriteLine("Monto inválido, se mantiene el anterior.");
            }
        }

        Console.Write("Fecha [" + ingresoFechas[i].ToString("dd/MM/yyyy") + "]: ");
        string fechaTexto = LeerLinea().Trim();

        if (fechaTexto != "")
        {
            DateTime nuevaFecha;

            if (DateTime.TryParseExact(fechaTexto, "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out nuevaFecha))
            {
                ingresoFechas[i] = nuevaFecha.Date;
            }
            else
            {
                Console.WriteLine("Fecha inválida, se mantiene la anterior.");
            }
        }

        Console.WriteLine("Ingreso modificado.");
    }

    static void EliminarIngreso()
    {
        if (ingresoDescripciones.Count == 0)
        {
            Console.WriteLine("No hay ingresos para eliminar.");
            return;
        }

        ListarIngresos();
        int i = LeerEntero("Número del ingreso a eliminar: ") - 1;

        if (i < 0 || i >= ingresoDescripciones.Count)
        {
            Console.WriteLine("Ese ingreso no existe.");
            return;
        }

        if (Confirmar("¿Está seguro de eliminarlo? (s/n): "))
        {
            ingresoDescripciones.RemoveAt(i);
            ingresoMontos.RemoveAt(i);
            ingresoFechas.RemoveAt(i);
            Console.WriteLine("Ingreso eliminado.");
        }
    }

    static void MenuCategorias()
    {
        bool volver = false;

        while (!volver)
        {
            try
            {
                MostrarMenuCrud("CATEGORÍAS");

                switch (LeerEntero("Opción: "))
                {
                    case 1: AgregarCategoria(); break;
                    case 2: ListarCategorias(); break;
                    case 3: BuscarCategorias(); break;
                    case 4: ModificarCategoria(); break;
                    case 5: EliminarCategoria(); break;
                    case 0: volver = true; break;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al trabajar con las categorías: " + ex.Message);
            }
        }
    }

    static void AgregarCategoria()
    {
        string nombre = LeerTexto("Nombre de la categoría: ");

        foreach (string existente in categorias.Values)
        {
            if (existente.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Ya existe una categoría con ese nombre.");
                return;
            }
        }

        categorias.Add(siguienteCategoria, nombre);
        siguienteCategoria++;
        Console.WriteLine("Categoría agregada.");
    }

    static void ListarCategorias()
    {
        if (categorias.Count == 0)
        {
            Console.WriteLine("No hay categorías registradas.");
            return;
        }

        Console.WriteLine("\nCategorías:");

        foreach (KeyValuePair<int, string> par in categorias)
        {
            Console.WriteLine(par.Key + ". " + par.Value);
        }
    }

    static void BuscarCategorias()
    {
        string texto = LeerTexto("Texto a buscar: ").ToLower();
        int encontradas = 0;

        foreach (KeyValuePair<int, string> par in categorias)
        {
            if (par.Value.ToLower().Contains(texto))
            {
                Console.WriteLine(par.Key + ". " + par.Value);
                encontradas++;
            }
        }

        if (encontradas == 0)
        {
            Console.WriteLine("No se encontraron categorías.");
        }
    }

    static void ModificarCategoria()
    {
        if (categorias.Count == 0)
        {
            Console.WriteLine("No hay categorías para modificar.");
            return;
        }

        ListarCategorias();
        int numero = LeerEntero("Número de la categoría a modificar: ");

        if (!categorias.ContainsKey(numero))
        {
            Console.WriteLine("Esa categoría no existe.");
            return;
        }

        string nuevoNombre = LeerTexto("Nuevo nombre: ");

        foreach (KeyValuePair<int, string> par in categorias)
        {
            if (par.Key != numero
                && par.Value.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Ya existe otra categoría con ese nombre.");
                return;
            }
        }

        categorias[numero] = nuevoNombre;
        Console.WriteLine("Categoría modificada.");
    }

    static void EliminarCategoria()
    {
        if (categorias.Count == 0)
        {
            Console.WriteLine("No hay categorías para eliminar.");
            return;
        }

        ListarCategorias();
        int numero = LeerEntero("Número de la categoría a eliminar: ");

        if (!categorias.ContainsKey(numero))
        {
            Console.WriteLine("Esa categoría no existe.");
            return;
        }

        if (gastoCategorias.Contains(numero))
        {
            Console.WriteLine("No se puede eliminar: hay gastos que usan esta categoría.");
            return;
        }

        if (Confirmar("¿Está seguro de eliminarla? (s/n): "))
        {
            categorias.Remove(numero);
            Console.WriteLine("Categoría eliminada.");
        }
    }

    static string NombreCategoria(int numero)
    {
        if (categorias.ContainsKey(numero))
        {
            return categorias[numero];
        }

        return "(sin categoría)";
    }

    static void MostrarBalance()
    {
        decimal totalIngresos = 0;
        decimal totalGastos = 0;

        foreach (decimal monto in ingresoMontos)
        {
            totalIngresos += monto;
        }

        foreach (decimal monto in gastoMontos)
        {
            totalGastos += monto;
        }

        decimal balance = totalIngresos - totalGastos;

        Console.WriteLine("\n--- BALANCE ---");
        Console.WriteLine("Total ingresos: " + totalIngresos.ToString("N2"));
        Console.WriteLine("Total gastos:   " + totalGastos.ToString("N2"));
        Console.WriteLine("Balance:        " + balance.ToString("N2"));

        if (balance < 0)
        {
            Console.WriteLine("Atención: está gastando más de lo que ingresa.");
        }

        if (gastoMontos.Count > 0)
        {
            Console.WriteLine("\nGastos por categoría:");

            foreach (KeyValuePair<int, string> par in categorias)
            {
                decimal subtotal = 0;

                for (int i = 0; i < gastoMontos.Count; i++)
                {
                    if (gastoCategorias[i] == par.Key)
                    {
                        subtotal += gastoMontos[i];
                    }
                }

                if (subtotal > 0)
                {
                    Console.WriteLine("   " + par.Value + ": " + subtotal.ToString("N2"));
                }
            }
        }
    }

    static string LeerLinea()
    {
        string linea = Console.ReadLine();

        if (linea == null)
        {
            Console.WriteLine("\nEntrada cerrada. Saliendo del programa.");
            Environment.Exit(0);
        }

        return linea;
    }

    static int LeerEntero(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            int valor;

            if (int.TryParse(LeerLinea(), out valor))
            {
                return valor;
            }

            Console.WriteLine("Debe escribir un número entero.");
        }
    }

    static bool TryParseMonto(string texto, out decimal valor)
    {
        texto = texto.Trim().Replace(',', '.');

        return decimal.TryParse(texto, NumberStyles.Number,
            CultureInfo.InvariantCulture, out valor) && valor > 0;
    }

    static decimal LeerMonto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            decimal valor;

            if (TryParseMonto(LeerLinea(), out valor))
            {
                return valor;
            }

            Console.WriteLine("Debe escribir un monto numérico mayor que cero.");
        }
    }

    static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = LeerLinea().Trim();

            if (texto != "")
            {
                return texto;
            }

            Console.WriteLine("Este campo no puede estar vacío.");
        }
    }

    static DateTime LeerFecha(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            DateTime fecha;

            if (DateTime.TryParseExact(LeerLinea().Trim(), "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
            {
                return fecha.Date;
            }

            Console.WriteLine("Fecha inválida. Use el formato dd/mm/aaaa.");
        }
    }

    static bool Confirmar(string mensaje)
    {
        Console.Write(mensaje);
        return LeerLinea().Trim().ToLower() == "s";
    }
}