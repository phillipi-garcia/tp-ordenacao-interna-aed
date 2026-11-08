# TP Ordenação Interna: Heapsort e Quicksort

Atividade avaliativa de **Algoritmos e Estrutura de Dados** (PUC Minas, Campus Contagem, 2026/2), Prof. Fábio Cordeiro.

## Integrantes

- Caique Villarreal
- João Pedro Vital Fortes
- Phillipi Garcia

## O que o programa faz

Aplicação prática em C# que simula o módulo de relatórios de uma fintech:

1. Gera um conjunto de `n` transações aleatórias (ID, valor, data e status), com semente fixa.
2. Ordena as transações por valor, em ordem crescente, com **Insertion Sort**, **Merge Sort**, **Quick Sort** e **Heap Sort**.
3. Mede o tempo de cada algoritmo com `Stopwatch` para n = 100, 1.000, 10.000, 100.000, 1.000.000, 10.000.000 e 100.000.000. Cada algoritmo ordena uma cópia do mesmo vetor.
4. Confere se cada resultado ficou ordenado e exibe as 10 maiores transações.

O Insertion Sort é pulado acima de 100.000 elementos, porque é O(n²). O caso de 100 milhões vem desativado por padrão, porque exige vários GB de memória. Para rodar esse caso, mude `Pular100Milhoes` para `false` em `Program.cs`.

## Como executar

Requer o .NET SDK.

```bash
cd TP_Ordenacao_Interna
dotnet run -c Release
```

## Apresentação

- [Vídeo da apresentação](apresentacao/video-heapsort-quicksort.mp4)
- [Slides em PDF](apresentacao/slides-heapsort-quicksort.pdf)
