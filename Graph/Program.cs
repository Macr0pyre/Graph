using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph gr = new Graph(false, false);
            Graph new_gr;

            bool flag = true;
            while (flag)
            {
                Console.WriteLine();
                Console.WriteLine("1. Создать пустой граф");
                Console.WriteLine("2. Создать граф с данными из файла");
                Console.WriteLine("3. Добавить вершину");
                Console.WriteLine("4. Добавить ребро (дугу)");
                Console.WriteLine("5. Удалить вершину");
                Console.WriteLine("6. Удалить ребро (дугу)");
                Console.WriteLine("7. Просмотреть список смежности графа");
                Console.WriteLine("8. Вывести граф в файл");
                Console.WriteLine("9. Вывести все вершины, смежные с данной");
                Console.WriteLine("10. Вершина, в которую есть дуга как из вершины u, так и из вершины v");
                Console.WriteLine("11. Провести топологическую сортировку");
                Console.WriteLine("12. Найти центр графа");
                Console.WriteLine("13. Найти в графе каркас минимального веса");
                Console.WriteLine("14. Определить, существует ли путь длиной не более L между двумя заданными вершинами графа.");
                Console.WriteLine("15. Определить множество вершин орграфа, расстояние от которых до заданной вершины не более N.");
                Console.WriteLine("16. Найти все такие пары вершин, что между ними существует путь сколько угодно малой длины.");
                Console.WriteLine("17. Найти максимальный поток в графе.");
                Console.WriteLine("18. Выход");
                Console.WriteLine();
                Console.Write("Введите номер действия: ");
                int i = int.Parse(Console.ReadLine());
                switch (i)
                {
                    case 1:
                        Console.WriteLine("Какой граф вы хотите создать?");
                        Console.WriteLine("1. Ориентированный взвешенный");
                        Console.WriteLine("2. Ориентированный невзвешенный");
                        Console.WriteLine("3. Неориентированный взвешенный");
                        Console.WriteLine("4. Неориентированный невзвешенный");
                        int j = int.Parse(Console.ReadLine());
                        if (j == 1)
                        {
                            gr = new Graph(true, true);
                            Console.WriteLine("Создан ориентированный взвешенный граф");
                        }
                        else if (j == 2)
                        {
                            gr = new Graph(true, false);
                            Console.WriteLine("Создан ориентированный невзвешенный граф");
                        }
                        else if (j == 3)
                        {
                            gr = new Graph(false, true);
                            Console.WriteLine("Создан неориентированный взвешенный граф");
                        }
                        else if (j == 4)
                        {
                            gr = new Graph(false, false);
                            Console.WriteLine("Создан неориентированный невзвешенный граф");
                        }
                        break;
                    case 2:
                        Console.WriteLine("Введите название файла");
                        string file_name = Console.ReadLine();
                        if (File.Exists(file_name + ".txt"))
                        {
                            gr = new Graph(file_name + ".txt");
                            if (gr.Get_oriented && gr.Get_weighted)
                                Console.WriteLine("Вы создали ориентированный взвешенный граф");
                            else if (gr.Get_oriented && !gr.Get_weighted)
                                Console.WriteLine("Вы создали ориентированный невзвешенный граф");
                            else if (!gr.Get_oriented && gr.Get_weighted)
                                Console.WriteLine("Вы создали неориентированный взвешенный граф");
                            else if (!gr.Get_oriented && !gr.Get_weighted)
                                Console.WriteLine("Вы создали неориентированный невзвешенный граф");
                        }
                        else
                            Console.WriteLine("Файла с таким названием не существует");
                        break;
                    case 3:
                        Console.WriteLine("Введите название добавляемой вершины:");
                        gr.AddVertex(Console.ReadLine());
                        break;
                    case 4:
                        Console.WriteLine("Введите название начальной вершины:");
                        string ver1 = Console.ReadLine();
                        Console.WriteLine("Введите название конечной вершины:");
                        string ver2 = Console.ReadLine();
                        if (gr.Get_weighted)
                        {
                            Console.WriteLine("Введите вес ребра (дуги):");
                            gr.AddEdge(ver1, ver2, Console.ReadLine());
                        }
                        else
                            gr.AddEdge(ver1, ver2);
                        break;
                    case 5:
                        Console.WriteLine("Введите название удаляемой вершины:");
                        gr.RemoveVertex(Console.ReadLine());
                        break;
                    case 6:
                        Console.WriteLine("Введите название начальной вершины:");
                        string ver11 = Console.ReadLine();
                        Console.WriteLine("Введите название конечной вершины:");
                        string ver22 = Console.ReadLine();
                        gr.RemoveEdge(ver11, ver22);
                        break;
                    case 7:
                        Console.WriteLine("Список смежности графа:");
                        gr.ConsoleOut();
                        Console.WriteLine();
                        break;
                    case 8:
                        Console.WriteLine("Введите название файла для вывода графа:");
                        gr.SaveInFile(Console.ReadLine() + ".txt");
                        break;
                    case 9:
                        Console.WriteLine("Введите название вершины:");
                        gr.OutAdjacent(Console.ReadLine());
                        break;
                    case 10:
                        Console.Write("Введите название вершины u: ");
                        string u = Console.ReadLine();
                        Console.Write("Введите название вершины v: ");
                        string v = Console.ReadLine();
                        gr.ex_19(u, v);
                        break;
                    case 12:
                        List<string> list = gr.Centre();
                        Console.WriteLine("Вершины центра графа:");
                        foreach (var l in list)
                            Console.Write("{0} ", l);
                        Console.WriteLine();
                        break;
                    case 11:
                        gr.TopologicalSort();
                        break;
                    case 13:
                        new_gr = new Graph(false, true);
                        gr.Kruskal(new_gr);
                        Console.WriteLine("Каркас минимального веса графа:");
                        new_gr.ConsoleOut();
                        break;
                    case 14:
                        Console.Write("Введите название первой вершины: ");
                        string v1 = Console.ReadLine();
                        Console.Write("Введите название второй вершины: ");
                        string v2 = Console.ReadLine();
                        Console.Write("Введите длину L: ");
                        int L = int.Parse(Console.ReadLine());
                        gr.Dijkstra(v1, v2, L);
                        break;
                    case 15:
                        Console.Write("Введите название вершины: ");
                        string v0 = Console.ReadLine();
                        Console.Write("Введите расстояние N: ");
                        int N = int.Parse(Console.ReadLine());
                        gr.Ford_Bellman(v0, N);
                        break;
                    case 16:
                        gr.Floyd();
                        break;
                    case 17:
                        Console.Write("Максимальный поток в графе: ");
                        Console.Write(gr.Max_flow());
                        Console.WriteLine();
                        break;
                    case 18:
                        Console.WriteLine("Выход");
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("Ошибка, введите корректный номер!");
                        break;
                }
            }
        }
    }
}
