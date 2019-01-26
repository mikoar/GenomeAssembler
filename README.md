# GenomeAssembler

## Summary

**GenomeAssembler** is a program for *de novo* genome assembly program. It uses [De Bruijn Graph](https://en.wikipedia.org/wiki/De_Bruijn_graph) approach to merge intersecting short genome sequencing reads into longer sequences (contigs).

## Overview

The method was inpsired by Velvet algorithm ([Zerbino et al.](http://www.genome.org/cgi/doi/10.1101/gr.074492.107)). First step is to represent reads as a graph of lenght *k* sequences, where nodes connected by an edge overlap by *k - 1* nucleotides.

Then the graph undergoes simplification process, i.e. linear connected subgraphs are merged.

Short chains of nodes disconnected on one end, called "tips", are predominantly result of sequencing errors and are removed in next step.

The original algorithm's essential part is removal of redundant paths - starting and ending at the same node and containing similar sequences. This feature is part of a roadmap and is not implemented yet.

After all, abiguous connections that failed to resolve in previous steps are removed based on coverage cutoff. Sequences corresponding to subgraphs with minimum length of 300bp are returned.

Initially [an error correction approach based on *k-mer* frequency](https://www.cs.jhu.edu/~langmea/resources/lecture_notes/error_correction.pdf)  was considered. However it has turned out to yield inappropriate results and was disabled.

# Getting started

## Prerequisites
Building from source requires .Net Core SDK >= 2.2

## Compile
```bash
git clone https://github.com/mikoar/GenomeAssembler
cd GenomeAssembler
dotnet publish Assembly/Assembly.csproj -c Release -o <output-dir>
```

## Usage
```bash
assembly [arguments] [options]

Arguments:
  reads                     fasta file with reads
  contigs                   contigs output fasta file

Options:
  --version                 Show version information
  -?|-h|--help              Show help information
  -d|--dot <DOT_FILE_PATH>  output graph to dot file
  -k|--k <K>                Mer length, default is 19
```

# License

This software is distributed under [WTFPL license](http://www.wtfpl.net/).
