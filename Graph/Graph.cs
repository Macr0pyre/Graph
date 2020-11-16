using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Graph
    {
        Dictionary<string, Dictionary<string, string>> vertices = new Dictionary<string, Dictionary<string, string>>();
        bool oriented = false;
        bool weighted = false;
        public bool Get_oriented
        {
            get
            {
                return oriented;
            }
        }
        public bool Get_weighted
        {
            get
            {
                return weighted;
            }
        }
        Dictionary<string, Dictionary<string, string>> Copy(Dictionary<string, Dictionary<string, string>> ver)
        {
            Dictionary<string, Dictionary<string, string>> copy_graph = new Dictionary<string, Dictionary<string, string>>();
            foreach (string elem in ver.Keys)
            {
                copy_graph.Add(elem, new Dictionary<string, string>(ver[elem]));
            }
            return copy_graph;
        }
        public Graph()
        {
        }
        public Graph(bool oriented, bool weighted)
        {
            this.oriented = oriented;
            this.weighted = weighted;
        }
        public Graph(Graph old_gr)
        {
            vertices = Copy(old_gr.vertices);
            oriented = old_gr.oriented;
            weighted = old_gr.weighted;
        }
        public Graph(string file_name)
        {
            using (StreamReader input = new StreamReader(file_name, Encoding.GetEncoding(1251)))
            {
                string fline = input.ReadLine();
                string[] lines = input.ReadToEnd().Split('\n').Select(p => p.Trim()).ToArray();
                if (fline[0] == '1')
                    oriented = true;
                if (fline[2] == '1')
                    weighted = true;
                foreach (string elem in lines)
                {
                    string[] s1 = elem.Split(':');
                    string[] s2 = s1[1].Split(';');
                    Dictionary<string, string> edges = new Dictionary<string, string>();
                    if (s2[0] != "")
                    {
                        if (weighted)
                        {
                            foreach (string el in s2)
                            {
                                string[] s3 = el.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                                edges.Add(s3[0], s3[1]);
                            }
                        }
                        else
                        {
                            edges = s2.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToDictionary(x => x, y => "");
                        }
                    }
                    vertices.Add(s1[0], edges);
                }
            }
        }
        public void SaveInFile(string out_name)
        {
            using (StreamWriter output = new StreamWriter(out_name, false))
            {
                if (oriented)
                    output.Write("1 ");
                else
                    output.Write("0 ");
                if (weighted)
                    output.WriteLine("1");
                else
                    output.WriteLine("0");
                foreach (string elem in vertices.Keys)
                {
                    output.Write("{0}:", elem);
                    if (vertices[elem].Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in vertices[elem])
                        {
                            if (weighted)
                            {
                                output.Write(" {0}, {1}", pair.Key, pair.Value);
                            }
                            else
                            {
                                output.Write(" {0}", pair.Key);
                            }
                            if (vertices[elem].Keys.Last() != pair.Key)
                                output.Write(";");
                        }
                    }
                    if (vertices.Keys.Last() != elem)
                        output.WriteLine();
                }
            }
        }
        public void AddVertex(string vertex_name)
        {
            try
            {
                vertices.Add(vertex_name, new Dictionary<string, string>());
            }
            catch
            {
                Console.WriteLine("Вершина с таким именем уже есть");
            }
        }
        public void AddVertexWithoutConsole(string vertex_name)
        {
            try
            {
                vertices.Add(vertex_name, new Dictionary<string, string>());
            }
            catch
            {
            }
        }
        public void RemoveVertex(string vertex_name)
        {
            if (vertices.ContainsKey(vertex_name))
            {
                vertices.Remove(vertex_name);
                List<string> l1 = vertices.Keys.ToList();
                foreach (string elem in l1)
                {
                    if (vertices[elem].Keys.Contains(vertex_name))
                    {
                        vertices[elem].Remove(vertex_name);
                    }
                }
            }
            else
                Console.WriteLine("Такой вершины нет в графе");
        }
        public void AddEdge(string ver1, string ver2, string weight = "")
        {
            if (vertices.ContainsKey(ver1) && vertices.ContainsKey(ver2))
            {
                if (oriented)
                {
                    if (!vertices[ver1].Keys.Contains(ver2))
                    {
                        vertices[ver1].Add(ver2, weight);
                    }
                    else
                        Console.WriteLine("Уже существует дуга из {0} в {1}", ver1, ver2);
                }
                else
                {
                    if (!vertices[ver1].Keys.Contains(ver2))
                    {
                        vertices[ver1].Add(ver2, weight);
                    }
                    else
                        Console.WriteLine("Вершина {0} уже соединена с {1}", ver2, ver1);
                    if (!vertices[ver2].Keys.Contains(ver1))
                    {
                        vertices[ver2].Add(ver1, weight);
                    }
                    //else
                    //    Console.WriteLine("Вершина {0} уже соединена с {1}", ver1, ver2);
                }
            }
            else
                Console.WriteLine("Вершин(ы) нет в графе");
        }
        public void AddEdgeWithoutConsole(string ver1, string ver2, string weight = "")
        {
            if (vertices.ContainsKey(ver1) && vertices.ContainsKey(ver2))
            {
                if (oriented)
                {
                    if (!vertices[ver1].Keys.Contains(ver2))
                    {
                        vertices[ver1].Add(ver2, weight);
                    }
                }
                else
                {
                    if (!vertices[ver1].Keys.Contains(ver2))
                    {
                        vertices[ver1].Add(ver2, weight);
                    }
                    if (!vertices[ver2].Keys.Contains(ver1))
                    {
                        vertices[ver2].Add(ver1, weight);
                    }
                }
            }
        }
        public void RemoveEdge(string ver1, string ver2)
        {
            if (vertices.ContainsKey(ver1) && vertices.ContainsKey(ver2))
            {
                if (oriented)
                {
                    if (vertices[ver1].Keys.Contains(ver2))
                    {
                        vertices[ver1].Remove(ver2);
                    }
                    else
                        Console.WriteLine("Нет дуги из {0} в {1}", ver1, ver2);
                }
                else
                {
                    if (vertices[ver1].Keys.Contains(ver2))
                    {
                        vertices[ver1].Remove(ver2);
                    }
                    else
                        Console.WriteLine("Нет ребра между {0} и {1}", ver1, ver2);
                    if (vertices[ver2].Keys.Contains(ver1))
                    {
                        vertices[ver2].Remove(ver1);
                    }
                    //else
                    //    Console.WriteLine("Нет ребра между {0} и {1}", ver2, ver1);
                }
            }
            else
                Console.WriteLine("Вершин(ы) нет в графе");
        }
        public void ConsoleOut()
        {
            foreach (string elem in vertices.Keys)
            {
                Console.Write("{0}:", elem);
                if (vertices[elem].Count != 0)
                {
                    foreach (KeyValuePair<string, string> pair in vertices[elem])
                    {
                        if (weighted)
                        {
                            Console.Write(" {0}, {1}", pair.Key, pair.Value);
                        }
                        else
                        {
                            Console.Write(" {0}", pair.Key);
                        }
                        if (vertices[elem].Keys.Last() != pair.Key)
                            Console.Write(";");
                    }
                }
                //if (vertices.Keys.Last() != elem)
                Console.WriteLine();
            }
            if (vertices.Count == 0)
                Console.WriteLine("Граф пуст");
        }
        public void OutAdjacent(string vertex_name) //вывод смежных вершин
        {
            if (vertices.Keys.Contains(vertex_name))
            {
                if (vertices[vertex_name].Count != 0)
                {
                    Console.Write("Вершины, смежные с {0}: ", vertex_name);
                    foreach (KeyValuePair<string, string> ver in vertices[vertex_name])
                    {
                        Console.Write("{0} ", ver.Key);
                    }
                }
                else
                    Console.Write("Нет смежных вершин.");
            }
            else
                Console.Write("Такой вершины нет в графе");
            Console.WriteLine();
        }
        public void ex_19(string u, string v)
        {
            if (vertices.ContainsKey(u) && vertices.ContainsKey(v))
            {
                bool fl = true;
                foreach (string ver in vertices[u].Keys)
                    foreach (string ver2 in vertices[v].Keys)
                    {
                        if (ver == ver2)
                        {
                            Console.Write("Вершина, смежная с {0} и {1}: {2}", u, v, ver);
                            fl = false;
                        }
                    }
                if (fl)
                    Console.Write("Вершины, удовлетворяющей условию, нет!");
            }
            else
                Console.Write("Неправильный ввод вершин(ы).");
            Console.WriteLine();
        }
        public Graph GraphUnion(Graph g2)
        {
            Graph out_gr = new Graph(this);
            if (this.weighted != g2.weighted)
            {
                Console.WriteLine("Взвешенность обоих графов должна быть одинаковой!");
                return new Graph();
            }
            else
            {

                if (this.oriented || g2.oriented)
                    out_gr.oriented = true;
                foreach (string ver in g2.vertices.Keys)
                {
                    out_gr.AddVertexWithoutConsole(ver);
                }
                foreach (var key in g2.vertices)
                {
                    foreach (var val in key.Value)
                    {
                        out_gr.AddEdgeWithoutConsole(key.Key, val.Key, val.Value);
                    }
                }
                return out_gr;
            }
        }
        bool CyclicVertex(string v, ref Dictionary<string, int> color)
        { //проверка вершины графа на цикличность
            color[v] = 1;
            foreach (string val in vertices[v].Keys)
            {
                if (color[val] == 0)
                { //если в вершину не входили ни разу
                    if (CyclicVertex(val, ref color))
                        return true;
                }
                else if (color[val] == 1)
                { //если в указанную вершину ранее входили, то значит, что найден цикл
                    return true;
                }
            }
            color[v] = 2; //указываем, что в вершину больше ни разу входить не будем
            return false;
        }
        bool Cyclic()
        {
            Dictionary<string, int> color = new Dictionary<string, int>();
            foreach (var ver in vertices)
            {
                color.Add(ver.Key, 0);
            }
            foreach (var ver in vertices.Keys)
            {
                if (CyclicVertex(ver, ref color))
                {
                    return true;
                }
            }
            return false;
        }
        void DFS(string v, ref Dictionary<string, bool> used, ref List<string> answer)
        {
            used[v] = true;
            foreach (string val in vertices[v].Keys)
            {
                if (!used[val])
                    DFS(val, ref used, ref answer);
            }
            answer.Add(v);
        }
        void BFS(out Dictionary<string, int> dist, string ver)
        {
            dist = new Dictionary<string, int>();
            Dictionary<string, bool> used = new Dictionary<string, bool>();
            foreach (var vert in vertices)
            {
                used.Add(vert.Key, false);
                dist.Add(vert.Key, 0);
            }
            used[ver] = true;
            Queue<string> q = new Queue<string>();
            q.Enqueue(ver);
            while (q.Count != 0)
            {
                string u = q.Dequeue();
                foreach (var in_ver in vertices[u])
                {
                    if (!used[in_ver.Key])
                    {
                        used[in_ver.Key] = true;
                        dist[in_ver.Key] = dist[u] + 1;
                        q.Enqueue(in_ver.Key);
                    }
                }
            }
        }
        public void TopologicalSort()
        {
            if (!this.Cyclic())
            {
                Dictionary<string, bool> used = new Dictionary<string, bool>();
                foreach (var ver in vertices)
                {
                    used.Add(ver.Key, false);
                }
                List<string> out_ = new List<string>();
                foreach (var ver in vertices)
                    if (!used[ver.Key])
                        DFS(ver.Key, ref used, ref out_);
                out_.Reverse();
                int cnt = 0;
                Dictionary<string, Dictionary<string, string>> copy = Copy(this.vertices);
                Dictionary<string, string> sootv = new Dictionary<string, string>();
                List<string> l1 = vertices.Keys.ToList();
                foreach (string elem in l1)
                {
                    sootv.Add(out_[cnt], elem);
                    cnt++;
                }
                cnt = 0;
                foreach (string elem in l1)
                {
                    vertices[elem] = copy[out_[cnt]];
                    cnt++;
                    List<KeyValuePair<string, string>> l2 = vertices[elem].ToList();
                    foreach (var val in l2)
                    {
                        string weight = val.Value;
                        vertices[elem].Remove(val.Key);
                        vertices[elem].Add(sootv[val.Key], weight);
                    }
                }
                Console.WriteLine("Сортировка проведена!");
            }
            else
                Console.WriteLine("Граф циклический");
        }
        Dictionary<string, int> Eccentricities()
        {
            Dictionary<string, int> dist;
            List<Dictionary<string, int>> l_dis = new List<Dictionary<string, int>>();
            Dictionary<string, int> eccent = new Dictionary<string, int>();
            foreach (var vert in vertices)
            {
                eccent.Add(vert.Key, 0);
            }
            foreach (var vert in vertices)
            {
                BFS(out dist, vert.Key);
                l_dis.Add(dist);
            }
            foreach (var item in l_dis)
            {
                foreach (var vert in vertices)
                {
                    eccent[vert.Key] = Math.Max(eccent[vert.Key], item[vert.Key]);
                }
            }
            return eccent;
        }
        int Radius()
        {
            Dictionary<string, int> eccents = Eccentricities();
            List<string> keys = vertices.Keys.ToList();
            int rad = eccents[keys[0]];
            for (int i = 1; i < keys.Count; i++)
            {
                rad = Math.Min(rad, eccents[keys[i]]);
            }
            return rad;
        }
        public List<string> Centre()
        {
            Dictionary<string, int> eccents = Eccentricities();
            int rad = Radius();
            List<string> centre = new List<string>();
            foreach (var ver in vertices)
            {
                if (eccents[ver.Key] == rad)
                    centre.Add(ver.Key);
            }
            return centre;
        }
        public void Kruskal(Graph MST)
        {
            if (!this.oriented && this.weighted)
            {
                foreach (string vert in vertices.Keys)
                {
                    MST.AddVertex(vert);
                }

                Dictionary<string, int> edges = new Dictionary<string, int>();
                foreach (string v1 in vertices.Keys)
                {
                    if (vertices[v1].Count != 0)
                    {
                        foreach (var v2 in vertices[v1])
                        {
                            if (!edges.ContainsKey(v1 + "-" + v2.Key))
                                edges.Add(v1 + "-" + v2.Key, int.Parse(v2.Value));
                        }
                    }
                }

                edges = edges.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                Dictionary<string, int> trees = new Dictionary<string, int>();
                int i = 1;
                foreach (string elem in vertices.Keys)
                {
                    trees.Add(elem, i);
                    i++;
                }
                foreach (string edge in edges.Keys)
                {
                    string a = edge.Substring(0, edge.IndexOf("-"));
                    string b = edge.Substring(edge.IndexOf("-") + 1, edge.Length - edge.IndexOf("-") - 1);
                    int w = edges[edge];
                    if (trees[a] != trees[b])
                    {
                        MST.AddEdge(a, b, w.ToString());
                        int old_id = trees[b];
                        int new_id = trees[a];
                        foreach (string cur_id in vertices.Keys)
                        {
                            if (trees[cur_id] == old_id)
                                trees[cur_id] = new_id;
                        }
                        i = new_id;
                    }
                }
                foreach(int elem in trees.Values)
                {
                    if (elem != i)
                    {
                        Console.WriteLine("Граф является лесом (несвязный).");
                        break;
                    }
                }
            }
            else
                Console.WriteLine("Граф должен быть взевешенным и неориентированным!");
        }
        public void Dijkstra(string v1, string v2, int L)
        {
            if (!vertices.ContainsKey(v1) || !vertices.ContainsKey(v2))
                Console.WriteLine("В графе нет такой(их) вершин(ы)!");
            else
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                foreach (string elem in vertices.Keys)
                    d.Add(elem, int.MaxValue);

                Dictionary<string, bool> used = new Dictionary<string, bool>();
                foreach (string elem in vertices.Keys)
                    used.Add(elem, false);

                string v;
                d[v1] = 0;

                for (int i = 0; i < vertices.Count; i++)
                {
                    if (!used[v2])
                    {
                        v = "";
                        foreach (string j in vertices.Keys)
                        {
                            if (!used[j] && (v == "" || d[j] < d[v]))
                            {
                                v = j;
                            }
                        }
                        if (d[v] == int.MaxValue) break;
                        used[v] = true;

                        Dictionary<string, string> s = vertices[v];

                        foreach (string k in s.Keys)
                        {
                            if (k != v1)
                            {
                                int w = int.Parse(s[k]);
                                if (d[v] + w < d[k])
                                {
                                    d[k] = d[v] + w;
                                }
                            }
                        }
                    }
                }
                if (d[v2] != int.MaxValue)
                {
                    if (d[v2] <= L)
                        Console.WriteLine("Путь между {0} и {1} равен {2} - не более L", v1, v2, d[v2]);
                    else
                        Console.WriteLine("Путь между {0} и {1} равен {2} - более L", v1, v2, d[v2]);
                }
                else
                    Console.WriteLine("Не существует пути между {0} и {1}", v1, v2);
            }
        }
        public void Ford_Bellman(string v0, int N)
        {
            Dictionary<KeyValuePair<string, string>, int> edge = new Dictionary<KeyValuePair<string, string>, int>();
            foreach (string v1 in vertices.Keys)
            {
                foreach (var v2 in vertices[v1])
                {
                    edge.Add(new KeyValuePair<string, string> (v1, v2.Key), int.Parse(v2.Value));
                }
            }
            Dictionary<string, Dictionary<string, int>> out_d = new Dictionary<string, Dictionary<string, int>>();

            foreach (string vvv in vertices.Keys)
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                foreach (string v in vertices.Keys)
                {
                    d.Add(v, int.MaxValue);
                }
                d[vvv] = 0;
                for (int i = 0; i < vertices.Count; i++)
                {
                    bool exit = true;
                    foreach (var e in edge.Keys)
                    {
                        string a = e.Key;
                        string b = e.Value;
                        int w = edge[e];
                        if (d[a] < int.MaxValue)
                        {
                            if (d[b] > d[a] + w)
                            {
                                d[b] = d[a] + w;
                                exit = false;
                            }
                        }
                    }
                    if (exit)
                        break;
                }
                out_d.Add(vvv, d);
            }

            List<string> vertices_FB = new List<string>();
            foreach (string it in out_d.Keys)
            {
                if (out_d[it][v0] <= N && it != v0)
                    vertices_FB.Add(it);
            }
            if (vertices_FB.Count != 0)
                Console.WriteLine(string.Join(" ", vertices_FB));
            else
                Console.WriteLine("Таких вершин нет в орграфе");
        }
    }
}
