using System;
using System.Diagnostics;

namespace OrdenacaoTransacoes;

class Transacao {
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public string Status { get; set; } = "";

    public override string ToString() {
        return $"ID:{Id:D7} | R${Valor,9:F2} | {Data:dd/MM/yyyy} | {Status}";
    }
}

static class Ordenacao {
    public static void InsertionSort(Transacao[] vetor) {
        for (int i = 1; i < vetor.Length; i++) {
            Transacao tmp = vetor[i];
            int j = i - 1;
            while (j >= 0 && vetor[j].Valor > tmp.Valor) {
                vetor[j + 1] = vetor[j];
                j--;
            }
            vetor[j + 1] = tmp;
        }
    }

    public static void MergeSort(Transacao[] vetor, int esq, int dir) {
        if (esq < dir) {
            int meio = (esq + dir) / 2;
            MergeSort(vetor, esq, meio);
            MergeSort(vetor, meio + 1, dir);
            Intercalar(vetor, esq, meio, dir);
        }
    }

    private static void Intercalar(Transacao[] vetor, int esq, int meio, int dir) {
        int nEsq = (meio + 1) - esq;
        int nDir = dir - meio;
        Transacao[] vetorEsq = new Transacao[nEsq + 1];
        Transacao[] vetorDir = new Transacao[nDir + 1];

        vetorEsq[nEsq] = new Transacao { Valor = decimal.MaxValue };
        vetorDir[nDir] = new Transacao { Valor = decimal.MaxValue };

        for (int iEsq = 0; iEsq < nEsq; iEsq++) {
            vetorEsq[iEsq] = vetor[esq + iEsq];
        }
        for (int iDir = 0; iDir < nDir; iDir++) {
            vetorDir[iDir] = vetor[(meio + 1) + iDir];
        }

        int e = 0, d = 0;
        for (int i = esq; i <= dir; i++) {
            if (vetorEsq[e].Valor <= vetorDir[d].Valor) {
                vetor[i] = vetorEsq[e++];
            } else {
                vetor[i] = vetorDir[d++];
            }
        }
    }

    public static void QuickSort(Transacao[] vetor, int esq, int dir) {
        int i = esq, j = dir;
        decimal pivo = vetor[(esq + dir) / 2].Valor;

        while (i <= j) {
            while (vetor[i].Valor < pivo) {
                i++;
            }
            while (vetor[j].Valor > pivo) {
                j--;
            }
            if (i <= j) {
                Swap(vetor, i, j);
                i++;
                j--;
            }
        }

        if (esq < j) {
            QuickSort(vetor, esq, j);
        }
        if (i < dir) {
            QuickSort(vetor, i, dir);
        }
    }

    public static void HeapSort(Transacao[] vetor) {
        int n = vetor.Length;

        for (int tam = 1; tam < n; tam++) {
            Construir(vetor, tam);
        }

        for (int tam = n - 1; tam > 0; tam--) {
            Swap(vetor, 0, tam);
            Reconstruir(vetor, tam);
        }
    }

    private static void Construir(Transacao[] vetor, int i) {
        while (i > 0 && vetor[i].Valor > vetor[(i - 1) / 2].Valor) {
            Swap(vetor, i, (i - 1) / 2);
            i = (i - 1) / 2;
        }
    }

    private static void Reconstruir(Transacao[] vetor, int tam) {
        int i = 0;
        while ((2 * i) + 1 < tam) {
            int filho = GetMaiorFilho(vetor, i, tam);
            if (vetor[i].Valor < vetor[filho].Valor) {
                Swap(vetor, i, filho);
                i = filho;
            } else {
                i = tam;
            }
        }
    }

    private static int GetMaiorFilho(Transacao[] vetor, int i, int tam) {
        int esq = (2 * i) + 1;
        int dir = esq + 1;
        if (dir >= tam || vetor[esq].Valor > vetor[dir].Valor) {
            return esq;
        }
        return dir;
    }

    private static void Swap(Transacao[] vetor, int a, int b) {
        Transacao tmp = vetor[a];
        vetor[a] = vetor[b];
        vetor[b] = tmp;
    }
}

class Program {
    const int LimiteInsertion = 100_000;

    const bool Pular100Milhoes = true;

    static readonly string[] StatusPossiveis = { "Aprovada", "Pendente", "Recusada", "Estornada" };

    static Transacao[] GerarDataset(int n, int semente = 42) {
        Random rnd = new Random(semente);
        Transacao[] dados = new Transacao[n];
        DateTime hoje = DateTime.Today;

        for (int i = 0; i < n; i++) {
            dados[i] = new Transacao {
                Id = i + 1,
                Valor = Math.Round((decimal)(rnd.NextDouble() * 10_000), 2),
                Data = hoje,
                Status = StatusPossiveis[rnd.Next(StatusPossiveis.Length)]
            };
        }
        return dados;
    }

    static double Medir(Action acao) {
        Stopwatch sw = Stopwatch.StartNew();
        acao();
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds;
    }

    static bool EstaOrdenado(Transacao[] vetor) {
        for (int i = 1; i < vetor.Length; i++) {
            if (vetor[i - 1].Valor > vetor[i].Valor) {
                return false;
            }
        }
        return true;
    }

    static string Testar(Transacao[] original, Action<Transacao[]> algoritmo, out Transacao[] copia) {
        copia = (Transacao[])original.Clone();
        Transacao[] alvo = copia;
        string tempo = Formatar(Medir(() => algoritmo(alvo)));
        if (!EstaOrdenado(copia)) {
            tempo += " (ERRO!)";
        }
        return tempo;
    }

    static void ExibirTop10(Transacao[] ordenado) {
        Console.WriteLine("\n10 maiores transações (da maior para a menor):");
        int inicio = Math.Max(0, ordenado.Length - 10);
        for (int i = ordenado.Length - 1; i >= inicio; i--) {
            Console.WriteLine("  " + ordenado[i]);
        }
    }

    static string Formatar(double ms) {
        return $"{ms,12:F2} ms";
    }

    static void Main() {
        int[] tamanhos = { 100, 1_000, 10_000, 100_000, 1_000_000, 10_000_000, 100_000_000 };

        Console.WriteLine("=== Comparação de algoritmos de ordenação (tempo em ms) ===");
        Console.WriteLine($"{"n",13} | {"Insertion",15} | {"Merge",15} | {"Quick",15} | {"Heap",15}");
        Console.WriteLine(new string('-', 87));

        Transacao[]? ultimoOrdenado = null;

        foreach (int n in tamanhos) {
            if (n == 100_000_000 && Pular100Milhoes) {
                Console.WriteLine($"{n,13:N0} | pulado (memória insuficiente por padrão; veja Pular100Milhoes)");
                continue;
            }

            Transacao[] original;
            try {
                original = GerarDataset(n);
            } catch (OutOfMemoryException) {
                Console.WriteLine($"{n,13:N0} | sem memória para gerar o dataset");
                continue;
            }

            string tInsertion = "pulado (O(n²))";
            if (n <= LimiteInsertion) {
                tInsertion = Testar(original, v => Ordenacao.InsertionSort(v), out _);
            }
            string tMerge = Testar(original, v => Ordenacao.MergeSort(v, 0, v.Length - 1), out _);
            string tQuick = Testar(original, v => Ordenacao.QuickSort(v, 0, v.Length - 1), out Transacao[] ordenadoQuick);
            string tHeap = Testar(original, v => Ordenacao.HeapSort(v), out _);

            Console.WriteLine($"{n,13:N0} | {tInsertion,15} | {tMerge,15} | {tQuick,15} | {tHeap,15}");
            ultimoOrdenado = ordenadoQuick;
        }

        if (ultimoOrdenado != null) {
            ExibirTop10(ultimoOrdenado);
        }
    }
}
