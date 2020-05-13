# SMT#

This project provides a parser for a limited subset of the [SMT-LIB standard](http://smtlib.cs.uiowa.edu/) for interaction with 
Satisfiability Modulo Theory (SMT) solvers. While the parser is generated using an attributed grammar
and the [Coco/R](http://www.ssw.uni-linz.ac.at/Coco/) compiler generator.
By now, mostly the theories of **FixedSizeBitVector** and **FloatingPoint** are supported and internally
represented in a graph-like structure of first-class expression types.

The tool then allows to print the resulting expressions (starting from the formula *leaves* - i.e. the variable usages)
as well as export of [GraphVIZ](https://graphviz.org/) code for visualization.
See the [DOT](https://graphviz.gitlab.io/_pages/doc/info/lang.html) format for more information.

This project was developed as a toy application to learn and explore some of the unique language features of C#, 
thus some components were specifically designed to embrace concepts like *runtime supported generic types*,
*delegates*, *properties* or *operator overloading*.

## Usage
The program inherently manages one formula that may be extended via directly input expressions or parsed files.
The state itself can be controlled via meta commands to reset the formula or get an overview of the currently stored expressions.
Note that SMT-LIB expressions for evaluation (`(check-sat)`, `(get-value (...))`) or assertion state control such as `push`, `pop` are simply 
ignored by the parser.

## Options
The project is presented as a basic command line app, sporting a number of interpreted commands.
Per default, any input string is treated as an SMT-LIB expression and therefore directly fed to the parser.
Commands have to be prefixed by "`:`":
- `:load`
  
  It allows input of a file name which must contain valid SMT-LIB expressions.
  The full file is read and parsed and the resulting expressions and declarations added to the current state.

- `:show`
  
  Prints all currently stored expressions to the command line.

- `:vars`

  Prints all declared variables to the command line.

- `:reset`

  Resets the current formula.
  
- `:dot`
  Allows input of a target file to which the current expressions are written in DOT format.

- `:exit`

  Stops the app.

## Q&A
- Which SMT-LIB logics are supported?

  Currently, a subset of definitions from the 
  [Core](http://smtlib.cs.uiowa.edu/theories-Core.shtml) (`Bool` sort etc.), 
  [FixedSizeBitVector](http://smtlib.cs.uiowa.edu/theories-FixedSizeBitVectors.shtml) and
  [FloatingPoint](http://smtlib.cs.uiowa.edu/theories-FloatingPoint.shtml) theories are implemented, 
  but the expression framework allows extension for additional operations or sorts.

- How can the GraphVIZ code be opened?

  If not directly installed on the system, there exist various online tools for visualizing DOT files.
  Examples:
  - https://dreampuf.github.io/GraphvizOnline/
  - http://webgraphviz.com/
  - http://sandbox.kidstrythisathome.com/erdos/

- Why are the expressions implemented as first-class objects?
  
  Of course, in its current state expressions could also be implemented in a more limited set of types (e.g. `Expression{Type = ADD, X = <..>, Y = <..>}`).
  This was not done on purpose, as to enable future, expression specific simplifications or implementation details (e.g. simplifications for logic combinators) as well as
  already specialize some expressions towards types (e.g. different `And` expressions for `FixedSizeBitVector` and `Bool`).
  
- Why is there no GUI?
  
  This project is primarily developed on linux using the .NET core utilities. Therefore, framework support for *Windows Forms* or *WPF* is lacking.
  Additionally, platform independence is (despite using C#) a major requirement.

## Future work
- Add SMT-LIB generator to convert expressions back to SMT-LIB
- Similarity check for added expressions (e.g. do not generate same expression twice)
- Add obfuscator for SMT-LIB (e.g. automated variable renaming, extraction of nested expressions)
- Update colors and labels in GraphViz exporter
- Add GraphViz generator (binary format)
