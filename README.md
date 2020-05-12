# SMT#

This project provides a parser for a limited subset of the SMT-LIB standard for interaction with 
Satisfiability Modulo Theory (SMT) solvers. While the parser is generated using an attributed grammar
and the [Coco/R](http://www.ssw.uni-linz.ac.at/Coco/) compiler generator.
By now, mostly the theories of **FixedSizeBitVector** and **FloatingPoint** are supported and internally
represented in a graph-like structure of first-class expression types.

The tool then allows to print the resulting expressions (starting from the formula *leaves* - i.e. the variable usages)
as well as export of GraphVIZ code for visualization.

## Options
TODO

## Open tasks
- Add SMT-LIB generator to convert expressions back to SMT-LIB
- Similarity check for added expressions (e.g. do not generate same expression twice)
- Add obfuscator for SMT-LIB
- Update colors and labels in GraphViz exporter
- Add GraphViz generator (binary format)
